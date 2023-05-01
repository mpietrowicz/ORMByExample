using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMByExample.Core.Impl
{
    internal class TransactionHibernate :Interfaces.ITransaction
    {
        private readonly NHibernate.ITransaction _transaction;

        public TransactionHibernate(NHibernate.ITransaction transaction)
        {
            _transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public void Dispose()
        {
            _transaction.Dispose();
        }

        public Task CommitAsync()
        {
           return _transaction.CommitAsync();
        }

        public Task RollbackAsync()
        {
            return _transaction.RollbackAsync();
        }
      
    }
}
