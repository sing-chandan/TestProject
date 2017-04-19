using System.Web;
using System.Web.Optimization;

namespace LiveMonitoringWeb
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));

            bundles.Add(new StyleBundle("~/bundles/lightbox").Include(
               "~/Content/css/lightbox.css"
                ));

            bundles.Add(new StyleBundle("~/bundles/devCss").Include(
               "~/dev-release/lib/ionicons.css",
               "~/Content/css/angular-datepicker.min.css",
    "~/dev-release/lib/angular-toastr.css",
    "~/dev-release/lib/animate.css",
    "~/dev-release/lib/bootstrap.css",
    "~/dev-release/lib/bootstrap-select.css",
    "~/dev-release/lib/bootstrap-switch.css",
    "~/dev-release/lib/bootstrap-tagsinput.css",
    "~/dev-release/lib/font-awesome.css",
    "~/dev-release/lib/fullcalendar.css",
    "~/dev-release/lib/leaflet.css",
    "~/dev-release/lib/angular-progress-button-styles.min.css",
    "~/dev-release/lib/chartist.min.css",
    "~/dev-release/lib/morris.css",
    "~/dev-release/lib/ion.rangeSlider.css",
    "~/dev-release/lib/ion.rangeSlider.skinFlat.css",
    "~/dev-release/lib/textAngular.css",
    "~/dev-release/lib/xeditable.css",
    "~/dev-release/lib/style.css",
    "~/dev-release/lib/select.css",
    "~/dev-release/app/main.css",
    "~/Content/css/lightbox.css"
    
               ));

            bundles.Add(new ScriptBundle("~/bundles/Projectjs").Include(
                "~/dev-release/app/theme/theme.module.js",
                "~/dev-release/app/pages/pages.module.js",
"~/dev-release/app/theme/components/components.module.js",
"~/dev-release/app/theme/inputs/inputs.module.js",
"~/dev-release/app/pages/admin/admin.module.js",

"~/dev-release/app/pages/reports/reports.module.js",
"~/dev-release/app/pages/admin/admin.module.js",


"~/dev-release/app/pages/dashboard/dashboard.module.js",




"~/dev-release/app/pages/tables/tables.module.js",
"~/dev-release/app/app.js",
"~/dev-release/app/theme/theme.config.js",
"~/dev-release/app/theme/theme.configProvider.js",
"~/dev-release/app/theme/theme.constants.js",
"~/dev-release/app/theme/theme.run.js",
"~/dev-release/app/theme/theme.service.js",
"~/dev-release/app/theme/components/toastrLibConfig.js",
"~/dev-release/app/theme/directives/animatedChange.js",
"~/dev-release/app/theme/directives/autoExpand.js",
"~/dev-release/app/theme/directives/autoFocus.js",
"~/dev-release/app/theme/directives/includeWithScope.js",
"~/dev-release/app/theme/directives/ionSlider.js",
"~/dev-release/app/theme/directives/ngFileSelect.js",
"~/dev-release/app/theme/directives/scrollPosition.js",
"~/dev-release/app/theme/directives/trackWidth.js",
"~/dev-release/app/theme/directives/zoomIn.js",
"~/dev-release/app/theme/services/baProgressModal.js",
"~/dev-release/app/theme/services/baUtil.js",
"~/dev-release/app/theme/services/fileReader.js",
"~/dev-release/app/theme/services/preloader.js",
"~/dev-release/app/theme/services/stopableInterval.js",
"~/dev-release/app/pages/profile/ProfileModalCtrl.js",
"~/dev-release/app/pages/profile/ProfilePageCtrl.js",

"~/dev-release/app/theme/components/backTop/backTop.directive.js",
"~/dev-release/app/theme/components/baSidebar/baSidebar.directive.js",
"~/dev-release/app/theme/components/baSidebar/baSidebar.service.js",
"~/dev-release/app/theme/components/baSidebar/BaSidebarCtrl.js",
"~/dev-release/app/theme/components/baSidebar/baSidebarHelpers.directive.js",
"~/dev-release/app/theme/components/baPanel/baPanel.directive.js",
"~/dev-release/app/theme/components/baPanel/baPanel.service.js",
"~/dev-release/app/theme/components/baPanel/baPanelBlur.directive.js",
"~/dev-release/app/theme/components/baPanel/baPanelBlurHelper.service.js",
"~/dev-release/app/theme/components/baPanel/baPanelSelf.directive.js",
"~/dev-release/app/theme/components/contentTop/contentTop.directive.js",
"~/dev-release/app/theme/components/msgCenter/msgCenter.directive.js",
"~/dev-release/app/theme/components/msgCenter/MsgCenterCtrl.js",
"~/dev-release/app/theme/components/pageTop/pageTop.directive.js",
"~/dev-release/app/theme/components/baWizard/baWizard.directive.js",
"~/dev-release/app/theme/components/baWizard/baWizardCtrl.js",
"~/dev-release/app/theme/components/baWizard/baWizardStep.directive.js",
"~/dev-release/app/theme/components/progressBarRound/progressBarRound.directive.js",
"~/dev-release/app/theme/components/widgets/widgets.directive.js",
"~/dev-release/app/theme/filters/image/appImage.js",
"~/dev-release/app/theme/filters/image/kameleonImg.js",
"~/dev-release/app/theme/filters/image/profilePicture.js",
"~/dev-release/app/theme/filters/text/removeHtml.js",
"~/dev-release/app/theme/inputs/baSwitcher/baSwitcher.js",

"~/dev-release/app/pages/topFilter/topFilter.directive.js",
"~/dev-release/app/pages/topFilter/TopFilterCtrl.js",


"~/dev-release/app/pages/dashboard/trafficChart/trafficChart.directive.js",
"~/dev-release/app/pages/dashboard/trafficChart/TrafficChartCtrl.js",
"~/dev-release/app/pages/dashboard/topappChart/topappChart.directive.js",
"~/dev-release/app/pages/dashboard/topappChart/TopappChartCtrl.js",

"~/dev-release/app/pages/dashboard/keyboardActivity/keyboardActivity.directive.js",
"~/dev-release/app/pages/dashboard/keyboardActivity/keyboardActivityCtrl.js",

"~/dev-release/app/pages/dashboard/clipboardActivity/clipboardActivity.directive.js",
"~/dev-release/app/pages/dashboard/clipboardActivity/clipboardActivityCtrl.js",

"~/dev-release/app/pages/dashboard/screenshot/screenshot.directive.js",
"~/dev-release/app/pages/dashboard/screenshot/screenshotCtrl.js",

"~/dev-release/app/theme/components/backTop/lib/jquery.backTop.min.js",


"~/dev-release/app/pages/admin/category/CategoryCtrl.js",
"~/dev-release/app/pages/admin/configuration/ConfigurationCtrl.js",
    "~/dev-release/app/pages/admin/createGroup/CreateGroupCtrl.js",
        "~/dev-release/app/pages/admin/createUser/CreateUserCtrl.js",
        "~/dev-release/app/pages/admin/myProfile/MyProfileCtrl.js",
        "~/dev-release/app/pages/admin/scheduleReport/ScheduleReportCtrl.js",
        "~/dev-release/app/pages/admin/subCategory/SubCategoryCtrl.js",
        "~/dev-release/app/pages/admin/userPermission/UserPermissionCtrl.js",


"~/dev-release/app/pages/reports/machinedetail/MachineDetailCtrl.js",
"~/dev-release/app/pages/reports/browserdetails/BrowserDetailsCtrl.js",
"~/dev-release/app/pages/reports/keylogger/KeyloggerCtrl.js",
"~/dev-release/app/pages/reports/projectsummary/ProjectSummaryCtrl.js",
"~/dev-release/app/pages/reports/idledetails/IdleDetailsCtrl.js",
"~/dev-release/app/pages/reports/appdetails/AppdetailsCtrl.js",
"~/dev-release/app/pages/reports/sitesdetails/SitesDetailsCtrl.js",
"~/dev-release/app/pages/reports/useridle/UserIdleCtrl.js",
"~/dev-release/app/pages/reports/productivity/ProductivityCtrl.js",
"~/dev-release/app/pages/tables/TablesPageCtrl.js",
"~/dev-release/app/theme/components/pageTop/PageTopCtrl.js",
"~/dev-release/app/pages/reports/screenshot/ScreenShotCtrl.js"



                ));


            bundles.Add(new ScriptBundle("~/bundles/shared").Include(
            "~/Views/Shared/application-configuration.js",
             "~/Scripts/lightbox.js"
         ));
        }
    }
}