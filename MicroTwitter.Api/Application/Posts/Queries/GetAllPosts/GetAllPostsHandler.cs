using MediatR;
using Microsoft.EntityFrameworkCore;
using MicroTwitter.Api.Domain.Models;
using MicroTwitter.Api.Infrastructure.Data;

namespace MicroTwitter.Api.Application.Posts.Queries.GetAllPosts
{
    public class GetAllPostsHandler : IRequestHandler <GetAllPostsQuery, List<PostViewModel>>
    {
        private readonly AppDbContext _context;
        public GetAllPostsHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<PostViewModel>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Posts
                 .Include(p => p.User)
                 .OrderByDescending(p => p.CreatedAt)
                 .Select(p => new PostViewModel
                 {
                     Id = p.Id,
                     Content = p.Content,
                     ImageUrl = p.ImageUrl,
                     Username = p.User.Username,
                     CreatedAt = p.CreatedAt
                 })
                 .ToListAsync(cancellationToken);
        }
    }
}
