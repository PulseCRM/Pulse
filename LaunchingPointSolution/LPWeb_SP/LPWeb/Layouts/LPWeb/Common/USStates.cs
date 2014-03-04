using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LPWeb.Layouts.LPWeb.Common
{
    class USStates
    {
        public static void Init(DropDownList StateList)
        {
            if (StateList == null)
                StateList = new DropDownList();
            StateList.Items.Clear();
            StateList.Items.Add(new ListItem("--select--", ""));
            StateList.SelectedValue = "";
            StateList.Items.Add(new ListItem("AL", "AL"));
            StateList.Items.Add(new ListItem("AK", "AK"));
            StateList.Items.Add(new ListItem("AR", "AR"));
            StateList.Items.Add(new ListItem("AZ", "AZ"));
            StateList.Items.Add(new ListItem("CA", "CA"));
            StateList.Items.Add(new ListItem("CO", "CO"));
            StateList.Items.Add(new ListItem("CT", "CT"));
            StateList.Items.Add(new ListItem("DC", "DC"));
            StateList.Items.Add(new ListItem("DE", "DE"));
            StateList.Items.Add(new ListItem("FL", "FL"));
            StateList.Items.Add(new ListItem("GA", "GA"));
            StateList.Items.Add(new ListItem("HI", "HI"));
            StateList.Items.Add(new ListItem("IA", "IA"));
            StateList.Items.Add(new ListItem("ID", "ID"));
            StateList.Items.Add(new ListItem("IL", "IL"));
            StateList.Items.Add(new ListItem("IN", "IN"));
            StateList.Items.Add(new ListItem("KS", "KS"));
            StateList.Items.Add(new ListItem("KY", "KY"));
            StateList.Items.Add(new ListItem("LA", "LA"));
            StateList.Items.Add(new ListItem("MA", "MA"));
            StateList.Items.Add(new ListItem("MD", "MD"));
            StateList.Items.Add(new ListItem("ME", "ME"));
            StateList.Items.Add(new ListItem("MI", "MI"));
            StateList.Items.Add(new ListItem("MN", "MN"));
            StateList.Items.Add(new ListItem("MO", "MO"));
            StateList.Items.Add(new ListItem("MS", "MS"));
            StateList.Items.Add(new ListItem("MT", "MT"));
            StateList.Items.Add(new ListItem("NC", "NC"));
            StateList.Items.Add(new ListItem("ND", "ND"));
            StateList.Items.Add(new ListItem("NE", "NE"));
            StateList.Items.Add(new ListItem("NH", "NH"));
            StateList.Items.Add(new ListItem("NJ", "NJ"));
            StateList.Items.Add(new ListItem("NM", "NM"));
            StateList.Items.Add(new ListItem("NV", "NV"));
            StateList.Items.Add(new ListItem("NY", "NY"));
            StateList.Items.Add(new ListItem("OH", "OH"));
            StateList.Items.Add(new ListItem("OK", "OK"));
            StateList.Items.Add(new ListItem("OR", "OR"));
            StateList.Items.Add(new ListItem("PA", "PA"));
            StateList.Items.Add(new ListItem("PR", "PR"));
            StateList.Items.Add(new ListItem("RI", "RI"));
            StateList.Items.Add(new ListItem("SC", "SC"));
            StateList.Items.Add(new ListItem("SD", "SD"));
            StateList.Items.Add(new ListItem("TN", "TN"));
            StateList.Items.Add(new ListItem("TX", "TX"));
            StateList.Items.Add(new ListItem("UT", "UT"));
            StateList.Items.Add(new ListItem("VA", "VA"));   
            StateList.Items.Add(new ListItem("VT", "VT"));                  
            StateList.Items.Add(new ListItem("WA", "WA"));
            StateList.Items.Add(new ListItem("WI", "WI"));
            StateList.Items.Add(new ListItem("WV", "WV"));
            StateList.Items.Add(new ListItem("WY", "WY"));
        
        }

        public static List<ListItem> GetStates()
        {
            List<ListItem> listItem = new List<ListItem>();
            listItem.Add(new ListItem("All", "-1"));
            listItem.Add(new ListItem("Alabama", "AL"));
            listItem.Add(new ListItem("Alaska", "AK"));
            listItem.Add(new ListItem("Arizona", "AZ"));
            listItem.Add(new ListItem("Arkansas", "AR"));
            listItem.Add(new ListItem("California", "CA"));
            listItem.Add(new ListItem("Colorado", "CO"));
            listItem.Add(new ListItem("Connecticut", "CT"));
            listItem.Add(new ListItem("Delaware", "DE"));
            listItem.Add(new ListItem("Florida", "FL"));
            listItem.Add(new ListItem("Georgia", "GA"));
            listItem.Add(new ListItem("Hawaii", "HI"));
            listItem.Add(new ListItem("Idaho", "ID"));
            listItem.Add(new ListItem("Illinois", "IL"));
            listItem.Add(new ListItem("Indiana", "IN"));
            listItem.Add(new ListItem("Iowa", "IA"));
            listItem.Add(new ListItem("Kansas", "KS"));
            listItem.Add(new ListItem("Kentucky", "KY"));
            listItem.Add(new ListItem("Louisiana", "LA"));
            listItem.Add(new ListItem("Maine", "ME"));
            listItem.Add(new ListItem("Maryland", "MD"));
            listItem.Add(new ListItem("Massachusetts", "MA"));
            listItem.Add(new ListItem("Michigan", "MI"));
            listItem.Add(new ListItem("Minnesota", "MN"));
            listItem.Add(new ListItem("Mississippi", "MS"));
            listItem.Add(new ListItem("Missouri", "MO"));
            listItem.Add(new ListItem("Montana", "MT"));
            listItem.Add(new ListItem("Nebraska", "NE"));
            listItem.Add(new ListItem("Nevada", "NV"));
            listItem.Add(new ListItem("New Hampshire", "NH"));
            listItem.Add(new ListItem("New Jersey", "NJ"));
            listItem.Add(new ListItem("New Mexico", "NM"));
            listItem.Add(new ListItem("New York", "NY"));
            listItem.Add(new ListItem("North Carolina", "NC"));
            listItem.Add(new ListItem("North Dakota", "ND"));
            listItem.Add(new ListItem("Ohio", "OH"));
            listItem.Add(new ListItem("Oklahoma", "OK"));
            listItem.Add(new ListItem("Oregon", "OR"));
            listItem.Add(new ListItem("Pennsylvania", "PA"));
            listItem.Add(new ListItem("Puerto Rico", "PR"));
            listItem.Add(new ListItem("Rhode Island", "RI"));
            listItem.Add(new ListItem("South Carolina", "SC"));
            listItem.Add(new ListItem("South Dakota", "SD"));
            listItem.Add(new ListItem("Tennessee", "TN"));
            listItem.Add(new ListItem("Texas", "TX"));
            listItem.Add(new ListItem("Utah", "UT"));
            listItem.Add(new ListItem("Vermont", "VT"));
            listItem.Add(new ListItem("Virginia", "VA"));
            listItem.Add(new ListItem("Washington", "WA"));
            listItem.Add(new ListItem("Washington DC", "DC"));
            listItem.Add(new ListItem("West Virginia", "WV"));
            listItem.Add(new ListItem("Wisconsin", "WI"));
            listItem.Add(new ListItem("Wyoming", "WY"));
            return listItem;
        }
    }
}
