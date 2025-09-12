using BlogApp.Business.Services;
using BlogApp.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        // GET: api/Comments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
        {
            var comments = await _commentService.GetAllCommentsAsync();
            return Ok(comments);
        }

        // GET: api/Comments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetComment(int id)
        {
            var comment = await _commentService.GetCommentByIdAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return comment;
        }

        // GET: api/Comments/post/5
        [HttpGet("post/{postId}")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetCommentsByPost(int postId)
        {
            var comments = await _commentService.GetCommentsByPostAsync(postId);
            return Ok(comments);
        }

        // GET: api/Comments/user/5
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetCommentsByUser(int userId)
        {
            var comments = await _commentService.GetCommentsByUserAsync(userId);
            return Ok(comments);
        }

        // POST: api/Comments
        [HttpPost]
        public async Task<ActionResult<Comment>> PostComment(Comment comment)
        {
            var createdComment = await _commentService.CreateCommentAsync(comment);
            return CreatedAtAction(nameof(GetComment), new { id = createdComment.Id }, createdComment);
        }

        // PUT: api/Comments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment(int id, Comment comment)
        {
            if (id != comment.Id)
            {
                return BadRequest();
            }

            if (!await _commentService.CommentExistsAsync(id))
            {
                return NotFound();
            }

            await _commentService.UpdateCommentAsync(comment);

            return NoContent();
        }

        // DELETE: api/Comments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            if (!await _commentService.CommentExistsAsync(id))
            {
                return NotFound();
            }

            await _commentService.DeleteCommentAsync(id);

            return NoContent();
        }
    }
}