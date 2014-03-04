using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// 业务逻辑类Template_Wfl_Stages 的摘要说明。
	/// </summary>
	public class Template_Wfl_Stages
	{
		private readonly LPWeb.DAL.Template_Wfl_Stages dal=new LPWeb.DAL.Template_Wfl_Stages();
		public Template_Wfl_Stages()
		{}
        public int Add(LPWeb.Model.Template_Wfl_Stages model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(LPWeb.Model.Template_Wfl_Stages model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int WflStageId)
        {

            dal.Delete(WflStageId);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.Template_Wfl_Stages GetModel(int WflStageId)
        {

            return dal.GetModel(WflStageId);
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
        public List<LPWeb.Model.Template_Wfl_Stages> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<LPWeb.Model.Template_Wfl_Stages> DataTableToList(DataTable dt)
        {
            List<LPWeb.Model.Template_Wfl_Stages> modelList = new List<LPWeb.Model.Template_Wfl_Stages>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LPWeb.Model.Template_Wfl_Stages model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LPWeb.Model.Template_Wfl_Stages();
                    if (dt.Rows[n]["WflStageId"].ToString() != "")
                    {
                        model.WflStageId = int.Parse(dt.Rows[n]["WflStageId"].ToString());
                    }
                    if (dt.Rows[n]["WflTemplId"].ToString() != "")
                    {
                        model.WflTemplId = int.Parse(dt.Rows[n]["WflTemplId"].ToString());
                    }
                    if (dt.Rows[n]["SequenceNumber"].ToString() != "")
                    {
                        model.SequenceNumber = int.Parse(dt.Rows[n]["SequenceNumber"].ToString());
                    }
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
                    if (dt.Rows[n]["DaysFromEstClose"].ToString() != "")
                    {
                        model.DaysFromEstClose = int.Parse(dt.Rows[n]["DaysFromEstClose"].ToString());
                    }
                    model.Name = dt.Rows[n]["Name"].ToString();
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

        public int GetMinStageSeqNumByWflTempID(int iWflTempID)
        {
            return dal.GetMinStageSeqNumByWflTempID(iWflTempID);
        }
        public int GetSecStageSeqNumByWflTempID(int iWflTempID)
        {
            return dal.GetSecStageSeqNumByWflTempID(iWflTempID);
        }
	}
}

