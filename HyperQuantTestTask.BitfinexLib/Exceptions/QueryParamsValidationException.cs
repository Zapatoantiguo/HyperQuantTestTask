using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperQuantTestTask.BitfinexLib.Exceptions
{
    public class QueryParamsValidationException : Exception
    {
        public QueryParamsValidationException(string message) : base(message)
        {
        }
    }
}
