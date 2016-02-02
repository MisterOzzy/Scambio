using Scambio.DataAccess.Repositories;
using Scambio.Domain.Models;

namespace Scambio.DataAccess.EntityFramework.Repositories
{
    public class PictureRepository : Repository<Picture>, IPictureRepository
    {
        public PictureRepository(ScambioContext dbContext) : base(dbContext)
        {
        }
    }
}