using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
    /// <summary>
    /// Company_LoanProgramDetails
    /// </summary>
    public class Company_LoanProgramDetails
    {
        private readonly LPWeb.DAL.Company_LoanProgramDetails dal = new LPWeb.DAL.Company_LoanProgramDetails();
        public Company_LoanProgramDetails()
        { }
        #region  Method

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return dal.GetMaxId();
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int LoanProgramID, int InvestorID)
        {
            return dal.Exists(LoanProgramID, InvestorID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(LPWeb.Model.Company_LoanProgramDetails model)
        {
            dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(LPWeb.Model.Company_LoanProgramDetails model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int LoanProgramID)
        {

            return dal.Delete(LoanProgramID);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string LoanProgramIDlist)
        {
            return dal.DeleteList(LoanProgramIDlist);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.Company_LoanProgramDetails GetModel(int LoanProgramID)
        {

            return dal.GetModel(LoanProgramID);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        //public LPWeb.Model.Company_LoanProgramDetails GetModelByCache(int LoanProgramID)
        //{

        //    string CacheKey = "Company_LoanProgramDetailsModel-" + LoanProgramID;
        //    object objModel = Maticsoft.Common.DataCache.GetCache(CacheKey);
        //    if (objModel == null)
        //    {
        //        try
        //        {
        //            objModel = dal.GetModel(LoanProgramID);
        //            if (objModel != null)
        //            {
        //                int ModelCache = Maticsoft.Common.ConfigHelper.GetConfigInt("ModelCache");
        //                Maticsoft.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
        //            }
        //        }
        //        catch { }
        //    }
        //    return (LPWeb.Model.Company_LoanProgramDetails)objModel;
        //}

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
        public List<LPWeb.Model.Company_LoanProgramDetails> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<LPWeb.Model.Company_LoanProgramDetails> DataTableToList(DataTable dt)
        {
            List<LPWeb.Model.Company_LoanProgramDetails> modelList = new List<LPWeb.Model.Company_LoanProgramDetails>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LPWeb.Model.Company_LoanProgramDetails model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LPWeb.Model.Company_LoanProgramDetails();
                    if (dt.Rows[n]["LoanProgramID"].ToString() != "")
                    {
                        model.LoanProgramID = int.Parse(dt.Rows[n]["LoanProgramID"].ToString());
                    }
                    //if (dt.Rows[n]["LenderCompanyId"].ToString() != "")
                    //{
                    //    model.LenderCompanyId = int.Parse(dt.Rows[n]["LenderCompanyId"].ToString());
                    //}
                    model.IndexType = dt.Rows[n]["IndexType"].ToString();
                    if (dt.Rows[n]["Margin"].ToString() != "")
                    {
                        model.Margin = decimal.Parse(dt.Rows[n]["Margin"].ToString());
                    }
                    if (dt.Rows[n]["FirstAdj"].ToString() != "")
                    {
                        model.FirstAdj = decimal.Parse(dt.Rows[n]["FirstAdj"].ToString());
                    }
                    if (dt.Rows[n]["SubAdj"].ToString() != "")
                    {
                        model.SubAdj = decimal.Parse(dt.Rows[n]["SubAdj"].ToString());
                    }
                    if (dt.Rows[n]["LifetimeCap"].ToString() != "")
                    {
                        model.LifetimeCap = decimal.Parse(dt.Rows[n]["LifetimeCap"].ToString());
                    }
                    if (dt.Rows[n]["InvestorID"].ToString() != "")
                    {
                        model.InvestorID = int.Parse(dt.Rows[n]["InvestorID"].ToString());
                    }
                    if (dt.Rows[n]["Enabled"].ToString() != "")
                    {
                        model.Enabled = bool.Parse(dt.Rows[n]["Enabled"].ToString());
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

        public LPWeb.Model.Company_LoanProgramDetails GetModel(int LoanProgramID, int InvestorID)
        {

            return dal.GetModel(LoanProgramID, InvestorID);
        }
        public bool UpdateEnabled(string IdList, bool enabled)
        {
            return dal.UpdateEnabled(IdList, enabled);
        }

        public bool UpdateEnabled(int LoanProgramID, int InvestorID, bool enabled)
        {
            return dal.UpdateEnabled(LoanProgramID, InvestorID, enabled);
        }

        public bool Delete(int LoanProgramID, int InvestorID)
        {
            return dal.Delete(LoanProgramID, InvestorID);
        }
    }
}

