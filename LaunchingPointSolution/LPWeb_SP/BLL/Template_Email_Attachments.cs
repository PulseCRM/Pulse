using System;
using System.Data;
using System.Collections.Generic;
using LPWeb.Model;
namespace LPWeb.BLL
{
    /// <summary>
    /// ҵ���߼���Template_Email_Attachments ��ժҪ˵����
    /// </summary>
    public class Template_Email_Attachments
    {
        private readonly LPWeb.DAL.Template_Email_Attachments dal = new LPWeb.DAL.Template_Email_Attachments();
        public Template_Email_Attachments()
        { }
        #region  ��Ա����
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(LPWeb.Model.Template_Email_Attachments model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(LPWeb.Model.Template_Email_Attachments model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int TemplAttachId)
        {

            dal.Delete(TemplAttachId);
        }
        /// <summary>
        /// ɾ����������
        /// </summary>
        public void DeleteIDList(string IDList)
        {
            dal.DeleteIDList(IDList);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public LPWeb.Model.Template_Email_Attachments GetModel(int TemplAttachId)
        {

            return dal.GetModel(TemplAttachId);
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        }

        public DataSet GetListWithOutFileImage(string strWhere)
        {
            return dal.GetListWithOutFileImage(strWhere);
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
        public List<LPWeb.Model.Template_Email_Attachments> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// ��������б�
        /// </summary>
        public List<LPWeb.Model.Template_Email_Attachments> DataTableToList(DataTable dt)
        {
            List<LPWeb.Model.Template_Email_Attachments> modelList = new List<LPWeb.Model.Template_Email_Attachments>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LPWeb.Model.Template_Email_Attachments model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LPWeb.Model.Template_Email_Attachments();
                    if (dt.Rows[n]["TemplAttachId"].ToString() != "")
                    {
                        model.TemplAttachId = int.Parse(dt.Rows[n]["TemplAttachId"].ToString());
                    }
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
                    model.FileType = dt.Rows[n]["FileType"].ToString();
                    if (dt.Rows[n]["FileImage"].ToString() != "")
                    {
                        model.FileImage = (byte[])dt.Rows[n]["FileImage"];
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
        //public DataSet GetList(int PageSize,int PageIndex,string strWhere)
        //{
        //return dal.GetList(PageSize,PageIndex,strWhere);
        //}

        #endregion  ��Ա����
    }
}

