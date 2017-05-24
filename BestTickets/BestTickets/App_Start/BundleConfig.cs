using System.Web.Optimization;

namespace BestTickets
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/indexScripts").Include(
                        "~/Scripts/jquery-{version}.min.js",
                        "~/Scripts/jquery.unobtrusive-ajax.min.js",
                        "~/Scripts/Custom/tickets.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/datepicker").Include(
                "~/Scripts/modernizr-{version}.js",
                "~/Scripts/yepnope.{version}.js",
                "~/Scripts/Custom/IEDatepicker.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/respond.js"));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/Site.css"));

            BundleTable.EnableOptimizations = true;
        }
    }
}
