using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.Common;
using LPWeb.LP_Service;
using LPWeb.Model;
using Utilities;
using LPWeb.Layouts.LPWeb.Common;


public partial class CompanyPipelineViewLoansView : BasePage
{
    LPWeb.BLL.CompanyLoanPointFields bll = new LPWeb.BLL.CompanyLoanPointFields();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindList();
        }
    }

    public void BindList()
    {
        var dt = bll.GetCompanyLoanPointFieldsInfo();

        gridList.DataSource = dt;
        gridList.DataBind();

        var oldData = "";
        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                if (!string.IsNullOrEmpty(oldData))
                {
                    oldData = oldData + ",";
                }
                oldData = oldData + dr["PointFieldId"].ToString();
            }
        }

        hidOldPointFieldID.Value = oldData;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        var oldData = hidOldPointFieldID.Value.Trim(); // 旧的fieldid
        var data = hidData.Value.Trim();//new list  may be Contains old fieldID

        try
        {
            if (!string.IsNullOrEmpty(oldData))
            {
                bll.DeleteList(oldData);
            }
            if (string.IsNullOrEmpty(data))
            {
                BindList();
                return;
            }

            var list = data.Replace("pid=", "").Replace("heading=", "").Split(';').ToList();   //"pid=" + pid + ",heading=" + heading;




            foreach (string item in list)
            {
                var kv = item.Split(',').ToList();

                if (kv.Count > 0)
                {
                    CompanyLoanPointFields model = new CompanyLoanPointFields();

                    model.PointFieldId = Convert.ToInt32(kv.FirstOrDefault());
                    model.Heading = kv.LastOrDefault().ToString();

                    bll.Add(model);
                }
            }
            string msg = "Saved successfully.";
            PageCommon.WriteJsEnd(this, msg, PageCommon.Js_RefreshSelf);
        }
        catch (Exception ex)
        {
            string msg = "Error:" + ex.Message;
            PageCommon.WriteJsEnd(this, msg, PageCommon.Js_RefreshSelf);
        }
    }
}

