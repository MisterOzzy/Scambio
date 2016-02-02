using System;

namespace Scambio.Domain.Models
{
    public class Picture
    {
        public Guid Id { get; set; }
        public string ServerId { get; set; }
        public string Secret { get; set; }
    }
}