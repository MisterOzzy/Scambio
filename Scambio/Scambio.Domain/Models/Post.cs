using System;
using System.Collections.Generic;

namespace Scambio.Domain.Models
{
    public class Post
    {
        private ICollection<Like> _likes;
        private ICollection<User> _users;
        public Guid Id { get; set; }
        public Guid AuthorId { get; set; }
        public User Author { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid? PictureId { get; set; }
        public Picture Picture { get; set; }
        public string Body { get; set; }

        public virtual ICollection<Like> Likes
        {
            get { return _likes ?? (_likes = new List<Like>()); }
            set { _likes = value; }
        }

        public virtual ICollection<User> PostedUsers
        {
            get { return _users ?? (_users = new List<User>()); }
            set { _users = value; }
        }
    }
}