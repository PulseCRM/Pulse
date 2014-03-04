using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// 业务逻辑类Template_Wfl_Tasks 的摘要说明。
	/// </summary>
	public class Template_Wfl_Tasks
	{
		private readonly LPWeb.DAL.Template_Wfl_Tasks dal=new LPWeb.DAL.Template_Wfl_Tasks();
		public Template_Wfl_Tasks()
		{}

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.Template_Wfl_Tasks model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(LPWeb.Model.Template_Wfl_Tasks model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int TemplTaskId)
        {

            dal.Delete(TemplTaskId);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.Template_Wfl_Tasks GetModel(int TemplTaskId)
        {

            return dal.GetModel(TemplTaskId);
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
        public List<LPWeb.Model.Template_Wfl_Tasks> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<LPWeb.Model.Template_Wfl_Tasks> DataTableToList(DataTable dt)
        {
            List<LPWeb.Model.Template_Wfl_Tasks> modelList = new List<LPWeb.Model.Template_Wfl_Tasks>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LPWeb.Model.Template_Wfl_Tasks model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LPWeb.Model.Template_Wfl_Tasks();
                    if (dt.Rows[n]["TemplTaskId"].ToString() != "")
                    {
                        model.TemplTaskId = int.Parse(dt.Rows[n]["TemplTaskId"].ToString());
                    }
                    if (dt.Rows[n]["WflStageId"].ToString() != "")
                    {
                        model.WflStageId = int.Parse(dt.Rows[n]["WflStageId"].ToString());
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
                    if (dt.Rows[n]["Type"].ToString() != "")
                    {
                        model.Type = int.Parse(dt.Rows[n]["Type"].ToString());
                    }
                    if (dt.Rows[n]["DaysDueFromCoe"].ToString() != "")
                    {
                        model.DaysDueFromCoe = int.Parse(dt.Rows[n]["DaysDueFromCoe"].ToString());
                    }
                    if (dt.Rows[n]["PrerequisiteTaskId"].ToString() != "")
                    {
                        model.PrerequisiteTaskId = int.Parse(dt.Rows[n]["PrerequisiteTaskId"].ToString());
                    }
                    if (dt.Rows[n]["DaysDueAfterPrerequisite"].ToString() != "")
                    {
                        model.DaysDueAfterPrerequisite = int.Parse(dt.Rows[n]["DaysDueAfterPrerequisite"].ToString());
                    }
                    if (dt.Rows[n]["OwnerRoleId"].ToString() != "")
                    {
                        model.OwnerRoleId = int.Parse(dt.Rows[n]["OwnerRoleId"].ToString());
                    }
                    if (dt.Rows[n]["WarningEmailId"].ToString() != "")
                    {
                        model.WarningEmailId = int.Parse(dt.Rows[n]["WarningEmailId"].ToString());
                    }
                    if (dt.Rows[n]["OverdueEmailId"].ToString() != "")
                    {
                        model.OverdueEmailId = int.Parse(dt.Rows[n]["OverdueEmailId"].ToString());
                    }
                    if (dt.Rows[n]["CompletionEmailId"].ToString() != "")
                    {
                        model.CompletionEmailId = int.Parse(dt.Rows[n]["CompletionEmailId"].ToString());
                    }
                    if (dt.Rows[n]["SequenceNumber"].ToString() != "")
                    {
                        model.SequenceNumber = int.Parse(dt.Rows[n]["SequenceNumber"].ToString());
                    }
                    model.Description = dt.Rows[n]["Description"].ToString();
                    if (dt.Rows[n]["DaysFromCreation"].ToString() != "")
                    {
                        model.DaysFromCreation = int.Parse(dt.Rows[n]["DaysFromCreation"].ToString());
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
        public void EnabledTemplTasks(string TemplTaskIds, bool bEnabled)
        { 
            dal.EnabledTemplTasks(TemplTaskIds, bEnabled);
        }

        public void DeleteTasks(string TemplTaskIds)
        {
            dal.DeleteTasks(TemplTaskIds);
        }

        public DataSet GetWorkflowStageTasks(int PageSize, int PageIndex, string orderName, string strWhere, out int recordCount)
        {
            return dal.GetWorkflowStageTasks(PageSize, PageIndex, orderName, strWhere, out recordCount);
        }
	}
}

