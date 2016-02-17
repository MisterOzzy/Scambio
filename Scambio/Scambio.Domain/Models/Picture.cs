using System;

namespace Scambio.Domain.Models
{
    public class Picture
    {
        public Guid Id { get; set; }
        public string Extension { get; set; }
        public string Secret { get; set; }
    }
}