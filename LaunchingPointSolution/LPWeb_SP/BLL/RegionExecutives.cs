using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
    /// <summary>
    /// RegionExecutives ��ժҪ˵����
    /// </summary>
    public class RegionExecutives
    {
        private readonly LPWeb.DAL.RegionExecutives dal = new LPWeb.DAL.RegionExecutives();
        public RegionExecutives()
        { }
        #region  ��Ա����
        /// <summary>
        /// ����һ������
        /// </summary>
        public void Add(LPWeb.Model.RegionExecutives model)
        {
            dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(LPWeb.Model.RegionExecutives model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int RegionId, int ExecutiveId)
        {

            dal.Delete(RegionId, ExecutiveId);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public LPWeb.Model.RegionExecutives GetModel(int RegionId, int ExecutiveId)
        {

            return dal.GetModel(RegionId, ExecutiveId);
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
        public List<LPWeb.Model.RegionExecutives> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// ��������б�
        /// </summary>
        public List<LPWeb.Model.RegionExecutives> DataTableToList(DataTable dt)
        {
            List<LPWeb.Model.RegionExecutives> modelList = new List<LPWeb.Model.RegionExecutives>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LPWeb.Model.RegionExecutives model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LPWeb.Model.RegionExecutives();
                    if (dt.Rows[n]["RegionId"].ToString() != "")
                    {
                        model.RegionId = int.Parse(dt.Rows[n]["RegionId"].ToString());
                    }
                    if (dt.Rows[n]["ExecutiveId"].ToString() != "")
                    {
                        model.ExecutiveId = int.Parse(dt.Rows[n]["ExecutiveId"].ToString());
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

        /// <summary>
        /// ����UserId ��ȡ��ѯ��������regions
        /// </summary>
        /// <param name="executiveId">userid</param>
        /// <returns></returns>
        public DataTable GetRegionsByExecutiveId(int executiveId)
        {
            return dal.GetRegionsByExecutiveId(executiveId);
        }

        #endregion  ��Ա����
    }
}

