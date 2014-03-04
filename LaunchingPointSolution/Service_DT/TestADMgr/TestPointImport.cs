using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataAccess;
using LP2.Service.Common;
using Framework;
using System.Collections;
using System.IO;
using EmailManager;

namespace TestADMgr
{
    public partial class TestPointImport : Form
    {
        public TestPointImport()
        {
            InitializeComponent();

            List<string> commands = new List<string>{ 
                                      "Add Note",
                                      "Calculate Due Dates",
                                      "Convert to Lead",
                                      "Dispose Loan",
                                      "Dispose Lead",
                                      "Email LSR",
                                      "Export Email Log",
                                      "Export Task History",
                                      "Generate Workflow",
                                      "Get LastModifiedTime",
                                      "Import All Loans",
                                      "Import Loans",
                                      "Load Marketing",
                                      "Load Prospect Campaigns",
                                      "Mass Generate Workflow",
                                      "Mass Reassign Loans",
                                      "Mass Update Stages",
                                      "Monitor All Loans",
                                      "Monitor Prospect Tasks",
                                      "Monitor A Loan",
                                      "Move Point File",
                                      "Process Loan Rules",
                                      "Reassign Contact",
                                      "Reassign Loan",
                                      "Send Email Que",
                                      "start campaign",
                                      "Submit Loan to DataTrac",
                                      "remove campaign",
                                      "update credit card",
                                      "Update Borrower",
                                      "Update Contact",
                                       "Update Est Close Date",                                     "Update Leadstar Company",
                                      "Update LeadStar Branch",                                      
                                      "update leadstar prospect",
                                      "UPdate Leadstar User",
                                      "Update Loan Info",
                                      "Update Stage"
                                    };
            commands.Sort();
            foreach (string cmd in commands)
            {
                comboCommand.Items.Add(cmd);
            }

            comboStages.Items.Add("Application");
            //comboStages.Items.Add("Open");
            comboStages.Items.Add("Sent to Processing");
            comboStages.Items.Add("HMDA Complete");
            comboStages.Items.Add("Submit");
            comboStages.Items.Add("Re-Submit");
            comboStages.Items.Add("Approve");
            comboStages.Items.Add("Clear to Close");
            comboStages.Items.Add("Docs");
            comboStages.Items.Add("Docs Drawn");
            comboStages.Items.Add("Docs Out");
            comboStages.Items.Add("Docs Received");
            comboStages.Items.Add("Fund");
            comboStages.Items.Add("Record");
            comboStages.Items.Add("Close");
            string filename = "--";
            TimeSpan ts = new TimeSpan(DateTime.Now.Ticks);
            string tempStr = ts.TotalSeconds.ToString();
            int index = tempStr.LastIndexOf('.');
            if (index > 0)
                tempStr = tempStr.Remove(index, tempStr.Length - index);

            filename = string.Format("{0}{1}", filename, tempStr.Substring(2, 8));
            //filename += tempStr.Substring(index + 1, tempStr.Length - (index+1)) + tempStr.Substring(0, index);

            //    tempStr = tempStr.Remove(index, 1);
            //tempStr = tempStr.Substring(tempStr.Length - 10, 10);
            //filename += tempStr;
            if (filename.Length > 10)
                filename = filename.Substring(0, 10);
        }

        //private void btnBrowse_Click(object sender, EventArgs e)
        //{
        //    DialogResult dr = openFileDialog1.ShowDialog();
        //    if (dr == DialogResult.OK)
        //    {
        //        tbPointFile.Text = openFileDialog1.FileName;
        //    }
        //}
        private void MonitorProspectTasks()
        {
            try
            {
                LP2Service.WorkflowManager wm = LP2Service.WorkflowManager.Instance;
                DataAccess.DataAccess da = new DataAccess.DataAccess();
                int reqId = 0;
                string err = "";
                da.AddRequestQueue(0, LP2.Service.Common.WorkflowCmd.GenerateWorkflow.ToString(), ref reqId, ref err);

                wm.MonitorProspectTasks();

            }
            catch (Exception ex)
            {
                rtbMsg.AppendText(ex.Message);
            }
        }
        private void MonitorAllLoans()
        {
            try
            {
                LP2Service.WorkflowManager wm = LP2Service.WorkflowManager.Instance;
                DataAccess.DataAccess da = new DataAccess.DataAccess();
                int reqId = 0;
                string err = "";
                da.AddRequestQueue(0, LP2.Service.Common.WorkflowCmd.GenerateWorkflow.ToString(), ref reqId, ref err);

                wm.MonitorLoans();

            }
            catch (Exception ex)
            {
                rtbMsg.AppendText(ex.Message);
            }
        }
        private void MonitorLoans()
        {
            if (txFileId.Text.Length <= 0)
            {
                MessageBox.Show("You must enter at least one file id. Multiple file ids can be separate using ';'.");
                return;
            }
            string[] sFileIds = txFileId.Text.Split(';');
            try
            {
                LP2Service.WorkflowManager wm = LP2Service.WorkflowManager.Instance;
                DataAccess.DataAccess da = new DataAccess.DataAccess();
                int reqId = 0;
                string err = "";
                da.AddRequestQueue(0, LP2.Service.Common.WorkflowCmd.GenerateWorkflow.ToString(), ref reqId, ref err);
                foreach (string sFileId in sFileIds)
                {
                    if (wm.MonitorLoan(Convert.ToInt32(sFileId), true, ref err) == false)
                    {
                        rtbMsg.AppendText(err);
                    }
                }
                //wm.MonitorLoans();
            }
            catch (Exception ex)
            {
                rtbMsg.AppendText(ex.Message);
            }

        }
        private void GetPointFilesInFolder(List<PointFolderInfo> folderList, System.IO.StreamWriter oFileStr)
        {
            List<Common.Table.PointFileInfo> fileList = null;
            int count = 0;
            string str = string.Empty;
            string err = string.Empty;
            string sqlCmd = string.Empty;
            DataAccess.DataAccess da = new DataAccess.DataAccess();
            foreach (PointFolderInfo pf in folderList)
            {
                count = 0;
                str = "Folder Name " + pf.Name + "Path " + pf.Path + " contains ";
                fileList = da.GetPointFileInfoByFolderId(pf.FolderId, ref err);

                if (fileList == null)
                    count = 0;
                else count = fileList.Count;
                str = string.Format("There are {0} files in the DB For Folder Name {1} Path {2}.", count, pf.Name, pf.Path);
                oFileStr.WriteLine(str);
                if (fileList == null || fileList.Count <= 0)
                    continue;
                foreach (Common.Table.PointFileInfo fi in fileList)
                {
                    sqlCmd = string.Empty;
                    count++;
                    try
                    {
                        if (!File.Exists(fi.Path))
                        {
                            str = fi.Path + " does not exist.";
                            oFileStr.WriteLine(str);
                            sqlCmd = string.Format("Update PointImportHistory set Error = '{0}', Severity='Error' where FileId={1}", "The Point file " + fi.Path + " is not found. It may have been removed from the folder.", fi.FileId);
                            focusIT.DbHelperSQL.ExecuteSql(sqlCmd);
                            continue;
                        }
                        Nullable<DateTime> LastModified = new Nullable<DateTime>();
                        DateTime fileModTime = File.GetLastWriteTime(fi.Path);
                        str = string.Format("{0} was last modified: {1}.", fi.Path, fileModTime.ToString());
                        oFileStr.WriteLine(str);
                        LastModified = fileModTime;
                        da.Update_Pointfile_LastModified(fi.FileId, LastModified);
                        //sqlCmd = string.Format("Update PointFiles set LastModifiedTime ='{0}' where FileId={1}", fileModTime, fi.FileId);
                        //focusIT.DbHelperSQL.ExecuteSql(sqlCmd);
                    }
                    catch (Exception ee)
                    {
                        rtbMsg.AppendText(ee.Message);
                        continue;
                    }
                }
                str = string.Format(" Total Point files processed: {0} ", count);
                oFileStr.WriteLine(str);

            }
        }
        private void GetPointModifiedTime()
        {
            int loanStatus = 1;
            System.IO.StreamWriter oFileStr = null;
            //if (tbReqData.Text.ToLower() == "prospect")
            //    loanStatus = 6;
            try
            {
                //LP2Service.PointMgr pm = LP2Service.PointMgr.Instance;
                DataAccess.DataAccess da = new DataAccess.DataAccess();
                string outputFile = @"C:\PointLastMod\PointFiles_LastModTime.txt";
                if (!Directory.Exists(Path.GetDirectoryName(outputFile)))
                    Directory.CreateDirectory(Path.GetDirectoryName(outputFile));
                if (!File.Exists(outputFile))
                    File.Create(outputFile);
                oFileStr = new System.IO.StreamWriter(outputFile);
                oFileStr.AutoFlush = true;
                string str = "Beginning..." + DateTime.Now.ToString() + Environment.NewLine;
                oFileStr.WriteLine(str);
                for (int i = 0; i <= 1; i++)
                {
                    string err = "";
                    List<PointFolderInfo> folderList = da.GetPointFolders(loanStatus, true, ref err);
                    if (folderList == null || folderList.Count <= 0)
                    {
                        str = string.Format("No folder found for LoanStatus={0}.", loanStatus);
                        oFileStr.WriteLine(str);
                    }
                    GetPointFilesInFolder(folderList, oFileStr);
                    str = "=================================================================================" + Environment.NewLine;
                    oFileStr.WriteLine();
                    oFileStr.WriteLine(str);
                    loanStatus = 6;
                }
                oFileStr.Close();
            }
            catch (Exception ex)
            {
                rtbMsg.AppendText(ex.Message);
            }
        }

