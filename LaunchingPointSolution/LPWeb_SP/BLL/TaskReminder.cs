using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPWeb.BLL
{
    [Serializable]
    public class TaskReminder
    {
        public int LoanTaskId;
        public int FileId;
        public DateTime Due;
        public String TaskName;
        public String Borrower;
    }
}
