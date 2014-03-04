using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// 业务逻辑类ContactBranches 的摘要说明。
	/// </summary>
	public class ContactBranches
	{
		private readonly LPWeb.DAL.ContactBranches dal=new LPWeb.DAL.ContactBranches();
		public ContactBranches()
		{}
		#region  成员方法
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(LPWeb.Model.ContactBranches model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(LPWeb.Model.ContactBranches model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ContactBranchId)
		{
			
			dal.Delete(ContactBranchId);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.ContactBranches GetModel(int ContactBranchId)
		{
			
			return dal.GetModel(ContactBranchId);
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
		public List<LPWeb.Model.ContactBranches> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LPWeb.Model.ContactBranches> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.ContactBranches> modelList = new List<LPWeb.Model.ContactBranches>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.ContactBranches model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.ContactBranches();
					if(dt.Rows[n]["ContactBranchId"].ToString()!="")
					{
						model.ContactBranchId=int.Parse(dt.Rows[n]["ContactBranchId"].ToString());
					}
					if(dt.Rows[n]["ContactCompanyId"].ToString()!="")
					{
						model.ContactCompanyId=int.Parse(dt.Rows[n]["ContactCompanyId"].ToString());
					}
					model.Name=dt.Rows[n]["Name"].ToString();
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
					model.Address=dt.Rows[n]["Address"].ToString();
					model.City=dt.Rows[n]["City"].ToString();
					model.State=dt.Rows[n]["State"].ToString();
					model.Zip=dt.Rows[n]["Zip"].ToString();
					model.Phone=dt.Rows[n]["Phone"].ToString();
					model.Fax=dt.Rows[n]["Fax"].ToString();
					if(dt.Rows[n]["PrimaryContact"].ToString()!="")
					{
						model.PrimaryContact=int.Parse(dt.Rows[n]["PrimaryContact"].ToString());
					}
					if(dt.Rows[n]["Modified"].ToString()!="")
					{
						model.Modified=DateTime.Parse(dt.Rows[n]["Modified"].ToString());
					}
					if(dt.Rows[n]["ModifiedBy"].ToString()!="")
					{
						model.ModifiedBy=int.Parse(dt.Rows[n]["ModifiedBy"].ToString());
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

		#endregion  成员方法

        public DataSet GetPartnerContacts(int iLoginUserID, int PageSize, int PageIndex, string strWhere, out int recordCount, string orderName, int orderType)
        {
            return dal.GetPartnerContacts(iLoginUserID, PageSize, PageIndex, strWhere, out recordCount, orderName, orderType);
        }

        public DataTable GePartnerContactsForSel(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            return dal.GePartnerContactsForSel(PageSize, PageIndex, strWhere, out count, orderName, orderType);
        }

        public bool DisableBranch(int iContactBranchID)
        {
            return dal.DisableBranch(iContactBranchID);
        }

        public bool DisableContact(int iContactID)
        {
            return dal.DisableContact(iContactID);
        }

        public bool RomoveBranch(int iContactBranchID)
        {
            return dal.RomoveBranch(iContactBranchID);
        }

         /// <summary>
        /// Insert ContactUser
        /// </summary>
        /// <param name="iUserID"></param>
        /// <param name="iContactID"></param>
        /// <returns></returns>
        public bool AssignUser2Contact(int iUserID, int iContactID)
        {
            return dal.AssignUser2Contact(iUserID, iContactID);
        }
        /// <summary>
        /// 检查branch name是否存在
        /// Alex 2011-04-14
        /// </summary>
        /// <param name="sRuleName"></param>
        /// <returns></returns>
        public bool IsExist_CreateBase(string sBranchName)
        {
            return dal.IsExist_CreateBase(sBranchName);
        }

        /// <summary>
        /// 检查branch name是否存在
        /// Alex 2011-04-14
        /// </summary>
        /// <param name="iRuleID"></param>
        /// <param name="sRuleName"></param>
        /// <returns></returns>
        public bool IsExist_EditBase(int iBranchID, string sBranchName)
        {
            return dal.IsExist_EditBase(iBranchID,sBranchName);
        }

         /// <summary>
        /// Update Contacts with BranchID
        /// </summary>
        /// <param name="iBranchID"></param>
        /// <param name="sContactIDs"></param>
        public void AddContactToBranch(int iBranchID, string sContactIDs)
        {
            dal.AddContactToBranch(iBranchID, sContactIDs);
        }

         /// <summary>
        /// Remove Contacts set BranchID=null
        /// </summary>
        /// <param name="iBranchID"></param>
        /// <param name="sContactIDs"></param>
        public void RemoveContactSetBranchNull(string sContactIDs)
        {
            dal.RemoveContactSetBranchNull(sContactIDs);
        }
	}
}

