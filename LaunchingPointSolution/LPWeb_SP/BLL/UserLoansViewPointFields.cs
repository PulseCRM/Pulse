using System;
using System.Data;
using System.Collections.Generic;
using LPWeb.Model;
namespace LPWeb.BLL
{
    /// <summary>
    /// UserLoansViewPointFields
    /// </summary>
    public class UserLoansViewPointFields
    {
        private readonly LPWeb.DAL.UserLoansViewPointFields dal = new LPWeb.DAL.UserLoansViewPointFields();
        public UserLoansViewPointFields()
        { }
        #region  Method
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int UserId, int PointFieldId)
        {
            return dal.Exists(UserId, PointFieldId);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(LPWeb.Model.UserLoansViewPointFields model)
        {
            dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(LPWeb.Model.UserLoansViewPointFields model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int UserId, int PointFieldId)
        {

            return dal.Delete(UserId, PointFieldId);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.UserLoansViewPointFields GetModel(int UserId, int PointFieldId)
        {

            return dal.GetModel(UserId, PointFieldId);
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
        public List<LPWeb.Model.UserLoansViewPointFields> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<LPWeb.Model.UserLoansViewPointFields> DataTableToList(DataTable dt)
        {
            List<LPWeb.Model.UserLoansViewPointFields> modelList = new List<LPWeb.Model.UserLoansViewPointFields>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LPWeb.Model.UserLoansViewPointFields model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LPWeb.Model.UserLoansViewPointFields();
                    if (dt.Rows[n]["UserId"].ToString() != "")
                    {
                        model.UserId = int.Parse(dt.Rows[n]["UserId"].ToString());
                    }
                    if (dt.Rows[n]["PointFieldId"].ToString() != "")
                    {
                        model.PointFieldId = int.Parse(dt.Rows[n]["PointFieldId"].ToString());
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

        public DataTable GetUserLoansViewPointFieldsInfo(int iUserID,int iFileID)
        {
            return dal.GetUserLoansViewPointFieldsInfo(iUserID, iFileID);
        }

        public string GetUserLoansViewPointFieldsCurrentValue(int iUserID, int iFileID, int iPointFieldID)
        {
            return dal.GetUserLoansViewPointFieldsCurrentValue(iUserID, iFileID, iPointFieldID);
        }

        public int GetUserLoansViewPointFieldsCount(int iUserID)
        {
            return dal.GetUserLoansViewPointFieldsCount(iUserID);
        }

        public DataTable GetUserLoansViewPointFieldsHeadingInfo(int iUserID)
        {
            return dal.GetUserLoansViewPointFieldsHeadingInfo(iUserID);
        }

        public DataTable GetUserLoansViewPointFieldsLabelHeadingInfo(int iUserID)
        {
            return dal.GetUserLoansViewPointFieldsLabelHeadingInfo(iUserID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iUserID"></param>
        /// <returns></returns>
        public DataTable GetUserPointFieldInfo(int iUserID)
        {
            return this.dal.GetUserPointFieldInfo(iUserID);
        }

        public bool DeleteAllByUser(int UserId)
        {
            return dal.DeleteAllByUser(UserId);
        }
    }
}