        private void GenerateWorkflow()
        {
            if ((txFileId.Text.Length <= 0) || (tbReqData.Text.Length <= 0))
            {
                MessageBox.Show("You must enter file id or the Workflow Template Id in the Request Data field. ");
                return;
            }
            string[] sFileIds = txFileId.Text.Split(';');
            try
            {
                LP2Service.WorkflowManager wm = LP2Service.WorkflowManager.Instance;
                DataAccess.DataAccess da = new DataAccess.DataAccess();
                int reqId = 0;
                string err = "";
                da.AddRequestQueue(0, LP2.Service.Common.WorkflowCmd.GenerateWorkflow.ToString(), ref reqId, ref err);

                int i = 0;
                LP2.Service.Common.GenerateWorkflowRequest msg = new GenerateWorkflowRequest();
                msg.hdr = new ReqHdr();
                msg.hdr.UserId = 0;

                msg.FileId = Convert.ToInt32(sFileIds[0]);

                msg.WorkflowTemplId = Convert.ToInt32(tbReqData.Text);
                LP2Service.WorkflowEvent we = new LP2Service.WorkflowEvent(LP2.Service.Common.WorkflowCmd.GenerateWorkflow, msg);
                wm.ProcessRequest(we);
            }
            catch (Exception ex)
            {
                rtbMsg.AppendText(ex.Message);
            }

        }
        private void MassGenerateWorkflow()
        {
            try
            {
                LP2Service.WorkflowManager wm = LP2Service.WorkflowManager.Instance;
                DataAccess.DataAccess da = new DataAccess.DataAccess();
                int reqId = 0;
                string err = "";
                da.AddRequestQueue(0, "MassGenerateWorkflow", ref reqId, ref err);
                string sqlCmd = "Select lw.FileId, lw.WflTemplId from LoanWflTempl lw inner join Loans l on lw.FileId=l.FileId where l.Status='Processing' and lw.WflTemplId IS NOT NULL";
                DataSet ds = focusIT.DbHelperSQL.Query(sqlCmd);
                if (ds == null || ds.Tables[0].Rows.Count <= 0)
                {
                    err = "No records found for " + sqlCmd;
                    rtbMsg.AppendText(err);
                    return;
                }
                int WflTemplId = 0;
                if (!string.IsNullOrEmpty(tbReqData.Text))
                    WflTemplId = Convert.ToInt32(tbReqData.Text);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    LP2.Service.Common.GenerateWorkflowRequest msg = new GenerateWorkflowRequest();
                    msg.hdr = new ReqHdr();
                    msg.hdr.UserId = 0;

                    msg.FileId = (int)dr["FileId"];
                    if (WflTemplId > 0)
                        msg.WorkflowTemplId = WflTemplId;
                    else
                        msg.WorkflowTemplId = (int)dr["WflTemplId"];

                    LP2Service.WorkflowEvent we = new LP2Service.WorkflowEvent(LP2.Service.Common.WorkflowCmd.GenerateWorkflow, msg);
                    wm.ProcessRequest(we);
                    sqlCmd = string.Format("Update LoanWflTempl set WflTemplId = {0}, ApplyDate=GetDate(), ApplyBy=0 WHERE FileId={1}", msg.WorkflowTemplId, msg.FileId);
                    focusIT.DbHelperSQL.ExecuteNonQuery(sqlCmd);
                }
            }
            catch (Exception ex)
            {
                rtbMsg.AppendText(ex.Message);
            }

        }
        private void ImportLoans()
        {
            if (txFileId.Text.Length <= 0)
            {
                MessageBox.Show("You did not enter at least one file id. Multiple file ids can be separated using ';'.");
                return;
            }
            string[] sFileId = txFileId.Text.Split(';');
            try
            {
                LP2Service.PointMgr pm = LP2Service.PointMgr.Instance;
                DataAccess.DataAccess da = new DataAccess.DataAccess();
                int reqId = 0;
                string err = "";
                da.AddRequestQueue(0, LP2.Service.Common.PointMgrCommandType.ImportLoans.ToString(), ref reqId, ref err);

                ImportLoansRequest req = new ImportLoansRequest();
                req.hdr = new ReqHdr();
                req.hdr.UserId = 25;
                req.FileIds = new int[sFileId.Length];
                int i = 0;
                foreach (string fileId in sFileId)
                {
                    req.FileIds[i] = Convert.ToInt32(fileId);
                    i++;
                }
                LP2Service.PointMgrEvent pe = new LP2Service.PointMgrEvent(reqId, PointMgrCommandType.ImportLoans, 0, req);

                if (pm.ProcessRequest(pe, ref err) == false)
                    rtbMsg.AppendText(err);
            }
            catch (Exception ex)
            {
                rtbMsg.AppendText(ex.Message);
            }

        }
        private void ExtendRateLock()
        {
            string err = "";
            if ((txFileId.Text.Length <= 0) || (tbReqData.Text.Length <= 0))
            {
                MessageBox.Show("You did not enter File id or days to extend and new Rate Lock Expiration in the Request Data field separated by ';'. ");
                return;
            }
            string[] sFileIds = txFileId.Text.Split(';');
            string[] sDates = tbReqData.Text.Split(';');
            try
            {
                LP2Service.PointMgr pm = LP2Service.PointMgr.Instance;
                ExtendRateLockRequest req = new ExtendRateLockRequest();
                req.hdr = new ReqHdr();
                req.hdr.UserId = 12;
                req.FileId = Convert.ToInt32(sFileIds[0]);
                req.DaysExtended = Convert.ToInt32(sDates[0]);
                req.NewDate = new DateTime();
                req.NewDate = DateTime.Parse(sDates[1]).Date;
                LP2Service.PointMgrEvent pe = new LP2Service.PointMgrEvent(0, PointMgrCommandType.ExtendRateLock, 2, req);
                if (pm.ProcessRequest(pe, ref err) == false)
                {
                    rtbMsg.AppendText(err);
                }
            }
            catch (Exception ex)
            {
                rtbMsg.AppendText(ex.Message);
            }

        }
        private void UpdateContact()
        {
            string err = "";
            if (txFileId.Text.Length <= 0)
            {
                MessageBox.Show("You must enter the contact id in the File Id field. ");
                return;
            }
            string contactId = txFileId.Text;
            string stage = (string)comboStages.SelectedItem;
            try
            {
                LP2Service.PointMgr pm = LP2Service.PointMgr.Instance;
                UpdateBorrowerRequest req = new UpdateBorrowerRequest();
                req.hdr = new ReqHdr();
                req.hdr.UserId = 12;
                req.ContactId = Convert.ToInt32(contactId);
                //LP2Service.PointMgrEvent pe = new LP2Service.PointMgrEvent(0, PointMgrCommandType.UpdateBorrower, req.ContactId, req);
                if (pm.UpdateBorrower(req, ref err) == false)
                {
                    rtbMsg.AppendText(err);
                }
            }
            catch (Exception ex)
            {
                rtbMsg.AppendText(ex.Message);
            }
        }
        private void UpdateBorrower()
        {
            string err = "";
            if (txFileId.Text.Length <= 0)
            {
                MessageBox.Show("You must enter the contact id in the File Id field. ");
                return;
            }
            string contactId = txFileId.Text;
            string stage = (string)comboStages.SelectedItem;
            try
            {
                LP2Service.PointMgr pm = LP2Service.PointMgr.Instance;
                UpdateBorrowerRequest req = new UpdateBorrowerRequest();
                req.hdr = new ReqHdr();
                req.hdr.UserId = 12;
                req.ContactId = Convert.ToInt32(contactId);
                //LP2Service.PointMgrEvent pe = new LP2Service.PointMgrEvent(0, PointMgrCommandType.UpdateBorrower, req.ContactId, req);
                if (pm.UpdateBorrower(req, ref err) == false)
                {
                    rtbMsg.AppendText(err);
                }
            }
            catch (Exception ex)
            {
                rtbMsg.AppendText(ex.Message);
            }
        }
        private void UpdateLoanInfo()
        {
            string err = "";
            if (txFileId.Text.Length <= 0)
            {
                MessageBox.Show("You must enter the FileId in the File Id field. ");
                return;
            }
            string FileId = txFileId.Text;
            string stage = (string)comboStages.SelectedItem;
            try
            {
                LP2Service.PointMgr pm = LP2Service.PointMgr.Instance;
                UpdateLoanInfoRequest req = new UpdateLoanInfoRequest();
                req.hdr = new ReqHdr();
                req.hdr.UserId = 12;
                req.FileId = Convert.ToInt32(FileId);
                //LP2Service.PointMgrEvent pe = new LP2Service.PointMgrEvent(0, PointMgrCommandType.UpdateBorrower, req.ContactId, req);
                if (pm.UpdateLoanInfo(req, ref err) == false)
                {
                    rtbMsg.AppendText(err);
                }
            }
            catch (Exception ex)
            {
                rtbMsg.AppendText(ex.Message);
            }
        }

