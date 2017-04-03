using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace GPOrder.Helpers
{
    public static partial class Extensions
    {
        public static IEnumerable<SelectListItem> GetAllCultures(this HtmlHelper htmlHelper)
        {
            return CultureInfo.GetCultures(CultureTypes.AllCultures)
                .OrderBy(c => c.NativeName)
                .Select(c => new SelectListItem
                {
                    Value = c.Name,
                    Text = c.NativeName,
                    Selected = Equals(c, CultureInfo.CurrentUICulture)
                });
        }

        public static IEnumerable<SelectListItem> GetAllTimeZones(this HtmlHelper htmlHelper)
        {
            return TimeZoneInfo.GetSystemTimeZones()
                .Select(t => new SelectListItem
                {
                    Value = t.Id,
                    Text = t.DisplayName,
                    Selected = Equals(t, TimeZoneInfo.Local)
                });
        }
    }
}