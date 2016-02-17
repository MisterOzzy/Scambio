using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Scambio.DataAccess.Repositories;
using Scambio.Domain.Models;

namespace Scambio.DataAccess.EntityFramework.Repositories
{
    internal class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ScambioContext dbContext) : base(dbContext)
        {
        }

        public User FindByUserName(string username)
        {
            return DbSet.FirstOrDefault(x => x.UserName == username);
        }

        public Task<User> FindByUserNameAsync(string username)
        {
            return DbSet.FirstOrDefaultAsync(x => x.UserName == username);
        }

        public Task<User> FindByUserNameAsync(System.Threading.CancellationToken cancellationToken, string username)
        {
            return DbSet.FirstOrDefaultAsync(x => x.UserName == username, cancellationToken);
        }

        public IEnumerable<Post> GetPosts(string username)
        {
            var user = FindByUserName(username);
            return user.PostsOnWall.ToList();
        }
    }
}