using System;
namespace LPWeb.Model
{
	/// <summary>
	/// ArchiveLeadStatus:ʵ����(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class CompanyTaskPick
	{
        public CompanyTaskPick()
		{}
		#region Model
        private int _taskNameID;
        private string _taskName;
		private bool _enabled;
        private int _sequenceNumber;
		/// <summary>
		/// 
		/// </summary>
        public int TaskNameID
		{
            set { _taskNameID = value; }
            get { return _taskNameID; }
		}
		/// <summary>
		/// 
		/// </summary>
        public string TaskName
		{
            set { _taskName = value; }
            get { return _taskName; }
		}
		/// <summary>
		/// 
		/// </summary>
		public bool Enabled
		{
			set{ _enabled=value;}
			get{return _enabled;}
		}

        public int SequenceNumber
        {
            set { _sequenceNumber = value; }
            get { return _sequenceNumber; }
        }
		#endregion Model

	}
}

