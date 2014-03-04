using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;

namespace LPWeb.Features.DeployMasterpage
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("13e6bf1b-b57c-4229-974e-85eaa57464cc")]
    public class DeployMasterpageEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        //public override void FeatureActivated(SPFeatureReceiverProperties properties)
        //{
        //    using (SPWeb curWeb = (SPWeb)properties.Feature.Parent)
        //    {
        //        using (SPWeb rotWeb = curWeb.Site.RootWeb)
        //        {
        //            // current full master url
        //            Uri masterUri = new Uri(rotWeb.Url + "/_catalogs/masterpage/LPWeb.master");

        //            // Master page used by all forms and pages on the site that are NOT publishing pages
        //            rotWeb.MasterUrl = masterUri.AbsolutePath;

        //            // Master page used by publishing page on the site
        //            rotWeb.CustomMasterUrl = masterUri.AbsolutePath;
        //            rotWeb.Update();
        //        }
        //    }
        //}


        // Uncomment the method below to handle the event raised before a feature is deactivated.

        //public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        //{
        //    using (SPWeb curWeb = (SPWeb)properties.Feature.Parent)
        //    {
        //        using (SPWeb rotWeb = curWeb.Site.RootWeb)
        //        {
        //            // current full master url
        //            Uri masterUri = new Uri(rotWeb.Url + "/_catalogs/masterpage/v4.master");

        //            // Master page used by all forms and pages on the site that are NOT publishing pages
        //            rotWeb.MasterUrl = masterUri.AbsolutePath;

        //            // Master page used by publishing page on the site
        //            rotWeb.CustomMasterUrl = masterUri.AbsolutePath;
        //            rotWeb.Update();
        //        }
        //    }
        //}


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
