using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
    /// <summary>
    /// LoanActivities 的摘要说明。
    /// </summary>
    public class LoanActivities
    {
        private readonly LPWeb.DAL.LoanActivities dal = new LPWeb.DAL.LoanActivities();
        public LoanActivities()
        { }

        /// <summary>
        /// get data for loan activities list
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="strWhere"></param>
        /// <param name="recordCount"></param>
        /// <param name="orderName"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public DataSet GetListForGridView(int PageSize, int PageIndex, string strWhere, out int recordCount, string orderName, int orderType)
        {
            return dal.GetListForGridView(PageSize, PageIndex, strWhere, out recordCount, orderName, orderType);
        }

        /// <summary>
        /// Get proformedby user of loan activity
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataTable GetProformedBy(string strWhere)
        {
            return dal.GetProformedBy(strWhere);
        }

        public void LoanReassignAcivities(int FileID, int UserID, int RoleID)
        {
            dal.LoanReassignAcivities(FileID, UserID, RoleID);
        }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.LoanActivities model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(LPWeb.Model.LoanActivities model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ActivityId)
        {

            dal.Delete(ActivityId);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.LoanActivities GetModel(int ActivityId)
        {

            return dal.GetModel(ActivityId);
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
        public List<LPWeb.Model.LoanActivities> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<LPWeb.Model.LoanActivities> DataTableToList(DataTable dt)
        {
            List<LPWeb.Model.LoanActivities> modelList = new List<LPWeb.Model.LoanActivities>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LPWeb.Model.LoanActivities model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LPWeb.Model.LoanActivities();
                    if (dt.Rows[n]["ActivityId"].ToString() != "")
                    {
                        model.ActivityId = int.Parse(dt.Rows[n]["ActivityId"].ToString());
                    }
                    if (dt.Rows[n]["FileId"].ToString() != "")
                    {
                        model.FileId = int.Parse(dt.Rows[n]["FileId"].ToString());
                    }
                    if (dt.Rows[n]["UserId"].ToString() != "")
                    {
                        model.UserId = int.Parse(dt.Rows[n]["UserId"].ToString());
                    }
                    model.ActivityName = dt.Rows[n]["ActivityName"].ToString();
                    if (dt.Rows[n]["ActivityTime"].ToString() != "")
                    {
                        model.ActivityTime = DateTime.Parse(dt.Rows[n]["ActivityTime"].ToString());
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
    }
}

