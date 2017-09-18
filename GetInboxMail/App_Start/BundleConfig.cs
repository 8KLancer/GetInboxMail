using System.Web;
using System.Web.Optimization;

namespace GetInboxMail
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Используйте версию Modernizr для разработчиков, чтобы учиться работать. Когда вы будете готовы перейти к работе,
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            //bundles.Add(new ScriptBundle("~bundles/jqGrid_js").Include(
            //    "~/Scripts/Guriddo_jqGrid_JS_5.2.1/js/*").Include(
            //    "~/Scripts/Guriddo_jqGrid_JS_5.2.1/plugins/*").Include(
            //    "~/Scripts/Guriddo_jqGrid_JS_5.2.1/src/*"
            //    ));

            //bundles.Add(new StyleBundle("~bundles/jqGrid_css").Include(
            //    "~/Scripts/Guriddo_jqGrid_JS_5.2.1/css/*").Include(
            //    ));
        }
    }
}
