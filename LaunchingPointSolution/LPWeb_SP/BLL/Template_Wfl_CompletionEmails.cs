using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;

namespace LPWeb.BLL
{
    /// <summary>
    /// Template_Wfl_CompletionEmails
    /// </summary>
    public partial class Template_Wfl_CompletionEmails
    {
        private readonly LPWeb.DAL.Template_Wfl_CompletionEmailsBase dal = new LPWeb.DAL.Template_Wfl_CompletionEmailsBase();
        public Template_Wfl_CompletionEmails()
        { }
        #region  Method
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int CompletionEmailId)
        {
            return dal.Exists(CompletionEmailId);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.Template_Wfl_CompletionEmails model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(LPWeb.Model.Template_Wfl_CompletionEmails model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int CompletionEmailId)
        {

            return dal.Delete(CompletionEmailId);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string CompletionEmailIdlist)
        {
            return dal.DeleteList(CompletionEmailIdlist);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.Template_Wfl_CompletionEmails GetModel(int CompletionEmailId)
        {

            return dal.GetModel(CompletionEmailId);
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
        public List<LPWeb.Model.Template_Wfl_CompletionEmails> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<LPWeb.Model.Template_Wfl_CompletionEmails> DataTableToList(DataTable dt)
        {
            List<LPWeb.Model.Template_Wfl_CompletionEmails> modelList = new List<LPWeb.Model.Template_Wfl_CompletionEmails>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LPWeb.Model.Template_Wfl_CompletionEmails model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LPWeb.Model.Template_Wfl_CompletionEmails();
                    if (dt.Rows[n]["CompletionEmailId"] != null && dt.Rows[n]["CompletionEmailId"].ToString() != "")
                    {
                        model.CompletionEmailId = int.Parse(dt.Rows[n]["CompletionEmailId"].ToString());
                    }
                    if (dt.Rows[n]["TemplTaskid"] != null && dt.Rows[n]["TemplTaskid"].ToString() != "")
                    {
                        model.TemplTaskid = int.Parse(dt.Rows[n]["TemplTaskid"].ToString());
                    }
                    if (dt.Rows[n]["TemplEmailId"] != null && dt.Rows[n]["TemplEmailId"].ToString() != "")
                    {
                        model.TemplEmailId = int.Parse(dt.Rows[n]["TemplEmailId"].ToString());
                    }
                    if (dt.Rows[n]["Enabled"] != null && dt.Rows[n]["Enabled"].ToString() != "")
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

