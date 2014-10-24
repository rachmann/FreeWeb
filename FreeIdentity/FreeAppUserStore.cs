using Dapper;
using Dapper.Contrib.Extensions;
using FreeIdentity.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace FreeIdentity
{
    public class FreeAppUserStore :
        IUserLoginStore<FreeAppUser, int>,
        IUserClaimStore<FreeAppUser, int>,
        IUserPasswordStore<FreeAppUser, int>,
        IUserRoleStore<FreeAppUser, int>,
        IUserSecurityStampStore<FreeAppUser, int>,
        IUserEmailStore<FreeAppUser, int>,
        IDisposable
    {
        //FIXME: ? Perhaps use this key in Claims - have seen as ValueType (??)
        //private readonly string ProviderNameKey = "DapperDbProvider";

        private readonly DbConnection _sqlConn;

        public IDbConnection GetOpenConnection(string connection)
        {
            var dbConnection = new SqlConnection(connection);
            dbConnection.Open();
            return dbConnection;
        }

        //public FreeAppUserStore(string connectionString)
        //{
        //    _sqlConn = GetOpenConnection(connectionString);
        //}

        //#region IUserStore

        public Task CreateAsync(FreeAppUser user)
        {
            return Task.Factory.StartNew(() =>
            {
                if (user != null)
                {
                    _sqlConn.Insert(user);
                }
            });
        }

        public FreeAppUserStore(DbConnection connection)
        {
            _sqlConn = connection;
        }
        #region IUserStore

        public static FreeAppUserStore Create(IdentityFactoryOptions<FreeAppUserStore> options, IOwinContext context)
        {
            return new FreeAppUserStore(context.Get<DbConnection>());
        }

        public Task UpdateAsync(FreeAppUser user)
        {

            return Task.Factory.StartNew(() =>
            {
                if (user != null)
                {
                    _sqlConn.Update(user);
                }
            });

        }

        public Task DeleteAsync(FreeAppUser user)
        {
            return Task.Factory.StartNew(() =>
            {
                if (user != null)
                {
                    _sqlConn.Delete(user);
                }
            });
        }

        Task<FreeAppUser> IUserStore<FreeAppUser, int>.FindByIdAsync(int userId)
        {
            return Task.Factory.StartNew(() => _sqlConn.Get<FreeAppUser>(userId));
        }

        Task<FreeAppUser> IUserStore<FreeAppUser, int>.FindByNameAsync(string userName)
        {
            return Task.Factory.StartNew(() => _sqlConn.Query<FreeAppUser>(
                "SELECT * FROM FreeAppUser WHERE FreeAppUser.UserName = @userName", 
                new { userName }).FirstOrDefault());
        }

        #endregion

        #region IUserEmailStore
        public Task SetEmailAsync(FreeAppUser user, string email)
        {
            return Task.Factory.StartNew(() => user.Email = email);
        }

        public Task<string> GetEmailAsync(FreeAppUser user)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(FreeAppUser user)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(FreeAppUser user, bool confirmed)
        {
            return Task.Factory.StartNew(() => user.EmailConfirmed = confirmed);
        }
    
        Task<FreeAppUser> IUserEmailStore<FreeAppUser, int>.FindByEmailAsync(string email)
        {
            return Task.Factory.StartNew(() => _sqlConn.Query<FreeAppUser>(
                "SELECT * FROM FreeAppUser WHERE FreeAppUser.Email = @email", 
                new { email }).FirstOrDefault());
        }
        #endregion

        #region IUserPasswordStore
        public Task SetPasswordHashAsync(FreeAppUser user, string passwordHash)
        {
            return Task.FromResult(user.PasswordHash = passwordHash);
        }

        public Task<string> GetPasswordHashAsync(FreeAppUser user)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(FreeAppUser user)
        {
            return Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
        }
        #endregion


        #region IUserRoleStore
        public Task AddToRoleAsync(FreeAppUser user, string roleName)
        {
            return Task.Factory.StartNew(() =>
            {
                //does this role exist?
                var roleItem = _sqlConn.Query<FreeAppRole>("SELECT * FROM FreeAppRole WHERE FreeAppRole.Name = @roleName", new { roleName }).FirstOrDefault();
                if (roleItem != null)
                {
                    //does this user & role combo already exist?
                    var roleUserItem = _sqlConn.Query<FreeAppUserRole>("SELECT * FROM FreeAppUserRole WHERE FreeAppUserRole.UserId = @UserId AND FreeAppUserRole.RoleId = @RoleId", new { UserId = user.Id, RoleId = roleItem.Id }).FirstOrDefault();
                    if (roleUserItem == null)
                    {
                        // no - so add
                        _sqlConn.Insert(new FreeAppUserRole { UserId = user.Id, RoleId = roleItem.Id });
                    }

                }
            });
        }

        public Task RemoveFromRoleAsync(FreeAppUser user, string roleName)
        {
            return Task.Factory.StartNew(() =>
            {
                //does this role exist?
                var roleItem = _sqlConn.Query<FreeAppRole>("SELECT * FROM FreeAppRole WHERE FreeAppRole.Name = @roleName", new { roleName }).FirstOrDefault();
                if (roleItem != null)
                {
                    //does this user & role combo already exist?
                    var roleUserItem = _sqlConn.Query<FreeAppUserRole>("SELECT * FROM FreeAppUserRole WHERE FreeAppUserRole.UserId = @UserId AND FreeAppUserRole.RoleId = @RoleId", new { UserId = user.Id, RoleId = roleItem.Id }).FirstOrDefault();
                    if (roleUserItem != null)
                    {
                        // yes - so delete
                        _sqlConn.Delete(new FreeAppUserRole {UserId = user.Id, RoleId = roleItem.Id });
                    }

                }
            });
        }

        public Task<IList<string>> GetRolesAsync(FreeAppUser user)
        {
            return Task.Factory.StartNew(() =>
            {
                //does this role exist?
                //does this user & role combo already exist?
                var results = _sqlConn.Query<FreeAppRole>(@"SELECT FreeAppRole.* FROM FreeAppUserRole ru
                             INNER JOIN FreeAppRole on FreeAppRole.Id = ru.RoleId WHERE ru.UserId = @Id", new { user.Id }).ToList();

                var retList = results.Select(r => r.Name).ToList();
                return (IList<string>)retList;
            });
        }

        public Task<bool> IsInRoleAsync(FreeAppUser user, string roleName)
        {
            return Task.Factory.StartNew(() =>
            {
                //does this role exist?
                var result = false;
                var roleItem = _sqlConn.Query<FreeAppRole>("SELECT * FROM FreeAppRole WHERE FreeAppRole.Name = @roleName", new { roleName }).FirstOrDefault();
                if (roleItem != null)
                {
                    //does this user & role combo already exist?
                    var roleUserItem = _sqlConn.Query<FreeAppUserRole>("SELECT * FROM FreeAppUserRole WHERE FreeAppUserRole.UserId = @UserId AND FreeAppUserRole.RoleId = @RoleId", new { UserId = user.Id, RoleId = roleItem.Id }).FirstOrDefault();
                    if (roleUserItem != null)
                    {
                        result = true;
                    }
                }
                return result;

            });
        }
        #endregion

        #region IUserSecurityStampStore

        public Task SetSecurityStampAsync(FreeAppUser user, string stamp)
        {
            return Task.Factory.StartNew(() => user.SecurityStamp = stamp);
        }

        public Task<string> GetSecurityStampAsync(FreeAppUser user)
        {
            return Task.FromResult(user.SecurityStamp);
        }
        #endregion

        #region IUserLoginStore

        public Task AddLoginAsync(FreeAppUser user, FreeAppUserLogin login)
        {
            return AddLoginAsync(user, new UserLoginInfo(login.LoginProvider, login.ProviderKey));
        }
        public Task AddLoginAsync(FreeAppUser user, UserLoginInfo login)
        {
            return Task.Factory.StartNew(() =>
            {
                //does this user exist?
                var userItem = _sqlConn.Get<FreeAppUser>(user.Id);
                if (userItem != null)
                {
                    var userLoginItem =
                        _sqlConn.Query<FreeAppUserLogin>(
                            "SELECT * FROM FreeAppUserLogin WHERE FreeAppUserLogin.UserId = @Id AND FreeAppUserLogin.ProviderKey = @ProviderKey",
                            new { user.Id, login.ProviderKey }).FirstOrDefault();
                    if (userLoginItem == null)
                    {
                        _sqlConn.Insert(
                            new FreeAppUserLogin
                            {
                                UserId = user.Id,
                                ProviderKey = login.ProviderKey,
                                LoginProvider = login.LoginProvider
                            });
                    }
                }
            });
        }
        public Task RemoveLoginAsync(FreeAppUser user, FreeAppUserLogin login)
        {
            return RemoveLoginAsync(user, new UserLoginInfo(login.LoginProvider, login.ProviderKey));
        }
        public Task RemoveLoginAsync(FreeAppUser user, UserLoginInfo login)
        {
            return Task.Factory.StartNew(() =>
            {
                //does this user exist?
                var userItem = _sqlConn.Get<FreeAppUser>(user.Id);
                if (userItem != null)
                {
                    var userLoginItem = _sqlConn.Query<FreeAppUserLogin>(
                        "SELECT * FROM FreeAppUserLogin WHERE FreeAppUserLogin.UserId = @Id AND FreeAppUserLogin.ProviderKey = @ProviderKey", 
                        new { user.Id, login.ProviderKey }).FirstOrDefault();

                    if (userLoginItem != null)
                    {
                        _sqlConn.Delete(userLoginItem);
                    }
                }
            });
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(FreeAppUser user)
        {
            return Task.Factory.StartNew(() =>
            {
                //does this user exist?
                List<UserLoginInfo> logins = null;
                var userItem = _sqlConn.Get<FreeAppUser>(user.Id);
                if (userItem != null)
                {
                    logins = _sqlConn.Query<FreeAppUserLogin>("SELECT * FROM [dbo].[FreeAppUserLogin] WHERE [dbo].[FreeAppUserLogin].[UserId] = @Id;", 
                        new { user.Id }).Select(culi => new UserLoginInfo(culi.LoginProvider, culi.ProviderKey)).ToList();
                }

                return (IList<UserLoginInfo>)logins;
            });
        }

        public Task<FreeAppUser> FindAsync(UserLoginInfo login)
        {
            return Task.Factory.StartNew(() =>
            {
                //does this user exist?
                FreeAppUser user = null;
                var freeAppUserLogin = _sqlConn.Query<FreeAppUserLogin>("SELECT * FROM [dbo].[FreeAppUserLogin] WHERE [dbo].[FreeAppUserLogin].[LoginProvider] = @LoginProvider AND [dbo].[FreeAppUserLogin].[ProviderKey] = @ProviderKey;",
                    new { login.LoginProvider, login.ProviderKey }).FirstOrDefault();
                if (freeAppUserLogin != null)
                {
                    user = _sqlConn.Get<FreeAppUser>(freeAppUserLogin.UserId);
                }

                return user;
            });
        }
        public Task<FreeAppUser> FindAsync(FreeAppUserLogin login)
        {
            return FindAsync(new UserLoginInfo(login.LoginProvider, login.ProviderKey));
        }
        #endregion

        #region IUserClaimStore Helpers

        public Task<IList<FreeAppUserClaimType>> GetClaimTypes()
        {
            return Task.Factory.StartNew(() =>
            {
                var claimTypes = _sqlConn.Query<FreeAppUserClaimType>(
                    "SELECT * FROM FreeAppUserClaimType")
                    .ToList();

                return (IList<FreeAppUserClaimType>)claimTypes;
            });
        }

        #endregion

        #region IUserClaimStore
        public Task<IList<Claim>> GetClaimsAsync(FreeAppUser user)
        {
            return Task.Factory.StartNew(() =>
            {
                //does this user exist?
                List<Claim> claims = null;
                var userItem = _sqlConn.Get<FreeAppUser>(user.Id);
                if (userItem != null)
                {
                    claims = _sqlConn.Query<FreeAppUserClaimJoined>("SELECT cuc.*, cuct.ClaimTypeCode FROM FreeAppUserClaim cuc INNER JOIN FreeAppUserClaimType cuct ON cuc.ClaimTypeId = cuct.TypeId WHERE cuc.UserId = @Id;",
                    new { user.Id }).Select(cuc =>
                        new Claim(cuc.ClaimTypeCode, cuc.ClaimValue, cuc.ClaimValueType, cuc.Issuer))
                           .ToList();
                }

                return (IList<Claim>)claims;
            });
        }

        public Task AddClaimAsync(FreeAppUser user, Claim claim)
        {
            return Task.Factory.StartNew(() =>
            {
                //does this user exist?
                var userItem = _sqlConn.Get<FreeAppUser>(user.Id);
                if (userItem != null)
                {

                    var oldClaim =
                        _sqlConn.Query<FreeAppUserClaim>(
                            "SELECT cuc.* FROM FreeAppUserClaim cuc "+
                               "INNER JOIN FreeAppUserClaimType cuct ON cuc.ClaimTypeId = cuct.TypeId "+
                               "WHERE cuc.UserId = @Id AND " +
                                   "cuct.ClaimTypeCode = @Type AND " +
                                   "cuc.ClaimValue = @Value AND " +
                                   "cuc.Issuer = @Issuer;",
                            new { user.Id, claim.Type, claim.Value, claim.Issuer })
                            .FirstOrDefault();
                    if (oldClaim == null)
                    {
                        // verify ClaimType
                        var theClaimType = _sqlConn.Query<FreeAppUserClaimType>(
                           "SELECT * FROM FreeAppUserClaimType WHERE ClaimTypeCode = @Type;",
                           new { claim.Type })
                           .FirstOrDefault();

                        if (theClaimType != null)
                        {
                            _sqlConn.Insert<FreeAppUserClaim>(new FreeAppUserClaim
                            {
                                UserId = user.Id,
                                ClaimTypeId = theClaimType.TypeId,
                                ClaimValue = claim.ValueType,
                                ClaimValueType = claim.ValueType,
                                Issuer = claim.Issuer
                            });
                        }
                    }
                }
                return Task.FromResult(0);
            });
        }

        public Task AddClaimAsync(FreeAppUser user, FreeAppUserClaim claim)
        {
            return Task.Factory.StartNew(() =>
            {
                //does this user exist?
                var userItem = _sqlConn.Get<FreeAppUser>(user.Id);
                if (userItem != null)
                {
                    var oldClaim =
                        _sqlConn.Query<FreeAppUserClaim>(
                            "SELECT * FROM FreeAppUserClaim cuc " +
                               "WHERE cuc.UserId = @Id AND " +
                                  "cuc.ClaimTypeId = @ClaimTypeId AND " +
                                  "cuc.ClaimValue = @ClaimValue AND " +
                                  "cuc.Issuer = @Issuer;",
                            new { user.Id, claim.ClaimTypeId, claim.ClaimValue, claim.Issuer })
                            .FirstOrDefault();
                    if (oldClaim == null)
                    {
                        // verify ClaimType
                       var theClaimType = _sqlConn.Query<FreeAppUserClaimType>(
                           "SELECT * FROM FreeAppUserClaimType WHERE TypeId = @ClaimTypeId;",
                           new {  claim.ClaimTypeId })
                           .FirstOrDefault();

                        if (theClaimType != null)
                        {
                            _sqlConn.Insert<FreeAppUserClaim>(claim);
                        }
                    }
                }
                return Task.FromResult(0);
            });
        }

        public Task RemoveClaimAsync(FreeAppUser user, Claim claim)
        {
            return Task.Factory.StartNew(() =>
            {
                //does this user exist?
                var userItem = _sqlConn.Get<FreeAppUser>(user.Id);
                if (userItem != null)
                {
                    var theClaimType = _sqlConn.Query<FreeAppUserClaimType>(
                       "SELECT * from FreeAppUserClaimType WHERE ClaimTypeCode = @Type;",
                       new { claim.Type })
                       .FirstOrDefault();
                    if (theClaimType != null)
                    {
                        var oldClaim = _sqlConn.Query<FreeAppUserClaim>(
                            "SELECT * from [dbo].[FreeAppUserClaim] " +
                                "WHERE UserId = @Id AND " +
                                    "ClaimTypeId = @TypeId AND " +
                                    "ClaimValue = @Value AND "+
                                    "Issuer = @Issuer;",
                            new {user.Id, theClaimType.TypeId, claim.Value, claim.Issuer})
                            .FirstOrDefault();
                        if (oldClaim != null)
                        {
                            _sqlConn.Delete(oldClaim);
                        }
                    }
                }
            });
        }
        #endregion

        public void Dispose()
        {
            if (_sqlConn != null)
            {
                if (_sqlConn.State == ConnectionState.Open || _sqlConn.State == ConnectionState.Fetching || _sqlConn.State == ConnectionState.Executing)
                    try
                    {
                        _sqlConn.Close();
                    }
                    catch
                    {
                        //absorb bad close
                    }
                _sqlConn.Dispose();
            }

        }
    }
}
