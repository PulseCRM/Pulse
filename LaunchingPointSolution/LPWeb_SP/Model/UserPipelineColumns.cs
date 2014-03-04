using System;
namespace LPWeb.Model
{
	/// <summary>
	/// 实体类UserPipelineColumns 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class UserPipelineColumns
	{
		public UserPipelineColumns()
		{}
		#region Model
		private int _userid;
		private bool _pointfolder;
		private bool _stage;
		private bool _branch;
		private bool _estimatedclose;
		private bool _alerts;
		private bool _loanofficer;
		private bool _amount;
		private bool _lien;
		private bool _rate;
		private bool _lender;
		private bool _lockexp;
		private bool _percentcompl;
		private bool _processor;
		private bool _taskcount;
		private bool _pointfilename;

        private bool _LastCompletedStage;
        private bool _LastStageComplDate;

        //gdc CR40
        private bool _closer;
        private bool _shipper;
        private bool _docprep;
        private bool _assistant;

        private bool _LoanProgram;

        // CR49
        private bool _Purpose;

        private bool _QuickLeadForm;

        //gdc CR51
        private bool _JrProcessor;

        private bool _LastLoanNote;

		/// <summary>
		/// 
		/// </summary>
		public int UserId
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool PointFolder
		{
			set{ _pointfolder=value;}
			get{return _pointfolder;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool Stage
		{
			set{ _stage=value;}
			get{return _stage;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool Branch
		{
			set{ _branch=value;}
			get{return _branch;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool EstimatedClose
		{
			set{ _estimatedclose=value;}
			get{return _estimatedclose;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool Alerts
		{
			set{ _alerts=value;}
			get{return _alerts;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool LoanOfficer
		{
			set{ _loanofficer=value;}
			get{return _loanofficer;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool Amount
		{
			set{ _amount=value;}
			get{return _amount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool Lien
		{
			set{ _lien=value;}
			get{return _lien;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool Rate
		{
			set{ _rate=value;}
			get{return _rate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool Lender
		{
			set{ _lender=value;}
			get{return _lender;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool LockExp
		{
			set{ _lockexp=value;}
			get{return _lockexp;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool PercentCompl
		{
			set{ _percentcompl=value;}
			get{return _percentcompl;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool Processor
		{
			set{ _processor=value;}
			get{return _processor;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool TaskCount
		{
			set{ _taskcount=value;}
			get{return _taskcount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool PointFileName
		{
			set{ _pointfilename=value;}
			get{return _pointfilename;}
		}
        public bool LastCompletedStage
        {
            set { _LastCompletedStage = value; }
            get { return _LastCompletedStage; }
        }
        public bool LastStageComplDate
        {
            set { _LastStageComplDate = value; }
            get { return _LastStageComplDate; }
        }


        //gdc CR40

        public bool Closer
        {
            set { _closer = value; }
            get { return _closer; }
        }

        public bool Shipper
        {
            set { _shipper = value; }
            get { return _shipper; }
        }

        public bool DocPrep
        {
            set { _docprep = value; }
            get { return _docprep; }
        }

        public bool Assistant
        {
            set { _assistant = value; }
            get { return _assistant; }
        }

        ///CR47
        public bool LoanProgram
        {
            set { _LoanProgram = value; }
            get { return _LoanProgram; }
        }

        ///CR49
        public bool Purpose
        {
            set { _Purpose = value; }
            get { return _Purpose; }
        }

        public bool QuickLeadForm
        {
            set { _QuickLeadForm = value; }
            get { return _QuickLeadForm; }
        }

        //gdc CR51
        public bool JrProcessor
        {
            set { _JrProcessor = value; }
            get { return _JrProcessor; }
        }


        public bool LastLoanNote
        {
            set { _LastLoanNote = value; }
            get { return _LastLoanNote; }
        }
		#endregion Model

	}
}

