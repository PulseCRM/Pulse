using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
using Utilities;

namespace LPWeb.Layouts.LPWeb.Pipeline
{
    public partial class SpecifyPointFilePopup : LayoutsPageBase
    {
        public int BranchID = 0;
        public int FileId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            bool bIsValid = PageCommon.ValidateQueryString(this, "BranchId", QueryStringType.ID);
            if (bIsValid == false)
            {
                PageCommon.RegisterJsMsg(this.Page, "Missing required query string", "Cancel();");
                return;
            }

            BranchID = Convert.ToInt32(this.Request.QueryString["BranchId"]);
            if (!string.IsNullOrEmpty(Request.QueryString["FileId"]))
            {
                FileId = Convert.ToInt32(Request.QueryString["FileId"]);
            }

            if (!IsPostBack)
            {
                BindData();
            }
        }

        public void BindData()
        {
            BLL.PointFolders bllPF = new BLL.PointFolders();

            var list = bllPF.GetList(" BranchId= " + BranchID.ToString() + " AND Enabled=1 AND LoanStatus=6 ");
            ddlFolder.DataSource = list;
            ddlFolder.DataTextField = "Name";
            ddlFolder.DataValueField = "FolderId";
            ddlFolder.DataBind();

            if (FileId != 0)
            {
                BLL.PointFiles bllPointfiles = new BLL.PointFiles();
                Model.PointFiles model = bllPointfiles.GetModel(FileId);
                if (model != null)
                {
                    txbFilename.Text = model.Name;
                    ddlFolder.SelectedValue = model.FolderId.ToString();
                }
                else
                {
                    PageCommon.RegisterJsMsg(this.Page, "Missing required query string.", "Cancel();");
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var filename = txbFilename.Text.Trim();

            if (string.IsNullOrEmpty(filename)
                ||(!filename.ToUpper().EndsWith(".PRS") && !filename.ToUpper().EndsWith(".BRW")))
            {
                PageCommon.AlertMsg(this.Page, "You have to specify a valid Point filename that ends with \".PRS\" or \".BRW\".");
                return;
            }

            if (filename.ToUpper().EndsWith(".PRS"))
            {
                filename = "\\PROSPECT\\" + filename;
            }
            else if (filename.ToUpper().EndsWith(".BRW"))
            {
                filename = "\\BORROWER\\" + filename;
            }
            else
            {
                return;
            }


            BLL.PointFiles bllPointfiles = new BLL.PointFiles();

            Model.PointFiles modelPointfile = new Model.PointFiles();

            if (FileId == 0)
            {
                modelPointfile.FolderId = Convert.ToInt32(ddlFolder.SelectedValue);
                modelPointfile.Name = filename;

                bllPointfiles.Add(modelPointfile);
            }
            else
            {
                modelPointfile = bllPointfiles.GetModel(FileId);

                if (modelPointfile == null)
                {
                    return;
                }
                modelPointfile.FolderId = Convert.ToInt32(ddlFolder.SelectedValue);
                modelPointfile.Name = filename;

                bllPointfiles.UpdateBase(modelPointfile);
            }
            PageCommon.RegisterJsMsg(this.Page, "", " Cancel();");
        }
    }
}
