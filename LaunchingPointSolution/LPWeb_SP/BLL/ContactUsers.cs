using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// ҵ���߼���ContactUsers ��ժҪ˵����
	/// </summary>
	public class ContactUsers
	{
		private readonly LPWeb.DAL.ContactUsers dal=new LPWeb.DAL.ContactUsers();
		public ContactUsers()
		{}
		#region  ��Ա����
		/// <summary>
		/// ����һ������
		/// </summary>
		public void Add(LPWeb.Model.ContactUsers model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(LPWeb.Model.ContactUsers model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int UserId,int ContactId)
		{
			
			dal.Delete(UserId,ContactId);
		}

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public LPWeb.Model.ContactUsers GetModel(int UserId,int ContactId)
		{
			
			return dal.GetModel(UserId,ContactId);
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
		public List<LPWeb.Model.ContactUsers> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public List<LPWeb.Model.ContactUsers> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.ContactUsers> modelList = new List<LPWeb.Model.ContactUsers>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.ContactUsers model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.ContactUsers();
					if(dt.Rows[n]["UserId"].ToString()!="")
					{
						model.UserId=int.Parse(dt.Rows[n]["UserId"].ToString());
					}
					if(dt.Rows[n]["ContactId"].ToString()!="")
					{
						model.ContactId=int.Parse(dt.Rows[n]["ContactId"].ToString());
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
					if(dt.Rows[n]["Created"].ToString()!="")
					{
						model.Created=DateTime.Parse(dt.Rows[n]["Created"].ToString());
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

        public int GetUserContactCount(int nUserId)
        {
            return dal.GetUserContactCount(nUserId);
        }
	}
}

