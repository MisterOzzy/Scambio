using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scambio.Logic.Interfaces
{
    public interface IUserService
    {
        UserInfo GetUser(string id);
        UserInfo GetUser(Guid id);
        void AddPost(Guid authorId, Guid wallOwnerId, string bodyPost);
        void AddPostWithPicture(Guid authorId, Guid wallOwnerId, string bodyPost, string pathToStorage);

    }
}
