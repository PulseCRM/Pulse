using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// ҵ���߼���Template_Email_Recipients ��ժҪ˵����
	/// </summary>
	public class Template_Email_Recipients
	{
		private readonly LPWeb.DAL.Template_Email_Recipients dal=new LPWeb.DAL.Template_Email_Recipients();
		public Template_Email_Recipients()
		{}
		#region  ��Ա����
		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(LPWeb.Model.Template_Email_Recipients model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(LPWeb.Model.Template_Email_Recipients model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int TemplRecipientId)
		{
			
			dal.Delete(TemplRecipientId);
		}

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public LPWeb.Model.Template_Email_Recipients GetModel(int TemplRecipientId)
		{
			
			return dal.GetModel(TemplRecipientId);
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
		public List<LPWeb.Model.Template_Email_Recipients> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public List<LPWeb.Model.Template_Email_Recipients> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.Template_Email_Recipients> modelList = new List<LPWeb.Model.Template_Email_Recipients>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.Template_Email_Recipients model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.Template_Email_Recipients();
					if(dt.Rows[n]["TemplRecipientId"].ToString()!="")
					{
						model.TemplRecipientId=int.Parse(dt.Rows[n]["TemplRecipientId"].ToString());
					}
					if(dt.Rows[n]["TemplEmailId"].ToString()!="")
					{
						model.TemplEmailId=int.Parse(dt.Rows[n]["TemplEmailId"].ToString());
					}
					model.EmailAddr=dt.Rows[n]["EmailAddr"].ToString();
					model.UserRoles=dt.Rows[n]["UserRoles"].ToString();
					model.ContactRoles=dt.Rows[n]["ContactRoles"].ToString();
					model.RecipientType=dt.Rows[n]["RecipientType"].ToString();
					if(dt.Rows[n]["TaskOwner"].ToString()!="")
					{
						if((dt.Rows[n]["TaskOwner"].ToString()=="1")||(dt.Rows[n]["TaskOwner"].ToString().ToLower()=="true"))
						{
						model.TaskOwner=true;
						}
						else
						{
							model.TaskOwner=false;
						}
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

