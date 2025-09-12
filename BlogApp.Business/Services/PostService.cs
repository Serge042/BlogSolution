using BlogApp.Data.Entities;
using BlogApp.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogApp.Business.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IPostTagRepository _postTagRepository;

        public PostService(
            IPostRepository postRepository,
            IUserRepository userRepository,
            ITagRepository tagRepository,
            IPostTagRepository postTagRepository)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
            _tagRepository = tagRepository;
            _postTagRepository = postTagRepository;
        }

        public async Task<Post> CreatePostAsync(Post post)
        {
            // Проверяем, существует ли автор
            if (await _userRepository.GetByIdAsync(post.UserId) == null)
            {
                throw new ArgumentException("Автор не найден");
            }

            post.CreatedAt = DateTime.UtcNow;
            post.UpdatedAt = DateTime.UtcNow;

            await _postRepository.AddAsync(post);
            await _postRepository.SaveChangesAsync();

            // Обрабатываем теги, если они предоставлены
            if (!string.IsNullOrEmpty(post.TagsInput))
            {
                await ProcessPostTagsAsync(post);
            }

            return post;
        }

        public async Task DeletePostAsync(int id)
        {
            var post = await _postRepository.GetByIdAsync(id);
            if (post == null)
            {
                throw new ArgumentException("Статья не найдена");
            }

            // Удаляем связанные теги
            await _postTagRepository.RemovePostTagsByPostIdAsync(id);

            _postRepository.Remove(post);
            await _postRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Post>> GetAllPostsAsync()
        {
            return await _postRepository.GetPostsWithUsersAsync();
        }

        public async Task<Post> GetPostByIdAsync(int id)
        {
            var post = await _postRepository.GetPostWithUserAndCommentsAsync(id);
            if (post == null)
            {
                throw new ArgumentException("Статья не найдена");
            }

            return post;
        }

        public async Task<IEnumerable<Post>> GetPostsByAuthorAsync(int authorId)
        {
            // Проверяем, существует ли автор
            if (await _userRepository.GetByIdAsync(authorId) == null)
            {
                throw new ArgumentException("Автор не найден");
            }

            return await _postRepository.GetPostsByAuthorAsync(authorId);
        }

        public async Task<IEnumerable<Post>> GetPostsWithTagsAsync()
        {
            return await _postRepository.GetPostsWithTagsAsync();
        }

        public async Task<Post> GetPostWithCommentsAsync(int id)
        {
            var post = await _postRepository.GetPostWithCommentsAsync(id);
            if (post == null)
            {
                throw new ArgumentException("Статья не найдена");
            }

            return post;
        }

        public async Task UpdatePostAsync(Post post)
        {
            var existingPost = await _postRepository.GetByIdAsync(post.Id);
            if (existingPost == null)
            {
                throw new ArgumentException("Статья не найдена");
            }

            // Проверяем, существует ли автор (если userId изменился)
            if (existingPost.UserId != post.UserId && await _userRepository.GetByIdAsync(post.UserId) == null)
            {
                throw new ArgumentException("Автор не найден");
            }

            // Обновляем поля
            existingPost.Title = post.Title;
            existingPost.Body = post.Body;
            existingPost.UserId = post.UserId;
            existingPost.UpdatedAt = DateTime.UtcNow;

            // Обрабатываем теги, если они предоставлены
            if (!string.IsNullOrEmpty(post.TagsInput))
            {
                // Удаляем старые теги
                await _postTagRepository.RemovePostTagsByPostIdAsync(post.Id);

                // Добавляем новые теги
                await ProcessPostTagsAsync(post);
            }

            _postRepository.Update(existingPost);
            await _postRepository.SaveChangesAsync();
        }

        public async Task<bool> PostExistsAsync(int id)
        {
            return await _postRepository.GetByIdAsync(id) != null;
        }

        public async Task<IEnumerable<Post>> SearchPostsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                throw new ArgumentException("Поисковый запрос не может быть пустым");
            }

            return await _postRepository.SearchPostsAsync(searchTerm);
        }

        private async Task ProcessPostTagsAsync(Post post)
        {
            var tagNames = post?.TagsInput?.Split(',', StringSplitOptions.RemoveEmptyEntries);

            foreach (var tagName in tagNames)
            {
                var trimmedName = tagName.Trim();

                // Ищем или создаем тег
                var tag = await _tagRepository.GetTagByNameAsync(trimmedName);
                if (tag == null)
                {
                    tag = new Tag { Name = trimmedName };
                    await _tagRepository.AddAsync(tag);
                    await _tagRepository.SaveChangesAsync();
                }

                // Создаем связь между постом и тегом
                var postTag = new PostTag
                {
                    PostId = post.Id,
                    TagId = tag.Id
                };

                await _postTagRepository.AddAsync(postTag);
            }

            await _postTagRepository.SaveChangesAsync();
        }

        public Task<IEnumerable<Post>> GetPostsByTagAsync(string tagName)
        {
            throw new NotImplementedException();
        }
    }
}