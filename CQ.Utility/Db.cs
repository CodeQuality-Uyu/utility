using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQ.Utility
{
    public static class Db
    {
        /// <summary>
        /// Creates a new Guid id without '-' character
        /// </summary>
        /// <returns></returns>
        public static string NewId()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        /// <summary>
        /// Checks if id has Guid format
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="ArgumentException"></exception>
        public static void ThrowIsInvalidId(string id)
        {
            var isIdInvalid = !IsIdValid(id);
            if (isIdInvalid)
            {
                throw new ArgumentException("The id is invalid");
            }
        }

        /// <summary>
        /// Checks if id is valid
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsIdValid(string id)
        {
            var isIdValid = Guid.TryParse(id, out Guid guidId);

            if(isIdValid)
            {
                return true;
            }

            if(id.Length < 24)
            {
                return false;
            }

            isIdValid = Guid.TryParse((id.Insert(8, "-").Insert(13, "-").Insert(18, "-").Insert(21, "-").Insert(23, "-")), out Guid guidFormattedId);
            
            return isIdValid;
        }

        /// <summary>
        /// Checks if id has Guid format and throw custom exception
        /// </summary>
        /// <typeparam name="TException"></typeparam>
        /// <param name="id"></param>
        /// <exception cref="TException"></exception>
        public static void ThrowIsInvalidId<TException>(string id)
            where TException : Exception, new()
        {
            var isIdInvalid = !IsIdValid(id);
            if (isIdInvalid)
            {
                throw new TException();
            }
        }
    }
}
