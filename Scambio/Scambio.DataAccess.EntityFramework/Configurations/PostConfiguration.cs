using System.Data.Entity.ModelConfiguration;
using Scambio.Domain.Models;

namespace Scambio.DataAccess.EntityFramework.Configurations
{
    public class PostConfiguration : EntityTypeConfiguration<Post>
    {
        public PostConfiguration()
        {
            Property(p => p.DateCreated).IsRequired().HasColumnType("datetime");
            Property(p => p.Body).IsOptional().HasColumnType("nvarchar");
            Property(p => p.PictureId).IsOptional();
            HasMany(p => p.PostedUsers).WithMany(u => u.PostsOnWall).Map(m =>
            {
                m.ToTable("Wall");
                m.MapLeftKey("PostId");
                m.MapRightKey("UserId");
            });
            HasRequired(p => p.Author).WithMany(u => u.OwnPosts).HasForeignKey(p => p.AuthorId).WillCascadeOnDelete(false);
            HasOptional(p => p.Picture).WithOptionalDependent();
        }
    }
}