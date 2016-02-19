using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scambio.Logic.Interfaces
{
    public interface IPostService
    {
        int GetLikeCount(Guid postId);
        void LikePost(Guid userId, Guid postId);
        void DeletePost(Guid postId);
        IEnumerable<UserInfo> GetLikedUsers(IUserService userService,string postId, string pictureStorage);
    }
}
