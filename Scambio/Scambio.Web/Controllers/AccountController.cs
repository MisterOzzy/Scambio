using System;
using System.Configuration;
using System.IO;
using System.Net;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Scambio.DataAccess.Infrastructure;
using Scambio.Domain.Models;
using Scambio.Logic;
using Scambio.Logic.Interfaces;
using Scambio.Web.Helpers;
using Scambio.Web.Identity;
using Scambio.Web.ViewModels;

namespace Scambio.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser, Guid> _userManager;
        private readonly IUserService _userService;
        private readonly IPictureService _pictureService;

        public AccountController(IUserStore<IdentityUser, Guid> userStore, IUnitOfWork unitOfWork)
        {
            _userManager = new UserManager<IdentityUser, Guid>(userStore);
            _pictureService = new PictureService(unitOfWork);
            _userService = new UserService(unitOfWork, _pictureService);

        }

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        private async Task SignInAsync(IdentityUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            //VISNEET
            var identity = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        ////public string Signed()
        ////{
        ////    return "Signed";
        ////}

        
        public ActionResult Settings()
        {
            var userId = HttpContext.User.Identity.GetUserId();
            var user = _userService.GetUser(new Guid(userId));
            ViewBag.User = user;
            return View(user);
        }

        [HttpPost]
        public ActionResult ChangeAvatar(HttpPostedFileBase newAvatar)
        {
            var pictureFolderStorage = ConfigurationManager.AppSettings["pictureStorage"];
            var pathToStorage = HostingEnvironment.MapPath($"~/{pictureFolderStorage}");

            var tmp = newAvatar.FileName.Split('.');
            var extension = tmp[tmp.Length - 1];
            var userId = HttpContext.User.Identity.GetUserId();

            var picture = new Picture()
            {
                Id = Guid.NewGuid(),
                Secret = Guid.NewGuid().ToString().Substring(0, 8),
                Extension = extension
            };

            _pictureService.AddPicture(picture);
            

            var filename = _pictureService.GeneratePictureFilename(picture);
            var pathToFile = Path.Combine(pathToStorage, userId);


            _pictureService.CreatePicture(filename, pathToFile, newAvatar.InputStream);

            string pathToAvatar = $"http://{Request.Url.Authority}/" +
                                  _pictureService.GetPictureLocation(pictureFolderStorage, new Guid(userId), picture.Id)
                                      .Replace(@"\", @"/");
            var cropPhotoViewModel = new CropPhotoViewModel()
            {
                PathToPicture = pathToAvatar,
                PictureId = picture.Id
            };


            return View("CropPhoto", cropPhotoViewModel);
        }

        [HttpPost]
        public ActionResult CropAvatar(string imagePath, string pictureId, int? cropPointX, int? cropPointY, int? imageCropWidth,
            int? imageCropHeight)
        {
            if (string.IsNullOrEmpty(imagePath)
                || !cropPointX.HasValue
                || !cropPointY.HasValue
                || !imageCropWidth.HasValue
                || !imageCropHeight.HasValue)
            {
                return new HttpStatusCodeResult((int)HttpStatusCode.BadRequest);
            }

            var pictureFolderStorage = ConfigurationManager.AppSettings["pictureStorage"];
            var pathToStorage = HostingEnvironment.MapPath($"~/{pictureFolderStorage}");
            var userId = HttpContext.User.Identity.GetUserId();
            var imagePathLocal = _pictureService.GetPictureLocation(pathToStorage, new Guid(userId), new Guid(pictureId));

            byte[] imageBytes = System.IO.File.ReadAllBytes(imagePathLocal);
            Stream croppedImage = ImageHelper.CropImage(imageBytes, cropPointX.Value, cropPointY.Value, imageCropWidth.Value, imageCropHeight.Value);


            var originalPicture = _pictureService.GetPicture(new Guid(pictureId));
            var croppedPictureFilename = _pictureService.GeneratePictureFilename(originalPicture.Id,
                originalPicture.Secret + "_ava", originalPicture.Extension);

            
            
            var pathToFile = Path.Combine(pathToStorage, userId);

            croppedImage.Seek(0, SeekOrigin.Begin);
            _pictureService.CreatePicture(croppedPictureFilename, pathToFile, croppedImage);
            _userService.ChangeAvatar(originalPicture, new Guid(userId));
            
            ////string tempFolderName = Server.MapPath("~/" + ConfigurationManager.AppSettings["Image.TempFolderName"]);
            ////string fileName = Path.GetFileName(imagePath);

            ////try
            ////{
            ////    FileHelper.SaveFile(croppedImage, Path.Combine(tempFolderName, fileName));
            ////}
            ////catch (Exception ex)
            ////{
            ////    //Log an error     
            ////    return new HttpStatusCodeResult((int)HttpStatusCode.InternalServerError);
            ////}

            ////string photoPath = string.Concat("/", ConfigurationManager.AppSettings["Image.TempFolderName"], "/", fileName);
            ////return Json(new { photoPath = photoPath }, JsonRequestBehavior.AllowGet);

            return Json(Url.RouteUrl("UserHome", new { username = HttpContext.User.Identity.GetUserName() })); 
            ////return RedirectToAction("UserPage", "Home", HttpContext.User.Identity.GetUserName());
        }


        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                
                var user = await _userManager.FindAsync(model.UserName, model.Password);
                if (user != null)
                {
                    await SignInAsync(user, model.RememberMe);
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser() { UserName = model.UserName, FirstName = model.FirstName, LastName = model.LastName, Email = model.Email, Birthday = model.Birthday};
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInAsync(user, isPersistent: false);
                    return RedirectToAction("UserPage", "Home", user);
                }
                //else
                //{
                //    AddErrors(result);
                //}
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        ////public ActionResult About()
        ////{
        ////    ViewBag.Message = "Your application description page.";

        ////    return View();
        ////}

        ////public ActionResult Contact()
        ////{
        ////    ViewBag.Message = "Your contact page.";

        ////    return View();
        ////}
    }
}