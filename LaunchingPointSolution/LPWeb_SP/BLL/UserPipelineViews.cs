using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
    public class UserPipelineViews
    {
        private readonly LPWeb.DAL.UserPipelineViews dal = new LPWeb.DAL.UserPipelineViews();

        public UserPipelineViews()
		{}

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(LPWeb.Model.UserPipelineViews model)
        {
            dal.Add(model);
        }

        public bool Update(LPWeb.Model.UserPipelineViews model)
        {
           return dal.Update(model);
        }


        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere, string filedOrder)
        {
            return dal.GetList(strWhere, filedOrder);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList_ViewName(string strWhere, string filedOrder)
        {
            return dal.GetList_ViewName(strWhere, filedOrder);
        
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.UserPipelineViews GetModel(int UserPipelineViewID)
        {
            return dal.GetModel(UserPipelineViewID);
        }


    }
}
