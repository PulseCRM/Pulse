using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPWeb.Model
{
    public class UserLicenses
    {
        private int _userLicenseId;
        private int _userid;
        private string _licenseNumber;
        
        /// <summary>
        /// 
        /// </summary>
        public int UserId
        {
            set { _userid = value; }
            get { return _userid; }
        }
        

        /// <summary>
        /// 
        /// </summary>
        public int UserLicenseId
        {
            set { _userLicenseId = value; }
            get { return _userLicenseId; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string LicenseNumber
        {
            set { _licenseNumber = value; }
            get { return _licenseNumber; }
        }


    }
}
