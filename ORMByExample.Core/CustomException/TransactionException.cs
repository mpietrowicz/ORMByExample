using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMByExample.Core.CustomException
{
    public class TransactionException : Exception
    {
        public TransactionException(string message, Exception ex) : base(message, ex)
        {
            
        }
    }
}
