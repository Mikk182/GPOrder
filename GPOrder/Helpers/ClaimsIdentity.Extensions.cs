using System;
using System.Globalization;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNet.Identity;

namespace GPOrder.Helpers
{
    public static partial class Extensions
    {
        public static CultureInfo GetUiCulture(this ClaimsIdentity claimsIdentity)
        {
            try
            {
                if (!claimsIdentity.IsAuthenticated)
                {
                    return CultureInfo.CurrentUICulture;
                }
                var cultureName = claimsIdentity.FindFirstValue("UiCulture");
                return CultureInfo.GetCultureInfo(cultureName);
            }
            catch (Exception ex)
            {
                Logger.TraceError(
                    ex,
                    "ClaimsIdentity.Extensions.GetUiCulture error: there is a problem with culture user '{0}'",
                    claimsIdentity.GetUserName());

                throw;
            }
        }

        public static TimeZoneInfo GetTimeZone(this ClaimsIdentity claimsIdentity)
        {
            try
            {
                if (!claimsIdentity.IsAuthenticated)
                {
                    return TimeZoneInfo.Local;
                }
                var tzId = claimsIdentity.FindFirstValue("TimeZone");
                return TimeZoneInfo.FindSystemTimeZoneById(tzId);
            }
            catch (Exception ex)
            {
                Logger.TraceError(
                    ex,
                    "ClaimsIdentity.Extensions.GetTimeZone error: there is a problem with timezone user '{0}'",
                    claimsIdentity.GetUserName());

                throw;
            }
        }

        /// <summary>
        /// from user tz
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static DateTime ConvertTimeToUtc(this DateTime dateTime, IPrincipal user)
        {
            try
            {
                return TimeZoneInfo.ConvertTimeToUtc(dateTime, (user.Identity as ClaimsIdentity).GetTimeZone());
            }
            catch (Exception ex)
            {
                Logger.TraceError(
                    ex,
                    "DateTime.Extensions.ConvertTimeFromUtc error: unable to convert date '{0}'",
                    dateTime);

                throw;
            }
        }

        /// <summary>
        /// to user tz
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static DateTime ConvertTimeFromUtc(this DateTime dateTime, IPrincipal user)
        {
            try
            {
                return TimeZoneInfo.ConvertTimeFromUtc(dateTime, (user.Identity as ClaimsIdentity).GetTimeZone());
            }
            catch (Exception ex)
            {
                Logger.TraceError(
                    ex,
                    "DateTime.Extensions.ConvertTimeFromUtc error: unable to convert date '{0}'",
                    dateTime);

                throw;
            }
        }
    }
}