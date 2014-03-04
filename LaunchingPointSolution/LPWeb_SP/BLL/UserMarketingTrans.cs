using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
    /// <summary>
    /// 业务逻辑类UserMarketingTrans 的摘要说明。
    /// </summary>
    public class UserMarketingTrans
    {
        private readonly LPWeb.DAL.UserMarketingTrans dal = new LPWeb.DAL.UserMarketingTrans();
        public UserMarketingTrans()
		{}
		#region  成员方法
		/// <summary>
		/// 增加一条数据
		/// </summary>
        public void Add(LPWeb.Model.UserMarketingTrans model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
        public void Update(LPWeb.Model.UserMarketingTrans model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int UserId)
		{
			
			dal.Delete(UserId);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
        public LPWeb.Model.UserMarketingTrans GetModel(int UserId)
		{
			
			return dal.GetModel(UserId);
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
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			return dal.GetList(Top,strWhere,filedOrder);
		}

        #endregion  成员方法

        public DataSet GetListForGridView(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            return dal.GetListForGridView(PageSize, PageIndex, strWhere, out count, orderName, orderType);
        }
    }
}
