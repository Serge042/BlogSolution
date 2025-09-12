using BlogApp.Data.Entities;
using BlogApp.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogApp.Business.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;
        private readonly IPostTagRepository _postTagRepository;
        private readonly IPostRepository _postRepository;

        public TagService(IPostRepository postRepository,ITagRepository tagRepository, IPostTagRepository postTagRepository)
        {
            _tagRepository = tagRepository;
            _postTagRepository = postTagRepository;
            _postRepository = postRepository;
        }

        public async Task<Tag> CreateTagAsync(Tag tag)
        {
            // Проверяем, существует ли тег с таким именем
            if (await _tagRepository.GetTagByNameAsync(tag.Name) != null)
            {
                throw new ArgumentException("Тег с таким именем уже существует");
            }

            await _tagRepository.AddAsync(tag);
            await _tagRepository.SaveChangesAsync();
            return tag;
        }

        public async Task DeleteTagAsync(int id)
        {
            var tag = await _tagRepository.GetByIdAsync(id);
            if (tag == null)
            {
                throw new ArgumentException("Тег не найден");
            }

            // Удаляем связи с постами
            await _postTagRepository.RemovePostTagsByTagIdAsync(id);

            _tagRepository.Remove(tag);
            await _tagRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Tag>> GetAllTagsAsync()
        {
            return await _tagRepository.GetAllAsync();
        }

        public async Task<Tag> GetTagByIdAsync(int id)
        {
            var tag = await _tagRepository.GetByIdAsync(id);
            if (tag == null)
            {
                throw new ArgumentException("Тег не найден");
            }

            return tag;
        }

        public async Task<Tag> GetTagByNameAsync(string name)
        {
            var tag = await _tagRepository.GetTagByNameAsync(name);
            if (tag == null)
            {
                throw new ArgumentException("Тег не найден");
            }

            return tag;
        }

        public async Task UpdateTagAsync(Tag tag)
        {
            var existingTag = await _tagRepository.GetByIdAsync(tag.Id);
            if (existingTag == null)
            {
                throw new ArgumentException("Тег не найден");
            }

            // Проверяем, не используется ли имя другим тегом
            var tagWithSameName = await _tagRepository.GetTagByNameAsync(tag.Name);
            if (tagWithSameName != null && tagWithSameName.Id != tag.Id)
            {
                throw new ArgumentException("Тег с таким именем уже существует");
            }

            existingTag.Name = tag.Name;

            _tagRepository.Update(existingTag);
            await _tagRepository.SaveChangesAsync();
        }

        public async Task<bool> TagExistsAsync(int id)
        {
            return await _tagRepository.GetByIdAsync(id) != null;
        }

        public async Task<IEnumerable<Post>> GetPostsByTagAsync(string tagName)
        {
            var tag = await _tagRepository.GetTagByNameAsync(tagName);
            if (tag == null)
            {
                throw new ArgumentException("Тег не найден");
            }

            return await _postRepository.GetPostsByTagAsync(tagName);
        }
    }
}