        private void UpdateStage()
        {
            string err = "";
            if (txFileId.Text.Length <= 0)
            {
                MessageBox.Show("You must enter file id. ");
                return;
            }
            string[] sFileIds = txFileId.Text.Split(';');
            string[] stage = tbReqData.Text.Split(';');
            try
            {
                LP2Service.PointMgr pm = LP2Service.PointMgr.Instance;
                UpdateStageRequest req = new UpdateStageRequest();
                req.hdr = new ReqHdr();
                req.hdr.UserId = 12;
                req.FileId = Convert.ToInt32(sFileIds[0]);
                req.StageList = new List<StageInfo>();
                StageInfo si = new StageInfo();
                si.StageName = stage[0];
                si.LoanStageId = Convert.ToInt32(stage[1]);
                si.Completed = DateTime.Today;
                req.StageList.Add(si);
                LP2Service.PointMgrEvent pe = new LP2Service.PointMgrEvent(0, PointMgrCommandType.UpdateStage, req.FileId, req);
                if (pm.ProcessRequest(pe, ref err) == false)
                {
                    rtbMsg.AppendText(err);
                }
            }
            catch (Exception ex)
            {
                rtbMsg.AppendText(ex.Message);
            }
        }
        private void ExportTaskHistory()
        {
            string err = "";

            try
            {
                LP2Service.PointMgr pm = LP2Service.PointMgr.Instance;
                if (pm.ExportTaskHistory(ref err) == false)
                {
                    rtbMsg.AppendText(err);
                }
            }
            catch (Exception ex)
            {
                rtbMsg.AppendText(ex.Message);
            }
        }
        private void AddNotes()
        {
            string err = "";
            if ((txFileId.Text.Length <= 0) || (tbReqData.Text.Length <= 0))
            {
                MessageBox.Show("You must enter file id and the UserId and Note in the Request Data field separated by ';'");
                return;
            }
            string[] sFileIds = txFileId.Text.Split(';');
            string[] sUserNote = tbReqData.Text.Split(';');
            if (sUserNote.Length <= 1)
            {
                MessageBox.Show("Both UserId and Note need to be entered in the Request Data field separated by ';'");
                return;
            }
            try
            {
                DataSet ds = null;
                string sqlCmd = "Select TOP 1 FirstName+' '+LastName as FullName from Users WHERE UserId=" + sUserNote[0];
                ds = focusIT.DbHelperSQL.Query(sqlCmd);
                if ((ds == null) || (ds.Tables[0].Rows.Count <= 0))
                {
                    MessageBox.Show(String.Format("Cannot find User record for UserId={0}.", sUserNote[0]));
                    return;
                }
                DataRow dr = ds.Tables[0].Rows[0];
                if (dr == null)
                {
                    MessageBox.Show(String.Format("Cannot find User record for UserId={0}.", sUserNote[0]));
                    return;
                }

                if (dr["FullName"] == DBNull.Value)
                {
                    MessageBox.Show(String.Format("The User record with UserId={0} is missing First and Last Names", sUserNote[0]));
                    return;
                }
                string Sender = dr["FullName"].ToString();

                LP2Service.PointMgr pm = LP2Service.PointMgr.Instance;
                AddNoteRequest req = new AddNoteRequest();
                req.hdr = new ReqHdr();
                req.hdr.UserId = Convert.ToInt32(sUserNote[0]);
                req.FileId = Convert.ToInt32(sFileIds[0]);
                req.Created = DateTime.Now;

                req.Sender = Sender;
                req.Note = sUserNote[1].Trim();
                LP2Service.PointMgrEvent pe = new LP2Service.PointMgrEvent(0, PointMgrCommandType.AddNote, req.FileId, req);
                if (pm.ProcessRequest(pe, ref err) == false)
                {
                    rtbMsg.AppendText(err);
                }
            }
            catch (Exception ex)
            {
                rtbMsg.AppendText(ex.Message);
            }
        }
        private void ExportEmailLog()
        {
            string err = "";
            if (txFileId.Text.Length <= 0)
            {
                MessageBox.Show("You did not enter File id in the FileId field. ");
                return;
            }
            try
            {
                DataAccess.DataAccess da = new DataAccess.DataAccess();
                LP2Service.PointMgr pm = LP2Service.PointMgr.Instance;
                int FileId = Convert.ToInt32(txFileId.Text.Trim());
                List<FieldMap> updFields = new List<FieldMap>();
                string ConvLog = "";
                string path = da.GetPointFilePath(FileId, ref err);
                if (path == "")
                {
                    MessageBox.Show("Cannot find the filepath for fileId=" + FileId);
                    return;
                }
                List<string> FieldArray = new List<string>();
                ArrayList FieldSeq = new ArrayList();
                PNTLib pnt = new PNTLib();
                if (pnt.ReadPointData(ref FieldArray, ref FieldSeq, path, ref err) == false)
                {
                    rtbMsg.AppendText(err);
                    return;
                }
                ConvLog = pnt.getPointField(FieldArray, FieldSeq, 15);
                if (ConvLog == null)
                    ConvLog = "";
                if (pm.ExportEmailLog(FileId, ref updFields, ConvLog, ref err) == false)
                    rtbMsg.AppendText(err);
                if (pnt.WritePointData(updFields, path, ref err) == false)
                    rtbMsg.AppendText(err);
            }
            catch (Exception ex)
            {
                rtbMsg.AppendText(ex.Message);
            }

        }
        private void ReassignContact()
        {
            string err = "";
            if ((txFileId.Text.Length <= 0) || (tbReqData.Text.Length <= 0))
            {
                MessageBox.Show("You did not enter File id or the Contact Id and Role Id separated by ';' in the Request Data field. ");
                return;
            }
            string[] sFileIds = txFileId.Text.Split(';');
            string[] sContactRoleId = tbReqData.Text.Split(';');         //ContactId;RoleId
            if (sContactRoleId.Length <= 1)
            {
                MessageBox.Show("You did not enter RoleId in the Request Data field, format: 'ContactId;RoleId'.");
                return;
            }
            try
            {
                int ContactId = Convert.ToInt32(sContactRoleId[0]);
                List<ReassignContactInfo> reassignList = new List<ReassignContactInfo>();
                int i = 0;
                foreach (string f in sFileIds)
                {
                    ReassignContactInfo reassignContact = new ReassignContactInfo();
                    reassignContact.FileId = Convert.ToInt32(f);
                    reassignContact.NewContactId = ContactId;
                    i++;
                    if (i >= sContactRoleId.Length - 1)
                        reassignContact.ContactRoleId = Convert.ToInt32(sContactRoleId[1]);
                    else
                        reassignContact.ContactRoleId = Convert.ToInt32(sContactRoleId[i]);
                    reassignList.Add(reassignContact);
                }

                LP2Service.PointMgr pm = LP2Service.PointMgr.Instance;
                ReassignContactRequest req = new ReassignContactRequest();
                req.hdr = new ReqHdr();
                req.hdr.UserId = 12;
                req.reassignContacts = reassignList;
                //LP2Service.PointMgrEvent pe = new LP2Service.PointMgrEvent(0, PointMgrCommandType.ReassignContact, req);
                //if (pm.ProcessRequest(pe, ref err) == false)
                if (pm.ReassignContact(req, ref err) == false)
                {
                    rtbMsg.AppendText(err);
                }
            }
            catch (Exception ex)
            {
                rtbMsg.AppendText(ex.Message);
            }

        }
        private void ReassignLoan()
        {
            string err = "";
            if ((txFileId.Text.Length <= 0) || (tbReqData.Text.Length <= 0))
            {
                MessageBox.Show("You did not enter File id or the User Id and Role Id separated by ';' in the Request Data field. ");
                return;
            }
            string[] sFileIds = txFileId.Text.Split(';');
            string[] sUserRoleId = tbReqData.Text.Split(';');         //UserId;RoleId
            if (sUserRoleId.Length <= 1)
            {
                MessageBox.Show("You did not enter RoleId in the Request Data field, format: 'UserId;RoleId'.");
                return;
            }
            try
            {
                LP2Service.PointMgr pm = LP2Service.PointMgr.Instance;
                ReassignLoanRequest req = new ReassignLoanRequest();
                req.hdr = new ReqHdr();
                req.hdr.UserId = 12;
                int userId = Convert.ToInt32(sUserRoleId[0]);
                List<ReassignUserInfo> reassignList = new List<ReassignUserInfo>();
                int i = 0;
                foreach (string f in sFileIds)
                {
                    ReassignUserInfo u = new ReassignUserInfo();
                    u.FileId = Convert.ToInt32(f);
                    i++;
                    u.NewUserId = userId;
                    u.RoleId = Convert.ToInt32(sUserRoleId[i]);
                    reassignList.Add(u);
                }
                req.reassignUsers = reassignList;
                //LP2Service.PointMgrEvent pe = new LP2Service.PointMgrEvent(0, PointMgrCommandType.ReassignLoan, req);
                //if (pm.ProcessRequest(pe, ref err) == false)
                if (pm.ReassignLoan(req, ref err) == false)
                {
                    rtbMsg.AppendText(err);
                }
            }
            catch (Exception ex)
            {
                rtbMsg.AppendText(ex.Message);
            }
        }
        private void MovePointFile()
        {
            string err = "";
            if ((txFileId.Text.Length <= 0) || (tbReqData.Text.Length <= 0))
            {
                MessageBox.Show("You did not enter File id or the ToFolderId and Loan Status separated by ';' in the Request Data field. ");
                return;
            }
            string[] sFileIds = txFileId.Text.Split(';');
            string[] sFolderStatus = tbReqData.Text.Split(';');         //NewFolderId;LoanStatus
            if (sFolderStatus.Length <= 1)
            {
                MessageBox.Show("You did not enter Loan Status in the Request Data field, format: 'ToFolderId;LoanStatus'.");
                return;
            }
            try
            {
                LP2Service.PointMgr pm = LP2Service.PointMgr.Instance;
                MoveFileRequest req = new MoveFileRequest();
                req.hdr = new ReqHdr();
                req.hdr.UserId = 12;
                req.FileId = Convert.ToInt32(sFileIds[0]);
                LP2.Service.Common.LoanStatusEnum loanStatus = (LoanStatusEnum)Enum.Parse(typeof(LoanStatusEnum), sFolderStatus[1], true); ;

                req.NewFolderId = Convert.ToInt32(sFolderStatus[0]);
                LP2Service.PointMgrEvent pe = new LP2Service.PointMgrEvent(0, PointMgrCommandType.MovePointFile, req.FileId, req);
                if (pm.ProcessRequest(pe, ref err) == false)
                {
                    rtbMsg.AppendText(err);
                }
            }
            catch (Exception ex)
            {
                rtbMsg.AppendText(ex.Message);
            }
        }
        private void DisposeLoan()
        {
            string err = "";
            if ((txFileId.Text.Length <= 0) || (tbReqData.Text.Length <= 0))
            {
                MessageBox.Show("You did not enter File id or the ToFolderId and Loan Status separated by ';' in the Request Data field. ");
                return;
            }
            string[] sFileIds = txFileId.Text.Split(';');
            string[] sFolderStatus = tbReqData.Text.Split(';');         //NewFolderId;LoanStatus
            if (sFolderStatus.Length <= 1)
            {
                MessageBox.Show("You did not enter Loan Status in the Request Data field, format: 'ToFolderId;LoanStatus'.");
                return;
            }
            try
            {
                LP2Service.PointMgr pm = LP2Service.PointMgr.Instance;
                DisposeLoanRequest req = new DisposeLoanRequest();
                req.hdr = new ReqHdr();
                req.hdr.UserId = 12;
                req.FileId = Convert.ToInt32(sFileIds[0]);

                req.LoanStatus = sFolderStatus[1];
                req.NewFolderId = Convert.ToInt32(sFolderStatus[0]);
                req.StatusDate = DateTime.Today.Date;
                LP2Service.PointMgrEvent pe = new LP2Service.PointMgrEvent(0, PointMgrCommandType.DisposeLoan, req.FileId, req);
                if (pm.ProcessRequest(pe, ref err) == false)
                {
                    rtbMsg.AppendText(err);
                }
            }
            catch (Exception ex)
            {
                rtbMsg.AppendText(ex.Message);
            }
        }

