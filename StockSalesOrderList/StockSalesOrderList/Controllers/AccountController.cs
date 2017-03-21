using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using StockSalesOrderList.App_GlobalResources;
using StockSalesOrderList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace StockSalesOrderList.Controllers
{
    /// <summary>
    /// アカウント用コントローラー
    /// </summary>
    [MyRequiredHttps]
    public class AccountController : BaseController
    {
        #region サインイン/サインアウト
        [AllowAnonymous]
        [HttpGet]
        public ActionResult SignIn()
        {
            if (Request.IsAuthenticated)
                return RedirectToHome();
            return View(new SignInViewModel());
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> SignIn([Bind(Include = "SinginId,Password")]SignInViewModel model)
        {
            bool signIn = false;
            SignInLogModel log = new SignInLogModel();
            log.ClientIp = GetClientIp();
            log.UserCode = model.SinginId;

            if (!ModelState.IsValid)
            {
                log.SetStatus(-1);
                dbContext.SignInLogModels.Add(log);
                dbContext.SaveChanges();

                return View(model);
            }

            UserModel user = await UserManager.FindByNameAsync(model.SinginId);
            if (user != null)
            {
                log.UserModelId = user.Id;

                if (user.Deleted)
                {
                    log.SetStatus(-2);
                    dbContext.SignInLogModels.Add(log);
                    dbContext.SaveChanges();

                    ModelState.AddModelError("", AuthResources.AuthError);
                    return View(model);
                }
                if (UserManager.IsLockedOut(user.Id))
                {
                    log.SetStatus(-3);
                    dbContext.SignInLogModels.Add(log);
                    dbContext.SaveChanges();

                    ModelState.AddModelError("", AuthResources.AuthUserLockedOut);
                    return View(model);
                }
                if (UserManager.CheckPassword(user, model.Password))
                {
                    if (user.Enabled == false)
                    {
                        log.SetStatus(-4);
                        dbContext.SignInLogModels.Add(log);
                        dbContext.SaveChanges();

                        ModelState.AddModelError("", AuthResources.AuthUserDisabled);
                        return View(model);
                    }
                    if (user.PasswordSkipCnt >= 3)
                    {
                        log.SetStatus(-5);
                        dbContext.SignInLogModels.Add(log);
                        dbContext.SaveChanges();

                        ModelState.AddModelError("", AuthResources.AuthPasswordNoChange);
                        return View(model);
                    }

                    signIn = true;
                }
                else
                {
                    UserManager.AccessFailed(user.Id);
                    if (UserManager.IsLockedOut(user.Id))
                    {
                        log.SetStatus(-6);
                        dbContext.SignInLogModels.Add(log);
                        dbContext.SaveChanges();

                        ModelState.AddModelError("", AuthResources.AuthUserLockedOutNow);
                        return View(model);
                    }
                }
            }
            if (signIn)
            {
                user = await UserManager.FindAsync(model.SinginId, model.Password);
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                ClaimsIdentity identity = await user.GenerateUserIdentityAsync(UserManager);
                AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, identity);
                await UserManager.ResetAccessFailedCountAsync(user.Id);

                log.SetStatus(0);
                dbContext.SignInLogModels.Add(log);
                dbContext.SaveChanges();

                if (user.Expiration < DateTime.Now)
                {
                    user.PasswordSkipCnt++;
                    await UserManager.UpdateAsync(user);
                    return RedirectToAction("ExpirationChangePassword");
                }
                // await UserManager.UpdateAsync(user);
            }
            else
            {
                log.SetStatus(-7);
                dbContext.SignInLogModels.Add(log);
                dbContext.SaveChanges();

                ModelState.AddModelError("", AuthResources.AuthError);
                return View(model);
            }
            return RedirectToHome();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignOut()
        {
            SignInLogModel log = new SignInLogModel();
            log.UserModelId = GetUserId();
            log.ClientIp = GetClientIp();
            log.UserCode =GetSigninId();
            log.SetStatus(5);
            dbContext.SignInLogModels.Add(log);
            dbContext.SaveChanges();

            Session.RemoveAll();
            Session.Abandon();
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToHome();
        }
        #endregion

        #region パスワード変更/リセット
        [Authorize]
        [HttpGet]
        public ActionResult ExpirationChangePassword()
        {
            ViewBag.Message = "前回のパスワード変更から一定期間が経過しましたので、パスワードを変更してください。";
            return View("ChangePassword", new ChangePasswordViewModel());
        }

        [Authorize]
        [HttpGet]
        public ActionResult ChangePassword()
        {
            ViewBag.Message = "現在のパスワードが不明の場合はパスワード変更できません。パスワードリセットを考慮してください。";
            return View(new ChangePasswordViewModel());
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(
            [Bind(Include = "CurrentPassword,NewPassword,ConfirmPassword")]ChangePasswordViewModel model)
        {
            int id =GetUserId();
            SignInLogModel log = new SignInLogModel();
            log.ClientIp = GetClientIp();
            log.UserModelId = id;
            log.UserCode = GetSigninId();

            if (!ModelState.IsValid)
                return View(model);

            if (model.CurrentPassword == model.NewPassword)
            {
                ModelState.AddModelError("NewPassword", AuthResources.PasswordChangeSame);
                return View(model);
            }

            IdentityResult result = UserManager.ChangePassword(id, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                AddErrors(result);
                return View(model);
            }

            log.SetStatus(1);
            dbContext.SignInLogModels.Add(log);
            dbContext.SaveChanges();
            return View("PasswordChanged");
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult ResetPassword()
        {
            return View(new RequestMailViewModel());
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> ResetPassword([Bind(Include = "SinginId,Email")]RequestMailViewModel model)
        {
            if (string.IsNullOrEmpty(model.SinginId) && string.IsNullOrEmpty(model.Email))
            {
                ModelState.AddModelError("", AuthResources.RequestMailNoParam);
                return View(model);
            }

            UserModel user = null;
            if (!string.IsNullOrEmpty(model.SinginId))
            {
                user = await UserManager.FindByNameAsync(model.SinginId);
            }
            else if (!string.IsNullOrEmpty(model.Email))
            {
                user = await UserManager.FindByEmailAsync(model.Email);
            }

            if (user == null || !UserManager.IsEmailConfirmed(user.Id))
            {
                ModelState.AddModelError("", AuthResources.ResetPasswordNoUserOrNoConfirm);
                return View(model);
            }

            string code = UserManager.GeneratePasswordResetToken(user.Id);
            string callbackUrl = Url.Action("Reset", "Account", new { key = user.Id, code = code }, protocol: Request.Url.Scheme);
            string linktext = AuthResources.MailResetPasswordBody + callbackUrl;
            UserManager.SendEmail(user.Id, AuthResources.MailResetPasswordSubject, linktext);
            return View("MailSendComplete");
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Reset(string key, string code)
        {
            if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(code))
            {
                return View("RequestMailError");
            }

            ResetViewModel model = new ResetViewModel();
            model.Key = Convert.ToInt32(key);
            model.Token = code;
            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Reset([Bind(Include = "NewPassword,ConfirmPassword,Key,Token")]ResetViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = UserManager.FindById(model.Key);
            if (user == null)
            {
                ModelState.AddModelError("", AuthResources.RequestMailNoUser);
                return View(model);
            }
            var result = UserManager.ResetPassword(user.Id, model.Token, model.NewPassword);
            if (!result.Succeeded)
            {
                AddErrors(result);
                return View(model);
            }

            SignInLogModel log = new SignInLogModel();
            log.ClientIp = GetClientIp();
            log.UserModelId = user.Id;
            log.UserCode = user.UserName;
            log.SetStatus(2);
            dbContext.SignInLogModels.Add(log);
            dbContext.SaveChanges();

            UserManager.ResetAccessFailedCount(user.Id);
            return View("ResetComplete");
        }
        #endregion

        #region メール認証
        [AllowAnonymous]
        [HttpGet]
        public ActionResult ConfirmEmail()
        {
            return View(new RequestMailViewModel());
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> ConfirmEmail([Bind(Include = "SinginId,Email")]RequestMailViewModel model)
        {
            if (string.IsNullOrEmpty(model.SinginId) && string.IsNullOrEmpty(model.Email))
            {
                ModelState.AddModelError("", AuthResources.RequestMailNoParam);
                return View(model);
            }

            UserModel user = null;
            if (!string.IsNullOrEmpty(model.SinginId))
            {
                user = await UserManager.FindByNameAsync(model.SinginId);
            }
            else if (!string.IsNullOrEmpty(model.Email))
            {
                user = await UserManager.FindByEmailAsync(model.Email);
            }

            if (user == null)
            {
                ModelState.AddModelError("", AuthResources.RequestMailNoUser);
                return View(model);
            }

            string code = UserManager.GenerateEmailConfirmationToken(user.Id);
            string callbackUrl = Url.Action("Confirm", "Account", new { key = user.Id, code = code }, protocol: Request.Url.Scheme);
            string linktext = AuthResources.MailConfirmBody + callbackUrl;
            await UserManager.SendEmailAsync(user.Id, AuthResources.MailConfirmSubject, linktext);
            return View("MailSendComplete");
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> Confirm(string key, string code)
        {
            if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(code))
            {
                return View("RequestMailError");
            }

            IdentityResult result = await UserManager.ConfirmEmailAsync(int.Parse(key), code);
            if (result.Succeeded)
            {
                var user = UserManager.FindById(int.Parse(key));
                SignInLogModel log = new SignInLogModel();
                log.ClientIp = GetClientIp();
                log.UserModelId = int.Parse(key);
                if (user != null)
                    log.UserCode = user.UserName;
                log.SetStatus(3);
                dbContext.SignInLogModels.Add(log);
                dbContext.SaveChanges();

                return View("MailConfirmComplete");
            }
            else
            {
                return View("RequestMailError");
            }
        }

        #endregion

        #region メールアドレス変更
        [Authorize]
        [HttpGet]
        public ActionResult ChangeMail()
        {
            return View(new ChangeMailViewModel());
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> ChangeMail([Bind(Include = "Email")]ChangeMailViewModel model)
        {
            int id = GetUserId();
            SignInLogModel log = new SignInLogModel();
            log.ClientIp = GetClientIp();
            log.UserModelId = id;
            log.UserCode = GetSigninId();

            if (!ModelState.IsValid)
                return View(model);

            IdentityResult result = await UserManager.SetEmailAsync(id, model.Email);
            if (!result.Succeeded)
            {
                AddErrors(result);
                return View(model);
            }

            log.SetStatus(4);
            dbContext.SignInLogModels.Add(log);
            dbContext.SaveChanges();
            return View("MailChanged");
        }
        #endregion
    }
}
