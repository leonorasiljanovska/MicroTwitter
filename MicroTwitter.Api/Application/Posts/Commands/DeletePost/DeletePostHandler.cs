using MediatR;
using Microsoft.EntityFrameworkCore;
using MicroTwitter.Api.Infrastructure.Data;

namespace MicroTwitter.Api.Application.Posts.Commands.DeletePost
{
    public class DeletePostHandler : IRequestHandler<DeletePostCommand, bool>
    {
        private readonly AppDbContext _context;

        public DeletePostHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _context.Posts.SingleOrDefaultAsync(p=>p.Id.Equals(request.Id) && p.UserId.Equals(request.UserId),cancellationToken);

            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync(cancellationToken);
                return true;

            }
            return false;
        }
    }
}
