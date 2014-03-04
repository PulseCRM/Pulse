using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// 业务逻辑类UserPipelineColumns 的摘要说明。
	/// </summary>
	public class UserPipelineColumns
	{
		private readonly LPWeb.DAL.UserPipelineColumns dal=new LPWeb.DAL.UserPipelineColumns();
		public UserPipelineColumns()
		{}
		#region  成员方法
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(LPWeb.Model.UserPipelineColumns model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(LPWeb.Model.UserPipelineColumns model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int UserId)
		{
			
			dal.Delete(UserId);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.UserPipelineColumns GetModel(int UserId)
		{
			
			return dal.GetModel(UserId);
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
		public List<LPWeb.Model.UserPipelineColumns> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LPWeb.Model.UserPipelineColumns> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.UserPipelineColumns> modelList = new List<LPWeb.Model.UserPipelineColumns>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.UserPipelineColumns model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.UserPipelineColumns();
					if(dt.Rows[n]["UserId"].ToString()!="")
					{
						model.UserId=int.Parse(dt.Rows[n]["UserId"].ToString());
					}
					if(dt.Rows[n]["PointFolder"].ToString()!="")
					{
						if((dt.Rows[n]["PointFolder"].ToString()=="1")||(dt.Rows[n]["PointFolder"].ToString().ToLower()=="true"))
						{
						model.PointFolder=true;
						}
						else
						{
							model.PointFolder=false;
						}
					}
					if(dt.Rows[n]["Stage"].ToString()!="")
					{
						if((dt.Rows[n]["Stage"].ToString()=="1")||(dt.Rows[n]["Stage"].ToString().ToLower()=="true"))
						{
						model.Stage=true;
						}
						else
						{
							model.Stage=false;
						}
					}
					if(dt.Rows[n]["Branch"].ToString()!="")
					{
						if((dt.Rows[n]["Branch"].ToString()=="1")||(dt.Rows[n]["Branch"].ToString().ToLower()=="true"))
						{
						model.Branch=true;
						}
						else
						{
							model.Branch=false;
						}
					}
					if(dt.Rows[n]["EstimatedClose"].ToString()!="")
					{
						if((dt.Rows[n]["EstimatedClose"].ToString()=="1")||(dt.Rows[n]["EstimatedClose"].ToString().ToLower()=="true"))
						{
						model.EstimatedClose=true;
						}
						else
						{
							model.EstimatedClose=false;
						}
					}
					if(dt.Rows[n]["Alerts"].ToString()!="")
					{
						if((dt.Rows[n]["Alerts"].ToString()=="1")||(dt.Rows[n]["Alerts"].ToString().ToLower()=="true"))
						{
						model.Alerts=true;
						}
						else
						{
							model.Alerts=false;
						}
					}
					if(dt.Rows[n]["LoanOfficer"].ToString()!="")
					{
						if((dt.Rows[n]["LoanOfficer"].ToString()=="1")||(dt.Rows[n]["LoanOfficer"].ToString().ToLower()=="true"))
						{
						model.LoanOfficer=true;
						}
						else
						{
							model.LoanOfficer=false;
						}
					}
					if(dt.Rows[n]["Amount"].ToString()!="")
					{
						if((dt.Rows[n]["Amount"].ToString()=="1")||(dt.Rows[n]["Amount"].ToString().ToLower()=="true"))
						{
						model.Amount=true;
						}
						else
						{
							model.Amount=false;
						}
					}
					if(dt.Rows[n]["Lien"].ToString()!="")
					{
						if((dt.Rows[n]["Lien"].ToString()=="1")||(dt.Rows[n]["Lien"].ToString().ToLower()=="true"))
						{
						model.Lien=true;
						}
						else
						{
							model.Lien=false;
						}
					}
					if(dt.Rows[n]["Rate"].ToString()!="")
					{
						if((dt.Rows[n]["Rate"].ToString()=="1")||(dt.Rows[n]["Rate"].ToString().ToLower()=="true"))
						{
						model.Rate=true;
						}
						else
						{
							model.Rate=false;
						}
					}
					if(dt.Rows[n]["Lender"].ToString()!="")
					{
						if((dt.Rows[n]["Lender"].ToString()=="1")||(dt.Rows[n]["Lender"].ToString().ToLower()=="true"))
						{
						model.Lender=true;
						}
						else
						{
							model.Lender=false;
						}
					}
					if(dt.Rows[n]["LockExp"].ToString()!="")
					{
						if((dt.Rows[n]["LockExp"].ToString()=="1")||(dt.Rows[n]["LockExp"].ToString().ToLower()=="true"))
						{
						model.LockExp=true;
						}
						else
						{
							model.LockExp=false;
						}
					}
					if(dt.Rows[n]["PercentCompl"].ToString()!="")
					{
						if((dt.Rows[n]["PercentCompl"].ToString()=="1")||(dt.Rows[n]["PercentCompl"].ToString().ToLower()=="true"))
						{
						model.PercentCompl=true;
						}
						else
						{
							model.PercentCompl=false;
						}
					}
					if(dt.Rows[n]["Processor"].ToString()!="")
					{
						if((dt.Rows[n]["Processor"].ToString()=="1")||(dt.Rows[n]["Processor"].ToString().ToLower()=="true"))
						{
						model.Processor=true;
						}
						else
						{
							model.Processor=false;
						}
					}
					if(dt.Rows[n]["TaskCount"].ToString()!="")
					{
						if((dt.Rows[n]["TaskCount"].ToString()=="1")||(dt.Rows[n]["TaskCount"].ToString().ToLower()=="true"))
						{
						model.TaskCount=true;
						}
						else
						{
							model.TaskCount=false;
						}
					}
					if(dt.Rows[n]["PointFileName"].ToString()!="")
					{
						if((dt.Rows[n]["PointFileName"].ToString()=="1")||(dt.Rows[n]["PointFileName"].ToString().ToLower()=="true"))
						{
						model.PointFileName=true;
						}
						else
						{
							model.PointFileName=false;
						}
					}
                    //model.LastLoanNote = false;
                    //if (dt.Rows[n]["LastLoanNote"].ToString() != "")
                    //{
                    //    if ((dt.Rows[n]["LastLoanNote"].ToString() == "1") || (dt.Rows[n]["LastLoanNote"].ToString().ToLower() == "true"))
                    //    {
                    //        model.PointFileName = true;
                    //    }
                    //    else
                    //    {
                    //        model.PointFileName = false;
                    //    }
                    //}
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
        /// copy user PipelineColsinfo
        /// </summary>
        /// <param name="nSourceUserID"></param>
        /// <param name="nDistUserID"></param>
        public void CopyUserPipelineColsInfo(int nSourceUserID, int nDistUserID)
        {
            Model.UserPipelineColumns userPipelineCols = dal.GetModel(nSourceUserID);
            if (null != userPipelineCols)
            {
                userPipelineCols.UserId = nDistUserID;
                dal.Add(userPipelineCols);
            }
        }

		#endregion  成员方法
	}
}

