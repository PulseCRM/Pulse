using System;
namespace LPWeb.Model
{
	/// <summary>
	/// 实体类Company_Point 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Company_Point
	{
		public Company_Point()
		{}
		#region Model
		private string _winpointinipath;
		private string _pointfieldidmappingfile;
		private string _cardexfile;
		private int? _pointimportintervalminutes;
        private string _mastersource;
        private string _tractoolslogin;
        private string _tractoolspwd;
        private bool? _auto_convertlead;
        private bool? _autoapplyprocessingworkflow;
        private bool? _autoapplyprospectworkflow;
        private bool _Enable_MultiBranchFolders;

		/// <summary>
		/// 
		/// </summary>
		public string WinpointIniPath
		{
			set{ _winpointinipath=value;}
			get{return _winpointinipath;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string PointFieldIDMappingFile
		{
			set{ _pointfieldidmappingfile=value;}
			get{return _pointfieldidmappingfile;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CardexFile
		{
			set{ _cardexfile=value;}
			get{return _cardexfile;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? PointImportIntervalMinutes
		{
			set{ _pointimportintervalminutes=value;}
			get{return _pointimportintervalminutes;}
		}

        /// <summary>
        /// 
        /// </summary>
        public string MasterSource
        {
            get { return _mastersource; }
            set { _mastersource = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TracToolsPwd
        {
            get { return _tractoolspwd; }
            set { _tractoolspwd = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TracToolsLogin
        {
            get { return _tractoolslogin; }
            set { _tractoolslogin = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool? Auto_ConvertLead
        {
            get { return _auto_convertlead; }
            set { _auto_convertlead = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool? AutoApplyProcessingWorkflow
        {
            get { return _autoapplyprocessingworkflow; }
            set { _autoapplyprocessingworkflow = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool? AutoApplyProspectWorkflow
        {
            get { return _autoapplyprospectworkflow; }
            set { _autoapplyprospectworkflow = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Enable_MultiBranchFolders
        {
            get { return _Enable_MultiBranchFolders; }
            set { _Enable_MultiBranchFolders = value; }
        }

		#endregion Model

	}
}

