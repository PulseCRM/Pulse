using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;

namespace LPWeb.BLL
{
    /// <summary>
    /// 业务逻辑类Template_Workflow 的摘要说明。
    /// </summary>
    public class Template_Workflow
    {
        private readonly LPWeb.DAL.Template_Workflow dal = new LPWeb.DAL.Template_Workflow();
        public Template_Workflow()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.Template_Workflow model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(LPWeb.Model.Template_Workflow model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int WflTemplId)
        {

            dal.Delete(WflTemplId);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.Template_Workflow GetModel(int WflTemplId)
        {

            return dal.GetModel(WflTemplId);
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
        public List<LPWeb.Model.Template_Workflow> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<LPWeb.Model.Template_Workflow> DataTableToList(DataTable dt)
        {
            List<LPWeb.Model.Template_Workflow> modelList = new List<LPWeb.Model.Template_Workflow>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LPWeb.Model.Template_Workflow model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LPWeb.Model.Template_Workflow();
                    if (dt.Rows[n]["WflTemplId"].ToString() != "")
                    {
                        model.WflTemplId = int.Parse(dt.Rows[n]["WflTemplId"].ToString());
                    }
                    model.Name = dt.Rows[n]["Name"].ToString();
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
                    model.Desc = dt.Rows[n]["Desc"].ToString();
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

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetTemplateWorkflows(int PageSize, int PageIndex, string strWhere, out int recordCount, string orderName, int orderType)
        {
            return dal.GetTemplateWorkflows(PageSize, PageIndex, strWhere, out recordCount, orderName, orderType);
        }

        public void DisableWorkflowTemplates(string TWFIDs)
        {
            dal.DisableWorkflowTemplates(TWFIDs);
        }

        public void EnableWorkflowTemplates(string TWFIDs)
        {
            dal.EnableWorkflowTemplates(TWFIDs);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetTemplateWorkflowTasks(int PageSize, int PageIndex, string strWhere, out int recordCount, string orderName, int orderType)
        {
            return dal.GetTemplateWorkflowTasks(PageSize, PageIndex, strWhere, out recordCount, orderName, orderType);
        }

        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int WorkflowTemplateAdd(LPWeb.Model.Template_Workflow model)
        {
            return dal.WorkflowTemplateAdd(model);
        }

        /// <summary>
        ///  CLONE
        /// </summary>
        public int WorkflowTemplateClone(int OldWflTemplId, string TemplateName)
        {
            return dal.WorkflowTemplateClone(OldWflTemplId, TemplateName);
        }

        //public void WorkflowTemplateDelete(string WflTemplIds)
        //{
        //    dal.WorkflowTemplateDelete(WflTemplIds);
        //}
        public void WorkflowTemplateDelete(int WflTemplId)
        { 
            dal.WorkflowTemplateDelete(WflTemplId);
        }
        #region neo

        /// <summary>
        /// insert workflow template
        /// neo 2011-02-08
        /// </summary>
        /// <param name="sWorkflowTemplateName"></param>
        /// <param name="bEnabled"></param>
        /// <param name="sDesc"></param>
        /// <param name="sWorkflowType"></param>
        /// <param name="bDefault"></param>
        /// <param name="sCalculationMethod"></param>
        /// <param name="StageList"></param>
        public void InsertWorkflowTemplate(string sWorkflowTemplateName, bool bEnabled, string sDesc, string sWorkflowType, bool bDefault, string sCalculationMethod, DataTable StageList)
        {
            dal.InsertWorkflowTemplateBase(sWorkflowTemplateName, bEnabled, sDesc, sWorkflowType, bDefault, sCalculationMethod, StageList);
        }

        /// <summary>
        /// Clone workflow template for template details
        /// Rocky 2011-09-08
        /// </summary>
        /// <param name="sWorkflowTemplateName"></param>
        /// <param name="bEnabled"></param>
        /// <param name="sDesc"></param>
        /// <param name="sWorkflowType"></param>
        /// <param name="bDefault"></param>
        /// <param name="sCalculationMethod"></param>
        /// <param name="StageList"></param>
        public void CloneWorkflowTemplate(string sWorkflowTemplateName, bool bEnabled, string sDesc, string sWorkflowType, bool bDefault, string sCalculationMethod, DataTable StageList)
        {
            dal.CloneWorkflowTemplateBase(sWorkflowTemplateName, bEnabled, sDesc, sWorkflowType, bDefault, sCalculationMethod, StageList);
        }

        /// <summary>
        /// update workflow template
        /// neo 2011-02-11
        /// </summary>
        /// <param name="iWorkflowTemplateID"></param>
        /// <param name="sWorkflowTemplateName"></param>
        /// <param name="bEnabled"></param>
        /// <param name="sDesc"></param>
        /// <param name="sWorkflowType"></param>
        /// <param name="bDefault"></param>
        /// <param name="sCalculationMethod"></param>
        /// <param name="StageList"></param>
        /// <param name="sRemovedStageIDs"></param>
        public void UpdateWorkflowTemplate(int iWorkflowTemplateID, string sWorkflowTemplateName, bool bEnabled, string sDesc, string sWorkflowType, bool bDefault, string sCalculationMethod, DataTable StageList, string sRemovedStageIDs)
        {
            dal.UpdateWorkflowTemplateBase(iWorkflowTemplateID, sWorkflowTemplateName, bEnabled, sDesc, sWorkflowType, bDefault, sCalculationMethod, StageList, sRemovedStageIDs);
        }

        public void UpdateWorkflowType(int iWflTemplId, string sWorkflowType)
        {
            dal.UpdateWorkflowType(iWflTemplId, sWorkflowType);
        }

        /// <summary>
        /// 检查workflow template name是否存在
        /// neo 2011-02-08
        /// </summary>
        /// <param name="sRuleName"></param>
        /// <returns></returns>
        public bool IsExist_Create(string sWorkflowTemplateNameName)
        {
            return dal.IsExist_CreateBase(sWorkflowTemplateNameName);
        }

        /// <summary>
        /// 检查workflow template是否存在
        /// neo 2011-02-08
        /// </summary>
        /// <param name="iRuleID"></param>
        /// <param name="sRuleName"></param>
        /// <returns></returns>
        public bool IsExist_Edit(int iWorkflowTemplateID, string sWorkflowTemplateNameName)
        {
            return dal.IsExist_EditBase(iWorkflowTemplateID, sWorkflowTemplateNameName);
        }

        /// <summary>
        /// get default workflow template count
        /// neo 2011-02-08
        /// </summary>
        /// <param name="sWorkflowType"></param>
        /// <returns></returns>
        public int GetDefaultWflTemplateCount(string sWorkflowType)
        {
            return dal.GetDefaultWflTemplateCountBase(sWorkflowType);
        }

        /// <summary>
        /// get workflow stage list(Template_Wfl_Stages)
        /// neo 2011-02-09
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public DataTable GetWflStageList(string sWhere)
        {
            return dal.GetWflStageListBase(sWhere);
        }

        /// <summary>
        /// get workflow template info
        /// neo 2011-02-09
        /// </summary>
        /// <param name="iWorkflowTemplateID"></param>
        /// <returns></returns>
        public DataTable GetWorkflowTemplateInfo(int iWorkflowTemplateID)
        {
            return dal.GetWorkflowTemplateInfoBase(iWorkflowTemplateID);
        }

        /// <summary>
        /// get stage list of workflow tempalte (Template_Wfl_Stages)
        /// neo 2011-02-09
        /// </summary>
        /// <param name="iWorkflowTemplateID"></param>
        /// <returns></returns>
        public DataTable GetWflStageList(int iWorkflowTemplateID)
        {
            return dal.GetWflStageListBase(iWorkflowTemplateID);
        }

        /// <summary>
        /// get stage info of workflow tempalte (Template_Wfl_Stages)
        /// neo 2011-02-16
        /// </summary>
        /// <param name="iWflStageID"></param>
        /// <returns></returns>
        public DataTable GetWflStageInfo(int iWflStageID)
        {
            return dal.GetWflStageInfoBase(iWflStageID);
        }

        /// <summary>
        /// delete workflow template
        /// neo 2011-02-11
        /// </summary>
        /// <param name="iWorkflowTemplateID"></param>
        public void DeleteWorkflowTemplate(int iWorkflowTemplateID)
        {
            dal.DeleteWorkflowTemplateBase(iWorkflowTemplateID);
        }

        /// <summary>
        /// update workflow stage
        /// neo 2011-02-16
        /// </summary>
        /// <param name="iWflStageID"></param>
        /// <param name="sWflStageName"></param>
        /// <param name="iSeq"></param>
        /// <param name="bEnabled"></param>
        /// <param name="iDaysFromEstClose"></param>
        /// <param name="iDaysFromCreation"></param>
        public void UpdateWflStage(int iWflStageID, Int16 iSeq, bool bEnabled, Int16 iDaysFromEstClose, Int16 iDaysFromCreation, Int16 iCalcMethod)
        {
            dal.UpdateWflStageBase(iWflStageID, iSeq, bEnabled, iDaysFromEstClose, iDaysFromCreation, iCalcMethod);
        }

        /// <summary>
        /// 检查sequece of workflow template是否存在
        /// neo 2011-02-16
        /// </summary>
        /// <param name="iWorkflowTemplateID"></param>
        /// <param name="iWflStageID"></param>
        /// <param name="iSeq"></param>
        /// <returns></returns>
        public bool IsWflStageSeqExist(int iWorkflowTemplateID, int iWflStageID, short iSeq)
        {
            return dal.IsWflStageSeqExistBase(iWorkflowTemplateID, iWflStageID, iSeq);
        }

        /// <summary>
        /// get default workflow template
        /// neo 2011-03-21
        /// </summary>
        /// <param name="sStatus"></param>
        /// <returns></returns>
        public DataTable GetDefaultWorkflowTemplateInfo(string sStatus)
        {
            return dal.GetDefaultWorkflowTemplateInfoBase(sStatus);
        }

         /// <summary>
        /// 
        /// </summary>
        /// <param name="sWhere"></param>
        /// <param name="sOrderBy"></param>
        /// <returns></returns>
        public DataTable GetWorkflowTemplateList(string sWhere, string sOrderBy)
        {
            return this.dal.GetWorkflowTemplateList(sWhere, sOrderBy);
        }

        #endregion
    }
}

