using System.Web;
using System.Web.Optimization;

namespace ArticoleCalarie.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include(
                        "~/Scripts/jquery-ui.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/style.css",
                      "~/Content/animate.min.css",
                      "~/Content/chosen.min.css",
                      "~/Content/customs-css.css",
                      "~/Content/flaticon.min.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/jquery-ui.min.css",
                      "~/Content/jquery.scrollbar.min.css",
                      "~/Content/magnific-popup.min.css",
                      "~/Content/owl.carousel.min.css",
                      "~/Content/timecircles.css"));

            bundles.Add(new StyleBundle("~/bundles/external").Include(
                      "~/Scripts/owl.carousel.min.js",
                      "~/Scripts/owl.thumbs.min.js",
                      "~/Scripts/magnific-popup.min.js",
                      "~/Scripts/mobilemenu.min.js",
                      "~/Scripts/imagesloaded.pkgd.min.js",
                      "~/Scripts/isotope.pkgd.min.js",
                      "~/Scripts/masonry.min.js",
                      "~/Scripts/jquery.plugin-countdown.min.js",
                      //"~/Scripts/jquery-countdown.min.js",
                      "~/Scripts/jquery-ui.min.js",
                      "~/Scripts/chosen.min.js",
                      "~/Scripts/timecircles.js",
                      "~/Scripts/jquery.scrollbar.min.js",
                      "~/Scripts/frontend.js"));

            //custom made scripts
            bundles.Add(new ScriptBundle("~/bundles/product-add").Include(
                    "~/Scripts/product-add.js"));
        }
    }
}
