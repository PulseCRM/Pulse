using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// GroupUsers ��ժҪ˵����
	/// </summary>
	public class GroupUsers
	{
		private readonly LPWeb.DAL.GroupUsers dal=new LPWeb.DAL.GroupUsers();
		public GroupUsers()
		{}
		#region  ��Ա����
		/// <summary>
		/// ����һ������
		/// </summary>
		public void Add(LPWeb.Model.GroupUsers model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(LPWeb.Model.GroupUsers model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int GroupId,int UserId)
		{
			
			dal.Delete(GroupId,UserId);
		}

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public LPWeb.Model.GroupUsers GetModel(int GroupId,int UserId)
		{
			
			return dal.GetModel(GroupId,UserId);
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
		public List<LPWeb.Model.GroupUsers> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public List<LPWeb.Model.GroupUsers> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.GroupUsers> modelList = new List<LPWeb.Model.GroupUsers>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.GroupUsers model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.GroupUsers();
					if(dt.Rows[n]["GroupId"].ToString()!="")
					{
						model.GroupId=int.Parse(dt.Rows[n]["GroupId"].ToString());
					}
					if(dt.Rows[n]["UserId"].ToString()!="")
					{
						model.UserId=int.Parse(dt.Rows[n]["UserId"].ToString());
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

        public DataSet GetGroupUsersForUserSetup(string strWhere)
        {
            return dal.GetGroupUsersForUserSetup(strWhere);
        }

        /// <summary>
        /// ����GroupUser��ϵ
        /// </summary>
        /// <param name="nUserId"></param>
        /// <param name="strSelectedIds"></param>
        public void SaveGroupUser(int nUserId, string strSelectedIds)
        {
            dal.SaveGroupUser(nUserId, strSelectedIds);
        }

		#endregion  ��Ա����
	}
}

