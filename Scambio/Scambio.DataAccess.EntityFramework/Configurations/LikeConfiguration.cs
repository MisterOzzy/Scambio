using System.Data.Entity.ModelConfiguration;
using Scambio.Domain.Models;

namespace Scambio.DataAccess.EntityFramework.Configurations
{
    public class LikeConfiguration : EntityTypeConfiguration<Like>
    {
        public LikeConfiguration()
        {
            HasRequired(l => l.Post).WithMany(p => p.Likes).HasForeignKey(l => l.PostId);
            HasRequired(l => l.User).WithMany(u => u.Likes).HasForeignKey(l => l.UserId);
        }
    }
}