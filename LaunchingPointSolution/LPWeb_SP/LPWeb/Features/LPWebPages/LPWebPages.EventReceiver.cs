using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.WebPartPages;
using WebPart = Microsoft.SharePoint.WebPartPages.WebPart;

namespace LPWeb.Features.LPWebPages
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("11d98b6e-43e1-4f6e-b5f1-b1b2bb078b16")]
    public class LPWebPagesEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            using (var site = (SPSite)properties.Feature.Parent)
            {
                using (var web = site.RootWeb)
                {
                    try
                    {
                        //web.AllowUnsafeUpdates = true;
                        var webPartManager = web.GetLimitedWebPartManager("LPWebPages/CompanyCalendar.aspx", PersonalizationScope.Shared);
                        var list = web.Lists["Calendar"];
                        if (list != null)
                        {
                            var listViewWebPart = new ListViewWebPart
                            {
                                ListName = list.ID.ToString("B").ToUpper(),
                                TitleUrl = list.DefaultViewUrl,
                                ViewType = ViewType.Calendar,
                                Title = "Company Calendar",
                                ViewGuid = string.Empty
                            };
                            //listViewWebPart.ID = "";
                            webPartManager.AddWebPart(listViewWebPart, "FullPage", 0);
                            web.Update();
                        }
                    }
                    catch (Exception ex)
                    {
                        SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory("LPWeb", TraceSeverity.Unexpected, EventSeverity.Error),
                            TraceSeverity.Unexpected, string.Format("LPWebPages.LPWebPagesEventReceiver.FeatureActivated error: {0}", ex.Message), ex.StackTrace);
                    }
                }
            }
        }


        // Uncomment the method below to handle the event raised before a feature is deactivated.

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            using (var site = (SPSite)properties.Feature.Parent)
            {
                using (var web = site.RootWeb)
                {
                    try
                    {
                        //web.AllowUnsafeUpdates = true;
                        SPLimitedWebPartManager webPartManager =
                            web.GetLimitedWebPartManager("LPWebPages/CompanyCalendar.aspx", PersonalizationScope.Shared);

                        //Retrive the webpart and remove
                        IList<WebPart> listFormWebParts = (from wp in webPartManager.WebParts.Cast<WebPart>()
                                                           where string.Compare(wp.Title, "Company Calendar", true) == 0
                                                           select wp).ToList();

                        //Check if there are any web parts found
                        if (listFormWebParts != null && listFormWebParts.Count > 0)
                        {
                            foreach (WebPart listFormWebPart in listFormWebParts)
                            {
                                webPartManager.DeleteWebPart(listFormWebPart);
                                web.Update();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory("LPWeb", TraceSeverity.Unexpected, EventSeverity.Error),
                            TraceSeverity.Unexpected, string.Format("LPWebPages.LPWebPagesEventReceiver.FeatureDeactivating error: {0}", ex.Message), ex.StackTrace);
                    }
                }
            }
        }


        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}
    }
}
