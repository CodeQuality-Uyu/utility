using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQ.Utility
{
    public static class Db
    {
        public static string NewId()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        public static void ThrowIdHasNotValidFormat(string id)
        {
            var isIdInvalid = !Guid.TryParse((id.Insert(8, "-").Insert(13, "-").Insert(18, "-").Insert(21, "-").Insert(23, "-")), out Guid guidId);
            if (isIdInvalid)
            {
                throw new ArgumentException("The id is invalid");
            }
        }

        public static void ThrowIdHasNotValidFormat<TException>(string id)
            where TException : Exception, new()
        {
            var isIdInvalid = !Guid.TryParse((id.Insert(8, "-").Insert(13, "-").Insert(18, "-").Insert(21, "-").Insert(23, "-")), out Guid guidId);
            if (isIdInvalid)
            {
                throw new TException();
            }
        }
    }
}
