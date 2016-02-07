using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Scambio.DataAccess.Repositories;

namespace Scambio.DataAccess.Infrastructure
{
    public interface IUnitOfWork
    {
        IExternalLoginRepository ExternalLoginRepository { get; }
        ILikeRepository LikeRepository { get; }
        IPictureRepository PictureRepository { get; }
        IPostRepository PostRepository { get; }
        IUserRepository UserRepository { get; }
        void Save();
        Task SaveAsync();
    }
}