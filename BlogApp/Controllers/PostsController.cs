using BlogApp.Business.Services;
using BlogApp.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        {
            var posts = await _postService.GetAllPostsAsync();
            return Ok(posts);
        }

        [HttpGet("with-tags")]
        public async Task<ActionResult<IEnumerable<Post>>> GetPostsWithTags()
        {
            var posts = await _postService.GetPostsWithTagsAsync();
            return Ok(posts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
            var post = await _postService.GetPostByIdAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            return post;
        }

        [HttpGet("{id}/with-comments")]
        public async Task<ActionResult<Post>> GetPostWithComments(int id)
        {
            var post = await _postService.GetPostWithCommentsAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            return post;
        }

        [HttpGet("author/{authorId}")]
        public async Task<ActionResult<IEnumerable<Post>>> GetPostsByAuthor(int authorId)
        {
            var posts = await _postService.GetPostsByAuthorAsync(authorId);
            return Ok(posts);
        }

        [HttpPost]
        public async Task<ActionResult<Post>> PostPost(Post post)
        {
            var createdPost = await _postService.CreatePostAsync(post);
            return CreatedAtAction(nameof(GetPost), new { id = createdPost.Id }, createdPost);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost(int id, Post post)
        {
            if (id != post.Id)
            {
                return BadRequest();
            }

            if (!await _postService.PostExistsAsync(id))
            {
                return NotFound();
            }

            await _postService.UpdatePostAsync(post);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            if (!await _postService.PostExistsAsync(id))
            {
                return NotFound();
            }

            await _postService.DeletePostAsync(id);

            return NoContent();
        }
    }
}