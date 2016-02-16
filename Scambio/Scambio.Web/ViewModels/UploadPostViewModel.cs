using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Scambio.Web.ViewModels
{
    public class UploadPostViewModel
    {
        public string Body { get; set; }
        public HttpPostedFileBase Picture { get; set; }
    }
}
