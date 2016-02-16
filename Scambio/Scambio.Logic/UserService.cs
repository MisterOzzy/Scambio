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
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
                Secret = Guid.NewGuid().ToString().Substring(0,8)
            };

            _unitOfWork.PictureRepository.Add(picture);
            _unitOfWork.Save();

            var filename = GeneratePictureFilename(picture.Id, picture.Secret) + "." + pictureExtension;
            var pathToFile = Path.Combine(pathToStorage, authorId.ToString());
            CreatePicture(filename, pathToFile, inputStream);

            AddPost(authorId, wallOwnerId, bodyPost, picture);
        }

        private void CreatePicture(string filename, string path, Stream inputStream)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var fullPath = Path.Combine(path, filename);

            FileStream fileStream = new FileStream(fullPath, FileMode.OpenOrCreate);
            inputStream.CopyTo(fileStream);
            fileStream.Close();

        }

        private string GeneratePictureFilename(Guid pictureId, string pictureSecret)
        {
            return $"{pictureId}_{pictureSecret}";
        }

    }
}
