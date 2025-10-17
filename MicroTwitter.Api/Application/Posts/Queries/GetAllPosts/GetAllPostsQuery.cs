using System.Collections.Generic;
using MediatR;
using MicroTwitter.Api.Domain.Models;
namespace MicroTwitter.Api.Application.Posts.Queries.GetAllPosts
{
    public class GetAllPostsQuery : IRequest<List<PostViewModel>>
    {
    }
}
