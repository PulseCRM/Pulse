using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
    /// <summary>
    /// ҵ���߼���Template_Stages ��ժҪ˵����
    /// </summary>
    public class Template_Stages
    {
        private readonly LPWeb.DAL.Template_Stages dal = new LPWeb.DAL.Template_Stages();
        public Template_Stages()
        { }
        #region  ��Ա����
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int TemplStageId)
        {
            return dal.Exists(TemplStageId);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(LPWeb.Model.Template_Stages model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(LPWeb.Model.Template_Stages model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int TemplStageId)
        {

            dal.Delete(TemplStageId);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public LPWeb.Model.Template_Stages GetModel(int TemplStageId)
        {

            return dal.GetModel(TemplStageId);
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
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
        public List<LPWeb.Model.Template_Stages> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(0, strWhere, "Name");
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// ��������б�
        /// </summary>
        public List<LPWeb.Model.Template_Stages> DataTableToList(DataTable dt)
        {
            List<LPWeb.Model.Template_Stages> modelList = new List<LPWeb.Model.Template_Stages>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LPWeb.Model.Template_Stages model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LPWeb.Model.Template_Stages();
                    if (dt.Rows[n]["TemplStageId"].ToString() != "")
                    {
                        model.TemplStageId = int.Parse(dt.Rows[n]["TemplStageId"].ToString());
                    }
                    model.Name = dt.Rows[n]["Name"].ToString();
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
                    if (dt.Rows[n]["SequenceNumber"].ToString() != "")
                    {
                        model.SequenceNumber = int.Parse(dt.Rows[n]["SequenceNumber"].ToString());
                    }
                    model.WorkflowType = dt.Rows[n]["WorkflowType"].ToString();
                    if (dt.Rows[n]["Custom"].ToString() != "")
                    {
                        if ((dt.Rows[n]["Custom"].ToString() == "1") || (dt.Rows[n]["Custom"].ToString().ToLower() == "true"))
                        {
                            model.Custom = true;
                        }
                        else
                        {
                            model.Custom = false;
                        }
                    }
                    if (dt.Rows[n]["PointStageNameField"].ToString() != "")
                    {
                        model.PointStageNameField = int.Parse(dt.Rows[n]["PointStageNameField"].ToString());
                    }
                    if (dt.Rows[n]["PointStageDateField"].ToString() != "")
                    {
                        model.PointStageDateField = int.Parse(dt.Rows[n]["PointStageDateField"].ToString());
                    }
                    model.Alias = dt.Rows[n]["Alias"].ToString();
                    if (dt.Rows[n]["DaysFromEstClose"].ToString() != "")
                    {
                        model.DaysFromEstClose = int.Parse(dt.Rows[n]["DaysFromEstClose"].ToString());
                    }
                    if (dt.Rows[n]["DaysFromCreation"].ToString() != "")
                    {
                        model.DaysFromCreation = int.Parse(dt.Rows[n]["DaysFromCreation"].ToString());
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



        #endregion  ��Ա����

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataSet GetStageList(int PageSize, int PageIndex, string strWhere, out int recordCount, string orderName, int orderType)
        {
            return dal.GetStageList(PageSize, PageIndex, strWhere, out recordCount, orderName, orderType);
        }

        #region neo

        /// <summary>
        /// get stage template list by WorkflowType
        /// neo 2011-02-09
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public DataTable GetStageTemplateList(string sWhere)
        {
            return dal.GetStageTemplateListBase(sWhere);
        }

        /// <summary>
        /// get worflow stage info
        /// neo 2011-02-16
        /// </summary>
        /// <param name="iWflStageID"></param>
        /// <returns></returns>
        public DataTable GetStageTemplateInfo(int iWflStageID)
        {
            return dal.GetStageTemplateInfoBase(iWflStageID);

        }

        /// <summary>
        /// Get max sequence by workflow type
        /// Rocky 2011-2-18
        /// </summary>
        /// <param name="sType"></param>
        /// <returns></returns>
        public int GetMaxSequence(string sType)
        {
            return dal.GetMaxSequenceBase(sType);
        }

        /// <summary>
        /// disable stage template
        /// </summary>
        /// <param name="StageIDs"></param>
        public void DisableStageTemplates(string StageIDs)
        {
            dal.DisableStageTemplates(StageIDs);
        }

        /// <summary>
        /// delete stage template
        /// </summary>
        /// <param name="StageIDs"></param>
        public void DeleteStageTemplates(string StageIDs)
        {
            dal.DeleteStageTemplates(StageIDs);
        }
        #endregion
    }
}

