using System.Data.Entity.ModelConfiguration;
using Scambio.Domain.Models;

namespace Scambio.DataAccess.EntityFramework.Configurations
{
    public class ClaimConfiguration : EntityTypeConfiguration<Claim>
    {
        public ClaimConfiguration()
        {
            Property(x => x.UserId).IsRequired();

            Property(x => x.ClaimType)
                .HasColumnType("nvarchar")
                .IsMaxLength()
                .IsOptional();

            Property(x => x.ClaimValue)
                .HasColumnType("nvarchar")
                .IsMaxLength()
                .IsOptional();

            HasRequired(x => x.User)
                .WithMany(x => x.Claims)
                .HasForeignKey(x => x.UserId);
        }
    }
}