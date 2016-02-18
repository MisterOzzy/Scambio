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
        UserInfo GetUser(string id);
        UserInfo GetUser(Guid id);
        void AddPost(Guid authorId, Guid wallOwnerId, string bodyPost, Picture picture = null);
        void AddPostWithPicture(Guid authorId, Guid wallOwnerId, string bodyPost, string pathToStorage, Stream inputStream, string pictureExtension);
        IEnumerable<Post> GetPostsByUsername(string username);
        void ChangeAvatar(Picture picture, Guid userId);
    }
}
