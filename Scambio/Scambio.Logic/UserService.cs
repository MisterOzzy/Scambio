using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scambio.DataAccess.Infrastructure;
using Scambio.Domain.Models;
using Scambio.Logic.Interfaces;

namespace Scambio.Logic
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPictureService _pictureService;
        public UserService(IUnitOfWork unitOfWork, IPictureService pictureService)
        {
            _unitOfWork = unitOfWork;
            _pictureService = pictureService;
        }
        public UserInfo GetUser(string id)
        {
            return GetUser(Guid.Parse(id));
        }

        public UserInfo GetUser(Guid id)
        {
            User user = _unitOfWork.UserRepository.GetById(id);
            var userInfo = new UserInfo()
            {
                Id = user.Id,
                Username = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Birthday = user.Birthday
            };

            return userInfo;
        }

        public void AddPost(Guid authorId, Guid wallOwnerId, string bodyPost, Picture picture = null)
        {
            var author = _unitOfWork.UserRepository.GetById(authorId);
            var post = new Post()
            {
                Author = author,
                Picture = picture,
                Body = bodyPost,
                DateCreated = DateTime.Now,
                Id = Guid.NewGuid()
            };

            _unitOfWork.PostRepository.Add(post);
            _unitOfWork.Save();
            
            var wallOwner = _unitOfWork.UserRepository.GetById(wallOwnerId);
            post.PostedUsers.Add(wallOwner);
            _unitOfWork.PostRepository.Update(post);
            _unitOfWork.Save();
        }

        
        public void AddPostWithPicture(Guid authorId, Guid wallOwnerId, string bodyPost, string pathToStorage, Stream inputStream, string pictureExtension)
        {
            var picture = new Picture()
            {
                Id = Guid.NewGuid(),
                Secret = Guid.NewGuid().ToString().Substring(0,8),
                Extension = pictureExtension
            };

            _unitOfWork.PictureRepository.Add(picture);
            _unitOfWork.Save();

            var filename = _pictureService.GeneratePictureFilename(picture);
            var pathToFile = Path.Combine(pathToStorage, authorId.ToString());

            _pictureService.CreatePicture(filename, pathToFile, inputStream);

            AddPost(authorId, wallOwnerId, bodyPost, picture);
        }

        public IEnumerable<Post> GetPostsByUsername(string username)
        {
            return _unitOfWork.UserRepository.GetPosts(username);
        }
    }
}
