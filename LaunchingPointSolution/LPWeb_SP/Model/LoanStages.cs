using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPWeb.Model
{
    public class LoanStages
    {
        public LoanStages()
		{}
		#region Model
		private int _loanstageid;
		private DateTime? _completed;
		private int? _sequencenumber;
		private int _fileid;
		private int? _daysfromestclose;
		private string _stagename;
		private int? _wfltemplid;
		private int? _wflstageid;
		/// <summary>
		/// 
		/// </summary>
		public int LoanStageId
		{
			set{ _loanstageid=value;}
			get{return _loanstageid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? Completed
		{
			set{ _completed=value;}
			get{return _completed;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? SequenceNumber
		{
			set{ _sequencenumber=value;}
			get{return _sequencenumber;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int FileId
		{
			set{ _fileid=value;}
			get{return _fileid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? DaysFromEstClose
		{
			set{ _daysfromestclose=value;}
			get{return _daysfromestclose;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string StageName
		{
			set{ _stagename=value;}
			get{return _stagename;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? WflTemplId
		{
			set{ _wfltemplid=value;}
			get{return _wfltemplid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? WflStageId
		{
			set{ _wflstageid=value;}
			get{return _wflstageid;}
		}
		#endregion Model
    }
}
