using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
    /// <summary>
    /// BranchManagers ��ժҪ˵����
    /// </summary>
    public class BranchManagers
    {
        private readonly LPWeb.DAL.BranchManagers dal = new LPWeb.DAL.BranchManagers();
        public BranchManagers()
        { }
        #region  ��Ա����
        /// <summary>
        /// ����һ������
        /// </summary>
        public void Add(LPWeb.Model.BranchManagers model)
        {
            dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(LPWeb.Model.BranchManagers model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int BranchId, int BranchMgrId)
        {

            dal.Delete(BranchId, BranchMgrId);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public LPWeb.Model.BranchManagers GetModel(int BranchId, int BranchMgrId)
        {

            return dal.GetModel(BranchId, BranchMgrId);
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
        public List<LPWeb.Model.BranchManagers> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// ��������б�
        /// </summary>
        public List<LPWeb.Model.BranchManagers> DataTableToList(DataTable dt)
        {
            List<LPWeb.Model.BranchManagers> modelList = new List<LPWeb.Model.BranchManagers>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LPWeb.Model.BranchManagers model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LPWeb.Model.BranchManagers();
                    if (dt.Rows[n]["BranchId"].ToString() != "")
                    {
                        model.BranchId = int.Parse(dt.Rows[n]["BranchId"].ToString());
                    }
                    if (dt.Rows[n]["BranchMgrId"].ToString() != "")
                    {
                        model.BranchMgrId = int.Parse(dt.Rows[n]["BranchMgrId"].ToString());
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

        /// <summary>
        /// ����UserId ��ȡ��ѯ��������Branch
        /// </summary>
        /// <param name="mgrId">userid</param>
        /// <returns></returns>
        public DataTable GetBranchesByBranchMgrId(int mgrId)
        {
            return dal.GetBranchesByBranchMgrId(mgrId);
        }
    }
}

