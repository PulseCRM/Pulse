using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// 业务逻辑类Template_Email_Recipients 的摘要说明。
	/// </summary>
	public class Template_Email_Recipients
	{
		private readonly LPWeb.DAL.Template_Email_Recipients dal=new LPWeb.DAL.Template_Email_Recipients();
		public Template_Email_Recipients()
		{}
		#region  成员方法
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(LPWeb.Model.Template_Email_Recipients model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(LPWeb.Model.Template_Email_Recipients model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int TemplRecipientId)
		{
			
			dal.Delete(TemplRecipientId);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.Template_Email_Recipients GetModel(int TemplRecipientId)
		{
			
			return dal.GetModel(TemplRecipientId);
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
		public List<LPWeb.Model.Template_Email_Recipients> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LPWeb.Model.Template_Email_Recipients> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.Template_Email_Recipients> modelList = new List<LPWeb.Model.Template_Email_Recipients>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.Template_Email_Recipients model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.Template_Email_Recipients();
					if(dt.Rows[n]["TemplRecipientId"].ToString()!="")
					{
						model.TemplRecipientId=int.Parse(dt.Rows[n]["TemplRecipientId"].ToString());
					}
					if(dt.Rows[n]["TemplEmailId"].ToString()!="")
					{
						model.TemplEmailId=int.Parse(dt.Rows[n]["TemplEmailId"].ToString());
					}
					model.EmailAddr=dt.Rows[n]["EmailAddr"].ToString();
					model.UserRoles=dt.Rows[n]["UserRoles"].ToString();
					model.ContactRoles=dt.Rows[n]["ContactRoles"].ToString();
					model.RecipientType=dt.Rows[n]["RecipientType"].ToString();
					if(dt.Rows[n]["TaskOwner"].ToString()!="")
					{
						if((dt.Rows[n]["TaskOwner"].ToString()=="1")||(dt.Rows[n]["TaskOwner"].ToString().ToLower()=="true"))
						{
						model.TaskOwner=true;
						}
						else
						{
							model.TaskOwner=false;
						}
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
	}
}

