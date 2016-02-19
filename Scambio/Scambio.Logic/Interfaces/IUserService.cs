using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scambio.Domain.Models;

namespace Scambio.Logic.Interfaces
{
    public interface IUserService
    {
        UserInfo GetUser(string username, string pictureStorage = null);
        UserInfo GetUser(Guid id, string pictureStorage = null);
        void AddPost(Guid authorId, Guid wallOwnerId, string bodyPost, Picture picture = null);
        void AddPostWithPicture(Guid authorId, Guid wallOwnerId, string bodyPost, string pathToStorage, Stream inputStream, string pictureExtension);
        IEnumerable<Post> GetPostsByUserId(string userId);
        void ChangeAvatar(Picture picture, Guid userId);
        IEnumerable<UserInfo> FindUsers(string query, string pictureStorage);
        UserInfo GetUserInfo(User user, string pictureStorage);
    }
}
