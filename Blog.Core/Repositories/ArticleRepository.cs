using Blog.Abstractions.Repositories;
using Blog.Model;
using Blog.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Repositories
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ArticleRepository : IRepository<Article, string>
    {
        private readonly BlogDbContext _context;

        public ArticleRepository()
        {
            _context = new BlogDbContext();
        }

        public TResult Add<TResult>(Article entity) where TResult : class
        {
            try
            {
                _context.Articles.Add(entity);
                _context.SaveChanges();
                return true as TResult;
            }
            catch (Exception)
            {
                return false as TResult;
            }
        }

        public Task<TResult> AddAsync<TResult>(Article entity) where TResult : class
        {
            return Task.FromResult(Add<TResult>(entity));
        }

        public TResult Delete<TResult>(Article entity) where TResult : class
        {
            try
            {
                _context.Articles.Remove(entity);
                _context.SaveChanges();
                return true as TResult;
            }
            catch (Exception)
            {
                return false as TResult;
            }
        }

        public Task<TResult> DeleteAsync<TResult>(Article entity) where TResult : class
        {
            return Task.FromResult(Delete<TResult>(entity));
        }

        public Article Get(string key)
        {
            return _context.Articles.FirstOrDefault(a => a.Id == key);
        }

        public IEnumerable<Article> GetAll()
        {
            return _context.Articles;
        }

        public Task<IEnumerable<Article>> GetAllAsync()
        {
            return Task.FromResult(GetAll());
        }

        public Task<Article> GetAsync(string key)
        {
            return Task.FromResult(_context.Articles.FirstOrDefault(a => a.Id == key));
        }

        public Article GetByName(string name)
        {
            return _context.Articles.FirstOrDefault(a => a.Title == name);
        }

        public Task<Article> GetByNameAsync(string name)
        {
            return Task.FromResult(GetByName(name));
        }

        public TResult Update<TResult>(Article entity) where TResult : class
        {
            try
            {
                _context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();
                return true as TResult;
            }
            catch (Exception)
            {
                return true as TResult;
            }
        }

        public Task<TResult> UpdateAsync<TResult>(Article entity) where TResult : class
        {
            return Task.FromResult(Update<TResult>(entity));
        }
    }
}
