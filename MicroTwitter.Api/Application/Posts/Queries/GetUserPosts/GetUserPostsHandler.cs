using MediatR;
using Microsoft.EntityFrameworkCore;
using MicroTwitter.Api.Domain.Models;
using MicroTwitter.Api.Infrastructure.Data;

namespace MicroTwitter.Api.Application.Posts.Queries.GetUserPosts
{
    public class GetUserPostsHandler : IRequestHandler<GetUserPostsQuery, List<Post>>
    {
        private readonly AppDbContext _context;
        
        public GetUserPostsHandler(AppDbContext context)
        {
            _context = context;
        }

        public Task<List<Post>> Handle(GetUserPostsQuery request, CancellationToken cancellationToken)
        {
            return _context.Posts
                .Where(post=>post.UserId==request.UserId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync(cancellationToken);
        }
    }
}
