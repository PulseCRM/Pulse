using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
    /// <summary>
    /// BranchManagers 的摘要说明。
    /// </summary>
    public class BranchManagers
    {
        private readonly LPWeb.DAL.BranchManagers dal = new LPWeb.DAL.BranchManagers();
        public BranchManagers()
        { }
        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(LPWeb.Model.BranchManagers model)
        {
            dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(LPWeb.Model.BranchManagers model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int BranchId, int BranchMgrId)
        {

            dal.Delete(BranchId, BranchMgrId);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.BranchManagers GetModel(int BranchId, int BranchMgrId)
        {

            return dal.GetModel(BranchId, BranchMgrId);
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
        public List<LPWeb.Model.BranchManagers> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
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

        /// <summary>
        /// 根据UserId 获取查询其所属的Branch
        /// </summary>
        /// <param name="mgrId">userid</param>
        /// <returns></returns>
        public DataTable GetBranchesByBranchMgrId(int mgrId)
        {
            return dal.GetBranchesByBranchMgrId(mgrId);
        }
    }
}

