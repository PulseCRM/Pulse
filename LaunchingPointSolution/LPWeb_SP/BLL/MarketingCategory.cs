using System;
using System.Data;
using System.Collections.Generic;
using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// MarketingCategory
	/// </summary>
	public class MarketingCategory
	{
		private readonly LPWeb.DAL.MarketingCategory dal=new LPWeb.DAL.MarketingCategory();
		public MarketingCategory()
		{}
		#region  Method

		/// <summary>
		/// �õ����ID
		/// </summary>
		public int GetMaxId()
		{
			return dal.GetMaxId();
		}

		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool Exists(int CategoryId)
		{
			return dal.Exists(CategoryId);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(LPWeb.Model.MarketingCategory model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public bool Update(LPWeb.Model.MarketingCategory model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public bool Delete(int CategoryId)
		{
			
			return dal.Delete(CategoryId);
		}
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public bool DeleteList(string CategoryIdlist )
		{
			return dal.DeleteList(CategoryIdlist );
		}

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public LPWeb.Model.MarketingCategory GetModel(int CategoryId)
		{
			
			return dal.GetModel(CategoryId);
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
		public List<LPWeb.Model.MarketingCategory> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public List<LPWeb.Model.MarketingCategory> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.MarketingCategory> modelList = new List<LPWeb.Model.MarketingCategory>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.MarketingCategory model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.MarketingCategory();
					if(dt.Rows[n]["CategoryId"].ToString()!="")
					{
						model.CategoryId=int.Parse(dt.Rows[n]["CategoryId"].ToString());
					}
					model.CategoryName=dt.Rows[n]["CategoryName"].ToString();
					model.GlobalId=dt.Rows[n]["GlobalId"].ToString();
					model.Description=dt.Rows[n]["Description"].ToString();
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
		/// ��ҳ��ȡ�����б�
		/// </summary>
		//public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		//{
			//return dal.GetList(PageSize,PageIndex,strWhere);
        //}

        /// <summary>
        /// get marketing categories in alphabetical order
        /// </summary>
        public DataSet GetListInAlphOrder(string strWhere)
        {
            return dal.GetListInAlphOrder(strWhere);
        }

		#endregion  Method
	}
}

