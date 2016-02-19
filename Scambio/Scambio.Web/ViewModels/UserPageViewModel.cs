using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scambio.Logic;

namespace Scambio.Web.ViewModels
{
    public class UserPageViewModel
    {
        public UserInfo LogginedUser { get; set; }
        public UserInfo CurrentUser { get; set; }
    }
}
