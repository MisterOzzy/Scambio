using System.Data.Entity.ModelConfiguration;
using Scambio.Domain.Models;

namespace Scambio.DataAccess.EntityFramework.Configurations
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            HasOptional(u => u.Avatar).WithOptionalDependent();

            Property(u => u.Birthday).HasColumnType("date").IsOptional();

            Property(u => u.PasswordHash)
                .HasColumnName("PasswordHash")
                .HasColumnType("nvarchar")
                .IsMaxLength()
                .IsOptional();

            Property(u => u.SecurityStamp)
                .HasColumnName("SecurityStamp")
                .HasColumnType("nvarchar")
                .IsMaxLength()
                .IsOptional();

            Property(u => u.UserName)
                .HasColumnName("UserName")
                .HasColumnType("nvarchar")
                .HasMaxLength(256)
                .IsRequired();

            HasMany(u => u.Claims)
                .WithRequired(c => c.User)
                .HasForeignKey(c => c.UserId);

            HasMany(u => u.Logins)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.UserId);

            Property(u => u.Email).IsRequired().HasColumnType("nvarchar");
        }
    }
}