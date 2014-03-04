using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
    /// <summary>
    /// LoanLocks
    /// </summary>
    public class LoanLocks
    {
        private readonly LPWeb.DAL.LoanLocks dal = new LPWeb.DAL.LoanLocks();
        public LoanLocks()
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
        public bool Exists(int FileId)
        {
            return dal.Exists(FileId);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(LPWeb.Model.LoanLocks model)
        {
            dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(LPWeb.Model.LoanLocks model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int FileId)
        {

            return dal.Delete(FileId);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string FileIdlist)
        {
            return dal.DeleteList(FileIdlist);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.LoanLocks GetModel(int FileId)
        {

            return dal.GetModel(FileId);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        //public LPWeb.Model.LoanLocks GetModelByCache(int FileId)
        //{

        //    string CacheKey = "LoanLocksModel-" + FileId;
        //    object objModel = Maticsoft.Common.DataCache.GetCache(CacheKey);
        //    if (objModel == null)
        //    {
        //        try
        //        {
        //            objModel = dal.GetModel(FileId);
        //            if (objModel != null)
        //            {
        //                int ModelCache = Maticsoft.Common.ConfigHelper.GetConfigInt("ModelCache");
        //                Maticsoft.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
        //            }
        //        }
        //        catch { }
        //    }
        //    return (LPWeb.Model.LoanLocks)objModel;
        //}

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
        public List<LPWeb.Model.LoanLocks> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<LPWeb.Model.LoanLocks> DataTableToList(DataTable dt)
        {
            List<LPWeb.Model.LoanLocks> modelList = new List<LPWeb.Model.LoanLocks>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LPWeb.Model.LoanLocks model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LPWeb.Model.LoanLocks();
                    if (dt.Rows[n]["FileId"].ToString() != "")
                    {
                        model.FileId = int.Parse(dt.Rows[n]["FileId"].ToString());
                    }
                    model.LockOption = dt.Rows[n]["LockOption"].ToString();
                    model.LockedBy = dt.Rows[n]["LockedBy"].ToString();
                    if (dt.Rows[n]["LockTime"].ToString() != "")
                    {
                        model.LockTime = DateTime.Parse(dt.Rows[n]["LockTime"].ToString());
                    }
                    if (dt.Rows[n]["LockTerm"].ToString() != "")
                    {
                        model.LockTerm = int.Parse(dt.Rows[n]["LockTerm"].ToString());
                    }
                    model.ConfirmedBy = dt.Rows[n]["ConfirmedBy"].ToString();
                    if (dt.Rows[n]["ConfirmTime"].ToString() != "")
                    {
                        model.ConfirmTime = DateTime.Parse(dt.Rows[n]["ConfirmTime"].ToString());
                    }
                    if (dt.Rows[n]["LockExpirationDate"].ToString() != "")
                    {
                        model.LockExpirationDate = DateTime.Parse(dt.Rows[n]["LockExpirationDate"].ToString());
                    }
                    if (dt.Rows[n]["Ext1Term"].ToString() != "")
                    {
                        model.Ext1Term = int.Parse(dt.Rows[n]["Ext1Term"].ToString());
                    }
                    if (dt.Rows[n]["Ext1LockExpDate"].ToString() != "")
                    {
                        model.Ext1LockExpDate = DateTime.Parse(dt.Rows[n]["Ext1LockExpDate"].ToString());
                    }
                    if (dt.Rows[n]["Ext1LockTime"].ToString() != "")
                    {
                        model.Ext1LockTime = DateTime.Parse(dt.Rows[n]["Ext1LockTime"].ToString());
                    }
                    model.Ext1LockedBy = dt.Rows[n]["Ext1LockedBy"].ToString();
                    if (dt.Rows[n]["Ext1ConfirmTime"].ToString() != "")
                    {
                        model.Ext1ConfirmTime = DateTime.Parse(dt.Rows[n]["Ext1ConfirmTime"].ToString());
                    }
                    if (dt.Rows[n]["Ext2Term"].ToString() != "")
                    {
                        model.Ext2Term = int.Parse(dt.Rows[n]["Ext2Term"].ToString());
                    }
                    if (dt.Rows[n]["Ext2LockExpDate"].ToString() != "")
                    {
                        model.Ext2LockExpDate = DateTime.Parse(dt.Rows[n]["Ext2LockExpDate"].ToString());
                    }
                    if (dt.Rows[n]["Ext2LockTime"].ToString() != "")
                    {
                        model.Ext2LockTime = DateTime.Parse(dt.Rows[n]["Ext2LockTime"].ToString());
                    }
                    model.Ext2LockedBy = dt.Rows[n]["Ext2LockedBy"].ToString();
                    if (dt.Rows[n]["Ext2ConfirmTime"].ToString() != "")
                    {
                        model.Ext2ConfirmTime = DateTime.Parse(dt.Rows[n]["Ext2ConfirmTime"].ToString());
                    }
                    if (dt.Rows[n]["Ext3Term"].ToString() != "")
                    {
                        model.Ext3Term = int.Parse(dt.Rows[n]["Ext3Term"].ToString());
                    }
                    if (dt.Rows[n]["Ext3LockExpDate"].ToString() != "")
                    {
                        model.Ext3LockExpDate = DateTime.Parse(dt.Rows[n]["Ext3LockExpDate"].ToString());
                    }
                    if (dt.Rows[n]["Ext3LockTime"].ToString() != "")
                    {
                        model.Ext3LockTime = DateTime.Parse(dt.Rows[n]["Ext3LockTime"].ToString());
                    }
                    model.Ext3LockedBy = dt.Rows[n]["Ext3LockedBy"].ToString();
                    if (dt.Rows[n]["Ext3ConfirmTime"].ToString() != "")
                    {
                        model.Ext3ConfirmTime = DateTime.Parse(dt.Rows[n]["Ext3ConfirmTime"].ToString());
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iFileId"></param>
        /// <returns></returns>
        public DataTable GetLoanLocksInfo(int iFileId)
        {
            return dal.GetLoanLocksInfo(iFileId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iFileId"></param>
        /// <param name="sLockOption"></param>
        public void UpdateLockOption(int iFileId, string sLockOption)
        {
            this.dal.UpdateLockOption(iFileId, sLockOption);
        }
    }
}

