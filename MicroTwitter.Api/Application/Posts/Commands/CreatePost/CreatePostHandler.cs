using MediatR;
using MicroTwitter.Api.Domain.Models;
using MicroTwitter.Api.Infrastructure.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace MicroTwitter.Api.Application.Posts.Commands.CreatePost
{
    public class CreatePostHandler : IRequestHandler<CreatePostCommand, PostViewModel>
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public CreatePostHandler(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<PostViewModel> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            if (string.IsNullOrWhiteSpace(request.Content) || request.Content.Length < 12 || request.Content.Length > 140)
                throw new ArgumentException("Post content must be between 12 and 140 characters.");


            var post = new Post
            {
                Id = Guid.NewGuid(),
                UserId = Guid.Parse(userId),
                Content = request.Content,
                ImageUrl = request.ImageUrl,
                CreatedAt = DateTime.UtcNow
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync(cancellationToken);
            var user = await _context.Users.FindAsync(post.UserId);

            return new PostViewModel
            {
                Id = post.Id,
                Content = post.Content,
                Username = user.Username,
                ImageUrl = post.ImageUrl,
                CreatedAt = DateTime.UtcNow

            };
        }
    }
}
