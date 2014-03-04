using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// Template_RuleConditions 的摘要说明。
	/// </summary>
	public class Template_RuleConditions
	{
		private readonly LPWeb.DAL.Template_RuleConditions dal=new LPWeb.DAL.Template_RuleConditions();
		public Template_RuleConditions()
        { }
        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.Template_RuleConditions model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(LPWeb.Model.Template_RuleConditions model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int RuleCondId)
        {

            dal.Delete(RuleCondId);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.Template_RuleConditions GetModel(int RuleCondId)
        {

            return dal.GetModel(RuleCondId);
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
        public List<LPWeb.Model.Template_RuleConditions> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<LPWeb.Model.Template_RuleConditions> DataTableToList(DataTable dt)
        {
            List<LPWeb.Model.Template_RuleConditions> modelList = new List<LPWeb.Model.Template_RuleConditions>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LPWeb.Model.Template_RuleConditions model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LPWeb.Model.Template_RuleConditions();
                    if (dt.Rows[n]["RuleCondId"].ToString() != "")
                    {
                        model.RuleCondId = int.Parse(dt.Rows[n]["RuleCondId"].ToString());
                    }
                    if (dt.Rows[n]["RuleId"].ToString() != "")
                    {
                        model.RuleId = int.Parse(dt.Rows[n]["RuleId"].ToString());
                    }
                    if (dt.Rows[n]["PointFieldId"].ToString() != "")
                    {
                        model.PointFieldId = decimal.Parse(dt.Rows[n]["PointFieldId"].ToString());
                    }
                    if (dt.Rows[n]["Condition"].ToString() != "")
                    {
                        model.Condition = int.Parse(dt.Rows[n]["Condition"].ToString());
                    }
                    model.Tolerance = dt.Rows[n]["Tolerance"].ToString();
                    model.ToleranceType = dt.Rows[n]["ToleranceType"].ToString();
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
	}
}

