using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using System.Security.Claims;
using prid1920_g10.Helpers;
using prid1920_g10.Models;
using PRID_Framework;

namespace prid1920_g10.Controllers {
    [Authorize]
    [Route("api/posts")]
    [ApiController]
    public class PostsController : ControllerBase {
        private readonly G10Context _context;

        public PostsController(G10Context context) {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostDTO>>> GetAll() {
            return (await this.GetQuestions().ToListAsync()).ToDTO();
        }

        private IQueryable<Post> GetQuestions() {
            return (
                from p in _context.Posts
                where p.Title != null
                select p
            );
        }

<<<<<<< HEAD
=======
        // [AllowAnonymous]
        // [HttpGet("tags/{id}")]
        // public async Task<ActionResult<IEnumerable<TagDTO>>> GetTagsById(int id) {
        //     var tags = GetTags(id);

        //     if (tags == null)
        //         return NotFound();
        //     return (await tags.ToListAsync()).ToDTO();
        // }

        // private IQueryable<Tag> GetTags(int postid) {
        //     var tagIds = GetTagIdsFromPostTags(postid);
        //     IQueryable<Tag> query = new IQueryable<Tag>();

        //     return (from tag in _context.Tags
        //             join t in tagIds on tag.Id equals t.Id
        //             where t.Id == tag.Id
        //             select tag);
        // }

        private IQueryable<int> GetTagIdsFromPostTags(int postid) {
            return (from pt in _context.PostTags
                    where pt.PostId == postid
                    select pt.TagId);
        }

>>>>>>> 157a877127ad7a6b6911707f4b01db1f0743e6f5
        [AllowAnonymous]
        [HttpGet("answers/{id}")]
        public async Task<ActionResult<IEnumerable<PostDTO>>> GetPostsById(int id) {
            var answers = GetAnswers(id);

            if (answers == null)
                return NotFound();
            return (await answers.ToListAsync()).ToDTO();
        }

        private IQueryable<Post> GetAnswers(int i) {
            return (
                from p in _context.Posts
                where p.ParentId == i
                select p
            );
        }

        [AllowAnonymous]
        [HttpGet("comments/{id}")]
        public async Task<ActionResult<IEnumerable<CommentDTO>>> GetCommentsById(int id) {
            var comments = GetComments(id);

            if (comments == null)
                return NotFound();
            return (await comments.ToListAsync()).ToDTO();
        }

        private IQueryable<Comment> GetComments(int i) {
            return (
                from c in _context.Comments
                where c.PostId == i
                select c
            );
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<PostDTO>> GetPostById(int id) {
            var post = await _context.Posts.FindAsync(id);

            if (post == null)
                return NotFound();
            return post.ToDTO();
        }

        [AllowAnonymous] //[Authorized(Role.Admin, Role.Member)]
        [HttpPost]
        public async Task<ActionResult<PostDTO>> PostPost(PostDTO data) {
            var post = await _context.Posts.FindAsync(data.Id);

            if (post != null) {
                var err = new ValidationErrors().Add("Post already in use", nameof(post.Title));
                return BadRequest(err);
            }
            
            if(data.Title == null)
                var parentId = GetParentId();

            var newPost = new Post() {
                Id = GetNewId(),
                Title = data.Title,
                Body = data.Body,
                Timestamp = data.Timestamp,
                ParentId = parentId,
                AuthorId = data.AuthorId,
                AcceptedAnswerId = data.AcceptedAnswerId,
                Tags = data.Tags,
            };
            _context.Posts.Add(newPost);
            var res = await _context.SaveChangesAsyncWithValidation();
            if (!res.IsEmpty)
                return BadRequest(res);

            return CreatedAtAction(nameof(GetPostById), new { Id = newPost.Id }, newPost.ToDTO());
        }

        private IQueryable<Post> GetParentId() {
            this.GetQuestions()
                .OrderByDescending(p => p.Id)
                .FirstOrDefault();

        }

        private int GetNewId() {
            return (from p in _context.Posts
                    select p.Id).Max() + 1;
        }

        [Authorized(Role.Admin, Role.Member)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost(int id, PostDTO dto) {
            if (id != dto.Id)
                return BadRequest();

            var post = await _context.Posts.FindAsync(id);

            if (post == null)
                return NotFound();

            post.Id = dto.Id;
            post.Title = dto.Title;
            post.Body = dto.Body;
<<<<<<< HEAD
            // post.Timestamp = dto.Timestamp;
=======
            //post.Timestamp = dto.Timestamp;
>>>>>>> 157a877127ad7a6b6911707f4b01db1f0743e6f5

            var res = await _context.SaveChangesAsyncWithValidation();

            if (!res.IsEmpty)
                return BadRequest(res);

            return NoContent();
        }

        [Authorized(Role.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id) {
            var post = await _context.Posts.FindAsync(id);

            if (post == null)
                return NotFound();

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
