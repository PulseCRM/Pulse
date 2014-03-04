using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// PointFiles 的摘要说明。
	/// </summary>
	public class PointFiles
	{
		private readonly LPWeb.DAL.PointFiles dal=new LPWeb.DAL.PointFiles();
		public PointFiles()
		{}
		#region  成员方法
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(LPWeb.Model.PointFiles model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(LPWeb.Model.PointFiles model)
		{
			dal.Update(model);
		}

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateBase(LPWeb.Model.PointFiles model)
        {
            dal.UpdateBase(model);
        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int FileId)
		{
			
			dal.Delete(FileId);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.PointFiles GetModel(int FileId)
		{
			
			return dal.GetModel(FileId);
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
		public List<LPWeb.Model.PointFiles> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LPWeb.Model.PointFiles> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.PointFiles> modelList = new List<LPWeb.Model.PointFiles>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.PointFiles model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.PointFiles();
					if(dt.Rows[n]["FileId"].ToString()!="")
					{
						model.FileId=int.Parse(dt.Rows[n]["FileId"].ToString());
					}
					if(dt.Rows[n]["FolderId"].ToString()!="")
					{
						model.FolderId=int.Parse(dt.Rows[n]["FolderId"].ToString());
					}
					model.Name=dt.Rows[n]["Name"].ToString();
					if(dt.Rows[n]["FirstImported"].ToString()!="")
					{
						model.FirstImported=DateTime.Parse(dt.Rows[n]["FirstImported"].ToString());
					}
					if(dt.Rows[n]["LastImported"].ToString()!="")
					{
						model.LastImported=DateTime.Parse(dt.Rows[n]["LastImported"].ToString());
					}
					if(dt.Rows[n]["Success"].ToString()!="")
					{
						if((dt.Rows[n]["Success"].ToString()=="1")||(dt.Rows[n]["Success"].ToString().ToLower()=="true"))
						{
						model.Success=true;
						}
						else
						{
							model.Success=false;
						}
					}
					model.CurrentImage=dt.Rows[n]["CurrentImage"].ToString();
					model.PreviousImage=dt.Rows[n]["PreviousImage"].ToString();
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
        public int GetPointFileBrancId(string FileId, string strWhere)
        {
            if (strWhere == null)
                strWhere = "";
            if (strWhere.Length <= 0)
                strWhere = "FileId=" + FileId;
            else
                strWhere = "FileId=" + FileId + " AND " + strWhere;
            return dal.GetPointFileBrancId(strWhere);
        }
		#endregion  成员方法
	}
}

