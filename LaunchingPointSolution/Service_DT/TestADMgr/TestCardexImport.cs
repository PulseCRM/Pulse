using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LP2.Service;
using focusIT;

namespace TestADMgr
{
    public partial class TestCardexImport : Form
    {
        CardexHelper cardex = null;
        DataAccess.DataAccess da = null;
        public TestCardexImport()
        {
            InitializeComponent();
            rtbMsg.Clear();
            try
            {
                cardex = new CardexHelper();
            }
            catch (Exception ex)
            {
                rtbMsg.AppendText(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            rtbMsg.Clear();
            int userId = 0;
            int reqId = 0;
            string path = @"C:\PNTTEMPL\database\POINTCDX.MDB";
            string err = "";
            try
            {               
                if (cardex.ImportCardex( userId, reqId, path, ref err) == false)
                {
                    rtbMsg.AppendText(err);
                    return;
                }
                string sqlCmd = "Select c.FirstName as FirstName, c.LastName as LastName, cc.Name as Company, c.Title as Title, cc.ServiceTypes as  ServiceTypes from Contacts c ";
                sqlCmd += " inner join ContactCompanies cc on c.ContactCompanyId=cc.ContactCompanyId";
                DataSet ds = DbHelperSQL.Query(sqlCmd);
                string temp = "";

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    temp = dr["FirstName"].ToString().Trim();
                    rtbMsg.AppendText("First Name = " + temp+" ");
                    temp = dr["LastName"].ToString().Trim();
                    rtbMsg.AppendText("Last Name = " + temp+" ");
                    temp = dr["Company"].ToString().Trim();
                    rtbMsg.AppendText("Company="+temp+" ");
                    temp = dr["Title"].ToString().Trim();
                    rtbMsg.AppendText("TItle=" + temp+" ");
                    temp = dr["ServiceTypes"].ToString().Trim();
                    rtbMsg.AppendText("ServiceTypes =" + temp+"\r\n");
                }
            }
            catch (Exception ex)
            {
                rtbMsg.AppendText(ex.Message);
            }
        }
    }
}
