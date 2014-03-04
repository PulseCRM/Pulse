using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.BLL;
using LPWeb.Common;
using Utilities;
using LPWeb.Layouts.LPWeb.Common;
using System.Data.SqlClient;
using System.Data;



    /// <summary>
    /// Email template list
    /// author: duanlijun
    /// date: 2012-09-25
    /// </summary>
public partial class LoanInfoTab : BasePage
    {
        ProspectEmployment pe = new ProspectEmployment();
        ProspectIncome pi = new ProspectIncome();
       
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 添加Start Date→Year

            this.ddlStartYear.Items.Add(new ListItem("year", ""));
            this.ddlEndYear.Items.Add(new ListItem("year", ""));

            DateTime saveNow = DateTime.Now;

            int iBeginYear = saveNow.Year;
            for (int i = 0; i < 41; i++)
            {
                int iNextYear = iBeginYear - i;
                this.ddlStartYear.Items.Add(iNextYear.ToString());
                this.ddlEndYear.Items.Add(iNextYear.ToString());
            }

            #endregion

            //Edit_HasLoanId_NoContactId

           

         


            //更新
            

            pe.UpdateProspectEmploymentAndProspectIncome(1, txtEmployer.Text.Trim(), ddlStartMonth.SelectedValue, ddlDependants.SelectedValue, ddlStartYear.SelectedValue, txtTP.Text.Trim(), ddlEndDate.SelectedValue, txtMonthlySalary.Text.Trim(), ddlEndYear.SelectedValue, txtProfession.Text.Trim(), txtYearsInField.Text.Trim());

            //第一种情况纯创建Create_NoContactId_NoLoanId
            #region
            //ContactId值 就是在插入到Contacts 获得的最新id

            pe.InsertProspectEmploymentAndProspectIncome(1, txtEmployer.Text.Trim(), ddlStartMonth.SelectedValue, ddlDependants.SelectedValue, ddlStartYear.SelectedValue, txtTP.Text.Trim(), ddlEndDate.SelectedValue, txtMonthlySalary.Text.Trim(), ddlEndYear.SelectedValue, txtProfession.Text.Trim(), txtYearsInField.Text.Trim());

            #endregion

            //第二种情况Create_HasContactId_NoLoan
            //获得数据 显示在页面上 

            #region 得到页面数据 仍然创建 使用ContactId值
            Get(2);
            pe.InsertProspectEmploymentAndProspectIncome(1, txtEmployer.Text.Trim(), ddlStartMonth.SelectedValue, ddlDependants.SelectedValue, ddlStartYear.SelectedValue, txtTP.Text.Trim(), ddlEndDate.SelectedValue, txtMonthlySalary.Text.Trim(), ddlEndYear.SelectedValue, txtProfession.Text.Trim(), txtYearsInField.Text.Trim());

            #endregion

            //第三种情况

            Get(2);

            pe.UpdateProspectEmploymentAndProspectIncome(1, txtEmployer.Text.Trim(), ddlStartMonth.SelectedValue, ddlDependants.SelectedValue, ddlStartYear.SelectedValue, txtTP.Text.Trim(), ddlEndDate.SelectedValue, txtMonthlySalary.Text.Trim(), ddlEndYear.SelectedValue, txtProfession.Text.Trim(), txtYearsInField.Text.Trim());

            //第四种情况

            Get(2);

            pe.UpdateProspectEmploymentAndProspectIncome(1, txtEmployer.Text.Trim(), ddlStartMonth.SelectedValue, ddlDependants.SelectedValue, ddlStartYear.SelectedValue, txtTP.Text.Trim(), ddlEndDate.SelectedValue, txtMonthlySalary.Text.Trim(), ddlEndYear.SelectedValue, txtProfession.Text.Trim(), txtYearsInField.Text.Trim());

        }

        public void Get(int iBorrowerID)
        {
            #region 得到页面数据
            //int? iBorrowerID = 2;

            DataTable dtProspectEmployment = pe.GetProspectEmployment((int)iBorrowerID);

            if (dtProspectEmployment.Rows.Count > 0)
            {
                txtEmployer.Text = dtProspectEmployment.Rows[0]["CompanyName"].ToString();
                ddlStartMonth.SelectedValue = dtProspectEmployment.Rows[0]["StartMonth"].ToString();
                string sSelfEmployed = dtProspectEmployment.Rows[0]["SelfEmployed"].ToString();
                if (sSelfEmployed == "True")
                {
                    this.ddlDependants.SelectedValue = "Yes";
                }
                else if (sSelfEmployed == "False")
                {
                    this.ddlDependants.SelectedValue = "No";
                }

                ddlStartYear.SelectedValue = dtProspectEmployment.Rows[0]["StartYear"].ToString();
                txtTP.Text = dtProspectEmployment.Rows[0]["Position"].ToString();
                ddlEndDate.SelectedValue = dtProspectEmployment.Rows[0]["EndMonth"].ToString();
                ddlEndYear.SelectedValue = dtProspectEmployment.Rows[0]["EndYear"].ToString();
                txtProfession.Text = dtProspectEmployment.Rows[0]["BusinessType"].ToString();
                txtYearsInField.Text = dtProspectEmployment.Rows[0]["YearsOnWork"].ToString();
            }

            DataTable dtProspectIncome = pi.GetProspectIncome((int)iBorrowerID);

            if (dtProspectEmployment.Rows.Count > 0)
            {
                txtMonthlySalary.Text = dtProspectEmployment.Rows[0]["Salary"].ToString();

            }
            #endregion
        }


     


     

       
    }

