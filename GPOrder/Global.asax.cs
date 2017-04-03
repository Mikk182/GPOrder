using System.Globalization;
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
        /// </summary>
        public class LocalizationControllerFactory : DefaultControllerFactory
        {
            public override IController CreateController(RequestContext requestContext, string controllerName)
            {
                var controller = base.CreateController(requestContext, controllerName);

                var principal = requestContext.HttpContext.User.Identity as ClaimsIdentity;
                if (principal != null && principal.IsAuthenticated)
                {
                    var userCulture = CultureInfo.GetCultureInfo(principal.GetUiCulture());
                    Thread.CurrentThread.CurrentUICulture = userCulture;
                }

                return controller;
            }
        }
    }
}
