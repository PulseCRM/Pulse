using System;
using System.Collections;
using System.Configuration;
using System.Reflection;
using System.Text;
using PulseLeads.Zillow.Controllers;
using PulseLeads.Zillow.Models;

namespace PulseLeads.Zillow._Code
{
    internal class Helpers
    {
        internal static string FormatSqlStringValue(string raw_value, int max_len)
        {
            if (string.IsNullOrEmpty(raw_value)) return string.Empty;
            raw_value = raw_value.Trim();
            if (raw_value.Length < max_len && max_len > 0) return raw_value;
            return (max_len > 0) ? raw_value.Substring(0, max_len) : raw_value;
        }

        internal static string ConvertStateNameToAbbrev(string us_state_name)
        {
            string name = us_state_name.Trim().ToUpper();

            Hashtable ht = new Hashtable();
            ht.Add("ALABAMA", "AL");
            ht.Add("ALASKA", "AK");
            ht.Add("ARIZONA", "AZ");
            ht.Add("ARKANSAS", "AR");
            ht.Add("CALIFORNIA", "CA");
            ht.Add("COLORADO", "CO");
            ht.Add("CONNECTICUT", "CT");
            ht.Add("DELAWARE", "DE");
            ht.Add("FLORIDA", "FL");
            ht.Add("GEORGIA", "GA");
            ht.Add("HAWAII", "HI");
            ht.Add("IDAHO", "ID");
            ht.Add("ILLINOIS", "IL");
            ht.Add("INDIANA", "IN");
            ht.Add("IOWA", "IA");
            ht.Add("KANSAS", "KS");
            ht.Add("KENTUCKY", "KY");
            ht.Add("LOUISIANA", "LA");
            ht.Add("MAINE", "ME");
            ht.Add("MARYLAND", "MD");
            ht.Add("MASSACHUSETTS", "MA");
            ht.Add("MICHIGAN", "MI");
            ht.Add("MINNESOTA", "MN");
            ht.Add("MISSISSIPPI", "MS");
            ht.Add("MISSOURI", "MO");
            ht.Add("MONTANA", "MT");
            ht.Add("NEBRASKA", "NE");
            ht.Add("NEVADA", "NV");
            ht.Add("NEW HAMPSHIRE", "NH");
            ht.Add("NEW JERSEY", "NJ");
            ht.Add("NEW MEXICO", "NM");
            ht.Add("NEW YORK", "NY");
            ht.Add("NORTH CAROLINA", "NC");
            ht.Add("NORTH DAKOTA", "ND");
            ht.Add("OHIO", "OH");
            ht.Add("OKLAHOMA", "OK");
            ht.Add("OREGON", "OR");
            ht.Add("PENNSYLVANIA", "PA");
            ht.Add("RHODE ISLAND", "RI");
            ht.Add("SOUTH CAROLINA", "SC");
            ht.Add("SOUTH DAKOTA", "SD");
            ht.Add("TENNESSEE", "TN");
            ht.Add("TEXAS", "TX");
            ht.Add("UTAH", "UT");
            ht.Add("VERMONT", "VT");
            ht.Add("VIRGINIA", "VA");
            ht.Add("WASHINGTON", "WA");
            ht.Add("WEST VIRGINIA", "WV");
            ht.Add("WISCONSIN", "WI");
            ht.Add("WYOMING", "WY");
            ht.Add("DISTRICT OF COLUMBIA", "DC");

            return ht.ContainsKey(name)
                       ? ht[name].ToString()
                       : ((name.Length == 2) ? name : string.Empty);
        }

        internal static string ReadSecurityToken()
        {
            return ConfigurationManager.AppSettings["SecurityToken"].ToString();
        }

