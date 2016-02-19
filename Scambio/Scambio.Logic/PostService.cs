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
    public class PostService : IPostService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PostService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public int GetLikeCount(Guid postId)
        {
            return _unitOfWork.LikeRepository.LikeCount(postId);
        }

        public void LikePost(Guid userId, Guid postId)
        {
            var like = _unitOfWork.LikeRepository.ContainLikeFromUser(userId, postId);

            if(like != null)
                _unitOfWork.LikeRepository.Delete(like);
            else
            {
                like = new Like()
                {
                    Id = Guid.NewGuid(),
                    PostId = postId,
                    UserId = userId
                };

                _unitOfWork.LikeRepository.Add(like);
            }

            _unitOfWork.Save();
        }

        public void DeletePost(Guid postId)
        {
            _unitOfWork.PostRepository.Delete(p => p.Id == postId);
            _unitOfWork.Save();
        }

        public IEnumerable<UserInfo> GetLikedUsers(IUserService userService,string postId, string pictureStorage)
        {
            var likes = _unitOfWork.PostRepository.GetLikes(new Guid(postId));
            
            var likedUserInfos = likes.Select(like => userService.GetUserInfo(like.User, pictureStorage)).ToList();

            return likedUserInfos;
        }


    }
}
