using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LP2.Service.Common;

namespace LP2Service
{
    public class WorkflowEvent : EventArgs
    {
        WorkflowCmd m_CmdType;
        object m_ReqMsg;

        public WorkflowEvent(WorkflowCmd cmd, object msg)
        {
            m_CmdType = cmd;
            m_ReqMsg = msg;
        }
        public WorkflowCmd RequestType
        {
            get { return m_CmdType; }
            set { m_CmdType = value; }
        }

        public object RequestMsg
        {
            get { return m_ReqMsg; }
            set { m_ReqMsg = value; }
        }

    }

    public interface IWorkflowManager
    {
        bool GenerateWorkflow(GenerateWorkflowRequest req, ref string err);
        bool CalculateDueDates(CalculateDueDatesRequest req, ref string err);
        bool MonitorLoans();
    }
}
