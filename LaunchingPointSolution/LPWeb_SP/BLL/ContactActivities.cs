using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// ҵ���߼���ContactActivities ��ժҪ˵����
	/// </summary>
	public class ContactActivities
	{
		private readonly LPWeb.DAL.ContactActivities dal=new LPWeb.DAL.ContactActivities();
		public ContactActivities()
		{}
		#region  ��Ա����
		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(LPWeb.Model.ContactActivities model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(LPWeb.Model.ContactActivities model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int ContactActivityId)
		{
			
			dal.Delete(ContactActivityId);
		}

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public LPWeb.Model.ContactActivities GetModel(int ContactActivityId)
		{
			
			return dal.GetModel(ContactActivityId);
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
		public List<LPWeb.Model.ContactActivities> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public List<LPWeb.Model.ContactActivities> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.ContactActivities> modelList = new List<LPWeb.Model.ContactActivities>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.ContactActivities model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.ContactActivities();
					if(dt.Rows[n]["ContactActivityId"].ToString()!="")
					{
						model.ContactActivityId=int.Parse(dt.Rows[n]["ContactActivityId"].ToString());
					}
					if(dt.Rows[n]["ContactId"].ToString()!="")
					{
						model.ContactId=int.Parse(dt.Rows[n]["ContactId"].ToString());
					}
					if(dt.Rows[n]["UserId"].ToString()!="")
					{
						model.UserId=int.Parse(dt.Rows[n]["UserId"].ToString());
					}
					model.ActivityName=dt.Rows[n]["ActivityName"].ToString();
					if(dt.Rows[n]["ActivityTime"].ToString()!="")
					{
						model.ActivityTime=DateTime.Parse(dt.Rows[n]["ActivityTime"].ToString());
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

		#endregion  ��Ա����


        public DataSet GetListForGridView(int PageSize, int PageIndex, string strWhere, out int recordCount, string orderName, int orderType)
        {
            return dal.GetListForGridView(PageSize, PageIndex, strWhere, out recordCount, orderName, orderType);
        }
        public DataTable GetProformedBy(string strWhere)
        {
            return dal.GetProformedBy(strWhere);
        }
	}
}

