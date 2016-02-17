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
using Scambio.Web.ViewModels;

namespace Scambio.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        private readonly IPictureService _pictureService;
        public HomeController(IUnitOfWork unitOfWork)
        {
            _pictureService = new PictureService(unitOfWork);
            _userService = new UserService(unitOfWork, _pictureService);
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
        public ActionResult AddPost()
        {
            HttpPostedFileBase picturePost = null;
            if (Request.Files.Count != 0)
                picturePost = Request.Files[0];

            var bodyPost = Request.Form["bodyPost"];

            if(picturePost == null && string.IsNullOrEmpty(bodyPost))
                return new EmptyResult();

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

            ////if (!string.IsNullOrEmpty(bodyPost) && picturePost == null)
                _userService.AddPost(new Guid(HttpContext.User.Identity.GetUserId()) , new Guid(HttpContext.User.Identity.GetUserId()), bodyPost);


            var postsForView = GetPostsByUsername(HttpContext.User.Identity.GetUserName());
            
            return PartialView("_PostsOnWall", postsForView);
        }

        public ActionResult PostsOnWall(string username)
        {
            return PartialView("_PostsOnWall", GetPostsByUsername(username));
        }

        private List<PostWallViewModel> GetPostsByUsername(string username)
        {
            var posts = _userService.GetPostsByUsername(HttpContext.User.Identity.GetUserName());
            var postsForView = new List<PostWallViewModel>();
            foreach (var post in posts)
            {
                var postView = new PostWallViewModel()
                {
                    BodyPost = post.Body,
                    DateCreated = post.DateCreated,
                    FirstNameAuthor = post.Author.FirstName,
                    LastNameAuthor = post.Author.LastName,
                    LikeCount = post.Likes.Count
                };

                if (post.Picture != null)
                {
                    var locationPicture = "/" + _pictureService.GetPictureLocation(ConfigurationManager.AppSettings["pictureStorage"],
                            post.AuthorId.Value, post.Picture);
                    postView.PictureLocation = locationPicture.Replace(@"\", @"/");
                }



                postsForView.Add(postView);

            }

            return postsForView;

        } 
    }
}