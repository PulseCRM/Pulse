using System;
using System.Data;
using System.Collections.Generic;
using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// ArchiveLeadStatus
	/// </summary>
	public class CompanyTaskPickList
	{
		private readonly LPWeb.DAL.CompanyTaskPickList dal=new LPWeb.DAL.CompanyTaskPickList();
        public CompanyTaskPickList()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
        //public int GetMaxId()
        //{
        //    return dal.GetMaxId();
        //}

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxSequenceNumber()
        {
            return dal.GetMaxSequenceNumber();
        }

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(string name)
		{
            return dal.Exists(name);
		}

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsSequenceNumber(int SeqNumber)
        {
            return dal.ExistsSequenceNumber(SeqNumber);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
        public int Add(LPWeb.Model.CompanyTaskPick model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
        public bool Update(LPWeb.Model.CompanyTaskPick model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(string LeadStatusId)
		{
			
			return dal.Delete(LeadStatusId);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string LeadStatusIdlist )
		{
			return dal.DeleteList(LeadStatusIdlist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
        public LPWeb.Model.CompanyTaskPick GetModel(string LeadStatusId)
		{
			
			return dal.GetModel(LeadStatusId);
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
        public List<LPWeb.Model.CompanyTaskPick> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
        public List<LPWeb.Model.CompanyTaskPick> DataTableToList(DataTable dt)
		{
            List<LPWeb.Model.CompanyTaskPick> modelList = new List<LPWeb.Model.CompanyTaskPick>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
                LPWeb.Model.CompanyTaskPick model;
				for (int n = 0; n < rowsCount; n++)
				{
                    model = new LPWeb.Model.CompanyTaskPick();
                    if (dt.Rows[n]["TaskNameID"].ToString() != "")
					{
                        model.TaskNameID = int.Parse(dt.Rows[n]["TaskNameID"].ToString());
					}
                    model.TaskName = dt.Rows[n]["TaskName"].ToString();
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

                    model.SequenceNumber = int.Parse(dt.Rows[n]["SequenceNumber"].ToString());
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
		/// 分页获取数据列表
		/// </summary>
		//public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		//{
			//return dal.GetList(PageSize,PageIndex,strWhere);
		//}

		#endregion  Method
	}
}

