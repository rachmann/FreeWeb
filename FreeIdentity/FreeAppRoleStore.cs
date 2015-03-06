using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DapperExtensions;
//using FreeIdentity.DapperExtensions;
using FreeIdentity.Models;
using Microsoft.AspNet.Identity;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Owin.Security.Provider;


namespace FreeIdentity
{
    public class FreeAppRoleStore : IQueryableRoleStore<FreeAppRole, int>, IRoleStore<FreeAppRole, int>, IDisposable
    {
        private bool _disposed;
        private readonly DbConnection _sqlConn;

        public IDbConnection GetOpenConnection(string connection)
        {
            var dbConnection = new SqlConnection(connection);
            dbConnection.Open();
            return dbConnection;
        }

        public FreeAppRoleStore(DbConnection connection)
        {
            _sqlConn = connection;
        }

        #region IQueryableRoleStore Members
        public IQueryable<FreeAppRole> Roles
        {
            get
            {
                var result = this._sqlConn.GetList<FreeAppRole>(new { }); 
                if (result != null)
                { return result.AsQueryable();}
                else
                {
                    return new List<FreeAppRole>().AsQueryable();
                }
                
            }
        }

        #endregion

        #region IRoleStore Members
        public Task CreateAsync(FreeAppRole role)
        {
            this.ThrowIfDisposed();

            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            return Task.Factory.StartNew(() => _sqlConn.Insert(role));
        }

        public Task DeleteAsync(FreeAppRole role)
        {
            this.ThrowIfDisposed();

            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            return Task.Factory.StartNew(() => _sqlConn.Delete(role));
        }
        public Task UpdateAsync(FreeAppRole role)
        {
            this.ThrowIfDisposed();

            if (role == null)
            {
                throw new ArgumentNullException("role");
            }
            return Task.Factory.StartNew(() => _sqlConn.Update(role));
        }

        public Task<FreeAppRole> FindByIdAsync(int roleId)
        {
            this.ThrowIfDisposed();

            return Task.Factory.StartNew(() => _sqlConn.Get<FreeAppRole>(roleId));
        }

        public Task<FreeAppRole> FindByNameAsync(string roleName)
        {
            this.ThrowIfDisposed();
            var predicate = Predicates.Field<FreeAppRole>(f => f.Name, Operator.Eq, roleName);
            return Task.Factory.StartNew(() => _sqlConn.GetList<FreeAppRole>(predicate).FirstOrDefault());
        }

        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected void Dispose(
            bool disposing)
        {
            this._disposed = true;
        }

        private void ThrowIfDisposed()
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException(base.GetType().Name);
            }
        }
        #endregion

        
    }

}
