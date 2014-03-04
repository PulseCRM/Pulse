using System;
using System.Data;
using System.Collections.Generic;
using LPWeb.Model;
using System.Text;

namespace LPWeb.BLL
{
    /// <summary>
    /// Template_Email 的摘要说明。
    /// </summary>
    public class Template_Email
    {
        private readonly LPWeb.DAL.Template_Email dal = new LPWeb.DAL.Template_Email();
        public Template_Email()
        { }

        /// <summary>
        /// get data for email template list
        /// </summary>
        public DataSet GetListForGridView(int PageSize, int PageIndex, string strWhere, out int recordCount, string orderName, int orderType)
        {
            return dal.GetListForGridView(PageSize, PageIndex, strWhere, out recordCount, orderName, orderType);
        }

        /// <summary>
        /// Disable the specified email template
        /// </summary>
        /// <param name="listIDs"></param>
        public void SetEmailTemplateDisabled(List<int> listIDs)
        {
            StringBuilder sbIds = new StringBuilder();

            foreach (int n in listIDs)
            {
                if (sbIds.Length > 0)
                    sbIds.Append(",");
                sbIds.Append(string.Format("'{0}'", n));
            }
            if (sbIds.Length > 0)
                dal.SetEmailTemplateDisabled(string.Format("TemplEmailId IN ({0})", sbIds.ToString()));
        }

        public void SetEmailTemplateEnabled(List<int> listIDs)
        {
            StringBuilder sbIds = new StringBuilder();

            foreach (int n in listIDs)
            {
                if (sbIds.Length > 0)
                    sbIds.Append(",");
                sbIds.Append(string.Format("'{0}'", n));
            }
            if (sbIds.Length > 0)
                dal.SetEmailTemplateEnabled(string.Format("TemplEmailId IN ({0})", sbIds.ToString()));
        }

