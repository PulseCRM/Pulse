using System;
using System.Data;
using System.Collections.Generic;
using LPWeb.Model;
namespace LPWeb.BLL
{
    /// <summary>
    /// ProspectAlerts
    /// </summary>
    public class ProspectAlerts
    {
        private readonly LPWeb.DAL.ProspectAlerts dal = new LPWeb.DAL.ProspectAlerts();
        public ProspectAlerts()
        { }
        #region  Method
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.ProspectAlerts model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(LPWeb.Model.ProspectAlerts model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ProspectAlertId)
        {

            return dal.Delete(ProspectAlertId);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string ProspectAlertIdlist)
        {
            return dal.DeleteList(ProspectAlertIdlist);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.ProspectAlerts GetModel(int ProspectAlertId)
        {

            return dal.GetModel(ProspectAlertId);
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
        public List<LPWeb.Model.ProspectAlerts> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<LPWeb.Model.ProspectAlerts> DataTableToList(DataTable dt)
        {
            List<LPWeb.Model.ProspectAlerts> modelList = new List<LPWeb.Model.ProspectAlerts>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LPWeb.Model.ProspectAlerts model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LPWeb.Model.ProspectAlerts();
                    if (dt.Rows[n]["ProspectAlertId"].ToString() != "")
                    {
                        model.ProspectAlertId = int.Parse(dt.Rows[n]["ProspectAlertId"].ToString());
                    }
                    if (dt.Rows[n]["ContactId"].ToString() != "")
                    {
                        model.ContactId = int.Parse(dt.Rows[n]["ContactId"].ToString());
                    }
                    model.Desc = dt.Rows[n]["Desc"].ToString();
                    if (dt.Rows[n]["DueDate"].ToString() != "")
                    {
                        model.DueDate = DateTime.Parse(dt.Rows[n]["DueDate"].ToString());
                    }
                    if (dt.Rows[n]["OwnerId"].ToString() != "")
                    {
                        model.OwnerId = int.Parse(dt.Rows[n]["OwnerId"].ToString());
                    }
                    if (dt.Rows[n]["ProspectTaskId"].ToString() != "")
                    {
                        model.ProspectTaskId = int.Parse(dt.Rows[n]["ProspectTaskId"].ToString());
                    }
                    model.AlertType = dt.Rows[n]["AlertType"].ToString();
                    if (dt.Rows[n]["Created"].ToString() != "")
                    {
                        model.Created = DateTime.Parse(dt.Rows[n]["Created"].ToString());
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
        /// 得到最早的Task Alert ID
        /// Alex 2011-02-16
        /// </summary>
        /// <param name="iLoanID"></param>
        /// <returns></returns>
        public int GetTaskAlertID(int iContactID)
        {
            return dal.GetProspectTaskAlertID(iContactID);
        }

        /// <summary>
        /// get prospect alert info
        /// neo 2011-03-17
        /// </summary>
        /// <param name="iProspectAlertID"></param>
        /// <returns></returns>
        public DataTable GetProspectAlertInfo(int iProspectAlertID)
        {
            return dal.GetProspectAlertInfoBase(iProspectAlertID);
        }
    }
}

