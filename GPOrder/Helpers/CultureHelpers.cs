using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using GPOrder.Models;

namespace GPOrder.Helpers
{
    public static partial class Extensions
    {
        public static IEnumerable<CultureInfo> GetAllCultures()
        {
            return CultureInfo.GetCultures(CultureTypes.NeutralCultures)
                .Where(c => !Equals(c, CultureInfo.InvariantCulture))
                .OrderBy(c => c.DisplayName);
        }

        public static IEnumerable<SelectListItem> GetAllCultures(this HtmlHelper htmlHelper)
        {
            return GetAllCultures()
                .Select(c => new SelectListItem
                {
                    Value = c.Name,
                    Text = c.DisplayName
                });
        }

        public static IEnumerable<SelectListItem> GetAllTimeZones(this HtmlHelper htmlHelper)
        {
            return TimeZoneInfo.GetSystemTimeZones()
                .Select(t => new SelectListItem
                {
                    Value = t.Id,
                    Text = t.DisplayName
                });
        }

        public static void CheckCultureAndTimeZone(this ChangeCultureViewModel model, ModelStateDictionary modelState)
        {
            if (GetAllCultures().All(c => c.Name != model.UiCulture))
            {
                modelState.AddModelError("UiCulture", "invalid culture");
            }

            if (TimeZoneInfo.GetSystemTimeZones().All(t => t.Id != model.TimeZone))
            {
                modelState.AddModelError("TimeZone", "invalid timeZone");
            }
        }
    }
}