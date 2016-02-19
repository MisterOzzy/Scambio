using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Scambio.DataAccess.Infrastructure;
using Scambio.Domain.Models;
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
        private readonly IPostService _postService;
        public HomeController(IUnitOfWork unitOfWork)
        {
            _pictureService = new PictureService(unitOfWork);
            _userService = new UserService(unitOfWork, _pictureService);
            _postService = new PostService(unitOfWork);
        }

        // GET: Home
        public ActionResult Index()
        {
            return RedirectToRoute("UserHome", new { username = HttpContext.User.Identity.GetUserName()});
        }

        //[Route("{username}")]
        public ActionResult UserPage(string username)
        {
            var userPageViewModel = GetUserPageViewModel(username);

            return View(userPageViewModel);
        }


        private UserPageViewModel GetUserPageViewModel(string userName)
        {
            var logginedUserId = HttpContext.User.Identity.GetUserId();
            var logginedUsername = HttpContext.User.Identity.GetUserName();
            var pictureStorage = ConfigurationManager.AppSettings["pictureStorage"];

            UserInfo logginedUserInfo = _userService.GetUser(new Guid(logginedUserId), pictureStorage);
            logginedUserInfo.OriginalAvatarLocation = _pictureService.GetOriginalAvatar(logginedUserInfo.AvatarLocation);

            UserInfo currentUserInfo = null;

            if (userName != logginedUsername)
            {
                currentUserInfo = _userService.GetUser(userName, pictureStorage);
                currentUserInfo.OriginalAvatarLocation = _pictureService.GetOriginalAvatar(currentUserInfo.AvatarLocation);
            }         
            else
                currentUserInfo = logginedUserInfo;

            var userPageViewModel = new UserPageViewModel() {CurrentUser = currentUserInfo, LogginedUser = logginedUserInfo};
            return userPageViewModel;
        }

        [HttpPost]
        public ActionResult AddPost()
        {
            HttpPostedFileBase picturePost = null;
            if (Request.Files.Count != 0)
                picturePost = Request.Files[0];

            var bodyPost = Request.Form["bodyPost"];
            var wallId = Request.Form["wallId"];

            if(picturePost == null && string.IsNullOrEmpty(bodyPost))
                return new EmptyResult();

            if (picturePost != null)
            {
                var pictureFolderStorage = ConfigurationManager.AppSettings["pictureStorage"];
                var pathToStorage = HostingEnvironment.MapPath($"~/{pictureFolderStorage}");

                var tmp = picturePost.FileName.Split('.');
                var extension = tmp[tmp.Length - 1];

                _userService.AddPostWithPicture(new Guid(HttpContext.User.Identity.GetUserId()),
                    new Guid(wallId), bodyPost, pathToStorage,
                    picturePost.InputStream, extension);
            }

            else
                _userService.AddPost(new Guid(HttpContext.User.Identity.GetUserId()) , new Guid(wallId), bodyPost);


            var postsForView = GetPostsByUserId(wallId);
            
            return PartialView("_PostsOnWall", postsForView);
        }

        [HttpPost]
        public JsonResult DeletePost()
        {
            var authorId = Request.Form["authorId"];
            var postId = Request.Form["postId"];
            var wallId = Request.Form["wallId"];
            var userId = HttpContext.User.Identity.GetUserId();

            if (userId != authorId && wallId != userId)
                return Json("");

            _postService.DeletePost(new Guid(postId));

            return Json("Deleted");
        }

        [HttpPost]
        public JsonResult LikePost()
        {
            var userId = HttpContext.User.Identity.GetUserId();
            var postId = Request.Form["postId"];
            _postService.LikePost(new Guid(userId), new Guid(postId));
            return Json(_postService.GetLikeCount(new Guid(postId)));
        }

        public ActionResult PostsOnWall(string userId)
        {
            return PartialView("_PostsOnWall", GetPostsByUserId(userId));
        }

        private List<PostWallViewModel> GetPostsByUserId(string userId)
        {
            var posts = _userService.GetPostsByUserId(userId);
            var postsForView = new List<PostWallViewModel>();
            foreach (var post in posts)
            {
                var postView = new PostWallViewModel()
                {
                    PostId = post.Id,
                    BodyPost = post.Body,
                    DateCreated = post.DateCreated,
                    FirstNameAuthor = post.Author.FirstName,
                    LastNameAuthor = post.Author.LastName,
                    AuthorUsername = post.Author.UserName,
                    LikeCount = _postService.GetLikeCount(post.Id),
                    AuthorId = post.AuthorId.Value,
                    AuthorAvatar = GetUserAvatarLocation(post.Author)
                };



                if (post.Picture != null)
                {
                    var locationPicture = "/" + _pictureService.GetPictureLocation(ConfigurationManager.AppSettings["pictureStorage"],
                            post.AuthorId.Value, post.Picture);
                    postView.PictureLocation = locationPicture.Replace(@"\", @"/");
                }
                postsForView.Add(postView);

            }

            return postsForView.OrderByDescending(p => p.DateCreated).ToList();
        }

        [HttpGet]
        public ActionResult FindUsers(string searchQuery)
        {
            IEnumerable<UserInfo> users = _userService.FindUsers(searchQuery,
                ConfigurationManager.AppSettings["pictureStorage"]);
            ViewBag.Users = users;


            var userPageViewModel = GetUserPageViewModel(HttpContext.User.Identity.GetUserName());
            return View(userPageViewModel);
        }

        [HttpPost]
        public ActionResult GetLikedUsers(string postId)
        {
            IEnumerable<UserInfo> likedUsers = _postService.GetLikedUsers(_userService, postId,
                ConfigurationManager.AppSettings["pictureStorage"]);

            return PartialView("_LikedUsers", likedUsers);
        }

        private string GetUserAvatarLocation(User user)
        {
            if (user.Avatar == null)
                return string.Empty;

            var avatarLocation = "/" + _pictureService.GetPictureLocation(ConfigurationManager.AppSettings["pictureStorage"],
                            user.Id, user.Avatar, "_ava");

            return avatarLocation.Replace(@"\", @"/");
        }
    }
}