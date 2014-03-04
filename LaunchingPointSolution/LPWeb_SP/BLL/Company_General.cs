using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
    /// <summary>
    /// Company_General 的摘要说明。
    /// </summary>
    public class Company_General
    {
        private readonly LPWeb.DAL.Company_General dal = new LPWeb.DAL.Company_General();
        public Company_General()
        { }
        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(LPWeb.Model.Company_General model)
        {
            dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(LPWeb.Model.Company_General model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete()
        {
            //该表无主键信息，请自定义主键/条件字段
            dal.Delete();
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.Company_General GetModel()
        {
            //该表无主键信息，请自定义主键/条件字段
            return dal.GetModel();
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
        public List<LPWeb.Model.Company_General> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<LPWeb.Model.Company_General> DataTableToList(DataTable dt)
        {
            List<LPWeb.Model.Company_General> modelList = new List<LPWeb.Model.Company_General>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LPWeb.Model.Company_General model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LPWeb.Model.Company_General();
                    model.Name = dt.Rows[n]["Name"].ToString();
                    model.AD_OU_Filter = dt.Rows[n]["AD_OU_Filter"].ToString();
                    model.Desc = dt.Rows[n]["Desc"].ToString();
                    model.Address = dt.Rows[n]["Address"].ToString();
                    model.City = dt.Rows[n]["City"].ToString();
                    model.State = dt.Rows[n]["State"].ToString();
                    model.Zip = dt.Rows[n]["Zip"].ToString();
                    if (dt.Rows[n]["ImportUserInterval"].ToString() != "")
                    {
                        model.ImportUserInterval = int.Parse(dt.Rows[n]["ImportUserInterval"].ToString());
                    }
                    model.Prefix = dt.Rows[n]["Prefix"].ToString();
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

        public DataSet GetUserForCompanyOverView(int UserID)
        {
            return dal.GetUserForCompanyOverView(UserID);
        }
        public DataTable GetCompanyOverviewByUser(int Level, int typeID, string type, string strWhere)
        {
            return dal.GetCompanyOverviewByUser(Level, typeID, type, strWhere);
        }

        public bool CheckMarketingEnabled()
        {
            return dal.CheckMarketingEnabled();
        }

        public void UpdateMarketingEnabled(bool @checked)
        {
            dal.UpdateMarketingEnabled(@checked);
        }
    }
}

