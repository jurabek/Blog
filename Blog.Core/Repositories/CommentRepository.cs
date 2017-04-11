using Blog.Abstractions.Repositories;
using Blog.Model;
using Blog.Model.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Core.Repositories
{
    public class CommentRepository : IRepository<Comment, string, bool>
    {
        private readonly BlogDbContext _context;

        public CommentRepository()
        {
            _context = new BlogDbContext();
        }

        public bool Add(Comment entity)
        {
            try
            {
                _context.Comments.Add(entity);
                return _context.SaveChanges() == 1 ? true : false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> AddAsync(Comment entity)
        {
            _context.Comments.Add(entity);
            return await _context.SaveChangesAsync() == 1 ? true : false;
        }

        public bool Delete(Comment entity)
        {
            _context.Comments.Remove(entity);
            return _context.SaveChanges() == 1 ? true : false;
        }

        public async Task<bool> DeleteAsync(Comment entity)
        {
            _context.Comments.Remove(entity);
            return await _context.SaveChangesAsync() == 1 ? true : false;
        }

        public Comment Get(string key)
        {
            return _context.Comments.Find(key);
        }

        public IEnumerable<Comment> GetAll()
        {
            return _context.Comments;
        }

        public Task<IEnumerable<Comment>> GetAllAsync()
        {
            return Task.FromResult(GetAll());
        }

        public Task<Comment> GetAsync(string key)
        {
            return Task.FromResult(_context.Comments.Find(key));
        }
        
        public bool Update(Comment entity)
        {
            _context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            return _context.SaveChanges() == 1 ? true : false;
        }

        public async Task<bool> UpdateAsync(Comment entity)
        {
            _context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            return await _context.SaveChangesAsync() == 1 ? true : false;
        }


        public Comment GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<Comment> GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }
    }
}
