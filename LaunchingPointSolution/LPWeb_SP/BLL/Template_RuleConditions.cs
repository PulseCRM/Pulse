using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// Template_RuleConditions ��ժҪ˵����
	/// </summary>
	public class Template_RuleConditions
	{
		private readonly LPWeb.DAL.Template_RuleConditions dal=new LPWeb.DAL.Template_RuleConditions();
		public Template_RuleConditions()
        { }
        #region  ��Ա����
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(LPWeb.Model.Template_RuleConditions model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(LPWeb.Model.Template_RuleConditions model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int RuleCondId)
        {

            dal.Delete(RuleCondId);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public LPWeb.Model.Template_RuleConditions GetModel(int RuleCondId)
        {

            return dal.GetModel(RuleCondId);
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
        public List<LPWeb.Model.Template_RuleConditions> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// ��������б�
        /// </summary>
        public List<LPWeb.Model.Template_RuleConditions> DataTableToList(DataTable dt)
        {
            List<LPWeb.Model.Template_RuleConditions> modelList = new List<LPWeb.Model.Template_RuleConditions>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LPWeb.Model.Template_RuleConditions model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LPWeb.Model.Template_RuleConditions();
                    if (dt.Rows[n]["RuleCondId"].ToString() != "")
                    {
                        model.RuleCondId = int.Parse(dt.Rows[n]["RuleCondId"].ToString());
                    }
                    if (dt.Rows[n]["RuleId"].ToString() != "")
                    {
                        model.RuleId = int.Parse(dt.Rows[n]["RuleId"].ToString());
                    }
                    if (dt.Rows[n]["PointFieldId"].ToString() != "")
                    {
                        model.PointFieldId = decimal.Parse(dt.Rows[n]["PointFieldId"].ToString());
                    }
                    if (dt.Rows[n]["Condition"].ToString() != "")
                    {
                        model.Condition = int.Parse(dt.Rows[n]["Condition"].ToString());
                    }
                    model.Tolerance = dt.Rows[n]["Tolerance"].ToString();
                    model.ToleranceType = dt.Rows[n]["ToleranceType"].ToString();
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
        public DataSet GetList(int PageSize, int PageIndex, string strWhere)
        {
            return dal.GetList(PageSize, PageIndex, strWhere);
        }

        #endregion  ��Ա����
	}
}