        internal static string FriendlyHtml(ZillowAttributeCollection zcol)
        {
            // Get Pulse Lead Request object from ZillowAttributeCollection
            PostLeadRequest pulse_lead_req = PulseLeadClient.GetPulseLeadRequest(zcol);

            // display results
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(@"<div class='col1'>Zillow Xml Attributes<br>");
            sb.AppendLine("<table class='imagetable'>");
            sb.AppendLine("<tr><th>GroupName</th><th>AttributeName</th><th>AttributeValue</th></tr>");
            foreach (ZillowAttribute attrib in zcol.ZillowAttributes)
                sb.AppendLine("<tr><td>" + attrib.GroupName + "</td>" + "<td>" + attrib.AttributeName + "</td>" + "<td>" + attrib.AttributeValue + "</td></tr>");
            sb.AppendLine("</table></div>");

            sb.AppendLine(@"<div class='col2'>Pulse Lead Request<br>");
            sb.AppendLine("<table class='imagetable'>");
            sb.AppendLine("<tr><th>Name</th><th>Value</th></tr>");
            foreach (FieldInfo f in pulse_lead_req.GetType().GetFields())
            {
                if (f.Name.Equals("Employment", StringComparison.OrdinalIgnoreCase) && pulse_lead_req.Employment != null)
                {
                    continue;
                }
                if (f.Name.Equals("OtherIncome", StringComparison.OrdinalIgnoreCase) && pulse_lead_req.OtherIncome != null)
                {
                    continue;
                }
                if (f.Name.Equals("LiquidAssets", StringComparison.OrdinalIgnoreCase) && pulse_lead_req.LiquidAssets != null)
                {
                    continue;
                }
                if (f.Name.Equals("RequestHeader", StringComparison.OrdinalIgnoreCase) && pulse_lead_req.RequestHeader != null)
                {
                    continue;
                }

                sb.AppendLine("<tr><td>" + f.Name + "</td>" + "<td>" + f.GetValue(pulse_lead_req) + "</td></tr>");
            }

            foreach (FieldInfo f in pulse_lead_req.GetType().GetFields())
            {
                if (f.Name.Equals("RequestHeader", StringComparison.OrdinalIgnoreCase) && pulse_lead_req.RequestHeader != null)
                {
                    sb.AppendLine("<tr><th colspan=2><strong>Request Header</strong></th></tr>");
                    foreach (FieldInfo f1 in pulse_lead_req.RequestHeader.GetType().GetFields())
                        sb.AppendLine("<tr><td>" + f1.Name + "</td>" + "<td>" + f1.GetValue(pulse_lead_req.RequestHeader) + "</td></tr>");
                }
                else if (f.Name.Equals("Employment", StringComparison.OrdinalIgnoreCase) && pulse_lead_req.Employment != null)
                {
                    sb.AppendLine("<tr><th colspan=2><strong>Employment</strong></th></tr>");
                    foreach (Employment employment in pulse_lead_req.Employment)
                    {
                        foreach (FieldInfo f1 in employment.GetType().GetFields())
                            sb.AppendLine("<tr><td>" + f1.Name + "</td>" + "<td>" + f1.GetValue(employment) + "</td></tr>");
                    }
                }
                else if (f.Name.Equals("OtherIncome", StringComparison.OrdinalIgnoreCase) && pulse_lead_req.OtherIncome != null)
                {
                    sb.AppendLine("<tr><th colspan=2><strong>OtherIncome</strong></th></tr>");
                    foreach (OtherIncome other_income in pulse_lead_req.OtherIncome)
                    {
                        foreach (FieldInfo f1 in other_income.GetType().GetFields())
                            sb.AppendLine("<tr><td>" + f1.Name + "</td>" + "<td>" + f1.GetValue(other_income) + "</td></tr>");
                    }
                }
                else if (f.Name.Equals("LiquidAssets", StringComparison.OrdinalIgnoreCase) && pulse_lead_req.LiquidAssets != null)
                {
                    sb.AppendLine("<tr><th colspan=2><strong>LiquidAssets</strong></th></tr>");
                    foreach (LiquidAssets liquid_assets in pulse_lead_req.LiquidAssets)
                    {
                        foreach (FieldInfo f1 in liquid_assets.GetType().GetFields())
                            sb.AppendLine("<tr><td>" + f1.Name + "</td>" + "<td>" + f1.GetValue(liquid_assets) + "</td></tr>");
                    }
                }
            }
            sb.AppendLine("</table></div>");

            return sb.ToString();
        }

    }
}