using System;
using System.Data;
using System.Collections.Generic;
using LPWeb.Model;
namespace LPWeb.BLL
{
    /// <summary>
    /// ҵ���߼���ServiceTypes ��ժҪ˵����
    /// </summary>
    public class ServiceTypes
    {
        private readonly LPWeb.DAL.ServiceTypes dal = new LPWeb.DAL.ServiceTypes();
        public ServiceTypes()
        { }

        #region  ��Ա����
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(LPWeb.Model.ServiceTypes model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(LPWeb.Model.ServiceTypes model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int ServiceTypeId)
        {

            dal.Delete(ServiceTypeId);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public LPWeb.Model.ServiceTypes GetModel(int ServiceTypeId)
        {

            return dal.GetModel(ServiceTypeId);
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        }
        /// <summary>
        /// ���ǰ��������
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            return dal.GetList(Top, strWhere, filedOrder);
        }
        /// <summary>
        /// ��������б�
        /// </summary>
        public List<LPWeb.Model.ServiceTypes> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// ��������б�
        /// </summary>
        public List<LPWeb.Model.ServiceTypes> DataTableToList(DataTable dt)
        {
            List<LPWeb.Model.ServiceTypes> modelList = new List<LPWeb.Model.ServiceTypes>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LPWeb.Model.ServiceTypes model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LPWeb.Model.ServiceTypes();
                    if (dt.Rows[n]["ServiceTypeId"].ToString() != "")
                    {
                        model.ServiceTypeId = int.Parse(dt.Rows[n]["ServiceTypeId"].ToString());
                    }
                    model.Name = dt.Rows[n]["Name"].ToString();
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
        /// ��������б�
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataSet GetList(int PageSize, int PageIndex, string strWhere)
        {
            return dal.GetList(PageSize, PageIndex, strWhere);
        }

        #endregion  ��Ա����

        #region neo

        /// <summary>
        /// get service type list
        /// neo 2011-04-06
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public DataTable GetServiceTypeList(string sWhere)
        {
            return dal.GetServiceTypeListBase(sWhere);
        }

        #endregion
        #region changke
        /// <summary>
        /// 
        /// </summary>
        public DataSet GetServiceTypes(int PageSize, int PageIndex, string strWhere, out int recordCount, string orderName, int orderType)
        {
            return dal.GetServiceTypes(PageSize, PageIndex, strWhere, out recordCount, orderName, orderType);
        }

        public bool IsSerivceTypeExsits(string strName)
        {
            return dal.IsSerivceTypeExsits(strName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strIds"></param>
        public void DeleteServiceType(string strIds)
        {
            List<int> listAllIds = GetAllIDs(strIds);
            if (listAllIds.Count > 0)
                dal.DeleteServiceType(listAllIds);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strIds"></param>
        public void EnableServiceType(string strIds)
        {
            List<int> listAllIds = GetAllIDs(strIds);
            if (listAllIds.Count > 0)
                dal.EnableServiceType(listAllIds);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strIds"></param>
        public void DisableServiceType(string strIds)
        {
            List<int> listAllIds = GetAllIDs(strIds);
            if (listAllIds.Count > 0)
                dal.DisableServiceType(listAllIds);
        }

        private List<int> GetAllIDs(string strAllIds)
        {
            List<int> listIds = new List<int>();
            foreach (string str in strAllIds.Split(new char[] { ',' }))
            {
                int nTemp = -1;
                if (!int.TryParse(str, out nTemp))
                    nTemp = -1;
                if (nTemp != -1)
                    listIds.Add(nTemp);
            }
            return listIds;
        }
        #endregion
    }
}

