using MediatR;
using MicroTwitter.Api.Domain.Models;

namespace MicroTwitter.Api.Application.Posts.Queries.GetUserPosts
{
    public class GetUserPostsQuery : IRequest<List<Post>>
    {
        public Guid UserId { get; set; }

    }
}
