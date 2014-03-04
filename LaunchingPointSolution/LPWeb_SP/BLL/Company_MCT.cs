using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;

namespace LPWeb.BLL
{
    public class Company_MCT
    {
        private readonly LPWeb.DAL.Company_MCT dal = new LPWeb.DAL.Company_MCT();
        public Company_MCT()
        { }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(LPWeb.Model.Company_MCT model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(LPWeb.Model.Company_MCT model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string ClientID, string PostURL, bool PostDataEnabled, int ActiveLoanInterval, int ArchivedLoanInterval, int ArchivedLoanDisposeMonth, string ArchivedLoanStatuses)
        {

            return dal.Delete(ClientID, PostURL, PostDataEnabled, ActiveLoanInterval, ArchivedLoanInterval, ArchivedLoanDisposeMonth, ArchivedLoanStatuses);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.Company_MCT GetModel(string ClientID, string PostURL, bool PostDataEnabled, int ActiveLoanInterval, int ArchivedLoanInterval, int ArchivedLoanDisposeMonth, string ArchivedLoanStatuses)
        {

            return dal.GetModel(ClientID, PostURL, PostDataEnabled, ActiveLoanInterval, ArchivedLoanInterval, ArchivedLoanDisposeMonth, ArchivedLoanStatuses);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.Company_MCT GetModel()
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
        public List<LPWeb.Model.Company_MCT> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<LPWeb.Model.Company_MCT> DataTableToList(DataTable dt)
        {
            List<LPWeb.Model.Company_MCT> modelList = new List<LPWeb.Model.Company_MCT>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LPWeb.Model.Company_MCT model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LPWeb.Model.Company_MCT();
                    if (dt.Rows[n]["ClientID"] != null && dt.Rows[n]["ClientID"].ToString() != "")
                    {
                        model.ClientID = dt.Rows[n]["ClientID"].ToString();
                    }
                    if (dt.Rows[n]["PostURL"] != null && dt.Rows[n]["PostURL"].ToString() != "")
                    {
                        model.PostURL = dt.Rows[n]["PostURL"].ToString();
                    }
                    if (dt.Rows[n]["PostDataEnabled"] != null && dt.Rows[n]["PostDataEnabled"].ToString() != "")
                    {
                        if ((dt.Rows[n]["PostDataEnabled"].ToString() == "1") || (dt.Rows[n]["PostDataEnabled"].ToString().ToLower() == "true"))
                        {
                            model.PostDataEnabled = true;
                        }
                        else
                        {
                            model.PostDataEnabled = false;
                        }
                    }
                    if (dt.Rows[n]["ActiveLoanInterval"] != null && dt.Rows[n]["ActiveLoanInterval"].ToString() != "")
                    {
                        model.ActiveLoanInterval = int.Parse(dt.Rows[n]["ActiveLoanInterval"].ToString());
                    }
                    if (dt.Rows[n]["ArchivedLoanInterval"] != null && dt.Rows[n]["ArchivedLoanInterval"].ToString() != "")
                    {
                        model.ArchivedLoanInterval = int.Parse(dt.Rows[n]["ArchivedLoanInterval"].ToString());
                    }
                    if (dt.Rows[n]["ArchivedLoanDisposeMonth"] != null && dt.Rows[n]["ArchivedLoanDisposeMonth"].ToString() != "")
                    {
                        model.ArchivedLoanDisposeMonth = int.Parse(dt.Rows[n]["ArchivedLoanDisposeMonth"].ToString());
                    }
                    if (dt.Rows[n]["ArchivedLoanStatuses"] != null && dt.Rows[n]["ArchivedLoanStatuses"].ToString() != "")
                    {
                        model.ArchivedLoanStatuses = dt.Rows[n]["ArchivedLoanStatuses"].ToString();
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
        public int GetRecordCount(string strWhere)
        {
            return dal.GetRecordCount(strWhere);
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            return dal.GetListByPage(strWhere, orderby, startIndex, endIndex);
        }

        /// <summary>
        /// Save Data
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Save(LPWeb.Model.Company_MCT model)
        {
            return dal.Save(model);
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {
            return dal.Delete();
        }
    }
}
