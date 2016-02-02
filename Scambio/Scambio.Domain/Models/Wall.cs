using System;

namespace Scambio.Domain.Models
{
    public class Wall
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public Guid WallOwnerId { get; set; }
        public Post Post { get; set; }
        public User WallOwner { get; set; }
    }
}