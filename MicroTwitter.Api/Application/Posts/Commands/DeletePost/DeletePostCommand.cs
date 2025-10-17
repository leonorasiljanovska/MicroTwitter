using MediatR;

namespace MicroTwitter.Api.Application.Posts.Commands.DeletePost
{
    public class DeletePostCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
    }
}
