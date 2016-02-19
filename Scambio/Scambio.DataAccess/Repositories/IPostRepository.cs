using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scambio.DataAccess.Infrastructure;
using Scambio.Domain.Models;

namespace Scambio.DataAccess.Repositories
{
    public interface IPostRepository : IRepository<Post>
    {
        IEnumerable<Like> GetLikes(Guid postId);

    }
}
