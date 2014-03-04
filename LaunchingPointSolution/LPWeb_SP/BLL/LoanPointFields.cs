using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
    /// <summary>
    /// LoanPointFields ��ժҪ˵����
    /// </summary>
    public class LoanPointFields
    {
        private readonly LPWeb.DAL.LoanPointFields dal = new LPWeb.DAL.LoanPointFields();
        public LoanPointFields()
        { }
        #region  ��Ա����
        /// <summary>
        /// ����һ������
        /// </summary>
        public void Add(LPWeb.Model.LoanPointFields model)
        {
            dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(LPWeb.Model.LoanPointFields model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int FileId, int PointFieldId)
        {

            dal.Delete(FileId, PointFieldId);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public LPWeb.Model.LoanPointFields GetModel(int FileId, int PointFieldId)
        {

            return dal.GetModel(FileId, PointFieldId);
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
        public List<LPWeb.Model.LoanPointFields> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// ��������б�
        /// </summary>
        public List<LPWeb.Model.LoanPointFields> DataTableToList(DataTable dt)
        {
            List<LPWeb.Model.LoanPointFields> modelList = new List<LPWeb.Model.LoanPointFields>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LPWeb.Model.LoanPointFields model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LPWeb.Model.LoanPointFields();
                    if (dt.Rows[n]["FileId"].ToString() != "")
                    {
                        model.FileId = int.Parse(dt.Rows[n]["FileId"].ToString());
                    }
                    if (dt.Rows[n]["PointFieldId"].ToString() != "")
                    {
                        model.PointFieldId = int.Parse(dt.Rows[n]["PointFieldId"].ToString());
                    }
                    model.PrevValue = dt.Rows[n]["PrevValue"].ToString();
                    model.CurrentValue = dt.Rows[n]["CurrentValue"].ToString();
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


        public DataSet GetProcessingList(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType, int userID, bool accessOtherLoans)
        {
            return dal.GetProcessingList(PageSize, PageIndex, strWhere, out count, orderName, orderType, userID, accessOtherLoans);
        }

        /// <summary>
        /// get LoanPointFields.CurrentValue
        /// neo 2012-12-17
        /// </summary>
        /// <param name="iFileId"></param>
        /// <param name="iPointFieldId"></param>
        /// <returns></returns>
        public DataTable GetPointFieldInfo(int iFileId, int iPointFieldId)
        {
            return this.dal.GetPointFieldInfo(iFileId, iPointFieldId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iFileId"></param>
        /// <param name="iPointFieldId"></param>
        /// <param name="sCurrentValue"></param>
        public void UpdatePointFieldValue(int iFileId, int iPointFieldId, string sCurrentValue)
        {
            this.dal.UpdatePointFieldValue(iFileId, iPointFieldId, sCurrentValue);
        }
        public void DeletePointFields(int iFileid, string sWhere)
        {
            this.dal.DeletePointFields(iFileid, sWhere);
        }
    }
}

