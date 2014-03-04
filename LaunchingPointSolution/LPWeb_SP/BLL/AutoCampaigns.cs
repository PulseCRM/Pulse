using System;
using System.Data;
using System.Collections.Generic;
using LPWeb.Model;
namespace LPWeb.BLL
{
    /// <summary>
    /// ҵ���߼���AutoCampaigns ��ժҪ˵����
    /// </summary>
    public class AutoCampaigns
    {
        private readonly LPWeb.DAL.AutoCampaigns dal = new LPWeb.DAL.AutoCampaigns();
        public AutoCampaigns()
        { }
        #region  ��Ա����
        /// <summary>
        /// ����һ������
        /// </summary>
        public void Add(LPWeb.Model.AutoCampaigns model)
        {
            dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(LPWeb.Model.AutoCampaigns model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int CampaignId)
        {

            dal.Delete(CampaignId);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public LPWeb.Model.AutoCampaigns GetModel(int CampaignId)
        {

            return dal.GetModel(CampaignId);
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
        public List<LPWeb.Model.AutoCampaigns> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// ��������б�
        /// </summary>
        public List<LPWeb.Model.AutoCampaigns> DataTableToList(DataTable dt)
        {
            List<LPWeb.Model.AutoCampaigns> modelList = new List<LPWeb.Model.AutoCampaigns>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LPWeb.Model.AutoCampaigns model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LPWeb.Model.AutoCampaigns();
                    if (dt.Rows[n]["CampaignId"].ToString() != "")
                    {
                        model.CampaignId = int.Parse(dt.Rows[n]["CampaignId"].ToString());
                    }
                    if (dt.Rows[n]["PaidBy"].ToString() != "")
                    {
                        model.PaidBy = int.Parse(dt.Rows[n]["PaidBy"].ToString());
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
                    if (dt.Rows[n]["SelectedBy"].ToString() != "")
                    {
                        model.SelectedBy = int.Parse(dt.Rows[n]["SelectedBy"].ToString());
                    }
                    if (dt.Rows[n]["Started"].ToString() != "")
                    {
                        model.Started = DateTime.Parse(dt.Rows[n]["Started"].ToString());
                    }
                    model.LoanType = dt.Rows[n]["LoanType"].ToString();
                    if (dt.Rows[n]["TemplStageId"].ToString() != "")
                    {
                        model.TemplStageId = int.Parse(dt.Rows[n]["TemplStageId"].ToString());
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

        public DataSet GetMarketingList()
        {
            return dal.GetMarketingList();
        }

        #endregion  ��Ա����

        public int GetMarketingCount(string sWhere)
        {
            return dal.GetMarketingCount(sWhere);
        }

        public DataSet GetMarketingList(int pageCount, int currentPageIndex, string condition)
        {
            return dal.GetMarketingList(pageCount, currentPageIndex, condition);
        }
        public DataSet GetMarketingList(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            return dal.GetMarketingList(PageSize, PageIndex, strWhere, out count, orderName, orderType);

        }

        /// <summary>
        /// Clear all company marketing data
        /// Rocky
        /// </summary>
        public void DeleteAll(string sType)
        {

            dal.DeleteAll(sType);
        }
    }
}

