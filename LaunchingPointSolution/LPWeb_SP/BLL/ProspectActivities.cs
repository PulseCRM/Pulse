using System;
using System.Data;
using System.Collections.Generic;
using LPWeb.Model;
namespace LPWeb.BLL
{
    /// <summary>
    /// 业务逻辑类ProspectActivities 的摘要说明。
    /// </summary>
    public class ProspectActivities
    {
        private readonly LPWeb.DAL.ProspectActivities dal = new LPWeb.DAL.ProspectActivities();
        public ProspectActivities()
        { }
        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.ProspectActivities model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(LPWeb.Model.ProspectActivities model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ProspectActivityId)
        {

            dal.Delete(ProspectActivityId);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.ProspectActivities GetModel(int ProspectActivityId)
        {

            return dal.GetModel(ProspectActivityId);
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
        public List<LPWeb.Model.ProspectActivities> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<LPWeb.Model.ProspectActivities> DataTableToList(DataTable dt)
        {
            List<LPWeb.Model.ProspectActivities> modelList = new List<LPWeb.Model.ProspectActivities>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LPWeb.Model.ProspectActivities model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LPWeb.Model.ProspectActivities();
                    if (dt.Rows[n]["ProspectActivityId"].ToString() != "")
                    {
                        model.ProspectActivityId = int.Parse(dt.Rows[n]["ProspectActivityId"].ToString());
                    }
                    if (dt.Rows[n]["ContactId"].ToString() != "")
                    {
                        model.ContactId = int.Parse(dt.Rows[n]["ContactId"].ToString());
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
        /// get data for loan activities list
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="strWhere"></param>
        /// <param name="recordCount"></param>
        /// <param name="orderName"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public DataSet GetListForGridView(int PageSize, int PageIndex, string strWhere, out int recordCount, string orderName, int orderType, int iContactID)
        {
            return dal.GetListForGridView(PageSize, PageIndex, strWhere, out recordCount, orderName, orderType, iContactID);
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

        public DataTable GetActivityTypeInfo(int iContactID)
        {
            return dal.GetActivityTypeInfo(iContactID);
        }
    }
}

