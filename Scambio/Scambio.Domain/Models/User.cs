using System;
using System.Collections.Generic;

namespace Scambio.Domain.Models
{
    public class User
    {
        #region Fields

        private ICollection<Claim> _claims;
        private ICollection<ExternalLogin> _externalLogins;
        private ICollection<Post> _postsOnWall;
        private ICollection<Like> _likes;

        #endregion

        #region Scalar Properties

        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime? Birthday { get; set; }
        public Guid? AvatarId { get; set; }
        public Picture Avatar { get; set; }
        public virtual string PasswordHash { get; set; }
        public virtual string SecurityStamp { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ICollection<Post> PostsOnWall
        {
            get { return _postsOnWall ?? (_postsOnWall = new List<Post>()); }
            set { _postsOnWall = value; }
        }

        public virtual ICollection<Like> Likes
        {
            get { return _likes ?? (_likes = new List<Like>()); }
            set { _likes = value; }
        }

        public virtual ICollection<Claim> Claims
        {
            get { return _claims ?? (_claims = new List<Claim>()); }
            set { _claims = value; }
        }

        public virtual ICollection<ExternalLogin> Logins
        {
            get
            {
                return _externalLogins ??
                       (_externalLogins = new List<ExternalLogin>());
            }
            set { _externalLogins = value; }
        }

        #endregion
    }
}