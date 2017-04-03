using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using GPOrder.Models;

namespace GPOrder.Helpers
{
    public static partial class Extensions
    {
        public static void CheckCultureAndTimeZone(this ChangeCultureViewModel model, ModelStateDictionary modelState)
        {
            if (CultureInfo.GetCultures(CultureTypes.AllCultures).All(c => c.Name != model.UiCulture))
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