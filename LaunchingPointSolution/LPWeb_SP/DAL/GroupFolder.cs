using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类EmailQue。
    /// </summary>
    public class GroupFolder : GroupFolderBase
    {
        public GroupFolder()
        { }
        /// <summary>
        /// save group folder
        /// </summary>
        /// <param name="iGroupID"></param>
        /// <param name="iID"></param>
        /// <param name="sType"></param>
        public void DoSaveGroupFolder(int iGroupID, int iID, string sType, int iOldGroupID)
        {
            try
            {
                //int iOldeGroupID = 0;
                ////get old group id
                //if (iID != 0)
                //{ 
                //    switch (sType.ToLower())
                //    {
                //        case "region":
                //            LPWeb.Model.Regions regionModel = new Model.Regions();
                //            Regions regionMgr = new Regions();
                //            regionModel = regionMgr.GetModel(iID);
                //            iOldeGroupID = Convert.ToInt32(regionModel.GroupID);
                //            break;
                //        case "division":
                //            LPWeb.Model.Divisions divModel = new Model.Divisions();
                //            Divisions divMgr = new Divisions();
                //            divModel = divMgr.GetModel(iID);
                //            iOldeGroupID = Convert.ToInt32(divModel.GroupID);
                //            break;
                //        case "branch":
                //            LPWeb.Model.Branches branchModel = new Model.Branches();
                //            Branches branchMgr = new Branches();
                //            branchModel = branchMgr.GetModel(iID);
                //            iOldeGroupID = Convert.ToInt32(branchModel.GroupID);
                //            break;
                //    }
                //}
                //Remove already folder by old group id
                this.DoDelGroupFolderByGroupID(iOldGroupID);
                //Remove already folder by group id
                this.DoDelGroupFolderByGroupID(iGroupID);

                DataTable dtFolderID = this.GetPointFolderIDByRegion(sType, iID);
                if (dtFolderID.Rows.Count < 1)
                {
                    return;
                }

                foreach (DataRow drFolder in dtFolderID.Rows)
                {
                    int iFolderID = Convert.ToInt32(drFolder[0].ToString());

                    LPWeb.Model.GroupFolder groupFolder = new Model.GroupFolder();
                    groupFolder.FolderId = iFolderID;
                    groupFolder.GroupId = iGroupID;
                    if (this.GetModel(iGroupID, iFolderID) == null)
                    {
                        this.Add(groupFolder);
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get point folder id by region id/division id/branch id
        /// </summary>
        /// <param name="sType"></param>
        /// <param name="iID"></param>
        /// <returns></returns>
        private DataTable GetPointFolderIDByRegion(string sType, int iID)
        {
            DataTable dtRe = new DataTable();
            string sSQL = "SELECT FolderId FROM PointFolders WHERE 1>0";// AND [Enabled]='true' ";
            try
            {
                switch (sType.ToLower())
                {
                    case "region":
                        sSQL += " AND BranchId IN(SELECT BranchId FROM Branches WHERE RegionID = " + iID.ToString() + ")";
                        break;
                    case "division":
                        sSQL += " AND BranchId IN(SELECT BranchId FROM Branches WHERE DivisionID = " + iID.ToString() + ")";
                        break;
                    case "branch":
                        sSQL += " AND BranchId =" + iID.ToString();
                        break;
                    case "company":
                        sSQL += "";
                        break;
                }
                sSQL += " ORDER BY FolderId";
                dtRe = DbHelperSQL.ExecuteDataTable(sSQL);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtRe;
        }

        /// <summary>
        /// Del group folder
        /// </summary>
        /// <param name="iGroupID"></param>
        private void DoDelGroupFolderByGroupID(int iGroupID)
        {
            try
            {
                string sSQL = "DELETE FROM GroupFolder WHERE GroupId=" + iGroupID;
                DbHelperSQL.ExecuteNonQuery(sSQL);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

