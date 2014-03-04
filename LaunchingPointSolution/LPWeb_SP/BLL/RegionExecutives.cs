using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
    /// <summary>
    /// RegionExecutives 的摘要说明。
    /// </summary>
    public class RegionExecutives
    {
        private readonly LPWeb.DAL.RegionExecutives dal = new LPWeb.DAL.RegionExecutives();
        public RegionExecutives()
        { }
        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(LPWeb.Model.RegionExecutives model)
        {
            dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(LPWeb.Model.RegionExecutives model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int RegionId, int ExecutiveId)
        {

            dal.Delete(RegionId, ExecutiveId);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.RegionExecutives GetModel(int RegionId, int ExecutiveId)
        {

            return dal.GetModel(RegionId, ExecutiveId);
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
        public List<LPWeb.Model.RegionExecutives> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
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

        /// <summary>
        /// 根据UserId 获取查询其所属的regions
        /// </summary>
        /// <param name="executiveId">userid</param>
        /// <returns></returns>
        public DataTable GetRegionsByExecutiveId(int executiveId)
        {
            return dal.GetRegionsByExecutiveId(executiveId);
        }

        #endregion  成员方法
    }
}

