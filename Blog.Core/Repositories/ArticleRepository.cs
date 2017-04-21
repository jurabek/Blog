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
    public class ArticleRepository : IRepository<Article, string, bool>
    {
        private readonly BlogDbContext _context;

        public ArticleRepository()
        {
            _context = new BlogDbContext();
        }

        public bool Add(Article entity)
        {
            try
            {
                _context.Articles.Add(entity);
                _context.SaveChanges();
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public Task<bool> AddAsync(Article entity)
        {
            return Task.FromResult(Add(entity));
        }

        public bool Delete(Article entity)
        {
            try
            {
                _context.Articles.Remove(entity);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Task<bool> DeleteAsync(Article entity)
        {
            return Task.FromResult(Delete(entity));
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
              
        public bool Update(Article entity)
        {
            try
            {
                _context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return true;
            }
        }

        public Task<bool> UpdateAsync(Article entity)
        {
            return Task.FromResult(Update(entity));
        }

        public void SaveChanges()
        {
            try
            {
                _context.SaveChanges();

            }
            catch (Exception ex)
            {

            }
        }
    }
}
