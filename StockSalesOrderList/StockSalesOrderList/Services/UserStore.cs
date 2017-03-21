using Microsoft.AspNet.Identity;
using StockSalesOrderList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace StockSalesOrderList.Services
{
    public class UserStore : IUserStore<UserModel, int>, IUserPasswordStore<UserModel, int>,
        IUserRoleStore<UserModel, int>, IUserLockoutStore<UserModel, int>, IUserSecurityStampStore<UserModel, int>,
        IUserEmailStore<UserModel, int>
    {
        #region Internal
        private DataContext db;

        public UserStore(DataContext applicationDbContext)
        {
            db = applicationDbContext;
        }

        private void CheckDisposed(UserModel user)
        {
            this.ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
        }

        private void ThrowIfDisposed()
        {
            if (this.disposedValue)
                throw new ObjectDisposedException(this.GetType().Name);
        }
        #endregion

        #region IUserStore
        Task IUserStore<UserModel, int>.CreateAsync(UserModel user)
        {
            CheckDisposed(user);
            user.Expiration = DateTime.Now.AddMonths(1).Date;
            db.UserModels.Add(user);
            db.SaveChanges();
            return Task.FromResult(user);
        }

        Task IUserStore<UserModel, int>.DeleteAsync(UserModel user)
        {
            CheckDisposed(user);
            user.Deleted = true;
            return Task.FromResult(default(object));
        }

        Task<UserModel> IUserStore<UserModel, int>.FindByIdAsync(int userId)
        {
            return Task.FromResult(db.UserModels.Where(x => x.Id == userId).SingleOrDefault());
        }

        Task<UserModel> IUserStore<UserModel, int>.FindByNameAsync(string userName)
        {
            return Task.FromResult(db.UserModels.Where(x => x.UserName == userName).SingleOrDefault());
        }

        Task IUserStore<UserModel, int>.UpdateAsync(UserModel user)
        {
            CheckDisposed(user);
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            return Task.FromResult(default(object));
        }
        #endregion

        #region IUserPasswordStore
        Task IUserPasswordStore<UserModel, int>.SetPasswordHashAsync(UserModel user, string passwordHash)
        {
            CheckDisposed(user);
            user.Password = passwordHash;
            user.Expiration = DateTime.Now.AddMonths(1).Date;
            user.PasswordSkipCnt = 0;
            return Task.FromResult(default(object));
        }

        Task<string> IUserPasswordStore<UserModel, int>.GetPasswordHashAsync(UserModel user)
        {
            CheckDisposed(user);
            return Task.FromResult(user.Password);
            // return Task.FromResult(db.UserModels.Where(x => x.Id == user.Id).Select(x => x.Password).SingleOrDefault());
        }

        Task<bool> IUserPasswordStore<UserModel, int>.HasPasswordAsync(UserModel user)
        {
            CheckDisposed(user);
            return Task.FromResult(!string.IsNullOrEmpty(user.Password));
            // return Task.FromResult(db.UserModels.Where(x => x.Id == user.Id).Select(x => x.Password).SingleOrDefault() != null);
        }
        #endregion

        #region IUserRoleStore
        Task IUserRoleStore<UserModel, int>.AddToRoleAsync(UserModel user, string roleName)
        {
            CheckDisposed(user);
            RoleModel role = db.RoleModels.Where(x => x.Name == roleName).SingleOrDefault();
            if (role != null)
            {
                UserRoleModel model = db.UserRoleModels.Where(ur => ur.UserModelId == user.Id)
                    .Where(ur => ur.RoleModelId == role.Id).SingleOrDefault();
                if (model == null)
                {
                    model = new UserRoleModel();
                    model.UserModelId = user.Id;
                    model.RoleModelId = role.Id;
                    db.UserRoleModels.Add(model);
                }
                else
                {
                    model.Deleted = false;
                    db.Entry(model).State = EntityState.Modified;
                }
                db.SaveChanges();
            }
            return Task.FromResult(default(object));
        }

        Task IUserRoleStore<UserModel, int>.RemoveFromRoleAsync(UserModel user, string roleName)
        {
            CheckDisposed(user);
            UserRoleModel model = db.UserRoleModels.Where(ur => ur.UserModelId == user.Id)
                .Where(ur => ur.RoleModel.Name == roleName).SingleOrDefault();
            if (model != null)
            {
                model.Deleted = true;
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
            }
            return Task.FromResult(default(object));
        }

        Task<IList<string>> IUserRoleStore<UserModel, int>.GetRolesAsync(UserModel user)
        {
            CheckDisposed(user);
            IList<string> result = db.UserRoleModels.Where(x => x.UserModelId == user.Id)
                .Where(y => y.Deleted == false).Select(y => y.RoleModel.Name).ToList<string>();
            return Task.FromResult(result);
        }

        Task<bool> IUserRoleStore<UserModel, int>.IsInRoleAsync(UserModel user, string roleName)
        {
            CheckDisposed(user);
            IList<string> result = db.UserRoleModels.Where(x => x.UserModelId == user.Id)
                .Where(x => x.Deleted == false).Select(x => x.RoleModel.Name).ToList<string>();
            return Task.FromResult(result.FirstOrDefault(x => x.ToLower() == roleName.ToLower()) != null);
        }
        #endregion

        #region IUserLockoutStore
        Task<DateTimeOffset> IUserLockoutStore<UserModel, int>.GetLockoutEndDateAsync(UserModel user)
        {
            CheckDisposed(user);
            return Task.FromResult(user.LockoutEndDataUtc);
        }

        Task IUserLockoutStore<UserModel, int>.SetLockoutEndDateAsync(UserModel user, DateTimeOffset lockoutEnd)
        {
            CheckDisposed(user);
            user.LockoutEndDataUtc = lockoutEnd.UtcDateTime;
            db.Entry(user).State = EntityState.Modified;
            return Task.FromResult(default(object));
            // return db.SaveChangesAsync();
        }

        Task<int> IUserLockoutStore<UserModel, int>.IncrementAccessFailedCountAsync(UserModel user)
        {
            CheckDisposed(user);
            user.AccessFailedCount++;
            return Task.FromResult(user.AccessFailedCount);
        }

        Task IUserLockoutStore<UserModel, int>.ResetAccessFailedCountAsync(UserModel user)
        {
            CheckDisposed(user);
            user.AccessFailedCount = 0;
            return Task.FromResult(default(object));
        }

        Task<int> IUserLockoutStore<UserModel, int>.GetAccessFailedCountAsync(UserModel user)
        {
            CheckDisposed(user);
            return Task.FromResult(user.AccessFailedCount);
        }

        Task<bool> IUserLockoutStore<UserModel, int>.GetLockoutEnabledAsync(UserModel user)
        {
            CheckDisposed(user);
            return Task.FromResult(user.LockoutEnabled);
        }

        Task IUserLockoutStore<UserModel, int>.SetLockoutEnabledAsync(UserModel user, bool enabled)
        {
            CheckDisposed(user);
            user.LockoutEnabled = enabled;
            return Task.FromResult(default(object));
        }
        #endregion

        #region IUserSecurityStampStore
        Task IUserSecurityStampStore<UserModel, int>.SetSecurityStampAsync(UserModel user, string stamp)
        {
            CheckDisposed(user);
            user.SecurityTimestamp = stamp;
            return Task.FromResult(default(object));
        }

        Task<string> IUserSecurityStampStore<UserModel, int>.GetSecurityStampAsync(UserModel user)
        {
            CheckDisposed(user);
            return Task.FromResult(user.SecurityTimestamp);
            // return Task.FromResult(db.UserModels.Where(x => x.Id == user.Id).Select(x => x.SecurityTimestamp).SingleOrDefault());
        }
        #endregion

        #region IUserEmailStore
        Task IUserEmailStore<UserModel, int>.SetEmailAsync(UserModel user, string email)
        {
            CheckDisposed(user);
            user.Email = email;
            return Task.FromResult(default(object));
        }

        Task<string> IUserEmailStore<UserModel, int>.GetEmailAsync(UserModel user)
        {
            CheckDisposed(user);
            return Task.FromResult(user.Email);
        }

        Task<bool> IUserEmailStore<UserModel, int>.GetEmailConfirmedAsync(UserModel user)
        {
            CheckDisposed(user);
            return Task.FromResult(user.EmailConfirmed);
        }

        Task IUserEmailStore<UserModel, int>.SetEmailConfirmedAsync(UserModel user, bool confirmed)
        {
            user.EmailConfirmed = confirmed;
            return Task.FromResult(default(object));
        }

        Task<UserModel> IUserEmailStore<UserModel, int>.FindByEmailAsync(string email)
        {
            return Task.FromResult(db.UserModels.Where(x => x.Email == email).SingleOrDefault());
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }
                disposedValue = true;
            }
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
