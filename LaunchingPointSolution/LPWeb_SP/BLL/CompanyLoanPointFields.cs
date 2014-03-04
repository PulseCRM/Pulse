using System;
using System.Data;
using System.Collections.Generic;
using LPWeb.Model;
namespace LPWeb.BLL
{
    /// <summary>
    /// CompanyLoanPointFields
    /// </summary>
    public class CompanyLoanPointFields
    {
        private readonly LPWeb.DAL.CompanyLoanPointFields dal = new LPWeb.DAL.CompanyLoanPointFields();
        public CompanyLoanPointFields()
        { }
        #region  Method
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int PointFieldId)
        {
            return dal.Exists(PointFieldId);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(LPWeb.Model.CompanyLoanPointFields model)
        {
            dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(LPWeb.Model.CompanyLoanPointFields model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int PointFieldId)
        {

            return dal.Delete(PointFieldId);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string PointFieldIdlist)
        {
            return dal.DeleteList(PointFieldIdlist);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.CompanyLoanPointFields GetModel(int PointFieldId)
        {

            return dal.GetModel(PointFieldId);
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
        public List<LPWeb.Model.CompanyLoanPointFields> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<LPWeb.Model.CompanyLoanPointFields> DataTableToList(DataTable dt)
        {
            List<LPWeb.Model.CompanyLoanPointFields> modelList = new List<LPWeb.Model.CompanyLoanPointFields>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LPWeb.Model.CompanyLoanPointFields model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LPWeb.Model.CompanyLoanPointFields();
                    if (dt.Rows[n]["PointFieldId"].ToString() != "")
                    {
                        model.PointFieldId = int.Parse(dt.Rows[n]["PointFieldId"].ToString());
                    }
                    model.Heading = dt.Rows[n]["Heading"].ToString();
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

        public DataTable GetCompanyLoanPointFieldsInfo()
        {
            return dal.GetCompanyLoanPointFieldsInfo();
        }
    }
}

