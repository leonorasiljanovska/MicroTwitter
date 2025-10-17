using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MicroTwitter.Api.Application.Posts.Commands.CreatePost;
using MicroTwitter.Api.Application.Posts.Commands.DeletePost;
using MicroTwitter.Api.Application.Posts.Queries.GetAllPosts;
using MicroTwitter.Api.Application.Posts.Queries.GetUserPosts;
using MicroTwitter.Api.Domain.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MicroTwitter.Tests
{
    public class PostHandlersTests
    {
        // ========================= CREATE POST =========================

        [Fact]
        public async Task CreatePostHandler_CreatesPost_WhenValidContent()
        {
            using var context = DbContextFactory.CreateInMemory();

            var userId = Guid.NewGuid();
            context.Users.Add(new User { Id = userId, Username = "testuser" });
            await context.SaveChangesAsync();

            var claims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }));
            var httpContextMock = new Mock<IHttpContextAccessor>();
            httpContextMock.Setup(a => a.HttpContext.User).Returns(claims);

            var handler = new CreatePostHandler(context, httpContextMock.Object);

            var command = new CreatePostCommand
            {
                Content = "Hello World",
                ImageUrl = "http://example.com/img.png"
            };

            var result = await handler.Handle(command, CancellationToken.None);

            // Verify saved to DB
            var post = await context.Posts.FirstAsync();
            post.Content.Should().Be(command.Content);
            post.UserId.Should().Be(userId);

            // Verify returned model
            result.Should().NotBeNull();
            result.Content.Should().Be(command.Content);
            result.ImageUrl.Should().Be(command.ImageUrl);
            result.Username.Should().Be("testuser");
            result.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Theory]
        [InlineData("Too short")] // <12 chars
        [InlineData("This is a very long post that exceeds the maximum allowed length of 140 characters. " +
                    "It should be rejected by the handler and not saved to the database.")]
        public async Task CreatePostHandler_RejectsInvalidLengthPosts(string content)
        {
            using var context = DbContextFactory.CreateInMemory();

            var userId = Guid.NewGuid();
            context.Users.Add(new User { Id = userId, Username = "testuser" });
            await context.SaveChangesAsync();

            var claims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }));
            var httpContextMock = new Mock<IHttpContextAccessor>();
            httpContextMock.Setup(a => a.HttpContext.User).Returns(claims);

            var handler = new CreatePostHandler(context, httpContextMock.Object);

            var command = new CreatePostCommand
            {
                Content = content
            };

            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<ArgumentException>();
        }

        // ========================= DELETE POST =========================

        [Fact]
        public async Task DeletePostHandler_DeletesPost_WhenUserIsOwner()
        {
            using var context = DbContextFactory.CreateInMemory();

            var userId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            context.Users.Add(new User { Id = userId, Username = "testuser" });
            context.Posts.Add(new Post { Id = postId, UserId = userId, Content = "Test", CreatedAt = DateTime.UtcNow });
            await context.SaveChangesAsync();

            var handler = new DeletePostHandler(context);

            var command = new DeletePostCommand
            {
                Id = postId,
                UserId = userId
            };

            var result = await handler.Handle(command, CancellationToken.None);

            result.Should().BeTrue();
            (await context.Posts.FindAsync(postId)).Should().BeNull();
        }

        [Fact]
        public async Task DeletePostHandler_ReturnsFalse_WhenUserNotOwner()
        {
            using var context = DbContextFactory.CreateInMemory();

            var userId = Guid.NewGuid();
            var otherUserId = Guid.NewGuid();
            var postId = Guid.NewGuid();

            context.Users.AddRange(
                new User { Id = userId, Username = "owner" },
                new User { Id = otherUserId, Username = "attacker" }
            );
            context.Posts.Add(new Post { Id = postId, UserId = userId, Content = "Test", CreatedAt = DateTime.UtcNow });
            await context.SaveChangesAsync();

            var handler = new DeletePostHandler(context);

            var command = new DeletePostCommand
            {
                Id = postId,
                UserId = otherUserId
            };

            var result = await handler.Handle(command, CancellationToken.None);

            result.Should().BeFalse();
            (await context.Posts.FindAsync(postId)).Should().NotBeNull();
        }

        [Fact]
        public async Task DeletePostHandler_ReturnsFalse_WhenPostDoesNotExist()
        {
            using var context = DbContextFactory.CreateInMemory();

            var userId = Guid.NewGuid();
            context.Users.Add(new User { Id = userId, Username = "testuser" });
            await context.SaveChangesAsync();

            var handler = new DeletePostHandler(context);

            var command = new DeletePostCommand
            {
                Id = Guid.NewGuid(), // non-existent post
                UserId = userId
            };

            var result = await handler.Handle(command, CancellationToken.None);

            result.Should().BeFalse();
        }

        // ========================= GET ALL POSTS =========================

        [Fact]
        public async Task GetAllPostsHandler_ReturnsAllPostsOrdered()
        {
            using var context = DbContextFactory.CreateInMemory();

            var user1 = new User { Id = Guid.NewGuid(), Username = "user1" };
            var user2 = new User { Id = Guid.NewGuid(), Username = "user2" };
            context.Users.AddRange(user1, user2);
            await context.SaveChangesAsync();

            var post1 = new Post { Id = Guid.NewGuid(), UserId = user1.Id, Content = "Post1", CreatedAt = DateTime.UtcNow.AddMinutes(-1) };
            var post2 = new Post { Id = Guid.NewGuid(), UserId = user2.Id, Content = "Post2", CreatedAt = DateTime.UtcNow };
            context.Posts.AddRange(post1, post2);
            await context.SaveChangesAsync();

            var handler = new GetAllPostsHandler(context);
            var query = new GetAllPostsQuery();

            var result = await handler.Handle(query, CancellationToken.None);

            result.Count.Should().Be(2);
            result[0].Content.Should().Be("Post2"); // newest first
            result[1].Content.Should().Be("Post1");
            result.All(p => !string.IsNullOrEmpty(p.Username)).Should().BeTrue();
        }

        [Fact]
        public async Task GetAllPostsHandler_ReturnsEmptyList_WhenNoPosts()
        {
            using var context = DbContextFactory.CreateInMemory();
            var handler = new GetAllPostsHandler(context);
            var query = new GetAllPostsQuery();
            var result = await handler.Handle(query, CancellationToken.None);
            result.Should().BeEmpty();
        }

        // ========================= GET USER POSTS =========================

        [Fact]
        public async Task GetUserPostsHandler_ReturnsOnlyUserPosts()
        {
            using var context = DbContextFactory.CreateInMemory();

            var user1 = new User { Id = Guid.NewGuid(), Username = "user1" };
            var user2 = new User { Id = Guid.NewGuid(), Username = "user2" };
            context.Users.AddRange(user1, user2);
            await context.SaveChangesAsync();

            context.Posts.AddRange(
                new Post { Id = Guid.NewGuid(), UserId = user1.Id, Content = "Post1", CreatedAt = DateTime.UtcNow },
                new Post { Id = Guid.NewGuid(), UserId = user2.Id, Content = "Post2", CreatedAt = DateTime.UtcNow }
            );
            await context.SaveChangesAsync();

            var handler = new GetUserPostsHandler(context);
            var query = new GetUserPostsQuery { UserId = user1.Id };

            var result = await handler.Handle(query, CancellationToken.None);

            result.Count.Should().Be(1);
            result[0].UserId.Should().Be(user1.Id);
            result[0].Content.Should().Be("Post1");
        }

        [Fact]
        public async Task GetUserPostsHandler_ReturnsEmpty_WhenUserHasNoPosts()
        {
            using var context = DbContextFactory.CreateInMemory();

            var user1 = new User { Id = Guid.NewGuid(), Username = "user1" };
            context.Users.Add(user1);
            await context.SaveChangesAsync();

            var handler = new GetUserPostsHandler(context);
            var query = new GetUserPostsQuery { UserId = user1.Id };

            var result = await handler.Handle(query, CancellationToken.None);

            result.Should().BeEmpty();
        }
    }
}
