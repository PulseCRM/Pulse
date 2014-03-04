using System;
using System.Data;
using System.Collections.Generic;
using LPWeb.Model;
namespace LPWeb.BLL
{
    /// <summary>
    /// ProspectTasks
    /// </summary>
    public class ProspectTasks
    {
        private readonly LPWeb.DAL.ProspectTasks dal = new LPWeb.DAL.ProspectTasks();
        public ProspectTasks()
        { }
        #region  Method
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ProspectTaskId)
        {
            return dal.Exists(ProspectTaskId);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.ProspectTasks model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(LPWeb.Model.ProspectTasks model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ProspectTaskId)
        {

            return dal.Delete(ProspectTaskId);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string ProspectTaskIdlist)
        {
            return dal.DeleteList(ProspectTaskIdlist);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.ProspectTasks GetModel(int ProspectTaskId)
        {

            return dal.GetModel(ProspectTaskId);
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
        public List<LPWeb.Model.ProspectTasks> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<LPWeb.Model.ProspectTasks> DataTableToList(DataTable dt)
        {
            List<LPWeb.Model.ProspectTasks> modelList = new List<LPWeb.Model.ProspectTasks>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LPWeb.Model.ProspectTasks model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LPWeb.Model.ProspectTasks();
                    if (dt.Rows[n]["ProspectTaskId"].ToString() != "")
                    {
                        model.ProspectTaskId = int.Parse(dt.Rows[n]["ProspectTaskId"].ToString());
                    }
                    if (dt.Rows[n]["ContactId"].ToString() != "")
                    {
                        model.ContactId = int.Parse(dt.Rows[n]["ContactId"].ToString());
                    }
                    model.TaskName = dt.Rows[n]["TaskName"].ToString();
                    model.Desc = dt.Rows[n]["Desc"].ToString();
                    if (dt.Rows[n]["OwnerId"].ToString() != "")
                    {
                        model.OwnerId = int.Parse(dt.Rows[n]["OwnerId"].ToString());
                    }
                    if (dt.Rows[n]["Due"].ToString() != "")
                    {
                        model.Due = DateTime.Parse(dt.Rows[n]["Due"].ToString());
                    }
                    if (dt.Rows[n]["WarningEmailTemplId"].ToString() != "")
                    {
                        model.WarningEmailTemplId = int.Parse(dt.Rows[n]["WarningEmailTemplId"].ToString());
                    }
                    if (dt.Rows[n]["OverdueEmailTemplId"].ToString() != "")
                    {
                        model.OverdueEmailTemplId = int.Parse(dt.Rows[n]["OverdueEmailTemplId"].ToString());
                    }
                    if (dt.Rows[n]["CompletionEmailTemplid"].ToString() != "")
                    {
                        model.CompletionEmailTemplid = int.Parse(dt.Rows[n]["CompletionEmailTemplid"].ToString());
                    }
                    if (dt.Rows[n]["Completed"].ToString() != "")
                    {
                        model.Completed = DateTime.Parse(dt.Rows[n]["Completed"].ToString());
                    }
                    if (dt.Rows[n]["CompletedBy"].ToString() != "")
                    {
                        model.CompletedBy = int.Parse(dt.Rows[n]["CompletedBy"].ToString());
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

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(int PageSize, int PageIndex, string strWhere, out int recordCount, string orderName, int orderType)
        {
            return dal.GetList(PageSize, PageIndex, strWhere, out recordCount, orderName, orderType);
        }

        /// <summary>
        /// 获得任务 Owner 信息
        /// </summary>
        public DataTable GetTaskOwnerInfoList()
        {
            return dal.GetOwnerInfo();
        }

        /// <summary>
        /// Update Complate Task
        /// Alex 2011-02-25
        /// </summary>
        /// <param name="iLoanTaskID"></param>
        public bool ComplateSelProspectTask(int iTaskID, int UserID,ref int iCompletionEmailTemplid)
        {
           return dal.ComplateSelProspectTask(iTaskID, UserID,ref iCompletionEmailTemplid);
        }

        /// <summary>
        ///  Delete Task
        /// </summary>
        /// <param name="iTaskIDs"></param>
        public void DeleteProspectTasks(string iTaskIDs,int iUserID)
        {
            dal.DeleteProspectTasks(iTaskIDs, iUserID);
        }

        public bool IsProspectTaskNameExists(int nId, string strName)
        {
            return dal.IsProspectTaskNameExists(nId, strName);
        }

        public void CheckProspectTaskAlert(int iProspectTaskID)
        {
            dal.CheckProspectTaskAlert(iProspectTaskID);
        }
    }
}

