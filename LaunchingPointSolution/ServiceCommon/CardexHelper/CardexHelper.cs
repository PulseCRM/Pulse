using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using LP2.Service.Common;

namespace LP2.Service
{
    public class CardexHelper
    {
        string m_AccessConString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=";
        string m_CardexFile = "";
        DataAccess.DataAccess m_DA = null;
        OleDbConnection m_OleConn = null;
        public CardexHelper()
        {
            string err = "";
            m_DA = new DataAccess.DataAccess();
            try
            {
                //m_CardexFile = m_DA.GetCardexFilePath(ref err);
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool CreateConnection(ref string err)
        {
            err = "";
            try
            {
                if (m_CardexFile == String.Empty)
                {
                    if (err != String.Empty)
                        throw new Exception(err);
                    err = "Cardex File is not configured in the Company Setup -->Point tab.";
                    throw new Exception(err);
                }
                if (!m_CardexFile.Trim().ToLower().EndsWith(".mdb"))
                {
                    err = "Cardex file is not valid; it must end with .MDB, Cardex File Path=" + m_CardexFile;
                    throw new Exception(err);
                }

                if (m_OleConn == null)
                    m_OleConn = new OleDbConnection(m_AccessConString + m_CardexFile);
                m_OleConn.Open();
            }
            catch (Exception ex)
            {
                err = "Failed to connect to Access Database, Connection String=" + m_AccessConString + m_CardexFile + ", Exception: " + ex.Message;
                throw new Exception(err);
            }
            return true;
        }

        public void Close()
        {
            m_DA = null;
            if (m_OleConn != null)
                m_OleConn.Close();
        }

        private bool ImportToDB(int userId, ref int reqId, DataSet ds, ref string err)
        {
            err = "";
            int total = ds.Tables[0].Rows.Count;
            int processed = 0;
            int failed = 0; 
            try 
            {
                if (ds == null)
                {
                    err = "CardexHelper::ImportToDB, Dataset is null.";
                    m_DA.UpdateRequestQueue(userId, "ImportCardex", ref reqId, false, err, ref err); return false;
                }
                    
                if (m_DA == null)
                   m_DA = new DataAccess.DataAccess();

                Address adr = new Address();
                string name = "";
                string tempFirst = "";
                string tempLast = "";
                string company = "";
                string category = "";
                string website = "";
                string title = "";
                int compId = 0;
                bool status = false;
                ContactInfo contact = new ContactInfo();
 
                m_DA.UpdateReqProgress(userId, "ImportCardex", ref reqId, total, processed, failed, ref err);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    processed++;
                    tempFirst = "";
                    tempLast = "";
                    company = "";
                    contact.BusinessPhone = "";
                    contact.Fax = "";
                    contact.Email = "";
                    compId = 0;
                    company = dr["COMPANY"].ToString();
                    adr.Street = dr["ADDRESS"].ToString().Trim();
                    adr.City = dr["CITY"].ToString().Trim();
                    adr.State = dr["STATE"].ToString().Trim();
                    adr.Zip = dr["ZIP"].ToString().Trim();
                    category = dr["CATEGORY"].ToString().Trim();
                    website = dr["WEBSITE"].ToString().Trim();
                    if (company != string.Empty)
                    {
                        status = m_DA.Save_CardexCompany(company, adr, website, category, ref compId, ref err);
                    }
                    name = dr["NAME"].ToString().Trim();
                    int index = name.IndexOf(' ');
                    if (index <= 0)
                    {
                        err = "Incorrect Contact Name specified, name="+name;
                        failed++;
                        m_DA.UpdateReqProgress(userId, "ImportCardex", ref reqId, total, processed, failed, ref err); 
                        continue;
                    }
                    tempFirst = name.Substring(0, index);
                    tempLast = name.Substring(index, name.Length-index);
                    if ((tempFirst == string.Empty) || (tempLast == string.Empty))
                    {
                        failed++;
                        m_DA.UpdateReqProgress(userId, "ImportCardex", ref reqId, total, processed, failed, ref err); 
                        continue;
                    }

                    title = dr["TITLE"].ToString().Trim();
                    contact.Email = dr["EMAIL"].ToString().Trim();
                    contact.BusinessPhone = dr["PHONE"].ToString().Trim();
                    contact.Fax = dr["FAX"].ToString().Trim();
                    contact.CellPhone = "";
                    if (m_DA.Save_CardexContact(tempFirst, tempLast, title, adr, contact, compId, ref err) == false)
                    {
                        failed++;
                        m_DA.UpdateReqProgress(userId, "ImportCardex", ref reqId, total, processed, failed, ref err); 
                        continue;
                    }
                    m_DA.UpdateReqProgress(userId, "ImportCardex", ref reqId, total, processed, failed, ref err); 
                }
            }
            catch (Exception ex)
            {
                err = ex.Message;
                throw ex;
            }
            return true;
        }

        public bool ImportCardex(int userId, int reqId, string cardexFile,  ref string err)
        {
            err = "";
            if ((cardexFile == null) || (cardexFile == String.Empty))
            {
                err = "Cardex Filename is empty.";
                return false;
            }

            string sqlCmd = "Select * From Cardex ";
            try
            {
                if (m_OleConn == null)
                    CreateConnection(ref err);
                OleDbCommand Command = m_OleConn.CreateCommand();
                Command.CommandText = sqlCmd;
                OleDbDataAdapter Adapter = new OleDbDataAdapter(Command);
                DataSet ds = new DataSet();
                Adapter.Fill(ds);

                if (ds.Tables[0].Rows.Count <= 0)
                {
                    err = "The Cardex file, "+m_CardexFile+" is empty.";
                    m_DA.UpdateRequestQueue(userId, "ImportCardex", ref reqId, false, err, ref err); 
                    return false;
                }

                if (ImportToDB(userId, ref reqId, ds, ref err) == false)
                {
                    m_DA.UpdateRequestQueue(userId, "ImportCardex", ref reqId, false, err, ref err); 
                    return false;
                }
            }
            catch (Exception ex)
            {
                err = "Failed to read the Cardex file,"+m_CardexFile+"; Exception:"+ex.Message;
                m_DA.UpdateRequestQueue(userId, "ImportCardex", ref reqId, false, err, ref err);
                throw new Exception(err);
            }
            err = "";
            m_DA.UpdateRequestQueue(userId, "ImportCardex", ref reqId, true, err, ref err); 
            return true;
        }

    }
}
