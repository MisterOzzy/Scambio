using System;
using System.Collections;
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

        public IEnumerable<Post> GetPosts(string id)
        {
            var user = GetById(new Guid(id));
            return user.PostsOnWall.ToList();
        }

        public IEnumerable<User> FindUsers(string query)
        {
            var queryArr = query.Split(' ');
            string query0 = string.Empty;

            if (query.Length >= 1)
                query0 = queryArr[0];

            IEnumerable<User> users = null;

            switch (queryArr.Length)
            {
                case 1:
                    
                    users =
                        DbContext.Users.Where(u => u.FirstName == query0 || u.LastName == query0);
                    break;
                case 2:

                    var query1 = queryArr[1];
                    users =
                        DbContext.Users.Where(
                            u =>
                                u.FirstName == query0 || u.FirstName == query1 ||
                                u.LastName == query0 || u.LastName == query1);
                    break;
                default:
                    users = new List<User>();
                    break;
            }

            return users.ToList();
        }
    }
}