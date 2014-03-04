using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
    /// <summary>
    /// LoanProfit
    /// </summary>
    public class LoanProfit
    {
        private readonly LPWeb.DAL.LoanProfit dal = new LPWeb.DAL.LoanProfit();
        public LoanProfit()
        { }
        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(LPWeb.Model.LoanProfit model)
        {
            dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(LPWeb.Model.LoanProfit model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int FileId)
        {

            dal.Delete(FileId);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.LoanProfit GetModel(int FileId)
        {

            return dal.GetModel(FileId);
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
        public List<LPWeb.Model.LoanProfit> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<LPWeb.Model.LoanProfit> DataTableToList(DataTable dt)
        {
            List<LPWeb.Model.LoanProfit> modelList = new List<LPWeb.Model.LoanProfit>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LPWeb.Model.LoanProfit model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LPWeb.Model.LoanProfit();
                    if (dt.Rows[n]["FileId"].ToString() != "")
                    {
                        model.FileId = int.Parse(dt.Rows[n]["FileId"].ToString());
                    }
                    model.CompensationPlan = dt.Rows[n]["CompensationPlan"].ToString();
                    if (dt.Rows[n]["LenderCredit"].ToString() != "")
                    {
                        model.LenderCredit = decimal.Parse(dt.Rows[n]["LenderCredit"].ToString());
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
        //public DataSet GetList(int PageSize,int PageIndex,string strWhere)
        //{
        //return dal.GetList(PageSize,PageIndex,strWhere);
        //}

        #endregion  成员方法

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iFileId"></param>
        /// <returns></returns>
        public DataTable GetLoanProfitInfo(int iFileId)
        {
            return dal.GetLoanProfitInfo(iFileId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iFileId"></param>
        /// <param name="sCompensationPlan"></param>
        /// <param name="sLenderCredit"></param>
        public void UpdateLoanProfit(int iFileId, string sCompensationPlan, string sLenderCredit)
        {
            this.dal.UpdateLoanProfit(iFileId, sCompensationPlan, sLenderCredit);
        }
    }
}

