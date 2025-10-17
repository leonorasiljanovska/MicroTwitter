using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroTwitter.Api.Application.Posts.Commands.CreatePost;
using MicroTwitter.Api.Application.Posts.Commands.DeletePost;
using MicroTwitter.Api.Application.Posts.Queries.GetAllPosts;
using MicroTwitter.Api.Application.Posts.Queries.GetUserPosts;
using MicroTwitter.Api.Domain.Models;
using MicroTwitter.Api.Infrastructure.Data;
using System.Security.Claims;

namespace MicroTwitter.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class PostsController :ControllerBase
    {
        private readonly IMediator _mediator;
        public PostsController(IMediator mediator) {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<Post>>> GetAllPosts()
        {
            var posts=await _mediator.Send(new GetAllPostsQuery());
            return Ok(posts);
        }

        [HttpGet("my-posts")]
        [Authorize]
        public async Task<ActionResult<List<Post>>> GetMyPosts()
        {
            
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var query = new GetUserPostsQuery { UserId = Guid.Parse(userId) };
            var posts = await _mediator.Send(query);
            return Ok(posts);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<PostViewModel>> CreatePost([FromBody] CreatePostCommand command)
        {
            var userIdString= User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(!Guid.TryParse(userIdString, out var userId))
            {
                return Unauthorized();
            }

            //command.UserId = userId;


            var post = await _mediator.Send(command);
            return CreatedAtAction(nameof(CreatePost), new { id = post.Id }, post);
        }

        [HttpDelete("{id}")]

        public async Task<ActionResult> DeletePost(Guid id)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            var result = await _mediator.Send(new DeletePostCommand { Id = id, UserId = userId });

            if(!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }


}