        private void ConvertToLead()
        {
            string err = "";
            if ((txFileId.Text.Length <= 0) || (tbReqData.Text.Length <= 0))
            {
                MessageBox.Show("You did not enter File Id or the NewFolderId in the Request Data field. ");
                return;
            }
            string[] sFileIds = txFileId.Text.Split(';');
            string[] sFolderStatus = tbReqData.Text.Split(';');         //NewFolderId

            try
            {
                LP2Service.PointMgr pm = LP2Service.PointMgr.Instance;
                ConvertToLeadRequest req = new ConvertToLeadRequest();

                req.hdr = new ReqHdr();
                req.hdr.UserId = 12;
                req.FileId = Convert.ToInt32(sFileIds[0]);
                req.NewFolderId = Convert.ToInt32(sFolderStatus[0]);
                LP2Service.PointMgrEvent pe = new LP2Service.PointMgrEvent(0, PointMgrCommandType.ConvertToLead, req.FileId, req);
                if (pm.ProcessRequest(pe, ref err) == false)
                {
                    rtbMsg.AppendText(err);
                }
            }
            catch (Exception ex)
            {
                rtbMsg.AppendText(ex.Message);
            }
        }
        private void DisposeLead()
        {
            string err = "";
            if ((txFileId.Text.Length <= 0) || (tbReqData.Text.Length <= 0))
            {
                MessageBox.Show("You did not enter File id or the ToFolderId and Loan Status separated by ';' in the Request Data field. ");
                return;
            }
            string[] sFileIds = txFileId.Text.Split(';');
            string[] sFolderStatus = tbReqData.Text.Split(';');         //NewFolderId;LoanStatus
            if (sFolderStatus.Length <= 1)
            {
                MessageBox.Show("You did not enter Loan Status in the Request Data field, format: 'ToFolderId;LoanStatus'.");
                return;
            }
            try
            {
                LP2Service.PointMgr pm = LP2Service.PointMgr.Instance;
                DisposeLeadRequest req = new DisposeLeadRequest();
                req.hdr = new ReqHdr();
                req.hdr.UserId = 12;
                req.FileId = Convert.ToInt32(sFileIds[0]);

                req.LoanStatus = sFolderStatus[1];
                req.NewFolderId = Convert.ToInt32(sFolderStatus[0]);
                req.StatusDate = DateTime.Today.Date;
                LP2Service.PointMgrEvent pe = new LP2Service.PointMgrEvent(0, PointMgrCommandType.DisposeLead, req.FileId, req);
                if (pm.ProcessRequest(pe, ref err) == false)
                {
                    rtbMsg.AppendText(err);
                }
            }
            catch (Exception ex)
            {
                rtbMsg.AppendText(ex.Message);
            }
        }
        private void ImportAll()
        {
            int loanStatus = 1;
            if (tbReqData.Text.ToLower() == "prospect")
                loanStatus = 6;
            try
            {
                LP2Service.PointMgr pm = LP2Service.PointMgr.Instance;
                DataAccess.DataAccess da = new DataAccess.DataAccess();
                int reqId = 0;
                string err = "";
                //List<PointFolderInfo> folderList = da.GetPointFolders(loanStatus, true, ref err);
                da.AddRequestQueue(0, LP2.Service.Common.PointMgrCommandType.ImportAllLoans.ToString(), ref reqId, ref err);
                //string[] paths = new string[folderList.Count];
                //int i = 0;
                //foreach (PointFolderInfo pf in folderList)
                //{
                //    paths[i] = pf.Path;
                //    i++;
                //}
                int FolderId = Convert.ToInt32(tbReqData.Text);
                PointFolderInfo pf = da.GetPointFolderInfo(FolderId, ref err);
                string[] paths = { pf.Path };
                ImportAllLoansRequest req = new ImportAllLoansRequest();
                req.hdr = new ReqHdr();
                req.hdr.UserId = 12;
                req.PointFolders = paths;
                LP2Service.PointMgrEvent pe = new LP2Service.PointMgrEvent(reqId, PointMgrCommandType.ImportAllLoans, 0, req);

                if (pm.ProcessRequest(pe, ref err) == false)
                    rtbMsg.AppendText(err);
            }
            catch (Exception ex)
            {
                rtbMsg.AppendText(ex.Message);
            }
        }
        private void ProcessLoanRules()
        {
            try
            {
                RuleManager.RuleMgr rm = RuleManager.RuleMgr.Instance;
                rm.ProcessLoanRules();
            }
            catch (Exception ex)
            {
                rtbMsg.AppendText(ex.Message);
            }
        }
        private void SendEmailQueue()
        {
            string err = string.Empty;
            try
            {
                EmailManager.EmailMgr em = EmailManager.EmailMgr.Instance;
                SendEmailRequest req = new SendEmailRequest();
                req.FileId = 2504;
                req.UserId = 1;
                req.ToUserIds = new int[1]{1};
                req.EmailTemplId = 35;
                em.SendEmail(req);
 
                //int emailQueId = 0;
                //int.TryParse(tbReqData.Text, out emailQueId);
                //req.EmailQueId = emailQueId;
                //SendStatus resp = em.SendEmailQue(ref req, ref err);
            }
            catch (Exception ex)
            {
                rtbMsg.AppendText(ex.Message);
            }
        }
        private void LoadMarketing()
        {
            try
            {
                MarketingManager.MkgMgr mm = MarketingManager.MkgMgr.Instance;
                mm.LoadLeadStarMarketing();
            }
            catch (Exception ex)
            {
                rtbMsg.AppendText(ex.Message);
            }
        }

