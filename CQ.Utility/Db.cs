using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQ.Utility;
public static class Db
{
    public static string NewId()
    {
        return Guid.NewGuid().ToString().Replace("-", "");
    }

    public static void ThrowIsInvalidId(string id, string propId)
    {
        var isIdInvalid = !IsIdValid(id);
        if (isIdInvalid)
        {
            throw new ArgumentException("The id is invalid", propId);
        }
    }

    public static bool IsIdValid(string id)
    {
        var isIdValid = Guid.TryParse(id, out Guid guidId);

        if (isIdValid)
        {
            return true;
        }

        if (id.Length < 24)
        {
            return false;
        }

        isIdValid = Guid.TryParse((id.Insert(8, "-").Insert(13, "-").Insert(18, "-").Insert(21, "-").Insert(23, "-")), out Guid guidFormattedId);

        return isIdValid;
    }

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
