using System;
using System.Data;
using System.Collections.Generic; 
using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// ҵ���߼���Template_Group_N_Rules ��ժҪ˵����
	/// </summary>
	public class Template_Group_N_Rules
	{
		private readonly LPWeb.DAL.Template_Group_N_Rules dal=new LPWeb.DAL.Template_Group_N_Rules();
		public Template_Group_N_Rules()
		{}
		#region  ��Ա����
		/// <summary>
		/// ����һ������
		/// </summary>
		public void Add(LPWeb.Model.Template_Group_N_Rules model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(LPWeb.Model.Template_Group_N_Rules model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int RuleGroupId,int RuleId)
		{
			
			dal.Delete(RuleGroupId,RuleId);
		}

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public LPWeb.Model.Template_Group_N_Rules GetModel(int RuleGroupId,int RuleId)
		{
			
			return dal.GetModel(RuleGroupId,RuleId);
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
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			return dal.GetList(Top,strWhere,filedOrder);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public List<LPWeb.Model.Template_Group_N_Rules> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public List<LPWeb.Model.Template_Group_N_Rules> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.Template_Group_N_Rules> modelList = new List<LPWeb.Model.Template_Group_N_Rules>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.Template_Group_N_Rules model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.Template_Group_N_Rules();
					if(dt.Rows[n]["RuleGroupId"].ToString()!="")
					{
						model.RuleGroupId=int.Parse(dt.Rows[n]["RuleGroupId"].ToString());
					}
					if(dt.Rows[n]["RuleId"].ToString()!="")
					{
						model.RuleId=int.Parse(dt.Rows[n]["RuleId"].ToString());
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
		public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		{
			return dal.GetList(PageSize,PageIndex,strWhere);
		}

		#endregion  ��Ա����
	}
}

