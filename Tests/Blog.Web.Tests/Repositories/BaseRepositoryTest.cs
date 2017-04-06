using Blog.Abstractions.Repositories;
using Microsoft.AspNet.Identity;
using Moq;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Web.Tests.Repositories
{
    public abstract class BaseRepositoryTest<TRepository, T, TKey> 
        where T : class, new()
        where TRepository : IRepository<T, TKey>
    {
        /// <summary>
        /// Initialize int on classes and setup mockups
        /// </summary>
        internal protected abstract IRepository<T, TKey> Repository { get; set; }

        [OneTimeSetUp]
        public abstract void Init();

        [Test]
        public virtual async Task GetAllAsyncShouldReturnSomeResult()
        {
            var result = await Repository.GetAllAsync();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }

        [Test]
        public virtual async Task AddAsyncShouldSuccess()
        {
            var result = await Repository.AddAsync<IdentityResult>(default(T));
            Assert.IsTrue(result.Succeeded);
        }

        [Test]
        public virtual async Task UpdateAsyncShouldSuccess()
        {
            var result = await Repository.UpdateAsync<IdentityResult>(default(T));
            Assert.IsTrue(result.Succeeded);
        }

        [Test]
        public virtual async Task DeleteAsyncShouldSuccess()
        {
            var result = await Repository.DeleteAsync<IdentityResult>(default(T));
            Assert.IsTrue(result.Succeeded);
        }

        [Test]
        public virtual async Task GetAsyncShouldReturnResult()
        {
            var result = await Repository.GetAsync(default(TKey));
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<T>(result);
        }

        [Test]
        public virtual async Task GetByNameAsyncShouldReturnResult()
        {
            var result = await Repository.GetByNameAsync(It.IsAny<string>());
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<T>(result);
        }

        [Test]
        public virtual void AddShouldSuccess()
        {
            var result = Repository.Add<IdentityResult>(default(T));
            Assert.IsTrue(result.Succeeded);
        }

        [Test]
        public virtual void UpdateShouldSuccess()
        {
            var result = Repository.Update<IdentityResult>(default(T));
            Assert.IsTrue(result.Succeeded);
        }

        [Test]
        public virtual void DeleteShouldSuccess()
        {
            var result = Repository.Delete<IdentityResult>(default(T));
            Assert.IsTrue(result.Succeeded);
        }

        [Test]
        public virtual void GetAllShouldReturnSomeResult()
        {
            var result = Repository.GetAll();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }

        [Test]
        public virtual void GetShouldReturnResult()
        {
            var result = Repository.Get(default(TKey));
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<T>(result);
        }

        [Test]
        public virtual void GetByNameShouldReturnResult()
        {
            var result = Repository.GetByName(It.IsAny<string>());
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<T>(result);
        }
    }
}
