using System;
using System.Data;
using System.Collections.Generic;
//using Maticsoft.Common;
using LPWeb.Model;
namespace LPWeb.BLL
{
    /// <summary>
    /// Company_EasyMortgage
    /// </summary>
    public class Company_EasyMortgage
    {
        private readonly LPWeb.DAL.Company_EasyMortgage dal = new LPWeb.DAL.Company_EasyMortgage();
        public Company_EasyMortgage()
        { }
        #region  Method
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists()
        {
            return dal.Exists();
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(LPWeb.Model.Company_EasyMortgage model)
        {
            dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(LPWeb.Model.Company_EasyMortgage model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete()
        {

            return dal.Delete();
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.Company_EasyMortgage GetModel()
        {

            return dal.GetModel();
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        //public LPWeb.Model.Company_EasyMortgage GetModelByCache()
        //{
			
        //    string CacheKey = "Company_EasyMortgageModel-" + ;
        //    object objModel = Maticsoft.Common.DataCache.GetCache(CacheKey);
        //    if (objModel == null)
        //    {
        //        try
        //        {
        //            objModel = dal.GetModel();
        //            if (objModel != null)
        //            {
        //                int ModelCache = Maticsoft.Common.ConfigHelper.GetConfigInt("ModelCache");
        //                Maticsoft.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
        //            }
        //        }
        //        catch{}
        //    }
        //    return (LPWeb.Model.Company_EasyMortgage)objModel;
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
        public List<LPWeb.Model.Company_EasyMortgage> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<LPWeb.Model.Company_EasyMortgage> DataTableToList(DataTable dt)
        {
            List<LPWeb.Model.Company_EasyMortgage> modelList = new List<LPWeb.Model.Company_EasyMortgage>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LPWeb.Model.Company_EasyMortgage model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LPWeb.Model.Company_EasyMortgage();
                    model.ClientID = dt.Rows[n]["ClientID"].ToString();
                    model.URL = dt.Rows[n]["URL"].ToString();
                    if (dt.Rows[n]["SyncIntervalHours"].ToString() != "")
                    {
                        model.SyncIntervalHours = int.Parse(dt.Rows[n]["SyncIntervalHours"].ToString());
                    }
                    if (dt.Rows[n]["Enabled"].ToString() != "")
                    {
                        if ((dt.Rows[n]["Enabled"].ToString() == "1") || (dt.Rows[n]["Enabled"].ToString().ToLower() == "true"))
                        {
                            model.Enabled = true;
                        }
                        else
                        {
                            model.Enabled = false;
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
        /// 分页获取数据列表
        /// </summary>
        //public DataSet GetList(int PageSize,int PageIndex,string strWhere)
        //{
        //return dal.GetList(PageSize,PageIndex,strWhere);
        //}

        #endregion  Method
    }
}

