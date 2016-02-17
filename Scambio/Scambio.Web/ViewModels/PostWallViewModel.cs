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
        public string FirstNameAuthor { get; set; }
        public string LastNameAuthor { get; set; }
        public DateTime DateCreated { get; set; }
        public int LikeCount { get; set; }
        public string BodyPost { get; set; }
        public string PictureLocation { get; set; }
    }
}
