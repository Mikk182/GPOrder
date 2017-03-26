using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using GPOrder.Models;

namespace GPOrder.ValidationHelpers
{
    public class ShopPictureValidation
    {
        public void Validate(ShopPicture sp, ModelStateDictionary modelState)
        {
        }

        public void Validate(HttpPostedFileBase file, ModelStateDictionary modelState)
        {
            if (file.ContentLength > Convert.ToDouble(WebConfigurationManager.AppSettings["maxRequestLength"]))
                modelState.AddModelError("upload", "File max size is 5Mo.");

            if (!file.ContentType.Contains("image"))
            {
                modelState.AddModelError("upload", "You must select a picture file (jpeg, jpg, png, bmp, gif).");
                return;
            }

            var formats = new[] { ".jpeg", ".jpg", ".png", "bmp", ".gif" };
            if (!formats.Any(f => file.FileName.EndsWith(f, StringComparison.OrdinalIgnoreCase)))
            {
                modelState.AddModelError("upload", "You must select a picture file (jpeg, jpg, png, bmp, gif).");
                return;
            }

            try
            {
                var allowedFormats = new[]
                {
                    ImageFormat.Jpeg,
                    ImageFormat.Png,
                    ImageFormat.Gif,
                    ImageFormat.Bmp
                };

                using (var img = Image.FromStream(file.InputStream))
                {
                    if (!allowedFormats.Contains(img.RawFormat))
                        modelState.AddModelError("upload", "Busted ! You tried to cheat ! You must select a picture file (jpeg, jpg, png, bmp, gif).");
                }
            }
            catch (Exception e)
            {
                modelState.AddModelError("upload", "Busted ! You tried to cheat ! You must select a picture file (jpeg, jpg, png, bmp, gif).");
            }
        }
    }
}