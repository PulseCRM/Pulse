using System;
using System.Data;
using System.Collections.Generic; 
using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// 业务逻辑类Template_RuleGroups 的摘要说明。
	/// </summary>
	public class Template_RuleGroups
	{
		private readonly LPWeb.DAL.Template_RuleGroups dal=new LPWeb.DAL.Template_RuleGroups();
		public Template_RuleGroups()
        { }

        /// <summary>
        /// get rulegroup list for gridview
        /// </summary>
        public DataSet GetListForGridView(int PageSize, int PageIndex, string strWhere, out int recordCount, string orderName, int orderType)
        {
            return dal.GetListForGridView(PageSize, PageIndex, strWhere, out recordCount, orderName, orderType);
        }

        /// <summary>
        /// get rule list of rule group for gridview
        /// </summary>
        public DataSet GetRuleListOfRuleGroup(int PageSize, int PageIndex, string strWhere, out int recordCount, string orderName, int orderType)
        {
            return dal.GetRuleListOfRuleGroup(PageSize, PageIndex, strWhere, out recordCount, orderName, orderType);
        }

        /// <summary>
        /// Add rule group info, clone a rule group when the parameter nSourceID is not null
        /// </summary>
        /// <param name="ruleGroup"></param>
        /// <param name="strRuleIds"></param>
        /// <param name="nSourceID"></param>
        public int AddRuleGroupInfo(Model.Template_RuleGroups ruleGroup, string strRuleIds, int? nSourceID)
        {
            return dal.AddRuleGroupInfo(ruleGroup, strRuleIds, nSourceID);
        }

        /// <summary>
        /// Save rule group info from rule group setup page
        /// </summary>
        /// <param name="ruleGroup"></param>
        /// <param name="strRuleIds"></param>
        public void UpdateRuleGroupInfo(Model.Template_RuleGroups ruleGroup, string strRuleIds)
        {
            dal.UpdateRuleGroupInfo(ruleGroup, strRuleIds);
        }

        public void UpdateRuleGroupRuleInfo(int nRuleGroupId, string strRuleIds)
        {
            Model.Template_RuleGroups ruleGroup = dal.GetModel(nRuleGroupId);
            UpdateRuleGroupInfo(ruleGroup, strRuleIds);
        }

        /// <summary>
        /// add rule to rule group
        /// </summary>
        /// <param name="nRuleGroupId"></param>
        /// <param name="nRuleId"></param>
        public void AddRuleToRuleGroup(int nRuleGroupId, int nRuleId)
        {
            this.AddRuleToRuleGroup(nRuleGroupId, new List<int>() { nRuleId });
        }

        /// <summary>
        /// add rules to rule group
        /// </summary>
        /// <param name="nRuleGroupId"></param>
        /// <param name="listRuleId"></param>
        public void AddRuleToRuleGroup(int nRuleGroupId, List<int> listRuleId)
        {
            dal.AddRuleToRuleGroup(nRuleGroupId, listRuleId);
        }

        /// <summary>
        /// remove rule from rule group
        /// </summary>
        /// <param name="nRuleGroupId"></param>
        /// <param name="strRuleIds"></param>
        public void RemoveRuleFromRuleGroup(int nRuleGroupId, string strRuleIds)
        {
            dal.RemoveRuleFromRuleGroup(nRuleGroupId, strRuleIds);
        }

        /// <summary>
        /// disable rule of rule group
        /// </summary>
        /// <param name="nRuleGroupId"></param>
        /// <param name="strRuleIds"></param>
        public void DisableRuleOfRuleGroup(int nRuleGroupId, string strRuleIds)
        {
            dal.DisableRuleOfRuleGroup(nRuleGroupId, strRuleIds);
        }

        /// <summary>
        /// delete rule of rule group
        /// </summary>
        /// <param name="nRuleGroupId"></param>
        /// <param name="strRuleIds"></param>
        public void DeleteRuleOfRuleGroup(int nRuleGroupId, string strRuleIds)
        {
            dal.DeleteRuleOfRuleGroup(nRuleGroupId, strRuleIds);
        }

        public bool DeleteRuleGroupInfo(int nId)
        {
            return dal.DeleteRuleGroupInfo(new List<int>() { nId });
        }

        /// <summary>
        /// disable rule group
        /// </summary>
        /// <param name="nId"></param>
        public void DisableRuleGroupInfo(int nId)
        {
            this.DisableRuleGroupInfo(new List<int>() { nId });
        }

        /// <summary>
        /// disable rule group
        /// </summary>
        /// <param name="listIds"></param>
        public void DisableRuleGroupInfo(List<int> listIds)
        {
            dal.DisableRuleGroupInfo(listIds);
        }

        public void EnableRuleGroupInfo(List<int> listIds)
        {
            dal.EnableRuleGroupInfo(listIds);
        }

        /// <summary>
        /// delete rule group
        /// </summary>
        /// <param name="listIds"></param>
        public bool DeleteRuleGroupInfo(List<int> listIds)
        {
            return dal.DeleteRuleGroupInfo(listIds);
        }

        public bool IsReferencedByLoan(int nId)
        {
            return dal.IsReferencedByLoan(nId);
        }

        public bool IsRuleGroupNameExists(int nId, string strName)
        {
            return dal.IsRuleGroupNameExists(nId, strName);
        }

        #region neo (Company Global Rules)

        /// <summary>
        /// get non-global(RuleScope=0) rule group
        /// neo 2011-03-19
        /// </summary>
        /// <returns></returns>
        public DataTable GetNonGlobalRuleGroupList()
        {
            return dal.GetNonGlobalRuleGroupListBase();
        }

        /// <summary>
        /// add company global rule group
        /// neo 2011-03-19
        /// </summary>
        /// <param name="sRuleGroupIDs"></param>
        public void AddGlobalRuleGroups(string sRuleGroupIDs)
        {
            dal.AddGlobalRuleGroupsBase(sRuleGroupIDs);
        }

        #endregion

        #region  成员方法
        /// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(LPWeb.Model.Template_RuleGroups model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(LPWeb.Model.Template_RuleGroups model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int RuleGroupId)
		{
			
			dal.Delete(RuleGroupId);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.Template_RuleGroups GetModel(int RuleGroupId)
		{
			
			return dal.GetModel(RuleGroupId);
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
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			return dal.GetList(Top,strWhere,filedOrder);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LPWeb.Model.Template_RuleGroups> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LPWeb.Model.Template_RuleGroups> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.Template_RuleGroups> modelList = new List<LPWeb.Model.Template_RuleGroups>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.Template_RuleGroups model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.Template_RuleGroups();
					if(dt.Rows[n]["RuleGroupId"].ToString()!="")
					{
						model.RuleGroupId=int.Parse(dt.Rows[n]["RuleGroupId"].ToString());
					}
					model.Name=dt.Rows[n]["Name"].ToString();
					model.Desc=dt.Rows[n]["Desc"].ToString();
					if(dt.Rows[n]["Enabled"].ToString()!="")
					{
						if((dt.Rows[n]["Enabled"].ToString()=="1")||(dt.Rows[n]["Enabled"].ToString().ToLower()=="true"))
						{
						model.Enabled=true;
						}
						else
						{
							model.Enabled=false;
						}
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
		public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		{
			return dal.GetList(PageSize,PageIndex,strWhere);
		}

		#endregion  成员方法
	}
}

