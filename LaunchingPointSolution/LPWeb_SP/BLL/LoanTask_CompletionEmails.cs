using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;

namespace LPWeb.BLL
{
    /// <summary>
    /// LoanTask_CompletionEmails
    /// </summary>
    public partial class LoanTask_CompletionEmails
    {
        private readonly LPWeb.DAL.LoanTask_CompletionEmailsBase dal = new LPWeb.DAL.LoanTask_CompletionEmailsBase();
        public LoanTask_CompletionEmails()
        { }
        #region  Method
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int TaskCompletionEmailId)
        {
            return dal.Exists(TaskCompletionEmailId);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.LoanTask_CompletionEmails model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(LPWeb.Model.LoanTask_CompletionEmails model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int TaskCompletionEmailId)
        {

            return dal.Delete(TaskCompletionEmailId);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string TaskCompletionEmailIdlist)
        {
            return dal.DeleteList(TaskCompletionEmailIdlist);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.LoanTask_CompletionEmails GetModel(int TaskCompletionEmailId)
        {

            return dal.GetModel(TaskCompletionEmailId);
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
        public List<LPWeb.Model.LoanTask_CompletionEmails> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<LPWeb.Model.LoanTask_CompletionEmails> DataTableToList(DataTable dt)
        {
            List<LPWeb.Model.LoanTask_CompletionEmails> modelList = new List<LPWeb.Model.LoanTask_CompletionEmails>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LPWeb.Model.LoanTask_CompletionEmails model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LPWeb.Model.LoanTask_CompletionEmails();
                    if (dt.Rows[n]["TaskCompletionEmailId"] != null && dt.Rows[n]["TaskCompletionEmailId"].ToString() != "")
                    {
                        model.TaskCompletionEmailId = int.Parse(dt.Rows[n]["TaskCompletionEmailId"].ToString());
                    }
                    if (dt.Rows[n]["LoanTaskid"] != null && dt.Rows[n]["LoanTaskid"].ToString() != "")
                    {
                        model.LoanTaskid = int.Parse(dt.Rows[n]["LoanTaskid"].ToString());
                    }
                    if (dt.Rows[n]["TemplEmailId"] != null && dt.Rows[n]["TemplEmailId"].ToString() != "")
                    {
                        model.TemplEmailId = int.Parse(dt.Rows[n]["TemplEmailId"].ToString());
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
    }
}

