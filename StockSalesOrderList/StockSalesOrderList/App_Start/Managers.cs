using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using StockSalesOrderList.App_GlobalResources;
using StockSalesOrderList.Models;
using StockSalesOrderList.Services;
using System;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StockSalesOrderList
{
    public class ApplicationUserManager : UserManager<UserModel, int>
    {
        public ApplicationUserManager(IUserStore<UserModel, int> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore(DataContext.CurrentContext));
            manager.UserValidator = new UserValidator<UserModel, int>(manager)
            {
                AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = true
            };
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                // RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true
            };
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromHours(6);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            manager.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<UserModel, int>
            {
                Subject = "SecurityCode",
                BodyFormat = "Your security code is: {0}"
            });
            manager.EmailService = new EmailService();

            IDataProtectionProvider dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<UserModel, int>(dataProtectionProvider.Create("ASP.NET Identity"))
                    { TokenLifespan = TimeSpan.FromHours(4) };
            }
            return manager;
        }
    }
    public class ApplicationSignInManager : SignInManager<UserModel, int>
    {
        public ApplicationSignInManager(UserManager<UserModel, int> userManager, IAuthenticationManager authenticationManager) : base(userManager, authenticationManager)
        {
        }
        public override Task<ClaimsIdentity> CreateUserIdentityAsync(UserModel user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }
    }
    public class ApplicationRoleManager : RoleManager<RoleModel, int>
    {
        public ApplicationRoleManager(IRoleStore<RoleModel, int> store)
            : base(store)
        {
        }
    }

    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            MailMessage msg = new MailMessage(AuthResources.EmailFrom, message.Destination);
            msg.Subject = message.Subject;
            msg.IsBodyHtml = false;
            msg.Body = message.Body.Replace(@"\r\n", System.Environment.NewLine);

            SmtpClient sc = new SmtpClient();
            sc.Host = AuthResources.SmtpHost;
            sc.Port = int.Parse(AuthResources.SmtpPort);
            sc.DeliveryMethod = SmtpDeliveryMethod.Network;
            sc.Credentials = new NetworkCredential(AuthResources.EmailUserName, AuthResources.EmailPassword);
            sc.EnableSsl = true;
            sc.Send(msg);

            msg.Dispose();
            sc.Dispose();
            return Task.FromResult(0);
        }
    }
}
