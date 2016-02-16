using System;
using System.Collections.Generic;
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

        public void AddPost(Guid authorId, Guid wallOwnerId, string bodyPost)
        {
            var author = _unitOfWork.UserRepository.GetById(authorId);
            var post = new Post()
            {
                Author = author,
                //AuthorId = authorId,
                Body = bodyPost,
                DateCreated = DateTime.Now,
                Id = Guid.NewGuid()
            };
            //author.OwnPosts.Add(post);
            //_unitOfWork.UserRepository.Update(author);
            //_unitOfWork.Save();
            
            _unitOfWork.PostRepository.Add(post);
            _unitOfWork.Save();
            var ajisdisa = author.OwnPosts.ToList();
            var wallOwner = _unitOfWork.UserRepository.GetById(wallOwnerId);
            //wallOwner.PostsOnWall.Add(post);
            post.PostedUsers.Add(wallOwner);
            _unitOfWork.PostRepository.Update(post);
            //_unitOfWork.UserRepository.Update(wallOwner);
            _unitOfWork.Save();
        }

        
        public void AddPostWithPicture(Guid authorId, Guid wallOwnerId, string bodyPost, string pathToStorage)
        {
            
        }
    }
}
