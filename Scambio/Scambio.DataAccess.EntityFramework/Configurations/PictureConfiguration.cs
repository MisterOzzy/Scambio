using System.Data.Entity.ModelConfiguration;
using Scambio.Domain.Models;

namespace Scambio.DataAccess.EntityFramework.Configurations
{
    public class PictureConfiguration : EntityTypeConfiguration<Picture>
    {
        public PictureConfiguration()
        {
            Property(p => p.Secret).IsRequired();
            Property(p => p.ServerId).IsOptional();
        }
    }
}