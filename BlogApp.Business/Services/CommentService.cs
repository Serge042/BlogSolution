using BlogApp.Data.Entities;
using BlogApp.Data.Interfaces;

namespace BlogApp.Business.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;

        public CommentService(
            ICommentRepository commentRepository,
            IUserRepository userRepository,
            IPostRepository postRepository)
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _postRepository = postRepository;
        }

        public async Task<Comment> CreateCommentAsync(Comment comment)
        {
            // Проверяем, существует ли пользователь
            if (await _userRepository.GetByIdAsync(comment.UserId) == null)
            {
                throw new ArgumentException("Пользователь не найден");
            }

            // Проверяем, существует ли пост
            if (await _postRepository.GetByIdAsync(comment.PostId) == null)
            {
                throw new ArgumentException("Статья не найдена");
            }

            comment.CreatedAt = DateTime.UtcNow;

            await _commentRepository.AddAsync(comment);
            await _commentRepository.SaveChangesAsync();
            return comment;
        }

        public async Task DeleteCommentAsync(int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null)
            {
                throw new ArgumentException("Комментарий не найден");
            }

            _commentRepository.Remove(comment);
            await _commentRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Comment>> GetAllCommentsAsync()
        {
            return await _commentRepository.GetAllAsync();
        }

        public async Task<Comment> GetCommentByIdAsync(int id)
        {
            var comment = await _commentRepository.GetCommentWithUserAsync(id);
            if (comment == null)
            {
                throw new ArgumentException("Комментарий не найден");
            }

            return comment;
        }

        public async Task<IEnumerable<Comment>> GetCommentsByPostAsync(int postId)
        {
            // Проверяем, существует ли пост
            if (await _postRepository.GetByIdAsync(postId) == null)
            {
                throw new ArgumentException("Статья не найдена");
            }

            return await _commentRepository.GetCommentsByPostAsync(postId);
        }

        public async Task<IEnumerable<Comment>> GetCommentsByUserAsync(int userId)
        {
            // Проверяем, существует ли пользователь
            if (await _userRepository.GetByIdAsync(userId) == null)
            {
                throw new ArgumentException("Пользователь не найден");
            }

            return await _commentRepository.GetCommentsByUserAsync(userId);
        }

        public async Task UpdateCommentAsync(Comment comment)
        {
            var existingComment = await _commentRepository.GetByIdAsync(comment.Id);
            if (existingComment == null)
            {
                throw new ArgumentException("Комментарий не найден");
            }

            // Проверяем, существует ли пользователь (если userId изменился)
            if (existingComment.UserId != comment.UserId && await _userRepository.GetByIdAsync(comment.UserId)== null)
            {
                throw new ArgumentException("Пользователь не найден");
            }

            // Проверяем, существует ли пост (если postId изменился)
            if (existingComment.PostId != comment.PostId && await _postRepository.GetByIdAsync(comment.PostId) == null)
            {
                throw new ArgumentException("Статья не найдена");
            }

            existingComment.Body = comment.Body;
            existingComment.UserId = comment.UserId;
            existingComment.PostId = comment.PostId;

            _commentRepository.Update(existingComment);
            await _commentRepository.SaveChangesAsync();
        }

        public async Task<bool> CommentExistsAsync(int id)
        {
            return await _commentRepository.GetByIdAsync(id) != null;
        }
    }
}