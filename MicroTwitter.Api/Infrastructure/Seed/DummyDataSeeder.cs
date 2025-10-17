using MicroTwitter.Api.Domain.Models;
using MicroTwitter.Api.Infrastructure.Data;

namespace MicroTwitter.Api.Infrastructure.Seed
{
    public class DummyDataSeeder
    {

        public static void Seed(AppDbContext context)
        {


            if(!context.Users.Any())
            {
                var users = new List<User>
                {
                     new User { 
                         Id = Guid.NewGuid(), 
                         Username = "Lea1", 
                         Email = "lea1@example.com", 
                         PasswordHash = Array.Empty<byte>(), 
                         PasswordSalt = Array.Empty<byte>() },

                     new User { 
                         Id = Guid.NewGuid(), 
                         Username = "Sim4e2", 
                         Email = "sim4e2@example.com", 
                         PasswordHash = Array.Empty<byte>(), 
                         PasswordSalt = Array.Empty<byte>() },

                     new User { 
                         Id = Guid.NewGuid(), 
                         Username = "Lili3", 
                         Email = "lili3@example.com", 
                         PasswordHash = Array.Empty<byte>(), 
                         PasswordSalt = Array.Empty<byte>() },

                     new User { 
                         Id = Guid.NewGuid(), 
                         Username = "Sar4e4", 
                         Email = "sar4e4@example.com", 
                         PasswordHash = Array.Empty<byte>(), 
                         PasswordSalt = Array.Empty<byte>() },

                     new User { 
                         Id = Guid.NewGuid(), 
                         Username = "Aleks5", 
                         Email = "aleks5@example.com", 
                         PasswordHash = Array.Empty<byte>(), 
                         PasswordSalt = Array.Empty<byte>() }
                     };
                context.Users.AddRange(users);
                context.SaveChanges();

                if (!context.Posts.Any())
                {
                    var usersList = context.Users.ToList();
                    var posts = new List<Post>
                    {
                        new Post {
                            Id = Guid.NewGuid(),
                            UserId = usersList[0].Id,
                            Content = "This is my first post on MicroTwitter!",
                            CreatedAt = DateTime.UtcNow.AddDays(-10),
                            ImageUrl = null
                        },
                        new Post {
                            Id = Guid.NewGuid(),
                            UserId = usersList[1].Id,
                            Content = "Loving the new features of MicroTwitter. #excited",
                            CreatedAt = DateTime.UtcNow.AddDays(-8),
                            ImageUrl = "https://picsum.photos/200/300?random=1"
                        },
                        new Post {
                            Id = Guid.NewGuid(),
                            UserId = usersList[2].Id,
                            Content = "Just had a great day at the park with friends.",
                            CreatedAt = DateTime.UtcNow.AddDays(-6),
                            ImageUrl = null
                        },
                        new Post {
                            Id = Guid.NewGuid(),
                            UserId = usersList[3].Id,
                            Content = "Check out this cool photo I took! #photography",
                            CreatedAt = DateTime.UtcNow.AddDays(-4),
                            ImageUrl = "https://picsum.photos/200/300?random=2"
                        },
                        new Post {
                            Id = Guid.NewGuid(),
                            UserId = usersList[4].Id,
                            Content = "Looking forward to the weekend. Any plans?",
                            CreatedAt = DateTime.UtcNow.AddDays(-2),
                            ImageUrl = null
                        }
                    };
                    context.Posts.AddRange(posts);
                    context.SaveChanges();
                }
            }
        }
    }
}
