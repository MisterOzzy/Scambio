using System;
using System.Linq;
using Scambio.DataAccess.Repositories;
using Scambio.Domain.Models;

namespace Scambio.DataAccess.EntityFramework.Repositories
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        public PostRepository(ScambioContext dbContext) : base(dbContext)
        {
            
        }

    }
}