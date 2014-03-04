using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// PointImportHistory 的摘要说明。
	/// </summary>
	public class PointImportHistory
	{
		private readonly LPWeb.DAL.PointImportHistory dal=new LPWeb.DAL.PointImportHistory();
		public PointImportHistory()
		{}
		#region  成员方法
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(LPWeb.Model.PointImportHistory model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(LPWeb.Model.PointImportHistory model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int HistoryId)
		{
			
			dal.Delete(HistoryId);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.PointImportHistory GetModel(int HistoryId)
		{
			
			return dal.GetModel(HistoryId);
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
		public List<LPWeb.Model.PointImportHistory> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LPWeb.Model.PointImportHistory> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.PointImportHistory> modelList = new List<LPWeb.Model.PointImportHistory>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.PointImportHistory model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.PointImportHistory();
					if(dt.Rows[n]["HistoryId"].ToString()!="")
					{
						model.HistoryId=int.Parse(dt.Rows[n]["HistoryId"].ToString());
					}
					if(dt.Rows[n]["FileId"].ToString()!="")
					{
						model.FileId=int.Parse(dt.Rows[n]["FileId"].ToString());
					}
					if(dt.Rows[n]["ImportTime"].ToString()!="")
					{
						model.ImportTime=DateTime.Parse(dt.Rows[n]["ImportTime"].ToString());
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
					model.Error=dt.Rows[n]["Error"].ToString();
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
        /// get data list
        /// </summary>
        public DataSet GetListForGridView(int PageSize, int PageIndex, string strWhere, out int recordCount, string orderName, int orderType)
        {
            return dal.GetListForGridView(PageSize, PageIndex, strWhere, out recordCount, orderName, orderType);
        }

        /// <summary>
        /// delete all import histories
        /// </summary>
        /// <param name="historyIDs"></param>
        public bool DeleteImportErrors(List<int> historyIDs)
        {
            return dal.DeleteImportHistory(historyIDs);
        }
		#endregion  成员方法
	}
}