        private void UpdateLeadStarCompany()
        {
            string err = "";

            try
            {
                MarketingManager.MkgMgr mm = MarketingManager.MkgMgr.Instance;
                mm.UpdateCompany(ref err);
                rtbMsg.AppendText(err);
            }
            catch (Exception ex)
            {
                rtbMsg.AppendText(ex.Message);
            }
        }
        private void UpdateLeadStarBranch()
        {
            string err = "";
            if (string.IsNullOrEmpty(txFileId.Text))
            {
                MessageBox.Show("Please specify the BranchId in the FileId field.");
                return;
            }
            int BranchId = Convert.ToInt32(txFileId.Text);
            try
            {
                MarketingManager.MkgMgr mm = MarketingManager.MkgMgr.Instance;
                mm.UpdateBranch(BranchId, ref err);
                rtbMsg.AppendText(err);
            }
            catch (Exception ex)
            {
                rtbMsg.AppendText(ex.Message);
            }
        }

        private void UpdateLeadStarUser()
        {
            string err = "";
            if (string.IsNullOrEmpty(txFileId.Text))
            {
                MessageBox.Show("Please specify the UserId in the FileId field.");
                return;
            }
            int UserId = Convert.ToInt32(txFileId.Text);
            try
            {
                MarketingManager.MkgMgr mm = MarketingManager.MkgMgr.Instance;
                mm.UpdateUser(UserId, ref err);
                rtbMsg.AppendText(err);
            }
            catch (Exception ex)
            {
                rtbMsg.AppendText(ex.Message);
            }
        }

