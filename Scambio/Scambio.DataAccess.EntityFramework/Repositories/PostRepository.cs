using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public IEnumerable<Like> GetLikes(Guid postId)
        {
            return DbContext.Likes.Where(like => like.PostId.Value == postId).Include(like => like.User).ToList();
        }
    }
}