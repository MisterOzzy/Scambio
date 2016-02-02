using System;

namespace Scambio.Domain.Models
{
    public class Like
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public Guid? PostId { get; set; }
        public User User { get; set; }
        public Post Post { get; set; }
    }
}