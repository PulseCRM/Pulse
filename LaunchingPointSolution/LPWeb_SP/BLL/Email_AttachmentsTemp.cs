using System;
using System.Data;
using System.Collections.Generic;
using LPWeb.Model;
namespace LPWeb.BLL
{
    /// <summary>
    /// Email_AttachmentsTemp
    /// </summary>
    public class Email_AttachmentsTemp
    {
        private readonly LPWeb.DAL.Email_AttachmentsTemp dal = new LPWeb.DAL.Email_AttachmentsTemp();
        public Email_AttachmentsTemp()
        { }
        #region  Method
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int Id)
        {
            return dal.Exists(Id);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.Email_AttachmentsTemp model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(LPWeb.Model.Email_AttachmentsTemp model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int Id)
        {

            return dal.Delete(Id);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string Idlist)
        {
            return dal.DeleteList(Idlist);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.Email_AttachmentsTemp GetModel(int Id)
        {

            return dal.GetModel(Id);
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
        public List<LPWeb.Model.Email_AttachmentsTemp> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<LPWeb.Model.Email_AttachmentsTemp> DataTableToList(DataTable dt)
        {
            List<LPWeb.Model.Email_AttachmentsTemp> modelList = new List<LPWeb.Model.Email_AttachmentsTemp>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LPWeb.Model.Email_AttachmentsTemp model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LPWeb.Model.Email_AttachmentsTemp();
                    if (dt.Rows[n]["Id"].ToString() != "")
                    {
                        model.Id = int.Parse(dt.Rows[n]["Id"].ToString());
                    }
                    model.Token = dt.Rows[n]["Token"].ToString();
                    if (dt.Rows[n]["TemplAttachId"].ToString() != "")
                    {
                        model.TemplAttachId = int.Parse(dt.Rows[n]["TemplAttachId"].ToString());
                    }
                    model.Name = dt.Rows[n]["Name"].ToString();
                    model.FileType = dt.Rows[n]["FileType"].ToString();
                    if (dt.Rows[n]["FileImage"].ToString() != "")
                    {
                        model.FileImage = (byte[])dt.Rows[n]["FileImage"];
                    }
                    if (dt.Rows[n]["CreateDateTime"].ToString() != "")
                    {
                        model.CreateDateTime = DateTime.Parse(dt.Rows[n]["CreateDateTime"].ToString());
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



        public DataSet GetList(int TemplEmailId, string Token)
        {
            return dal.GetList(TemplEmailId, Token);
        }

        public DataSet GetListWithFileImage(int TemplEmailId, string Token)
        {
            return dal.GetListWithFileImage(TemplEmailId, Token);
        }


        public bool DeleteByToken(string Token)
        {
            return dal.DeleteByToken(Token);
        }
    }
}

