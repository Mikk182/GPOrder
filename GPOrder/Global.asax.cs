using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using GPOrder.Helpers;

namespace GPOrder
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ControllerBuilder.Current.SetControllerFactory(typeof(LocalizationControllerFactory));
        }

        /// <summary>
        /// seul moyen que j'ai trouvé pour injecter la culture de l'utilisateur dans le context...
        /// TODO: upgrade to NET Core!
        /// </summary>
        public class LocalizationControllerFactory : DefaultControllerFactory
        {
            public override IController CreateController(RequestContext requestContext, string controllerName)
            {
                var controller = base.CreateController(requestContext, controllerName);

                var principal = requestContext.HttpContext.User.Identity as ClaimsIdentity;
                // si l'utilisateur est logué on set CurrentUICulture à partir de son choix
                if (principal != null && principal.IsAuthenticated)
                {
                    var userCulture = CultureInfo.GetCultureInfo(principal.GetUiCulture());
                    Thread.CurrentThread.CurrentUICulture = userCulture;
                }
                else
                {
                    // sinon on essaye de déterminer sa culture à partir de la requête (navigateur)
                    var userLanguage = requestContext.HttpContext.Request.UserLanguages;
                    if (userLanguage != null && userLanguage.Any())
                    {
                        try
                        {
                            var userCulture = CultureInfo.GetCultureInfo(userLanguage.First());
                            Thread.CurrentThread.CurrentUICulture = userCulture;
                        }
                        catch (CultureNotFoundException ex)
                        {
                            Logger.TraceError(ex, "[LocalizationControllerFactory] culture not found for : {0}", userLanguage.First());
                        }
                    }
                }

                return controller;
            }
        }
    }
}
