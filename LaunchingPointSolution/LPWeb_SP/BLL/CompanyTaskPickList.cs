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
		/// �õ����ID
		/// </summary>
        //public int GetMaxId()
        //{
        //    return dal.GetMaxId();
        //}

        /// <summary>
        /// �õ����ID
        /// </summary>
        public int GetMaxSequenceNumber()
        {
            return dal.GetMaxSequenceNumber();
        }

		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool Exists(string name)
		{
            return dal.Exists(name);
		}

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool ExistsSequenceNumber(int SeqNumber)
        {
            return dal.ExistsSequenceNumber(SeqNumber);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
        public int Add(LPWeb.Model.CompanyTaskPick model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
        public bool Update(LPWeb.Model.CompanyTaskPick model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public bool Delete(string LeadStatusId)
		{
			
			return dal.Delete(LeadStatusId);
		}
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public bool DeleteList(string LeadStatusIdlist )
		{
			return dal.DeleteList(LeadStatusIdlist );
		}

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
        public LPWeb.Model.CompanyTaskPick GetModel(string LeadStatusId)
		{
			
			return dal.GetModel(LeadStatusId);
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
        public List<LPWeb.Model.CompanyTaskPick> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// ��������б�
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

		#endregion  Method
	}
}

