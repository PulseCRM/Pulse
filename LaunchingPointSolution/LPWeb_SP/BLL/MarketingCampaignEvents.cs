using System;
using System.Data;
using System.Collections.Generic;
using LPWeb.Model;
namespace LPWeb.BLL
{
    /// <summary>
    /// MarketingCampaignEvents
    /// </summary>
    public class MarketingCampaignEvents
    {
        private readonly LPWeb.DAL.MarketingCampaignEvents dal = new LPWeb.DAL.MarketingCampaignEvents();
        public MarketingCampaignEvents()
        { }
        #region  Method
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.MarketingCampaignEvents model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(LPWeb.Model.MarketingCampaignEvents model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int CampaignEventId)
        {

            return dal.Delete(CampaignEventId);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string CampaignEventIdlist)
        {
            return dal.DeleteList(CampaignEventIdlist);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.MarketingCampaignEvents GetModel(int CampaignEventId)
        {

            return dal.GetModel(CampaignEventId);
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
        public List<LPWeb.Model.MarketingCampaignEvents> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<LPWeb.Model.MarketingCampaignEvents> DataTableToList(DataTable dt)
        {
            List<LPWeb.Model.MarketingCampaignEvents> modelList = new List<LPWeb.Model.MarketingCampaignEvents>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LPWeb.Model.MarketingCampaignEvents model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LPWeb.Model.MarketingCampaignEvents();
                    if (dt.Rows[n]["CampaignEventId"].ToString() != "")
                    {
                        model.CampaignEventId = int.Parse(dt.Rows[n]["CampaignEventId"].ToString());
                    }
                    if (dt.Rows[n]["WeekNo"].ToString() != "")
                    {
                        model.WeekNo = int.Parse(dt.Rows[n]["WeekNo"].ToString());
                    }
                    model.Action = dt.Rows[n]["Action"].ToString();
                    model.EventContent = dt.Rows[n]["EventContent"].ToString();
                    model.EventURL = dt.Rows[n]["EventURL"].ToString();
                    model.GlobalId = dt.Rows[n]["GlobalId"].ToString();
                    if (dt.Rows[n]["CampaignId"].ToString() != "")
                    {
                        model.CampaignId = int.Parse(dt.Rows[n]["CampaignId"].ToString());
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