        private void StartCampaign()
        {
            string err = "";
            if (string.IsNullOrEmpty(txFileId.Text))
            {
                MessageBox.Show("Please specify the UserId in the FileId field.");
                return;
            }

            StartCampaignRequest req = new StartCampaignRequest();

            req.FileId = new int[1] { 515 };
            int UserId = Convert.ToInt32(txFileId.Text);
            req.hdr = new ReqHdr();

            req.hdr.UserId = UserId;
            req.CampaignId = 1;

            try
            {
                MarketingManager.MkgMgr mm = MarketingManager.MkgMgr.Instance;
                mm.StartCampaign(req, ref err);
                rtbMsg.AppendText(err);
            }
            catch (Exception ex)
            {
                rtbMsg.AppendText(ex.Message);
            }
        }

        private void RemoveCampaign()
        {
            string err = "";
            if (string.IsNullOrEmpty(txFileId.Text))
            {
                MessageBox.Show("Please specify the UserId in the FileId field.");
                return;
            }

            RemoveCampaignRequest req = new RemoveCampaignRequest();

            req.FileId = 288;
            int UserId = Convert.ToInt32(txFileId.Text);
            req.hdr = new ReqHdr();

            req.hdr.UserId = UserId;
            req.CampaignId = 1;

            try
            {
                MarketingManager.MkgMgr mm = MarketingManager.MkgMgr.Instance;
                mm.RemoveCampaign(req, ref err);
                rtbMsg.AppendText(err);
            }
            catch (Exception ex)
            {
                rtbMsg.AppendText(ex.Message);
            }
        }

        private void UpdateCreditCard()
        {
            string err = "";
            if (string.IsNullOrEmpty(txFileId.Text))
            {
                MessageBox.Show("Please specify the UserId in the FileId field.");
                return;
            }

            UpdateCreditCardRequest req = new UpdateCreditCardRequest();

            int UserId = Convert.ToInt32(txFileId.Text);
            req.hdr = new ReqHdr();

            req.hdr.UserId = UserId;

            req.Card_Exp_Month = "11";
            req.Card_Exp_Year = "2011";

            req.Card_First_Name = "john";
            req.Card_IsDefault = "true";
            req.Card_Last_Name = "doe";
            req.Card_Number = "411111111111";
            req.Card_SIC = "789";

            req.Card_Type = CreditCardType.VISA;

            try
            {
                MarketingManager.MkgMgr mm = MarketingManager.MkgMgr.Instance;
                mm.UpdateCreditCard(req, ref err);
                rtbMsg.AppendText(err);
            }
            catch (Exception ex)
            {
                rtbMsg.AppendText(ex.Message);
            }
        }

        private void UpdateLeadStarProspect()
        {
            string err = "";
            if (string.IsNullOrEmpty(txFileId.Text))
            {
                MessageBox.Show("Please specify the UserId in the FileId field.");
                return;
            }

            int FileId = Convert.ToInt32(txFileId.Text);

            try
            {
                MarketingManager.MkgMgr mm = MarketingManager.MkgMgr.Instance;
                mm.UpdateProspect(FileId, ref err);
                rtbMsg.AppendText(err);
            }
            catch (Exception ex)
            {
                rtbMsg.AppendText(ex.Message);
            }
        }

