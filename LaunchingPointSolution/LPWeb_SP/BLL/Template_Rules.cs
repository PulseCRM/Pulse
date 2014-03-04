using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
    /// <summary>
    /// Template_Rules 的摘要说明。
    /// </summary>
    public class Template_Rules
    {
        private readonly LPWeb.DAL.Template_Rules dal = new LPWeb.DAL.Template_Rules();
        public Template_Rules()
        { }

        /// <summary>
        /// get rule list for rule selection list
        /// </summary>
        public DataSet GetListForRuleSelection(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            return dal.GetListForRuleSelection(PageSize, PageIndex, strWhere, out count, orderName, orderType);
        }

        /// <summary>
        /// get rule current selected list for rule selection page
        /// </summary>
        public DataSet GetListOfCurrSelectedRule(List<string> listCurrIds)
        {
            return dal.GetListOfCurrSelectedRule(listCurrIds);
        }

        /// <summary>
        /// Disabled rule template
        /// </summary>
        /// <param name="RuleIDs"></param>
        public void DisableRuleTemplates(string RuleIDs)
        {
            dal.DisableRuleTemplates(RuleIDs);
        }

        public void EnableRuleTemplates(string RuleIDs)
        {
            dal.EnableRuleTemplates(RuleIDs);
        }

        /// <summary>
        /// Delete rule template
        /// </summary>
        /// <param name="RuleIDs"></param>
        public void DeleteRuleTemplate(string RuleIDs)
        {
            dal.DeleteRuleTemplate(RuleIDs);
        }

        /// <summary>
        /// get rule with alert email template info
        /// </summary>
        /// <param name="iRuleID"></param>
        /// <returns></returns>
        public DataTable GetRuleWithAlertEmailTpltInfo(int iRuleID)
        {
            return dal.GetRuleWithAlertEmailTpltInfo(iRuleID);
        }

        #region neo

        /// <summary>
        /// insert rule and conditions  ,Add: AutoCampaignId 
        /// neo 2011-01-12
        /// Modify: Alex 2011-08-01
        /// </summary>
        /// <param name="sName"></param>
        /// <param name="sDesc"></param>
        /// <param name="iAlertEmailTemplId"></param>
        /// <param name="bAckReq"></param>
        /// <param name="iRecomEmailTemplid"></param>
        /// <param name="sAdvFormula"></param>
        /// <param name="ConditionList"></param>
        /// <returns></returns>
        public int InsertRule(string sName, string sDesc, int iAlertEmailTemplId, bool bAckReq, int iRecomEmailTemplid, string sAdvFormula, Int16 iRuleScope, Int16 iLoanTarget, DataTable ConditionList, int AutoCampaignId)
        {
            return dal.InsertRuleBase(sName, sDesc, iAlertEmailTemplId, bAckReq, iRecomEmailTemplid, sAdvFormula, iRuleScope, iLoanTarget, ConditionList, AutoCampaignId);
        }

        /// <summary>
        /// insert rule and conditions
        /// neo 2011-01-12
        /// </summary>
        /// <param name="sName"></param>
        /// <param name="sDesc"></param>
        /// <param name="iAlertEmailTemplId"></param>
        /// <param name="bAckReq"></param>
        /// <param name="iRecomEmailTemplid"></param>
        /// <param name="sAdvFormula"></param>
        /// <param name="ConditionList"></param>
        /// <returns></returns>
        public int InsertRule(string sName, string sDesc, int iAlertEmailTemplId, bool bAckReq, int iRecomEmailTemplid, string sAdvFormula, Int16 iRuleScope, Int16 iLoanTarget, DataTable ConditionList)
        {
            return dal.InsertRuleBase(sName, sDesc, iAlertEmailTemplId, bAckReq, iRecomEmailTemplid, sAdvFormula, iRuleScope, iLoanTarget, ConditionList);
        }

        /// <summary>
        /// update rule and conditions
        /// neo 2011-01-12
        /// </summary>
        /// <param name="iRuleID"></param>
        /// <param name="sName"></param>
        /// <param name="sDesc"></param>
        /// <param name="bEnabled"></param>
        /// <param name="iAlertEmailTemplId"></param>
        /// <param name="bAckReq"></param>
        /// <param name="iRecomEmailTemplid"></param>
        /// <param name="sAdvFormula"></param>
        /// <param name="ConditionList"></param>
        public void UpdateRule(int iRuleID, string sName, string sDesc, bool bEnabled, int iAlertEmailTemplId, bool bAckReq, int iRecomEmailTemplid, string sAdvFormula, Int16 iRuleScope, Int16 iLoanTarget, DataTable ConditionList)
        {
            dal.UpdateRuleBase(iRuleID, sName, sDesc, bEnabled, iAlertEmailTemplId, bAckReq, iRecomEmailTemplid, sAdvFormula, iRuleScope, iLoanTarget, ConditionList);
        }

        /// <summary>
        /// update rule and conditions,Add: AutoCampaignId 
        /// neo 2011-01-12
        /// Modify: Alex 2011-08-01
        /// </summary>
        /// <param name="iRuleID"></param>
        /// <param name="sName"></param>
        /// <param name="sDesc"></param>
        /// <param name="bEnabled"></param>
        /// <param name="iAlertEmailTemplId"></param>
        /// <param name="bAckReq"></param>
        /// <param name="iRecomEmailTemplid"></param>
        /// <param name="sAdvFormula"></param>
        /// <param name="ConditionList"></param>
        public void UpdateRule(int iRuleID, string sName, string sDesc, bool bEnabled, int iAlertEmailTemplId, bool bAckReq, int iRecomEmailTemplid, string sAdvFormula, Int16 iRuleScope, Int16 iLoanTarget, DataTable ConditionList, int AutoCampaignId)
        {
            dal.UpdateRuleBase(iRuleID, sName, sDesc, bEnabled, iAlertEmailTemplId, bAckReq, iRecomEmailTemplid, sAdvFormula, iRuleScope, iLoanTarget, ConditionList, AutoCampaignId);
        }

        /// <summary>
        /// get condition list
        /// neo 2011-01-12
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public DataTable GetConditionList(string sWhere)
        {
            return dal.GetConditionListBase(sWhere);
        }

        /// <summary>
        /// get condition list with PointFieldDesc info
        /// neo 2011-01-12
        /// </summary>
        /// <param name="iRuleID"></param>
        /// <returns></returns>
        public DataTable GetConditionList(int iRuleID)
        {
            return dal.GetConditionListBase(iRuleID);
        }

        /// <summary>
        /// 检查rule name是否存在
        /// neo 2011-01-12
        /// </summary>
        /// <param name="sRuleName"></param>
        /// <returns></returns>
        public bool IsExist_Create(string sRuleName)
        {
            return dal.IsExist_CreateBase(sRuleName);
        }

        /// <summary>
        /// 检查rule name是否存在
        /// neo 2011-01-12
        /// </summary>
        /// <param name="iRuleID"></param>
        /// <param name="sRuleName"></param>
        /// <returns></returns>
        public bool IsExist_Edit(int iRuleID, string sRuleName)
        {
            return dal.IsExist_EditBase(iRuleID, sRuleName);
        }

        /// <summary>
        /// get rule info
        /// neo 2011-01-12
        /// </summary>
        /// <param name="iRuleID"></param>
        /// <returns></returns>
        public DataTable GetRuleInfo(int iRuleID)
        {
            return dal.GetRuleInfoBase(iRuleID);
        }

        /// <summary>
        /// 检查是否被引用
        /// neo 2011-01-12
        /// </summary>
        /// <param name="iRuleID"></param>
        /// <returns></returns>
        public bool bIsRef(int iRuleID)
        {
            return dal.bIsRefBase(iRuleID);
        }

        /// <summary>
        /// delete rule
        /// neo 2011-01-12
        /// </summary>
        /// <param name="iRuleID"></param>
        public void DeleteRule(int iRuleID)
        {
            dal.DeleteRuleBase(iRuleID);
        }

        #region Company Global Rules

        /// <summary>
        /// get non-global(RuleScope=0) rule
        /// neo 2011-03-19
        /// </summary>
        /// <returns></returns>
        public DataTable GetNonGlobalRuleList()
        {
            return dal.GetNonGlobalRuleListBase();
        }

        /// <summary>
        /// add company global rule
        /// neo 2011-03-19
        /// </summary>
        /// <param name="sRuleIDs"></param>
        public void AddGlobalRules(string sRuleIDs)
        {
            dal.AddGlobalRulesBase(sRuleIDs);
        }

        #endregion

        #endregion

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.Template_Rules model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(LPWeb.Model.Template_Rules model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int RuleId)
        {

            dal.Delete(RuleId);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.Template_Rules GetModel(int RuleId)
        {

            return dal.GetModel(RuleId);
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
        public List<LPWeb.Model.Template_Rules> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<LPWeb.Model.Template_Rules> DataTableToList(DataTable dt)
        {
            List<LPWeb.Model.Template_Rules> modelList = new List<LPWeb.Model.Template_Rules>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LPWeb.Model.Template_Rules model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LPWeb.Model.Template_Rules();
                    if (dt.Rows[n]["RuleId"].ToString() != "")
                    {
                        model.RuleId = int.Parse(dt.Rows[n]["RuleId"].ToString());
                    }
                    model.Name = dt.Rows[n]["Name"].ToString();
                    model.desc = dt.Rows[n]["desc"].ToString();
                    if (dt.Rows[n]["Enabled"].ToString() != "")
                    {
                        if ((dt.Rows[n]["Enabled"].ToString() == "1") || (dt.Rows[n]["Enabled"].ToString().ToLower() == "true"))
                        {
                            model.Enabled = true;
                        }
                        else
                        {
                            model.Enabled = false;
                        }
                    }
                    if (dt.Rows[n]["AlertEmailTemplId"].ToString() != "")
                    {
                        model.AlertEmailTemplId = int.Parse(dt.Rows[n]["AlertEmailTemplId"].ToString());
                    }
                    if (dt.Rows[n]["AckReq"].ToString() != "")
                    {
                        if ((dt.Rows[n]["AckReq"].ToString() == "1") || (dt.Rows[n]["AckReq"].ToString().ToLower() == "true"))
                        {
                            model.AckReq = true;
                        }
                        else
                        {
                            model.AckReq = false;
                        }
                    }
                    if (dt.Rows[n]["RecomEmailTemplid"].ToString() != "")
                    {
                        model.RecomEmailTemplid = int.Parse(dt.Rows[n]["RecomEmailTemplid"].ToString());
                    }
                    model.AdvFormula = dt.Rows[n]["AdvFormula"].ToString();
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


        public DataSet GetCompanyRules(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            return dal.GetCompanyRules(PageSize, PageIndex, strWhere, out count, orderName, orderType);
        }
    }
}

