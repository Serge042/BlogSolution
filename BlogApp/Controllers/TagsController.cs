using BlogApp.Business.Services;
using BlogApp.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }

        // GET: api/Tags
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tag>>> GetTags()
        {
            var tags = await _tagService.GetAllTagsAsync();
            return Ok(tags);
        }

        // GET: api/Tags/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tag>> GetTag(int id)
        {
            var tag = await _tagService.GetTagByIdAsync(id);

            if (tag == null)
            {
                return NotFound();
            }

            return tag;
        }

        // GET: api/Tags/name/dotnet
        [HttpGet("name/{name}")]
        public async Task<ActionResult<Tag>> GetTagByName(string name)
        {
            var tag = await _tagService.GetTagByNameAsync(name);

            if (tag == null)
            {
                return NotFound();
            }

            return tag;
        }

        // POST: api/Tags
        [HttpPost]
        public async Task<ActionResult<Tag>> PostTag(Tag tag)
        {
            var createdTag = await _tagService.CreateTagAsync(tag);
            return CreatedAtAction(nameof(GetTag), new { id = createdTag.Id }, createdTag);
        }

        // PUT: api/Tags/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTag(int id, Tag tag)
        {
            if (id != tag.Id)
            {
                return BadRequest();
            }

            if (!await _tagService.TagExistsAsync(id))
            {
                return NotFound();
            }

            await _tagService.UpdateTagAsync(tag);

            return NoContent();
        }

        // DELETE: api/Tags/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            if (!await _tagService.TagExistsAsync(id))
            {
                return NotFound();
            }

            await _tagService.DeleteTagAsync(id);

            return NoContent();
        }
    }
}