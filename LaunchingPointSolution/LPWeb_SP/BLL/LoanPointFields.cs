using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
    /// <summary>
    /// LoanPointFields 的摘要说明。
    /// </summary>
    public class LoanPointFields
    {
        private readonly LPWeb.DAL.LoanPointFields dal = new LPWeb.DAL.LoanPointFields();
        public LoanPointFields()
        { }
        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(LPWeb.Model.LoanPointFields model)
        {
            dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(LPWeb.Model.LoanPointFields model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int FileId, int PointFieldId)
        {

            dal.Delete(FileId, PointFieldId);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.LoanPointFields GetModel(int FileId, int PointFieldId)
        {

            return dal.GetModel(FileId, PointFieldId);
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
        public List<LPWeb.Model.LoanPointFields> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<LPWeb.Model.LoanPointFields> DataTableToList(DataTable dt)
        {
            List<LPWeb.Model.LoanPointFields> modelList = new List<LPWeb.Model.LoanPointFields>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LPWeb.Model.LoanPointFields model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LPWeb.Model.LoanPointFields();
                    if (dt.Rows[n]["FileId"].ToString() != "")
                    {
                        model.FileId = int.Parse(dt.Rows[n]["FileId"].ToString());
                    }
                    if (dt.Rows[n]["PointFieldId"].ToString() != "")
                    {
                        model.PointFieldId = int.Parse(dt.Rows[n]["PointFieldId"].ToString());
                    }
                    model.PrevValue = dt.Rows[n]["PrevValue"].ToString();
                    model.CurrentValue = dt.Rows[n]["CurrentValue"].ToString();
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


        public DataSet GetProcessingList(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType, int userID, bool accessOtherLoans)
        {
            return dal.GetProcessingList(PageSize, PageIndex, strWhere, out count, orderName, orderType, userID, accessOtherLoans);
        }

        /// <summary>
        /// get LoanPointFields.CurrentValue
        /// neo 2012-12-17
        /// </summary>
        /// <param name="iFileId"></param>
        /// <param name="iPointFieldId"></param>
        /// <returns></returns>
        public DataTable GetPointFieldInfo(int iFileId, int iPointFieldId)
        {
            return this.dal.GetPointFieldInfo(iFileId, iPointFieldId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iFileId"></param>
        /// <param name="iPointFieldId"></param>
        /// <param name="sCurrentValue"></param>
        public void UpdatePointFieldValue(int iFileId, int iPointFieldId, string sCurrentValue)
        {
            this.dal.UpdatePointFieldValue(iFileId, iPointFieldId, sCurrentValue);
        }
        public void DeletePointFields(int iFileid, string sWhere)
        {
            this.dal.DeletePointFields(iFileid, sWhere);
        }
    }
}

