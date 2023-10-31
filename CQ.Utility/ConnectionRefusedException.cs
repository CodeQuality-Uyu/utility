using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQ.Utility
{
    public class ConnectionRefusedException : Exception
    {
        public readonly string Connection;

        public ConnectionRefusedException(string connection)
        {
            Connection = connection;
        }
    }
}
