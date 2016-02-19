using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scambio.Domain.Models;

namespace Scambio.Web.ViewModels
{
    public class PostWallViewModel
    {
        public Guid PostId { get; set; }
        public Guid AuthorId { get; set; }
        public string FirstNameAuthor { get; set; }
        public string LastNameAuthor { get; set; }
        public DateTime DateCreated { get; set; }
        public int LikeCount { get; set; }
        public string BodyPost { get; set; }
        public string PictureLocation { get; set; }
        public string AuthorAvatar { get; set; }
        public string AuthorUsername { get; set; }
    }
}
