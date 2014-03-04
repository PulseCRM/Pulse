using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// LoanRules 的摘要说明。
	/// </summary>
	public class LoanRules
	{
		private readonly LPWeb.DAL.LoanRules dal=new LPWeb.DAL.LoanRules();
		public LoanRules()
        { }
        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.LoanRules model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(LPWeb.Model.LoanRules model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int LoanRuleId)
        {

            dal.Delete(LoanRuleId);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.LoanRules GetModel(int LoanRuleId)
        {

            return dal.GetModel(LoanRuleId);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        }
        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            return dal.GetList(Top, strWhere, filedOrder);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<LPWeb.Model.LoanRules> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<LPWeb.Model.LoanRules> DataTableToList(DataTable dt)
        {
            List<LPWeb.Model.LoanRules> modelList = new List<LPWeb.Model.LoanRules>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LPWeb.Model.LoanRules model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LPWeb.Model.LoanRules();
                    if (dt.Rows[n]["LoanRuleId"].ToString() != "")
                    {
                        model.LoanRuleId = int.Parse(dt.Rows[n]["LoanRuleId"].ToString());
                    }
                    if (dt.Rows[n]["Fileid"].ToString() != "")
                    {
                        model.Fileid = int.Parse(dt.Rows[n]["Fileid"].ToString());
                    }
                    if (dt.Rows[n]["RuleGroupId"].ToString() != "")
                    {
                        model.RuleGroupId = int.Parse(dt.Rows[n]["RuleGroupId"].ToString());
                    }
                    if (dt.Rows[n]["RuleId"].ToString() != "")
                    {
                        model.RuleId = int.Parse(dt.Rows[n]["RuleId"].ToString());
                    }
                    if (dt.Rows[n]["Applied"].ToString() != "")
                    {
                        model.Applied = DateTime.Parse(dt.Rows[n]["Applied"].ToString());
                    }
                    if (dt.Rows[n]["AppliedBy"].ToString() != "")
                    {
                        model.AppliedBy = int.Parse(dt.Rows[n]["AppliedBy"].ToString());
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(int PageSize, int PageIndex, string strWhere)
        {
            return dal.GetList(PageSize, PageIndex, strWhere);
        }

        #endregion  成员方法

        #region neo

        /// <summary>
        /// add rule to loan
        /// neo 2011-01-14
        /// </summary>
        /// <param name="RuleIDs"></param>
        /// <param name="iLoanID"></param>
        /// <param name="iAppliedByID"></param>
        public void AddRuleToLoan(string[] RuleIDs, int iLoanID, int iAppliedByID)
        {
            dal.AddRuleToLoanBase(RuleIDs, iLoanID, iAppliedByID);
        }

        /// <summary>
        /// add rule group to loan
        /// neo 2011-01-15
        /// </summary>
        /// <param name="RuleGroupIDs"></param>
        /// <param name="iLoanID"></param>
        /// <param name="iAppliedByID"></param>
        public void AddRuleGroupToLoan(string[] RuleGroupIDs, int iLoanID, int iAppliedByID)
        {
            dal.AddRuleGroupToLoanBase(RuleGroupIDs, iLoanID, iAppliedByID);
        }

        /// <summary>
        /// 从Loan中移除Rule
        /// neo 2011-01-14
        /// </summary>
        /// <param name="iLoanID"></param>
        /// <param name="sLoanRuleIDs"></param>
        public void RemoveRuleFromLoan(int iLoanID, string sLoanRuleIDs)
        {
            dal.RemoveRuleFromLoanBase(iLoanID, sLoanRuleIDs);
        }

        /// <summary>
        /// enable/disable rule in loan
        /// neo 2011-01-15
        /// </summary>
        /// <param name="sLoanRuleIDs"></param>
        /// <param name="bEnabled"></param>
        public void Enable(string sLoanRuleIDs, bool bEnabled)
        {
            dal.EnableBase(sLoanRuleIDs, bEnabled);
        }

        #endregion
	}
}

