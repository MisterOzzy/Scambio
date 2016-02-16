using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Scambio.DataAccess.Infrastructure;
using Scambio.Logic;
using Scambio.Logic.Interfaces;
using Scambio.Web.Identity;

namespace Scambio.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        public HomeController(IUnitOfWork unitOfWork)
        {
            _userService = new UserService(unitOfWork);
        }

        // GET: Home
        public ActionResult Index()
        {
            //return RedirectToRoute("UserHome", HttpContext.User.Identity.GetUserName());
            return RedirectToAction("UserPage", new { username = HttpContext.User.Identity.GetUserName()});
            //if (HttpContext.User.Identity.IsAuthenticated)
            //{
            //    return null;
            //}
            //else
            //{
            //    return RedirectToAction("Login", "Account");

            //}
            //return HttpContext.User.Identity.GetUserId();

            //return RedirectToAction("Login", "Account");
        }

        //[Route("{username}")]
        public ActionResult UserPage(string username)
        {
            var userId = HttpContext.User.Identity.GetUserId();
            UserInfo userInfo = _userService.GetUser(userId);
            ViewBag.User = userInfo;
            return View();
        }

        [HttpPost]
        public ActionResult AddPost(string bodyPost, HttpPostedFileBase picturePost)
        {
            
            if (picturePost != null)
            {
                var pictureFolderStorage = ConfigurationManager.AppSettings["pictureStorage"];
                var pathToStorage = HostingEnvironment.MapPath($"~/{pictureFolderStorage}");

                var tmp = picturePost.FileName.Split('.');
                var extension = tmp[tmp.Length - 1];

                _userService.AddPostWithPicture(new Guid(HttpContext.User.Identity.GetUserId()),
                    new Guid(HttpContext.User.Identity.GetUserId()), bodyPost, pathToStorage,
                    picturePost.InputStream, extension);
            }
                
            else
                _userService.AddPost(new Guid(HttpContext.User.Identity.GetUserId()) , new Guid(HttpContext.User.Identity.GetUserId()), bodyPost);

            return RedirectToAction("UserPage", new {username = HttpContext.User.Identity.GetUserName()});
        }
    }
}