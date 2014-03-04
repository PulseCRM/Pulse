using System;
using System.Data;
using System.Collections.Generic;
using LPWeb.Model;

namespace LPWeb.BLL
{
    /// <summary>
    /// 业务逻辑类UserProspectColumns 的摘要说明。
    /// </summary>
    public class UserProspectColumns
    {
        private readonly LPWeb.DAL.UserProspectColumns dal=new LPWeb.DAL.UserProspectColumns();
        public UserProspectColumns()
		{}
        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(LPWeb.Model.UserProspectColumns model)
        {
            dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(LPWeb.Model.UserProspectColumns model)
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
        public LPWeb.Model.UserProspectColumns GetModel(int UserId)
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
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            return dal.GetList(Top, strWhere, filedOrder);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<LPWeb.Model.UserProspectColumns> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<LPWeb.Model.UserProspectColumns> DataTableToList(DataTable dt)
        {
            List<LPWeb.Model.UserProspectColumns> modelList = new List<LPWeb.Model.UserProspectColumns>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LPWeb.Model.UserProspectColumns model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LPWeb.Model.UserProspectColumns();
                    if (dt.Rows[0]["UserId"].ToString() != "")
                    {
                        model.UserId = int.Parse(dt.Rows[0]["UserId"].ToString());
                    }
                    if (dt.Rows[0]["Pv_Created"].ToString() != "")
                    {
                        if ((dt.Rows[0]["Pv_Created"].ToString() == "1") || (dt.Rows[0]["Pv_Created"].ToString().ToLower() == "true"))
                        {
                            model.Pv_Created = true;
                        }
                        else
                        {
                            model.Pv_Created = false;
                        }
                    }
                    if (dt.Rows[0]["PV_LeadSource"].ToString() != "")
                    {
                        if ((dt.Rows[0]["PV_LeadSource"].ToString() == "1") || (dt.Rows[0]["PV_LeadSource"].ToString().ToLower() == "true"))
                        {
                            model.Pv_Leadsource = true;
                        }
                        else
                        {
                            model.Pv_Leadsource = false;
                        }
                    }
                    if (dt.Rows[0]["PV_RefCode"].ToString() != "")
                    {
                        if ((dt.Rows[0]["PV_RefCode"].ToString() == "1") || (dt.Rows[0]["PV_RefCode"].ToString().ToLower() == "true"))
                        {
                            model.Pv_Refcode = true;
                        }
                        else
                        {
                            model.Pv_Refcode = false;
                        }
                    }
                    if (dt.Rows[0]["PV_LoanOfficer"].ToString() != "")
                    {
                        if ((dt.Rows[0]["PV_LoanOfficer"].ToString() == "1") || (dt.Rows[0]["PV_LoanOfficer"].ToString().ToLower() == "true"))
                        {
                            model.Pv_Loanofficer = true;
                        }
                        else
                        {
                            model.Pv_Loanofficer = false;
                        }
                    }
                    if (dt.Rows[0]["PV_Branch"].ToString() != "")
                    {
                        if ((dt.Rows[0]["PV_Branch"].ToString() == "1") || (dt.Rows[0]["PV_Branch"].ToString().ToLower() == "true"))
                        {
                            model.Pv_Branch = true;
                        }
                        else
                        {
                            model.Pv_Branch = false;
                        }
                    }
                    if (dt.Rows[0]["PV_Progress"].ToString() != "")
                    {
                        if ((dt.Rows[0]["PV_Progress"].ToString() == "1") || (dt.Rows[0]["PV_Progress"].ToString().ToLower() == "true"))
                        {
                            model.Pv_Progress = true;
                        }
                        else
                        {
                            model.Pv_Progress = false;
                        }
                    }

                    if (dt.Rows[0]["LV_Ranking"].ToString() != "")
                    {
                        if ((dt.Rows[0]["LV_Ranking"].ToString() == "1") || (dt.Rows[0]["LV_Ranking"].ToString().ToLower() == "true"))
                        {
                            model.Lv_Ranking = true;
                        }
                        else
                        {
                            model.Lv_Ranking = false;
                        }
                    }
                    if (dt.Rows[0]["LV_Amount"].ToString() != "")
                    {
                        if ((dt.Rows[0]["LV_Amount"].ToString() == "1") || (dt.Rows[0]["LV_Amount"].ToString().ToLower() == "true"))
                        {
                            model.Lv_Amount = true;
                        }
                        else
                        {
                            model.Lv_Amount = false;
                        }
                    }
                    if (dt.Rows[0]["LV_Rate"].ToString() != "")
                    {
                        if ((dt.Rows[0]["LV_Rate"].ToString() == "1") || (dt.Rows[0]["LV_Rate"].ToString().ToLower() == "true"))
                        {
                            model.Lv_Rate = true;
                        }
                        else
                        {
                            model.Lv_Rate = false;
                        }
                    }
                    if (dt.Rows[0]["LV_LoanOfficer"].ToString() != "")
                    {
                        if ((dt.Rows[0]["LV_LoanOfficer"].ToString() == "1") || (dt.Rows[0]["LV_LoanOfficer"].ToString().ToLower() == "true"))
                        {
                            model.Lv_Loanofficer = true;
                        }
                        else
                        {
                            model.Lv_Loanofficer = false;
                        }
                    }
                    if (dt.Rows[0]["LV_Lien"].ToString() != "")
                    {
                        if ((dt.Rows[0]["LV_Lien"].ToString() == "1") || (dt.Rows[0]["LV_Lien"].ToString().ToLower() == "true"))
                        {
                            model.Lv_Lien = true;
                        }
                        else
                        {
                            model.Lv_Lien = false;
                        }
                    }
                    if (dt.Rows[0]["LV_Progress"].ToString() != "")
                    {
                        if ((dt.Rows[0]["LV_Progress"].ToString() == "1") || (dt.Rows[0]["LV_Progress"].ToString().ToLower() == "true"))
                        {
                            model.Lv_Progress = true;
                        }
                        else
                        {
                            model.Lv_Progress = false;
                        }
                    }
                    if (dt.Rows[0]["LV_Branch"].ToString() != "")
                    {
                        if ((dt.Rows[0]["LV_Branch"].ToString() == "1") || (dt.Rows[0]["LV_Branch"].ToString().ToLower() == "true"))
                        {
                            model.Lv_Branch = true;
                        }
                        else
                        {
                            model.Lv_Branch = false;
                        }
                    }
                    if (dt.Rows[0]["LV_LoanProgram"].ToString() != "")
                    {
                        if ((dt.Rows[0]["LV_LoanProgram"].ToString() == "1") || (dt.Rows[0]["LV_LoanProgram"].ToString().ToLower() == "true"))
                        {
                            model.Lv_Loanprogram = true;
                        }
                        else
                        {
                            model.Lv_Loanprogram = false;
                        }
                    }

                    if (dt.Rows[0]["LV_LeadSource"].ToString() != "")
                    {
                        if ((dt.Rows[0]["LV_LeadSource"].ToString() == "1") || (dt.Rows[0]["LV_LeadSource"].ToString().ToLower() == "true"))
                        {
                            model.Lv_Leadsource = true;
                        }
                        else
                        {
                            model.Lv_Leadsource = false;
                        }
                    }
                    if (dt.Rows[0]["LV_RefCode"].ToString() != "")
                    {
                        if ((dt.Rows[0]["LV_RefCode"].ToString() == "1") || (dt.Rows[0]["LV_RefCode"].ToString().ToLower() == "true"))
                        {
                            model.Lv_Refcode = true;
                        }
                        else
                        {
                            model.Lv_Refcode = false;
                        }
                    }
                    if (dt.Rows[0]["LV_EstClose"].ToString() != "")
                    {
                        if ((dt.Rows[0]["LV_EstClose"].ToString() == "1") || (dt.Rows[0]["LV_EstClose"].ToString().ToLower() == "true"))
                        {
                            model.Lv_Estclose = true;
                        }
                        else
                        {
                            model.Lv_Estclose = false;
                        }
                    }
                    if (dt.Rows[0]["LV_PointFilename"].ToString() != "")
                    {
                        if ((dt.Rows[0]["LV_PointFilename"].ToString() == "1") || (dt.Rows[0]["LV_PointFilename"].ToString().ToLower() == "true"))
                        {
                            model.Lv_Pointfilename = true;
                        }
                        else
                        {
                            model.Lv_Pointfilename = false;
                        }
                    }
                    if (dt.Rows[n]["PV_Referral"].ToString() != "")
                    {
                        if ((dt.Rows[n]["PV_Referral"].ToString() == "1") || (dt.Rows[n]["PV_Referral"].ToString().ToLower() == "true"))
                        {
                            model.Pv_Referral = true;
                        }
                        else
                        {
                            model.Pv_Referral = false;
                        }
                    }
                    if (dt.Rows[n]["PV_Partner"].ToString() != "")
                    {
                        if ((dt.Rows[n]["PV_Partner"].ToString() == "1") || (dt.Rows[n]["PV_Partner"].ToString().ToLower() == "true"))
                        {
                            model.Pv_Partner = true;
                        }
                        else
                        {
                            model.Pv_Partner = false;
                        }
                    }
                    if (dt.Rows[n]["LV_Referral"].ToString() != "")
                    {
                        if ((dt.Rows[n]["LV_Referral"].ToString() == "1") || (dt.Rows[n]["LV_Referral"].ToString().ToLower() == "true"))
                        {
                            model.Lv_Referral = true;
                        }
                        else
                        {
                            model.Lv_Referral = false;
                        }
                    }
                    if (dt.Rows[n]["LV_Partner"].ToString() != "")
                    {
                        if ((dt.Rows[n]["LV_Partner"].ToString() == "1") || (dt.Rows[n]["LV_Partner"].ToString().ToLower() == "true"))
                        {
                            model.Lv_Partner = true;
                        }
                        else
                        {
                            model.Lv_Partner = false;
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
        public DataSet GetList(int PageSize, int PageIndex, string strWhere)
        {
            return dal.GetList(PageSize, PageIndex, strWhere);
        }

        /// <summary>
        /// copy user PipelineColsinfo
        /// </summary>
        /// <param name="nSourceUserID"></param>
        /// <param name="nDistUserID"></param>
        public void CopyUserPipelineColsInfo(int nSourceUserID, int nDistUserID)
        {
            Model.UserProspectColumns userPipelineCols = dal.GetModel(nSourceUserID);
            if (null != userPipelineCols)
            {
                userPipelineCols.UserId = nDistUserID;
                dal.Add(userPipelineCols);
            }
        }

        #endregion  成员方法
    }
}
