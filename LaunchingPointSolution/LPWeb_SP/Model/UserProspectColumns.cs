using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPWeb.Model
{
    /// <summary>
    /// 实体类UserPipelineColumns 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class UserProspectColumns
    {
        public UserProspectColumns()
		{}

		#region Model
		private int _userid;
        private bool _pv_created;
        private bool _pv_leadsource;
        private bool _pv_refcode;
        private bool _pv_loanofficer;
        private bool _pv_branch;
        private bool _pv_progress;
        private bool _lv_ranking;
        private bool _lv_amount;
        private bool _lv_rate;
        private bool _lv_loanofficer;
        private bool _lv_lien;
        private bool _lv_progress;
        private bool _lv_branch;
        private bool _lv_loanprogram;
        private bool _lv_leadsource;
        private bool _lv_refcode;
        private bool _lv_estclose;
        private bool _lv_pointfilename;
        private bool _pv_referral;
        private bool _pv_partner;
        private bool _lv_referral;
        private bool _lv_partner;
        private bool _LastCompletedStage;
        private bool _LastStageComplDate;

        /// <summary>
        /// 
        /// </summary>
        public int UserId
        {
            set { _userid = value; }
            get { return _userid; }
        }
        public bool Pv_Created
        {
            get { return _pv_created; }
            set { _pv_created = value; }
        }
        public bool Pv_Leadsource
        {
            get { return _pv_leadsource; }
            set { _pv_leadsource = value; }
        }
        public bool Pv_Refcode
        {
            get { return _pv_refcode; }
            set { _pv_refcode = value; }
        }
        public bool Pv_Loanofficer
        {
            get { return _pv_loanofficer; }
            set { _pv_loanofficer = value; }
        }
        public bool Pv_Branch
        {
            get { return _pv_branch; }
            set { _pv_branch = value; }
        }
        public bool Pv_Progress
        {
            get { return _pv_progress; }
            set { _pv_progress = value; }
        }
        public bool Lv_Ranking
        {
            get { return _lv_ranking; }
            set { _lv_ranking = value; }
        }
        public bool Lv_Amount
        {
            get { return _lv_amount; }
            set { _lv_amount = value; }
        }
        public bool Lv_Rate
        {
            get { return _lv_rate; }
            set { _lv_rate = value; }
        }
        public bool Lv_Loanofficer
        {
            get { return _lv_loanofficer; }
            set { _lv_loanofficer = value; }
        }
        public bool Lv_Lien
        {
            get { return _lv_lien; }
            set { _lv_lien = value; }
        }
        public bool Lv_Progress
        {
            get { return _lv_progress; }
            set { _lv_progress = value; }
        }
        public bool Lv_Branch
        {
            get { return _lv_branch; }
            set { _lv_branch = value; }
        }
        public bool Lv_Loanprogram
        {
            get { return _lv_loanprogram; }
            set { _lv_loanprogram = value; }
        }
        public bool Lv_Leadsource
        {
            get { return _lv_leadsource; }
            set { _lv_leadsource = value; }
        }
        public bool Lv_Refcode
        {
            get { return _lv_refcode; }
            set { _lv_refcode = value; }
        }
        public bool Lv_Estclose
        {
            get { return _lv_estclose; }
            set { _lv_estclose = value; }
        }
        public bool Lv_Pointfilename
        {
            get { return _lv_pointfilename; }
            set { _lv_pointfilename = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool Pv_Referral
        {
            set { _pv_referral = value; }
            get { return _pv_referral; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool Pv_Partner
        {
            set { _pv_partner = value; }
            get { return _pv_partner; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool Lv_Referral
        {
            set { _lv_referral = value; }
            get { return _lv_referral; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool Lv_Partner
        {
            set { _lv_partner = value; }
            get { return _lv_partner; }
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
        #endregion Model
    }
}
