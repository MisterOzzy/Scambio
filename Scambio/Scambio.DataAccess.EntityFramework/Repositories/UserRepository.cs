using Scambio.DataAccess.Repositories;
using Scambio.Domain.Models;

namespace Scambio.DataAccess.EntityFramework.Repositories
{
    internal class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ScambioContext dbContext) : base(dbContext)
        {
        }
    }
}