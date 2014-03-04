using System;
using System.Data;
using System.Collections.Generic;
using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// ҵ���߼���GroupFolder ��ժҪ˵����
	/// </summary>
	public class GroupFolder
	{
		private readonly LPWeb.DAL.GroupFolder dal=new LPWeb.DAL.GroupFolder();
		public GroupFolder()
		{}
		#region  ��Ա����
		/// <summary>
		/// ����һ������
		/// </summary>
		public void Add(LPWeb.Model.GroupFolder model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(LPWeb.Model.GroupFolder model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int GroupId,int FolderId)
		{
			
			dal.Delete(GroupId,FolderId);
		}

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public LPWeb.Model.GroupFolder GetModel(int GroupId,int FolderId)
		{
			
			return dal.GetModel(GroupId,FolderId);
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
		public List<LPWeb.Model.GroupFolder> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public List<LPWeb.Model.GroupFolder> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.GroupFolder> modelList = new List<LPWeb.Model.GroupFolder>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.GroupFolder model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.GroupFolder();
					if(dt.Rows[n]["GroupId"].ToString()!="")
					{
						model.GroupId=int.Parse(dt.Rows[n]["GroupId"].ToString());
					}
					if(dt.Rows[n]["FolderId"].ToString()!="")
					{
						model.FolderId=int.Parse(dt.Rows[n]["FolderId"].ToString());
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

        /// <summary>
        /// ����һ������
        /// </summary>
        public void DoSaveGroupFolder(int iGroupId, int iId, string sType, int iOldGroupId)
        {

            this.dal.DoSaveGroupFolder(iGroupId, iId, sType, iOldGroupId);
        }
		#endregion  ��Ա����
	}
}

