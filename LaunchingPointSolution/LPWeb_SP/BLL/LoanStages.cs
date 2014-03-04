using System;
using System.Data;
using System.Collections.Generic;
using LPWeb.Model;

namespace LPWeb.BLL
{
    public class LoanStages
    {
        private readonly LPWeb.DAL.LoanStages dal = new LPWeb.DAL.LoanStages();
        public LoanStages()
        { }
        #region  成员方法
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int LoanStageId)
        {
            return dal.Exists(LoanStageId);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.LoanStages model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(LPWeb.Model.LoanStages model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int LoanStageId)
        {

            dal.Delete(LoanStageId);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.LoanStages GetModel(int LoanStageId)
        {

            return dal.GetModel(LoanStageId);
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
        public List<LPWeb.Model.LoanStages> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<LPWeb.Model.LoanStages> DataTableToList(DataTable dt)
        {
            List<LPWeb.Model.LoanStages> modelList = new List<LPWeb.Model.LoanStages>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LPWeb.Model.LoanStages model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LPWeb.Model.LoanStages();
                    if (dt.Rows[n]["LoanStageId"].ToString() != "")
                    {
                        model.LoanStageId = int.Parse(dt.Rows[n]["LoanStageId"].ToString());
                    }
                    if (dt.Rows[n]["Completed"].ToString() != "")
                    {
                        model.Completed = DateTime.Parse(dt.Rows[n]["Completed"].ToString());
                    }
                    if (dt.Rows[n]["SequenceNumber"].ToString() != "")
                    {
                        model.SequenceNumber = int.Parse(dt.Rows[n]["SequenceNumber"].ToString());
                    }
                    if (dt.Rows[n]["FileId"].ToString() != "")
                    {
                        model.FileId = int.Parse(dt.Rows[n]["FileId"].ToString());
                    }
                    if (dt.Rows[n]["DaysFromEstClose"].ToString() != "")
                    {
                        model.DaysFromEstClose = int.Parse(dt.Rows[n]["DaysFromEstClose"].ToString());
                    }
                    model.StageName = dt.Rows[n]["StageName"].ToString();
                    if (dt.Rows[n]["WflTemplId"].ToString() != "")
                    {
                        model.WflTemplId = int.Parse(dt.Rows[n]["WflTemplId"].ToString());
                    }
                    if (dt.Rows[n]["WflStageId"].ToString() != "")
                    {
                        model.WflStageId = int.Parse(dt.Rows[n]["WflStageId"].ToString());
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

//        /// <summary>
//        /// 获得数据列表
//        /// </summary>
        //public DataSet GetList(int PageSize,int PageIndex,string strWhere)
        //{
        //return dal.GetList(PageSize,PageIndex,strWhere);
        //}

        /// <summary>
        /// Gets the loan stage setup info.
        /// </summary>
        /// <param name="fileId">The file id.</param>
        /// <returns></returns>
        public DataSet GetLoanStageSetupInfo(int fileId)
        {
            return dal.GetLoanStageSetupInfo(fileId);
        }

        /// <summary>
        /// Get the loan Stage Alias
        /// </summary>
        /// <param name="WorkflowType"></param>
        /// <returns></returns>
        public DataTable GetLoanStageAlias(string WorkflowType)
        {
            return dal.GetLoanStageAlias(WorkflowType);
        }

        #endregion  成员方法
    }
}
