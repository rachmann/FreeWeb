using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeMailGun
{

    public enum MailGunResponseCode
    {
        Unknown = 0,
        Successful = 200,
        BadRequest = 400,
        Unauthorized = 401,
        RequestFailed = 402,
        NotFound = 404,
        ServerError_500 = 500,
        ServerError_502 = 502,
        ServerError_503 = 503,
        ServerError_504 = 504,
    }

}
