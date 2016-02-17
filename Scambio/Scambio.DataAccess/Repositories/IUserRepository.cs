using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Scambio.DataAccess.Infrastructure;
using Scambio.Domain.Models;

namespace Scambio.DataAccess.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        User FindByUserName(string username);
        Task<User> FindByUserNameAsync(string username);
        Task<User> FindByUserNameAsync(CancellationToken cancellationToken, string username);
        IEnumerable<Post> GetPosts(string username);
    }
}
