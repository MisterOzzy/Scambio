using System;
using System.Linq;
using Scambio.DataAccess.Repositories;
using Scambio.Domain.Models;

namespace Scambio.DataAccess.EntityFramework.Repositories
{
    public class LikeRepository : Repository<Like>, ILikeRepository
    {
        public LikeRepository(ScambioContext dbContext) : base(dbContext)
        {

        }


        public int LikeCount(Guid postId)
        {
            return DbContext.Likes.Count(p => p.PostId.Value == postId);
        }

        public Like ContainLikeFromUser(Guid userId, Guid postId)
        {
            return DbContext.Likes.FirstOrDefault(l => l.PostId.Value == postId && l.UserId.Value == userId);
        }
    }
}