        private void LoadProspectCampaigns()
        {
            string err = "";
            if (string.IsNullOrEmpty(txFileId.Text))
            {
                MessageBox.Show("Please specify the FileId in the FileId field.");
                return;
            }
            int FileId = Convert.ToInt32(txFileId.Text);
            try
            {
                MarketingManager.MkgMgr mm = MarketingManager.MkgMgr.Instance;
                mm.LoadProspectCampaigns(FileId, ref err);
                rtbMsg.AppendText(err);
            }
            catch (Exception ex)
            {
                rtbMsg.AppendText(ex.Message);
            }
        }
        private int GetUserIdByName(string fullName, int BranchId, bool LoanOfficer)
        {
            if (string.IsNullOrEmpty(fullName))
                return -1;

            string temp = fullName.Trim();
            if (string.IsNullOrEmpty(temp))
                return -1;
            string firstname = string.Empty;
            string lastname = string.Empty;
            string mName = string.Empty;
            int firstBlank = temp.IndexOf(' ');
            int lastBlank = temp.LastIndexOf(' ');
            int firstComma = temp.IndexOf(',');
            if (firstBlank > 0)
            {
                firstname = temp.Substring(0, firstBlank);
                lastname = temp.Substring(lastBlank + 1, temp.Length - (lastBlank + 1));
            }
            else if (firstComma > 0)
            {
                lastname = temp.Substring(0, firstComma);
                if (lastBlank > 0)
                    firstname = temp.Substring(lastBlank + 1, temp.Length - (lastBlank + 1));
                else
                    firstname = temp.Substring(firstComma + 1, temp.Length - (firstComma + 1));
            }
            if (string.IsNullOrEmpty(firstname) || string.IsNullOrEmpty(lastname))
                return -1;
            DataAccess.DataAccess da = new DataAccess.DataAccess();
            string err = string.Empty;
            int UserId = -1;
            if (LoanOfficer)
                UserId = da.Find_LO(BranchId, firstname + ' ' + lastname, ref err);
            else
                UserId = da.Find_User(BranchId, firstname + ' ' + lastname, ref err);
            if (UserId <= 0 && !string.IsNullOrEmpty(err))
            {
                rtbMsg.AppendText(err);
            }
            return UserId;
        }
        private void MassReassignLoans()
        {
            int cnt = 0;
            int missing_cnt = 0;
            int RoleId = 3;  // Loan Officer
            string sqlCmd = "select v.*, l.LOS_LoanOfficer from lpvw_pipelineinfo v inner join loans l on v.FileId=l.FileId where l.LOS_LoanOfficer in ('Chris McRae', 'Tania McRae', 'Greg Perez')";
            DataSet ds = focusIT.DbHelperSQL.Query(sqlCmd);
            if (ds == null || ds.Tables[0].Rows.Count == 0)
            {
                rtbMsg.AppendText("No results found for " + sqlCmd);
                return;
            }
            List<PointFieldInfo> PFIList = new List<PointFieldInfo>();
            int BranchId = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr == null)
                    continue;
                PointFieldInfo pf = new PointFieldInfo
                {
                    fileId = Convert.ToInt32(dr["FileId"]),
                    //prevValue = dr["PrevValue"] == DBNull.Value ? string.Empty : dr["PrevValue"].ToString(),
                    //curValue = dr["CurrentValue"] == DBNull.Value ? string.Empty : dr["CurrentValue"].ToString()
                };
                PFIList.Add(pf);
                //sqlCmd = string.Format("Select BranchId from Loans where FileId={0}", pf.fileId);
                //object obj = focusIT.DbHelperSQL.GetSingle(sqlCmd);
                //if (obj != null)
                //{
                //    BranchId = obj == DBNull.Value ? -1 : (int)obj;
                //    pf.branchId = BranchId;
                //    PFIList.Add(pf);
                //}
                //else
                //{
                //    string test = "";
                //}
            }
            LP2Service.PointMgr pm = LP2Service.PointMgr.Instance;
            string err = string.Empty;
            foreach (PointFieldInfo pf in PFIList)
            {
                if (pf.fileId > 0)
                {
                    //pf.prevUser = 0;
                    //pf.prevUser = GetUserIdByName(pf.prevValue, pf.branchId, true);
                    //pf.curUser = GetUserIdByName(pf.curValue, pf.branchId, true);
                    //if (pf.prevUser <= 0)
                    //{
                    //    string bb = "";
                    //}
                    ReassignLoanRequest req = new ReassignLoanRequest();
                    req.hdr = new ReqHdr();
                    req.hdr.UserId = 1;
                    List<ReassignUserInfo> reassignList = new List<ReassignUserInfo>();
                    int i = 0;
                    ReassignUserInfo u = new ReassignUserInfo();
                    u.FileId = pf.fileId;
                    u.NewUserId = 33;
                    u.RoleId = RoleId;
                    reassignList.Add(u);
                    req.reassignUsers = reassignList;
                    cnt = cnt + 1;
                    if (pm.ReassignLoan(req, ref err) == false)
                    {
                        //err = string.Format("Can not find the loan officer name: '{0}', in the usertable, Fileid={1}", pf.prevValue, pf.fileId);
                        //err = err + "\r\n";
                        err = err + "\r\n\r\n";
                        rtbMsg.AppendText(err);

                        missing_cnt = missing_cnt + 1;
                    }
                }
            }
            err = "\r\n\r\n " + "Reassign Total Count: " + cnt.ToString() + "\r\n";
            rtbMsg.AppendText(err);
            err = "\r\n" + "Point file Missing Count: " + missing_cnt.ToString() + "\r\n";
            rtbMsg.AppendText(err);
        }
        private void MassReassignLoans_original()
        {
            int RoleId = 3;  // Loan Officer
            string sqlCmd = "Select * from LoanPointFields where PrevValue<> CurrentValue and PointFieldId=19 and ChangeTime >= '10/27/2011' order by FileId asc ";
            DataSet ds = focusIT.DbHelperSQL.Query(sqlCmd);
            if (ds == null || ds.Tables[0].Rows.Count == 0)
            {
                rtbMsg.AppendText("No results found for " + sqlCmd);
                return;
            }
            List<PointFieldInfo> PFIList = new List<PointFieldInfo>();
            int BranchId = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr == null)
                    continue;
                PointFieldInfo pf = new PointFieldInfo
                {
                    fileId = Convert.ToInt32(dr["FileId"]),
                    prevValue = dr["PrevValue"] == DBNull.Value ? string.Empty : dr["PrevValue"].ToString(),
                    curValue = dr["CurrentValue"] == DBNull.Value ? string.Empty : dr["CurrentValue"].ToString()
                };
                sqlCmd = string.Format("Select BranchId from Loans where FileId={0}", pf.fileId);
                object obj = focusIT.DbHelperSQL.GetSingle(sqlCmd);
                if (obj != null)
                {
                    BranchId = obj == DBNull.Value ? -1 : (int)obj;
                    pf.branchId = BranchId;
                    PFIList.Add(pf);
                }
                else
                {
                    string test = "";
                }
            }
            LP2Service.PointMgr pm = LP2Service.PointMgr.Instance;
            string err = string.Empty;
            foreach (PointFieldInfo pf in PFIList)
            {
                if (pf.fileId > 0)
                {
                    pf.prevUser = 0;
                    pf.prevUser = GetUserIdByName(pf.prevValue, pf.branchId, true);
                    pf.curUser = GetUserIdByName(pf.curValue, pf.branchId, true);
                    if (pf.prevUser <= 0)
                    {
                        string bb = "";
                    }
                    ReassignLoanRequest req = new ReassignLoanRequest();
                    req.hdr = new ReqHdr();
                    req.hdr.UserId = 1;
                    List<ReassignUserInfo> reassignList = new List<ReassignUserInfo>();
                    int i = 0;
                    ReassignUserInfo u = new ReassignUserInfo();
                    u.FileId = pf.fileId;
                    u.NewUserId = pf.prevUser;
                    u.RoleId = RoleId;
                    reassignList.Add(u);
                    req.reassignUsers = reassignList;
                    if (pm.ReassignLoan(req, ref err) == false)
                    {
                        err = string.Format("Can not find the loan officer name: '{0}', in the usertable, Fileid={1}", pf.prevValue, pf.fileId);
                        err = err + "\r\n\r\n";
                        rtbMsg.AppendText(err);
                    }
                }
            }
        }

        private void MassUpdateStages()
        {
            int cnt = 0;
            int missing_cnt = 0;
            int RoleId = 3;  // Loan Officer
            string sqlCmd = "select l.FileId,l.DateProcessing,ls.StageName, ls.Completed, ls.LoanStageId from Loans l left join LoanStages ls on l.FileId=ls.FileId where l.DateProcessing IS NOT NULL and ls.StageName='Sent to Processing' and ls.Completed IS NULL";
            DataSet ds = focusIT.DbHelperSQL.Query(sqlCmd);
            if (ds == null || ds.Tables[0].Rows.Count == 0)
            {
                rtbMsg.AppendText("No results found for " + sqlCmd);
                return;
            }

            string err = string.Empty;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr == null)
                    continue;
                int LoanStageId = (int)dr["LoanStageId"];
                DateTime Completed = (DateTime)dr["DateProcessing"];
                int FileId = (int)dr["FileId"];
                sqlCmd = string.Format("Update LoanStages set Completed='{0}' WHERE LoanStageId={1} and FileID={2}", Completed.ToString(), LoanStageId, FileId);
                focusIT.DbHelperSQL.ExecuteNonQuery(sqlCmd);
                cnt++;
                sqlCmd = string.Format("Update LoanTasks set Completed='{0}' WHERE LoanStageid={1} and FileId={2} and Completed IS NULL", Completed.ToString(), LoanStageId, FileId);
                focusIT.DbHelperSQL.ExecuteNonQuery(sqlCmd);
            }

            err = "\r\n\r\n " + "Complete Stages Total Count: " + cnt.ToString() + "\r\n";
            rtbMsg.AppendText(err);
        }
        private void EmailLSR()
        {
            LP2Service.ReportManager rm = LP2Service.ReportManager.Instance;
            rm.EmailLSR();
        }

        private void SubmitLoanToDataTrac()
        {
            string err = string.Empty;
            if (string.IsNullOrEmpty(txFileId.Text))
            {
                MessageBox.Show("Please specify the FileId in the FileId field.");
                return;
            }
            int FileId = Convert.ToInt32(txFileId.Text);
            try
            {
                LP2Service.PointMgr pm = LP2Service.PointMgr.Instance;
                GetPointFileInfoReq req = new GetPointFileInfoReq();
                req.hdr = new ReqHdr();
                req.FileId = FileId;
                GetPointFileInfoResp resp = pm.GetPointFileInfo(req);
                //DataTracManager.DataTracMgr dm = new DataTracManager.DataTracMgr();
                //string origType = "Branch";
                //string loanProgram = "30 years fixed";
                //dm.SubmitLoan(FileId, origType, loanProgram, ref err);
                //if (!string.IsNullOrEmpty(err))
                //    rtbMsg.AppendText(err);
            }
            catch (Exception ex)
            {
                rtbMsg.AppendText(ex.Message);
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            rtbMsg.Clear();
            if (comboCommand.SelectedIndex == -1 || comboCommand.SelectedItem == null)
            {
                MessageBox.Show("You must select a command to execute.");
                return;
            }
            string cmd = (string)comboCommand.SelectedItem;

            switch (cmd.Trim().ToLower())
            {
                case "add note": AddNotes(); break;
                case "convert to lead": ConvertToLead(); break;
                case "dispose loan": DisposeLoan(); break;
                case "dispose lead": DisposeLead(); break;
                case "import all loans": ImportAll(); break;
                case "import loans": ImportLoans(); break;
                case "move point file": MovePointFile(); break;
                case "update est close date": break;
                case "export task history": ExportTaskHistory(); break;
                case "update stage": UpdateStage(); break;
                case "reassign contact": ReassignContact(); break;
                case "reassign loan": ReassignLoan(); break;
                case "monitor all loans": MonitorAllLoans(); break;
                case "monitor a loan": MonitorLoans(); break;
                case "monitor prospect tasks": MonitorProspectTasks(); break;
                case "generate workflow": GenerateWorkflow(); break;
                case "calculate due dates": break;
                case "process loan rules": ProcessLoanRules(); break;
                case "export email log": ExportEmailLog(); break;
                case "update borrower": UpdateBorrower(); break;
                case "update contact": UpdateContact(); break;
                case "update leadstar company": UpdateLeadStarCompany(); break;
                case "update leadstar branch": UpdateLeadStarBranch(); break;
                case "update leadstar user": UpdateLeadStarUser(); break;
                case "update loan info": UpdateLoanInfo(); break;
                case "start campaign": StartCampaign(); break;
                case "remove campaign": RemoveCampaign(); break;
                case "update credit card": UpdateCreditCard(); break;
                case "update leadstar prospect": UpdateLeadStarProspect(); break;
                case "load marketing": LoadMarketing(); break;
                case "load prospect campaigns": LoadProspectCampaigns(); break;
                case "email lsr": EmailLSR(); break;
                case "submit loan to datatrac": SubmitLoanToDataTrac(); break;
                case "get lastmodifiedtime": GetPointModifiedTime(); break;
                case "mass reassign loans": MassReassignLoans(); break;
                case "mass update stages": MassUpdateStages(); break;
                case "mass generate workflow": MassGenerateWorkflow(); break;
                case "send email que": SendEmailQueue(); break;
            }
        }
    }
    public class PointFieldInfo
    {
        public int fileId;
        public int branchId;
        public string prevValue;
        public int prevUser;
        public string curValue;
        public int curUser;
    }
}
