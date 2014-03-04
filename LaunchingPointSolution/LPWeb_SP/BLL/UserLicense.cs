using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
using System.Text;
using LPWeb.DAL;

namespace LPWeb.BLL
{
    public class UserLicenses
    {
        private readonly LPWeb.DAL.UserLicenses dal = new LPWeb.DAL.UserLicenses();

        #region  成员方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.UserLicenses model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(LPWeb.Model.UserLicenses model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int UserLicenseId)
        {
            dal.Delete(UserLicenseId);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.UserLicenses GetModel(int UserLicenseId)
        {

            return dal.GetModel(UserLicenseId);
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
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetList(int PageSize, int PageIndex, string strWhere)
        {
            return dal.GetList(PageSize, PageIndex, strWhere);
        }

        #endregion  成员方法

        public void UpdatebatchByUserID(List<Model.UserLicenses> userLin)
        {
            dal.UpdatebatchByUserID(userLin);
        }

    }

}