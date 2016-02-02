using System.Data.Entity.ModelConfiguration;
using Scambio.Domain.Models;

namespace Scambio.DataAccess.EntityFramework.Configurations
{
    internal class ExternalLoginConfiguration : EntityTypeConfiguration<ExternalLogin>
    {
        public ExternalLoginConfiguration()
        {
            Property(x => x.LoginProvider)
                .HasColumnType("nvarchar")
                .HasMaxLength(128)
                .IsRequired();

            Property(x => x.ProviderKey)
                .HasColumnType("nvarchar")
                .HasMaxLength(128)
                .IsRequired();

            Property(x => x.UserId)
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            HasRequired(x => x.User)
                .WithMany(x => x.Logins)
                .HasForeignKey(x => x.UserId);
        }
    }
}