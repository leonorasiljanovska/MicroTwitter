using MediatR;
using MicroTwitter.Api.Domain.Models;

namespace MicroTwitter.Api.Application.Posts.Commands.CreatePost
{
    public class CreatePostCommand : IRequest<PostViewModel>
    {
        public string Content { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }  //optional Image

    }
}
