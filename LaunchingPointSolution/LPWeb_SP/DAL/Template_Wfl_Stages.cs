using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类Template_Wfl_Stages。
	/// </summary>
    public class Template_Wfl_Stages : Template_Wfl_StagesBase
	{
		public Template_Wfl_Stages()
		{}

        public int GetMinStageSeqNumByWflTempID(int iWflTempID)
        {
            int iRe = 0;

            string sSQL = "SELECT MIN(SequenceNumber) FROM Template_Wfl_Stages WHERE WflTemplId = " + iWflTempID.ToString();

            try
            {
                object objRe = DbHelperSQL.GetSingle(sSQL);
                if (objRe != null)
                {
                    iRe = Convert.ToInt32(objRe);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return iRe;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iWflTempID"></param>
        /// <returns></returns>
        public int GetSecStageSeqNumByWflTempID(int iWflTempID)
        {
            int iRe = 0;

            string sSQL = "SELECT TOP 1 SequenceNumber FROM Template_Wfl_Stages WHERE  WflTemplId = " + iWflTempID.ToString() + " AND SequenceNumber > (SELECT MIN(SequenceNumber) FROM Template_Wfl_Stages WHERE WflTemplId = " + iWflTempID.ToString() + ") ORDER BY SequenceNumber";

            try
            {
                object objRe = DbHelperSQL.GetSingle(sSQL);
                if (objRe != null)
                {
                    iRe = Convert.ToInt32(objRe);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return iRe;
        }
	}
}

