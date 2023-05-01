using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using ORMByExample.Core.Interfaces;

namespace ORMByExample.Core.Impl
{
    public class UnitOfWorkHibernate : IUnitOfWork 
    {
        public ISession Session { get; }

        public UnitOfWorkHibernate(ISession session)
        {
            Session = session ?? throw new ArgumentNullException(nameof(session));
        }

        public void Transaction(Action action)
        {
            using var tran = Session.BeginTransaction();
            try
            {
                action();
                tran.Commit();
            }
            catch (Exception e)
            {
                tran.Rollback();
                throw new TransactionException($"Transaction",e);
            }

        }
        
        public void TransactionCustom(Action<Interfaces.ITransaction> action)
        {
            using var tran = Session.BeginTransaction();
            var transaction = new TransactionHibernate(tran);
            try
            {
             
                action(transaction);
                transaction.Commit();
            }
            catch (Exception e)
            {
                transaction.Rollback();
                throw new TransactionException($"TransactionCustom",e);
            }

        }

        public TReturn Transaction<TReturn>(Func<TReturn> action)
        {
            using var tran = Session.BeginTransaction();
            try
            {
                return action();
                tran.Commit();
            }
            catch (Exception e)
            {
                tran.Rollback();
                throw new TransactionException($"Transaction TReturn:{typeof(TReturn).FullName}",e);
            }

        }
        public async Task<TReturn> TransactionAsync<TReturn>(Func<Task<TReturn>> action)
        {
            using var tran = Session.BeginTransaction();
            try
            {
                return await action();
                await tran.CommitAsync();
            }
            catch (Exception e)
            {
                await tran.RollbackAsync();
                throw new TransactionException($"TransactionAsync TReturn:{typeof(TReturn).FullName}",e);
            }

        }
        public async Task TransactionAsync(Func<Task> action)
        {
            using var tran = Session.BeginTransaction();
            try
            {
                await action();
                await tran.CommitAsync();
            }
            catch (Exception e)
            {
                await tran.RollbackAsync();
                throw new TransactionException($"TransactionAsync",e);

            }

        }


        public void SaveChanges()
        {
            Session.Flush();
        }

        #region Disposing
        private bool _disposed = false;
        public void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    Session.Dispose();
                }
            }
            this._disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
