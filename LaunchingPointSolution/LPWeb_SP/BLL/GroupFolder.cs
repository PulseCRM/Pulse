using System;
using System.Data;
using System.Collections.Generic;
using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// 业务逻辑类GroupFolder 的摘要说明。
	/// </summary>
	public class GroupFolder
	{
		private readonly LPWeb.DAL.GroupFolder dal=new LPWeb.DAL.GroupFolder();
		public GroupFolder()
		{}
		#region  成员方法
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(LPWeb.Model.GroupFolder model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(LPWeb.Model.GroupFolder model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int GroupId,int FolderId)
		{
			
			dal.Delete(GroupId,FolderId);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.GroupFolder GetModel(int GroupId,int FolderId)
		{
			
			return dal.GetModel(GroupId,FolderId);
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
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			return dal.GetList(Top,strWhere,filedOrder);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LPWeb.Model.GroupFolder> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
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
		/// 获得数据列表
		/// </summary>
		public DataSet GetAllList()
		{
			return GetList("");
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		{
			return dal.GetList(PageSize,PageIndex,strWhere);
		}

        /// <summary>
        /// 保存一条数据
        /// </summary>
        public void DoSaveGroupFolder(int iGroupId, int iId, string sType, int iOldGroupId)
        {

            this.dal.DoSaveGroupFolder(iGroupId, iId, sType, iOldGroupId);
        }
		#endregion  成员方法
	}
}