        /// <summary>
        /// delete all specified email templates
        /// </summary>
        /// <param name="listIDs"></param>
        public void DeleteEmailTemplates(List<int> listIDs)
        {
            foreach (int n in listIDs)
            {
                try
                {
                    dal.DeleteEmailTemplateInfo(n);
                }
                catch
                {
                    continue;
                }
            }
        }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.Template_Email model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(LPWeb.Model.Template_Email model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int TemplEmailId)
        {

            dal.Delete(TemplEmailId);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.Template_Email GetModel(int TemplEmailId)
        {

            return dal.GetModel(TemplEmailId);
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
        public List<LPWeb.Model.Template_Email> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<LPWeb.Model.Template_Email> DataTableToList(DataTable dt)
        {
            List<LPWeb.Model.Template_Email> modelList = new List<LPWeb.Model.Template_Email>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LPWeb.Model.Template_Email model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LPWeb.Model.Template_Email();
                    if (dt.Rows[n]["TemplEmailId"].ToString() != "")
                    {
                        model.TemplEmailId = int.Parse(dt.Rows[n]["TemplEmailId"].ToString());
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
                    model.Name = dt.Rows[n]["Name"].ToString();
                    model.Desc = dt.Rows[n]["Desc"].ToString();
                    if (dt.Rows[n]["FromUserRoles"].ToString() != "")
                    {
                        model.FromUserRoles = int.Parse(dt.Rows[n]["FromUserRoles"].ToString());
                    }
                    model.FromEmailAddress = dt.Rows[n]["FromEmailAddress"].ToString();
                    model.Content = dt.Rows[n]["Content"].ToString();
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

        #region neo

        /// <summary>
        /// get email template
        /// neo 2010-11-17
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public DataTable GetEmailTemplate(string sWhere)
        {
            return dal.GetEmailTemplateBase(sWhere);
        }

        /// <summary>
        /// get email template info
        /// neo 2010-12-09
        /// </summary>
        /// <param name="iEmailTemplateID"></param>
        /// <returns></returns>
        public DataTable GetEmailTemplateInfo(int iEmailTemplateID)
        {
            return dal.GetEmailTemplateInfoBase(iEmailTemplateID);
        }

        /// <summary>
        /// 检查email template是否存在
        /// neo 2010-12-07
        /// </summary>
        /// <param name="sEmailTemplateName"></param>
        /// <returns></returns>
        public bool IsExist_Create(string sEmailTemplateName)
        {
            return dal.IsExist_CreateBase(sEmailTemplateName);
        }

        /// <summary>
        /// 检查email template是否存在
        /// neo 2010-12-09
        /// </summary>
        /// <param name="iEmailTemplateID"></param>
        /// <param name="sEmailTemplateName"></param>
        /// <returns></returns>
        public bool IsExist_Edit(int iEmailTemplateID, string sEmailTemplateName)
        {
            return dal.IsExist_EditBase(iEmailTemplateID, sEmailTemplateName);
        }

        /// <summary>
        /// insert email template
        /// neo 2010-11-17
        /// </summary>
        /// <param name="sEmailTemplateName"></param>
        /// <param name="sDesc"></param>
        /// <param name="iFromUserRoles"></param>
        /// <param name="sFromEmailAddress"></param>
        /// <param name="sContent"></param>
        /// <param name="sSubject"></param>
        /// <param name="sEmailList_To"></param>
        /// <param name="sUserRoleIDs_To"></param>
        /// <param name="sContactRoleIDs_To"></param>
        /// <param name="sEmailList_CC"></param>
        /// <param name="sUserRoleIDs_CC"></param>
        /// <param name="sContactRoleIDs_CC"></param>
        /// <param name="sTaskOwnerChecked_To"></param>
        /// <param name="sTaskOwnerChecked_CC"></param>
        public void InsertEmailTemplate(string sEmailTemplateName, string sDesc, int iFromUserRoles, string sFromEmailAddress, string sContent, string sSubject, string sEmailList_To, string sUserRoleIDs_To, string sContactRoleIDs_To, string sEmailList_CC, string sUserRoleIDs_CC, string sContactRoleIDs_CC, string sTaskOwnerChecked_To, string sTaskOwnerChecked_CC, bool chkLeadCreated, string sSenderName, int iEmailSkinID, bool Enabled)
        {
            dal.InsertEmailTemplateBase(sEmailTemplateName, sDesc, iFromUserRoles, sFromEmailAddress, sContent, sSubject, sEmailList_To, sUserRoleIDs_To, sContactRoleIDs_To, sEmailList_CC, sUserRoleIDs_CC, sContactRoleIDs_CC, sTaskOwnerChecked_To, sTaskOwnerChecked_CC, chkLeadCreated, sSenderName, iEmailSkinID, Enabled);
        }
        
        /// <summary>
        /// 获取recipient list
        /// neo 2010-12-09
        /// </summary>
        /// <param name="iEmailTemplateID"></param>
        /// <returns></returns>
        public DataTable GetRecipientList(int iEmailTemplateID)
        {
            return dal.GetRecipientListBase(iEmailTemplateID);
        }

        /// <summary>
        /// update email template
        /// neo 2010-12-09
        /// </summary>
        /// <param name="iEmailTemplateID"></param>
        /// <param name="sEmailTemplateName"></param>
        /// <param name="bEnalbed"></param>
        /// <param name="sDesc"></param>
        /// <param name="iFromUserRoles"></param>
        /// <param name="sFromEmailAddress"></param>
        /// <param name="sContent"></param>
        /// <param name="sSubject"></param>
        /// <param name="sEmailList_To"></param>
        /// <param name="sUserRoleIDs_To"></param>
        /// <param name="sContactRoleIDs_To"></param>
        /// <param name="sEmailList_CC"></param>
        /// <param name="sUserRoleIDs_CC"></param>
        /// <param name="sContactRoleIDs_CC"></param>
        /// <param name="sTaskOwnerChecked_To"></param>
        /// <param name="sTaskOwnerChecked_CC"></param>
        public void UpdateEmailTemplate(int iEmailTemplateID, string sEmailTemplateName, bool bEnalbed, string sDesc, int iFromUserRoles, string sFromEmailAddress, string sContent, string sSubject, string sEmailList_To, string sUserRoleIDs_To, string sContactRoleIDs_To, string sEmailList_CC, string sUserRoleIDs_CC, string sContactRoleIDs_CC, string sTaskOwnerChecked_To, string sTaskOwnerChecked_CC, bool chkLeadCreated, string sSenderName, int iEmailSkinID)
        {
            dal.UpdateEmailTemplateBase(iEmailTemplateID, sEmailTemplateName, bEnalbed, sDesc, iFromUserRoles, sFromEmailAddress, sContent, sSubject, sEmailList_To, sUserRoleIDs_To, sContactRoleIDs_To, sEmailList_CC, sUserRoleIDs_CC, sContactRoleIDs_CC, sTaskOwnerChecked_To, sTaskOwnerChecked_CC, chkLeadCreated, sSenderName, iEmailSkinID);
        }

        /// <summary>
        /// 检查是否被引用
        /// neo 2010-12-11
        /// </summary>
        /// <param name="iEmailTemplateID"></param>
        /// <returns></returns>
        public bool bIsRef(int iEmailTemplateID)
        {
            return dal.bIsRefBase(iEmailTemplateID);
        }

        /// <summary>
        /// delete email template
        /// neo 2010-12-11
        /// </summary>
        /// <param name="iEmailTemplateID"></param>
        public void DeleteEmailTemplate(int iEmailTemplateID)
        {
            //dal.DeleteEmailTemplateBase(iEmailTemplateID);
            dal.DeleteEmailTemplateInfo(iEmailTemplateID);
        }

        /// <summary>
        /// get point field list
        /// neo 2010-12-17
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public DataTable GetPointFieldList(string sWhere)
        {
            return dal.GetPointFieldListBase(sWhere);
        }

        #endregion
        /// <summary>
        /// Gets the email recipients.
        /// </summary>
        /// <param name="templEmailId">The templ email id.</param>
        /// <param name="fileId">The file id.</param>
        /// <returns></returns>
        public DataSet GetEmailRecipients(int templEmailId, int fileId)
        {
            return dal.GetEmailRecipients(templEmailId, fileId);
        }
    }
}

