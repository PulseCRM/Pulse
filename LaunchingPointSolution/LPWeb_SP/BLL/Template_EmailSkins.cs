using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace LPWeb.BLL
{
    public class Template_EmailSkins
    {
        private readonly LPWeb.DAL.Template_EmailSkins dal = new LPWeb.DAL.Template_EmailSkins();


        #region  Method
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int EmailSkinId)
        {
            return dal.Exists(EmailSkinId);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.Template_EmailSkins model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(LPWeb.Model.Template_EmailSkins model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int EmailSkinId)
        {

            return dal.Delete(EmailSkinId);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string EmailSkinIdlist)
        {
            return dal.DeleteList(EmailSkinIdlist);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.Template_EmailSkins GetModel(int EmailSkinId)
        {

            return dal.GetModel(EmailSkinId);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        //public LPWeb.Model.Template_EmailSkins GetModelByCache(int EmailSkinId)
        //{

        //    string CacheKey = "Template_EmailSkinsModel-" + EmailSkinId;
        //    object objModel = Maticsoft.Common.DataCache.GetCache(CacheKey);
        //    if (objModel == null)
        //    {
        //        try
        //        {
        //            objModel = dal.GetModel(EmailSkinId);
        //            if (objModel != null)
        //            {
        //                int ModelCache = Maticsoft.Common.ConfigHelper.GetConfigInt("ModelCache");
        //                Maticsoft.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
        //            }
        //        }
        //        catch { }
        //    }
        //    return (LPWeb.Model.Template_EmailSkins)objModel;
        //}

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
        public List<LPWeb.Model.Template_EmailSkins> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<LPWeb.Model.Template_EmailSkins> DataTableToList(DataTable dt)
        {
            List<LPWeb.Model.Template_EmailSkins> modelList = new List<LPWeb.Model.Template_EmailSkins>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LPWeb.Model.Template_EmailSkins model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LPWeb.Model.Template_EmailSkins();
                    if (dt.Rows[n]["EmailSkinId"].ToString() != "")
                    {
                        model.EmailSkinId = int.Parse(dt.Rows[n]["EmailSkinId"].ToString());
                    }
                    model.Name = dt.Rows[n]["Name"].ToString();
                    model.Desc = dt.Rows[n]["Desc"].ToString();
                    model.HTMLBody = dt.Rows[n]["HTMLBody"].ToString();
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
                    if (dt.Rows[n]["Default"].ToString() != "")
                    {
                        if ((dt.Rows[n]["Default"].ToString() == "1") || (dt.Rows[n]["Default"].ToString().ToLower() == "true"))
                        {
                            model.Default = true;
                        }
                        else
                        {
                            model.Default = false;
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

        public DataTable GetEmailSkinList(string sWhere, string sOrderby)
        {
            return dal.GetEmailSkinList(sWhere, sOrderby);
        }

        public DataSet GetListForGridView(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            return dal.GetListForGridView(PageSize, PageIndex, strWhere, out count, orderName, orderType);
        }

        public void SetDisable(string Ids)
        {
            dal.SetDisable(Ids);
        }

        public void SetTmpEmail_SkinIdNull(string Ids)
        {
            dal.SetTmpEmail_SkinIdNull(Ids);
        }

        #region neo

        public bool IsDulplicated_Add(string sEmailSkinName)
        {
            return dal.IsDulplicated_Add(sEmailSkinName);
        }

        public void InsertEmailSkin(string sEmailSkinName, string sDesc, string sHtmlBody, bool bEnabled, bool bDefault)
        {
            dal.InsertEmailSkin(sEmailSkinName, sDesc, sHtmlBody, bEnabled, bDefault);
        }

        public bool IsDulplicated_Edit(int iEmailSkinID, string sEmailSkinName)
        {
            return dal.IsDulplicated_Edit(iEmailSkinID, sEmailSkinName);
        }

        public void UpdateEmailSkin(int iEmailSkinID, string sEmailSkinName, string sDesc, string sHtmlBody, bool bEnabled, bool bDefault)
        {
            dal.UpdateEmailSkin(iEmailSkinID, sEmailSkinName, sDesc, sHtmlBody, bEnabled, bDefault);
        }

        public DataTable GetEmailSkinInfo(int iEmailSkinID)
        {
            return dal.GetEmailSkinInfo(iEmailSkinID);
        }

        public void CloneEmailSkin(int iEmailSkinID)
        {
            dal.CloneEmailSkin(iEmailSkinID);
        }

        public void DeleteEmailSkin(int iEmailSkinID)
        {
            dal.DeleteEmailSkin(iEmailSkinID);
        }

        #endregion
    }
}
