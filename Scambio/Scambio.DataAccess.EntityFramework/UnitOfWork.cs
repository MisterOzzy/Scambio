using System;
using System.Threading.Tasks;
using Scambio.DataAccess.EntityFramework.Repositories;
using Scambio.DataAccess.Infrastructure;
using Scambio.DataAccess.Repositories;

namespace Scambio.DataAccess.EntityFramework
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private readonly ScambioContext _dbContext;
        private readonly IDbFactory<ScambioContext> _dbFactory;
        private IExternalLoginRepository _externalLoginRepository;
        private bool _isDisposed;
        private ILikeRepository _likeRepository;
        private IPictureRepository _pictureRepository;
        private IPostRepository _postRepository;
        private IUserRepository _userRepository;

        public UnitOfWork(IDbFactory<ScambioContext> dbFactory)
        {
            _dbFactory = dbFactory;
            _dbContext = _dbFactory.Initialize();
        }

        public IUserRepository UserRepository => _userRepository ?? (_userRepository = new UserRepository(_dbContext));
        public ILikeRepository LikeRepository => _likeRepository ?? (_likeRepository = new LikeRepository(_dbContext));
        public IPostRepository PostRepository => _postRepository ?? (_postRepository = new PostRepository(_dbContext));

        public IExternalLoginRepository ExternalLoginRepository
            => _externalLoginRepository ?? (_externalLoginRepository = new ExternalLoginRepository(_dbContext));

        public IPictureRepository PictureRepository
            => _pictureRepository ?? (_pictureRepository = new PictureRepository(_dbContext));

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public Task SaveAsync()
        {
            return _dbContext.SaveChangesAsync();
        }

        private void Dispose(bool disposing)
        {
            if (!_isDisposed && disposing)
                DisposeCore();

            _isDisposed = true;
        }

        private void DisposeCore()
        {
            _userRepository = null;
            _likeRepository = null;
            _pictureRepository = null;
            _postRepository = null;
            _externalLoginRepository = null;
            _dbContext.Dispose();
        }
    }
}