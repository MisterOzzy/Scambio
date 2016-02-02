using System.Data.Entity;
using Scambio.DataAccess.EntityFramework.Configurations;
using Scambio.Domain.Models;

namespace Scambio.DataAccess.EntityFramework
{
    public class ScambioContext : DbContext
    {
        public ScambioContext(string connectionName = "ScambioDb") : base(connectionName)
        {
        }

        public IDbSet<User> Users { get; set; }
        public IDbSet<ExternalLogin> Logins { get; set; }
        public IDbSet<Like> Likes { get; set; }
        public IDbSet<Picture> Pictures { get; set; }
        public IDbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ClaimConfiguration());
            modelBuilder.Configurations.Add(new ExternalLoginConfiguration());
            modelBuilder.Configurations.Add(new LikeConfiguration());
            modelBuilder.Configurations.Add(new PictureConfiguration());
            modelBuilder.Configurations.Add(new PostConfiguration());
            modelBuilder.Configurations.Add(new UserConfiguration());
        }
    }
}