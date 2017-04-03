using System;
using System.Security.Claims;
using Microsoft.AspNet.Identity;

namespace GPOrder.Helpers
{
    public static class Extensions
    {
        public static string GetUiCulture(this ClaimsIdentity claimsIdentity)
        {
            if (claimsIdentity == null)
            {
                throw new ArgumentNullException("claimsIdentity");
            }

            return claimsIdentity.FindFirstValue("UiCulture");
        }

        public static string GetTimeZone(this ClaimsIdentity claimsIdentity)
        {
            if (claimsIdentity == null)
            {
                throw new ArgumentNullException("claimsIdentity");
            }

            return claimsIdentity.FindFirstValue("TimeZone");
        }
    }
}