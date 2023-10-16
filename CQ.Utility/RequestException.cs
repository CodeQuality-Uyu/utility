using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQ.Utility
{
    public class RequestException<TError> : Exception where TError : class
    {
        public TError ErrorBody { get; }

        public RequestException(TError errorBody)
        {
            this.ErrorBody = errorBody;
        }
    }
}
