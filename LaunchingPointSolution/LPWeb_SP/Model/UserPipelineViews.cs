using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPWeb.Model
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// gdc CR45 
    /// </remarks>
    public class UserPipelineViews
    {
        //[UserPipelineViewID],[UserId],[PipelineType],[ViewName],[Enabled],[ViewFilter],[OrgTypeFilter],[OrgFilter],[StageFilter],[ContactTypeFilter],[ContactFilter],[DateTypeFilter],[DateFilter]

        public int UserPipelineViewID { get; set; }

        public int UserId { get; set; }

        public string PipelineType { get; set; }

        public string  ViewName { get; set; }

        public bool  Enabled { get; set; }

        public string ViewFilter { get; set; }

        public string ViewFilterDisplay
        {
            get
            {
                switch (ViewFilter)
                {
                    case "All":
                        return "All Loans";
                    case "Active":
                        return "Active Loans";
                    case "Archived":
                        return "Archived Loans";
                    default: return ViewFilter;
                }
            }

            set 
            {
                switch (value)
                {
                    case "All Loans":
                        ViewFilter = "All";
                        break;
                    case "Active Loans":
                        ViewFilter = "Active";
                        break;
                    case "Archived Loans":
                        ViewFilter = "Archived";
                        break;
                    default: ViewFilter = value;
                        break;
                }
            
            }
        }


        public string OrgTypeFilter { get; set; }

        public string OrgTypeFilterDisplay
        {
            get
            {
                switch (OrgTypeFilter)
                {
                    case "All":
                        return "All organization types";
                    default: return OrgTypeFilter;
                }
            }

            set
            {
                switch (value)
                {
                    case "All organization types":
                        OrgTypeFilter = "All";
                        break;
                    default: OrgTypeFilter = value;
                        break;
                }

            }
        }

        public string OrgFilter { get; set; }

        public string StageFilter { get; set; }

        public string ContactTypeFilter { get; set; }

        public string ContactFilter { get; set; }

        public string DateTypeFilter { get; set; }

        public string DateTypeFilterDisplay
        {
            get
            {
                switch (DateTypeFilter)
                {
                    case "All":
                        return "All dates";
                    default: return DateTypeFilter;
                }
            }

            set
            {
                switch (value)
                {
                    case "All dates":
                        DateTypeFilter = "All";
                        break;
                    default: DateTypeFilter = value;
                        break;
                }

            }
        }


        public string DateFilter { get; set; }

        public string AdvancedLoanFilters { get; set; }


    }
}
