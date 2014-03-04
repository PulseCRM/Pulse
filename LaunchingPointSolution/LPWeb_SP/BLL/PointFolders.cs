using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// PointFolders 的摘要说明。
	/// </summary>
	public class PointFolders
	{
		private readonly LPWeb.DAL.PointFolders dal=new LPWeb.DAL.PointFolders();
		public PointFolders()
		{}

        public DataTable GetPointFolder_Executive(int iUserID, string sRegionIDs, string sDivisionIDs, string sBranchIDs)
        {
            return this.dal.GetPointFolder_Executive(iUserID, sRegionIDs, sDivisionIDs, sBranchIDs);
        }

        public DataTable GetPointFolder_BranchManager(int iUserID, string sRegionIDs, string sDivisionIDs, string sBranchIDs)
        {
            return this.dal.GetPointFolder_BranchManager(iUserID, sRegionIDs, sDivisionIDs, sBranchIDs);
        }

        public DataTable GetPointFolder_User(int iUserID, string sRegionIDs, string sDivisionIDs, string sBranchIDs)
        {
            return this.dal.GetPointFolder_User(iUserID, sRegionIDs, sDivisionIDs, sBranchIDs);
        }

		#region  成员方法
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(LPWeb.Model.PointFolders model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(LPWeb.Model.PointFolders model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int FolderId)
		{
			
			dal.Delete(FolderId);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.PointFolders GetModel(int FolderId)
		{
			
			return dal.GetModel(FolderId);
		}

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.PointFolders GePonitFolderModel(int FolderId)
        {

            return dal.GePonitFolderModel(FolderId);
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
        public DataSet GetListByLoanId (int LoanId, string strWhere)
        {
            return dal.GetListByLoanId(LoanId, strWhere);
        }

        /// <summary>
        ///  查找Loan 对应的LoanOfficerBranchID
        /// </summary>
        /// <param name="iLoanID"></param>
        /// <returns></returns>
        public string GetLoanOfficerBranchID(int iLoanID, string sType)
        {
            return dal.GetLoanOfficerBranchID(iLoanID, sType);
        }

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LPWeb.Model.PointFolders> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LPWeb.Model.PointFolders> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.PointFolders> modelList = new List<LPWeb.Model.PointFolders>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.PointFolders model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.PointFolders();
					if(dt.Rows[n]["FolderId"].ToString()!="")
					{
						model.FolderId=int.Parse(dt.Rows[n]["FolderId"].ToString());
					}
					model.Name=dt.Rows[n]["Name"].ToString();
					if(dt.Rows[n]["BranchId"].ToString()!="")
					{
						model.BranchId=int.Parse(dt.Rows[n]["BranchId"].ToString());
					}
                    model.BranchName = dt.Rows[n]["BranchName"].ToString();
					model.Path=dt.Rows[n]["Path"].ToString();
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
					if(dt.Rows[n]["ImportCount"].ToString()!="")
					{
						model.ImportCount=int.Parse(dt.Rows[n]["ImportCount"].ToString());
					}
					if(dt.Rows[n]["LastImport"].ToString()!="")
					{
						model.LastImport=DateTime.Parse(dt.Rows[n]["LastImport"].ToString());
					}
					if(dt.Rows[n]["LoanStatus"].ToString()!="")
					{
						model.LoanStatus=int.Parse(dt.Rows[n]["LoanStatus"].ToString());
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
        /// 更新Point Folder状态
        /// </summary>
        public void UpdatePointFolderEnabled(string sFolderIDs, bool bEnabled)
        {

            dal.UpdatePointFolderEnabled(sFolderIDs,bEnabled);
        }

        /// <summary>
        /// 更新Point Folder 是否自动命名
        /// </summary>
        /// <param name="FolderID"></param>
        /// <param name="AutoNaming"></param>
        /// <param name="Prefix"></param>
        public void UpdatePointFolderAutoNaming(int FolderID, bool AutoNaming, string Prefix, bool RandomFileNaming, string FilenameLength)
        {
            dal.UpdatePointFolderAutoNaming(FolderID, AutoNaming, Prefix, RandomFileNaming, FilenameLength);
        }

        /// <summary>
        /// 设置默认Point
        /// </summary>
        /// <param name="sFolderID"></param>
        /// <param name="BranchId"></param>
        public void SetDefaultPoint(string sFolderID, int BranchId, bool IsCancel)
        {
            dal.SetDefaultPoint(sFolderID, BranchId, IsCancel);
        }
		#endregion  成员方法
	}
}

