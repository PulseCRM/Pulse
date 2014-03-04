using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LP2.Service.Common;

namespace LP2Service
{
    #region Point Manager Command Types Enum 
  
    public abstract class MyParam
    {
    }

    #endregion
    #region Point Manager Event
    public class PointMgrEvent : EventArgs
    {
        PointMgrCommandType m_reqType;
        int m_fileId;
        int m_reqId;
        object m_obj;

        public PointMgrEvent()
        {
            m_reqType = PointMgrCommandType.Unknown;
        }
        
        public PointMgrEvent(int reqId, PointMgrCommandType reqType, int fileId, object req)
        {
            m_reqType = reqType;
            m_fileId = fileId;
            m_reqId = reqId;
            m_obj = req;
        }
        
        public PointMgrEvent (int reqId, PointMgrCommandType reqType, object req)
        {
            m_reqType = reqType;
            m_reqId = reqId;
            m_obj = req;
        }

        public PointMgrCommandType RequestType
        {
            get { return m_reqType; }
            set { m_reqType = value; }
        }
        public object Request
        {
            get { return m_obj; }
            set { m_obj = value; }
        }
        public int FileId
        {
            get { return m_fileId; }
            set { m_fileId = value; }
        }
        public int ReqId
        {
            get { return m_reqId; }
            set { m_reqId = value; }
        }
    }
    #endregion
    #region IPointMgr
    public interface IPointMgr
    {
        bool ProcessRequest(PointMgrEvent e, ref string err);
        bool ImportAllLoans(PointMgrEvent e, bool force);
     }
    #endregion

}
