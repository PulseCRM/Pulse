using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// ҵ���߼���WebAccounts ��ժҪ˵����
	/// </summary>
	public class WebAccounts
	{
		private readonly LPWeb.DAL.WebAccounts dal=new LPWeb.DAL.WebAccounts();
		public WebAccounts()
		{}
		#region  ��Ա����
		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(LPWeb.Model.WebAccounts model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(LPWeb.Model.WebAccounts model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int WebAccountId)
		{
			
			dal.Delete(WebAccountId);
		}

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public LPWeb.Model.WebAccounts GetModel(int WebAccountId)
		{
			
			return dal.GetModel(WebAccountId);
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
		public List<LPWeb.Model.WebAccounts> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public List<LPWeb.Model.WebAccounts> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.WebAccounts> modelList = new List<LPWeb.Model.WebAccounts>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.WebAccounts model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.WebAccounts();
					if(dt.Rows[n]["WebAccountId"].ToString()!="")
					{
						model.WebAccountId=int.Parse(dt.Rows[n]["WebAccountId"].ToString());
					}
					if(dt.Rows[n]["Enabled"].ToString()!="")
					{
						if((dt.Rows[n]["Enabled"].ToString()=="1")||(dt.Rows[n]["Enabled"].ToString().ToLower()=="true"))
						{
						model.Enabled=true;
						}
						else
						{
							model.Enabled=false;
						}
					}
					if(dt.Rows[n]["LastLogin"].ToString()!="")
					{
						model.LastLogin=DateTime.Parse(dt.Rows[n]["LastLogin"].ToString());
					}
					model.Password=dt.Rows[n]["Password"].ToString();
					model.PasswordQuestion=dt.Rows[n]["PasswordQuestion"].ToString();
					model.PasswordAnswer=dt.Rows[n]["PasswordAnswer"].ToString();
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

