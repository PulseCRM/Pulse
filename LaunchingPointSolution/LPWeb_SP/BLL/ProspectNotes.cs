using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
    /// <summary>
    /// 业务逻辑类ProspectNotes 的摘要说明。
    /// </summary>
    public class ProspectNotes
    {
        private readonly LPWeb.DAL.ProspectNotes dal = new LPWeb.DAL.ProspectNotes();
        public ProspectNotes()
        { }
        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.ProspectNotes model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(LPWeb.Model.ProspectNotes model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int PropsectNoteId)
        {

            dal.Delete(PropsectNoteId);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.ProspectNotes GetModel(int PropsectNoteId)
        {

            return dal.GetModel(PropsectNoteId);
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
        public List<LPWeb.Model.ProspectNotes> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<LPWeb.Model.ProspectNotes> DataTableToList(DataTable dt)
        {
            List<LPWeb.Model.ProspectNotes> modelList = new List<LPWeb.Model.ProspectNotes>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LPWeb.Model.ProspectNotes model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LPWeb.Model.ProspectNotes();
                    if (dt.Rows[n]["PropsectNoteId"].ToString() != "")
                    {
                        model.PropsectNoteId = int.Parse(dt.Rows[n]["PropsectNoteId"].ToString());
                    }
                    if (dt.Rows[n]["ContactId"].ToString() != "")
                    {
                        model.ContactId = int.Parse(dt.Rows[n]["ContactId"].ToString());
                    }
                    if (dt.Rows[n]["Created"].ToString() != "")
                    {
                        model.Created = DateTime.Parse(dt.Rows[n]["Created"].ToString());
                    }
                    model.Sender = dt.Rows[n]["Sender"].ToString();
                    model.Note = dt.Rows[n]["Note"].ToString();
                    if (dt.Rows[n]["Exported"].ToString() != "")
                    {
                        if ((dt.Rows[n]["Exported"].ToString() == "1") || (dt.Rows[n]["Exported"].ToString().ToLower() == "true"))
                        {
                            model.Exported = true;
                        }
                        else
                        {
                            model.Exported = false;
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
        public DataSet GetList(int PageSize, int PageIndex, string strWhere)
        {
            return dal.GetList(PageSize, PageIndex, strWhere);
        }

        #endregion  成员方法


        /// <summary>
        /// Gets the prospect notes.
        /// </summary>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="queryCondition">The query condition.</param>
        /// <param name="recordCount">The record count.</param>
        /// <param name="orderName">Name of the order.</param>
        /// <param name="orderType">Type of the order.</param>
        /// <returns></returns>
        public DataSet GetProspectNotes(int pageSize, int pageIndex, string queryCondition, out int recordCount, string orderName, int orderType, int iContactID)
        {
            return dal.GetProspectNotes(pageSize, pageIndex, queryCondition, out recordCount, orderName, orderType, iContactID);
        }

        public DataTable GetLoanNoteTypeInfo(int iContactID)
        {
            return dal.GetLoanNoteTypeInfo(iContactID);
        }

        public DataTable GetLoanNoteTypeInfoForAdd(int iContactID)
        {
            return dal.GetLoanNoteTypeInfoForAdd(iContactID);
        }
    }
}
