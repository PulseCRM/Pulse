using System;
using System.Data;
using System.Collections.Generic;
using LPWeb.Model;
namespace LPWeb.BLL
{
    /// <summary>
    /// UserRecentItems
    /// </summary>
    public class UserRecentItems
    {
        private readonly LPWeb.DAL.UserRecentItems dal = new LPWeb.DAL.UserRecentItems();
        public UserRecentItems()
        { }
        #region  Method
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int UserId, DateTime LastAccessed)
        {
            return dal.Exists(UserId, LastAccessed);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(LPWeb.Model.UserRecentItems model)
        {
            dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(LPWeb.Model.UserRecentItems model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int UserId, DateTime LastAccessed)
        {

            return dal.Delete(UserId, LastAccessed);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.UserRecentItems GetModel(int UserId, DateTime LastAccessed)
        {

            return dal.GetModel(UserId, LastAccessed);
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
        public List<LPWeb.Model.UserRecentItems> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<LPWeb.Model.UserRecentItems> DataTableToList(DataTable dt)
        {
            List<LPWeb.Model.UserRecentItems> modelList = new List<LPWeb.Model.UserRecentItems>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LPWeb.Model.UserRecentItems model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LPWeb.Model.UserRecentItems();
                    if (dt.Rows[n]["UserId"].ToString() != "")
                    {
                        model.UserId = int.Parse(dt.Rows[n]["UserId"].ToString());
                    }
                    if (dt.Rows[n]["LastAccessed"].ToString() != "")
                    {
                        model.LastAccessed = DateTime.Parse(dt.Rows[n]["LastAccessed"].ToString());
                    }
                    if (dt.Rows[n]["FileId"].ToString() != "")
                    {
                        model.FileId = int.Parse(dt.Rows[n]["FileId"].ToString());
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
        /// 分页获取数据列表
        /// </summary>
        //public DataSet GetList(int PageSize,int PageIndex,string strWhere)
        //{
        //return dal.GetList(PageSize,PageIndex,strWhere);
        //}

        #endregion  Method

        public DataSet GetUserRecentItems(int iUserID)
        {
            return dal.GetUserRecentItems(iUserID);
        }

        public string GetUserRecentItemsBorrowerInfo(int iFileID)
        {
            return dal.GetUserRecentItemsBorrowerInfo(iFileID);
        }

        public string GetLoanStatusbyFileID(int iFileID)
        {
            return dal.GetLoanStatusbyFileID(iFileID);
        }

        public void InsertUserRecentItems(int iFileID, int iUserID)
        { 
            dal.InsertUserRecentItems(iFileID,iUserID);
        }

        public void DeleteItemsByFileID(int iFileID)
        {
            dal.DeleteItemsByFileID(iFileID);
        }
    }
}

