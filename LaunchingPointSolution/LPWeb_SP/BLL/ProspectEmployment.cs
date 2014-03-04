using System;
using System.Data;
using System.Collections.Generic;
using LPWeb.Model;
namespace LPWeb.BLL
{
    /// <summary>
    /// ProspectEmployment
    /// </summary>
    public class ProspectEmployment
    {
        private readonly LPWeb.DAL.ProspectEmployment dal = new LPWeb.DAL.ProspectEmployment();
        public ProspectEmployment()
        { }
        #region  Method

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return dal.GetMaxId();
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int EmplId)
        {
            return dal.Exists(EmplId);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.ProspectEmployment model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(LPWeb.Model.ProspectEmployment model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int EmplId)
        {

            return dal.Delete(EmplId);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string EmplIdlist)
        {
            return dal.DeleteList(EmplIdlist);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.ProspectEmployment GetModel(int EmplId)
        {

            return dal.GetModel(EmplId);
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
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            return dal.GetList(Top, strWhere, filedOrder);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<LPWeb.Model.ProspectEmployment> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<LPWeb.Model.ProspectEmployment> DataTableToList(DataTable dt)
        {
            List<LPWeb.Model.ProspectEmployment> modelList = new List<LPWeb.Model.ProspectEmployment>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LPWeb.Model.ProspectEmployment model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LPWeb.Model.ProspectEmployment();
                    if (dt.Rows[n]["EmplId"] != null && dt.Rows[n]["EmplId"].ToString() != "")
                    {
                        model.EmplId = int.Parse(dt.Rows[n]["EmplId"].ToString());
                    }
                    if (dt.Rows[n]["ContactId"] != null && dt.Rows[n]["ContactId"].ToString() != "")
                    {
                        model.ContactId = int.Parse(dt.Rows[n]["ContactId"].ToString());
                    }
                    if (dt.Rows[n]["SelfEmployed"] != null && dt.Rows[n]["SelfEmployed"].ToString() != "")
                    {
                        if ((dt.Rows[n]["SelfEmployed"].ToString() == "1") || (dt.Rows[n]["SelfEmployed"].ToString().ToLower() == "true"))
                        {
                            model.SelfEmployed = true;
                        }
                        else
                        {
                            model.SelfEmployed = false;
                        }
                    }
                    if (dt.Rows[n]["Position"] != null && dt.Rows[n]["Position"].ToString() != "")
                    {
                        model.Position = dt.Rows[n]["Position"].ToString();
                    }
                    if (dt.Rows[n]["StartYear"] != null && dt.Rows[n]["StartYear"].ToString() != "")
                    {
                        model.StartYear = decimal.Parse(dt.Rows[n]["StartYear"].ToString());
                    }
                    if (dt.Rows[n]["StartMonth"] != null && dt.Rows[n]["StartMonth"].ToString() != "")
                    {
                        model.StartMonth = decimal.Parse(dt.Rows[n]["StartMonth"].ToString());
                    }
                    if (dt.Rows[n]["EndYear"] != null && dt.Rows[n]["EndYear"].ToString() != "")
                    {
                        model.EndYear = decimal.Parse(dt.Rows[n]["EndYear"].ToString());
                    }
                    if (dt.Rows[n]["EndMonth"] != null && dt.Rows[n]["EndMonth"].ToString() != "")
                    {
                        model.EndMonth = decimal.Parse(dt.Rows[n]["EndMonth"].ToString());
                    }
                    if (dt.Rows[n]["YearsOnWork"] != null && dt.Rows[n]["YearsOnWork"].ToString() != "")
                    {
                        model.YearsOnWork = decimal.Parse(dt.Rows[n]["YearsOnWork"].ToString());
                    }
                    if (dt.Rows[n]["Phone"] != null && dt.Rows[n]["Phone"].ToString() != "")
                    {
                        model.Phone = dt.Rows[n]["Phone"].ToString();
                    }
                    if (dt.Rows[n]["ContactBranchId"] != null && dt.Rows[n]["ContactBranchId"].ToString() != "")
                    {
                        model.ContactBranchId = int.Parse(dt.Rows[n]["ContactBranchId"].ToString());
                    }
                    if (dt.Rows[n]["CompanyName"] != null && dt.Rows[n]["CompanyName"].ToString() != "")
                    {
                        model.CompanyName = dt.Rows[n]["CompanyName"].ToString();
                    }
                    if (dt.Rows[n]["Address"] != null && dt.Rows[n]["Address"].ToString() != "")
                    {
                        model.Address = dt.Rows[n]["Address"].ToString();
                    }
                    if (dt.Rows[n]["City"] != null && dt.Rows[n]["City"].ToString() != "")
                    {
                        model.City = dt.Rows[n]["City"].ToString();
                    }
                    if (dt.Rows[n]["State"] != null && dt.Rows[n]["State"].ToString() != "")
                    {
                        model.State = dt.Rows[n]["State"].ToString();
                    }
                    if (dt.Rows[n]["Zip"] != null && dt.Rows[n]["Zip"].ToString() != "")
                    {
                        model.Zip = dt.Rows[n]["Zip"].ToString();
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
        /// 分页获取数据列表
        /// </summary>
        //public DataSet GetList(int PageSize,int PageIndex,string strWhere)
        //{
        //return dal.GetList(PageSize,PageIndex,strWhere);
        //}

        #endregion  Method


        public DataSet GetProspectEmployment(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            return dal.GetProspectEmployment(PageSize, PageIndex, strWhere, out count, orderName, orderType);
        }

        public DataSet ProspectContacts(string strWhere)
        {
            return dal.ProspectContacts(strWhere);
        }

        public DataTable GetEmployment(int emplid)
        {
            return dal.GetEmployment(emplid);
        }

        public void UpdateProspectEmploymentAndProspectIncome(int FileId, string Employer, string StartMonth, string Dependants,string StartYear,string TP,string EndDate,string MonthlySalary,string EndYear,string Profession,string YearsInField)
        {
            dal.UpdateProspectEmploymentAndProspectIncome(FileId,Employer,StartMonth,Dependants,StartYear,TP,EndDate,MonthlySalary,EndYear,Profession,YearsInField);
        }


        public void InsertProspectEmploymentAndProspectIncome(int FileId, string Employer, string StartMonth, string Dependants, string StartYear, string TP, string EndDate, string MonthlySalary, string EndYear, string Profession, string YearsInField)
        {
            dal.InsertProspectEmploymentAndProspectIncome(FileId, Employer, StartMonth, Dependants, StartYear, TP, EndDate, MonthlySalary, EndYear, Profession, YearsInField);
        }


        public DataTable GetProspectEmployment(int iContactID)
        {
            return dal.GetProspectEmployment(iContactID);
        }


    }
}

