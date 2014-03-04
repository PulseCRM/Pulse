using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices;
using System.Collections;

namespace LP2.Service.ADHelper
{
public class ADUserDetail
{
    #region Properties
        private String _firstName;
        private String _middleName;
        private String _lastName;
        private String _loginName;
        private String _loginNameWithDomain;
        private String _streetAddress;
        private String _city;
        private String _state;
        private String _postalCode;
        private String _country;
        private String _homePhone;
        private String _extension;
        private String _mobile;
        private String _fax;
        private String _emailAddress;
        private String _title;
        private String _company;
        private String _manager;
        private String _managerName;
        private String _department;
        private String _password;
        private bool _disabled;
    #endregion
    #region Accessor Methods
            public String Department
        {
            get { return _department; }
        }

        public String FirstName
        {
            get { return _firstName; }
        }

        public String MiddleName
        {
            get { return _middleName; }
        }

        public String LastName
        {
            get { return _lastName; }
        }
 
        public String LoginName
        {
            get { return _loginName; }
        }
 
        public String LoginNameWithDomain
        {
            get { return _loginNameWithDomain; }
        }
 
        public String StreetAddress
        {
            get { return _streetAddress; }
        }
 
        public String City
        {
            get { return _city; }
        }
 
        public String State
        {
            get { return _state; }
        }
 
        public String PostalCode
        {
            get { return _postalCode; }
        }
 
        public String Country
        {
            get { return _country; }
        }
 
        public String HomePhone
        {
            get { return _homePhone; }
        }

        public String Extension
        {
            get { return _extension; }
        }

        public String Mobile
        {
            get { return _mobile; }
        }
 
        public String Fax
        {
            get { return _fax; }
        }
 
        public String EmailAddress
        {
            get { return _emailAddress; }
        }
 
        public String Title
        {
            get { return _title; }
        }

        public String Company
        {
            get { return _company; }
        }

        public String Password
        {
            get { return _password; }
        }
        public ADUserDetail Manager
        {
            get
            {
                if (!String.IsNullOrEmpty(_managerName))
                {
                    ActiveDirectoryHelper ad = new ActiveDirectoryHelper();
                    return ad.GetUserByFullName(_managerName);
                }
                return null;
            }
        }
 
        public String ManagerName
        {
            get { return _managerName; }
        }
        public ADUserDetail(string userName)
        {
            _loginName = userName;
        }
        public bool AccountDisabled
        {
            get { return _disabled; }
            set { _disabled = value; }
        }
        
        public ADUserDetail(string userName, bool disabled, string first, string last, string email, string password)
        {
            _firstName = first;
            _lastName = last;
            _loginName = userName;
            _emailAddress = email;
            _password = password;
            _disabled = disabled;
        }

        public ADUserDetail(string userName, bool disabled, string first, string last, string email)
        {
            _firstName = first;
            _lastName = last;
            _loginName = userName;
            _emailAddress = email;
            _disabled = disabled;
        }

        public ADUserDetail(string userName, bool disabled)
        {
            _loginName = userName;
            _disabled = disabled;
        }

        private ADUserDetail(DirectoryEntry directoryUser)
        {
            //String domainAddress;
            //String domainName;
            _firstName = GetProperty(directoryUser, ADProperties.FIRSTNAME);
            _middleName = GetProperty(directoryUser, ADProperties.MIDDLENAME);
            _lastName = GetProperty(directoryUser, ADProperties.LASTNAME);
            _loginName = GetProperty(directoryUser, ADProperties.LOGINNAME);
            _emailAddress = GetProperty(directoryUser, ADProperties.EMAILADDRESS);
            string uac = GetProperty(directoryUser, ADProperties.USERACCOUNTCONTROL);
            Int64 i_uac = 0;
            try
            {
                i_uac = Convert.ToInt64(uac);
            }
            catch (Exception ex)
            {

            }
            byte[] myByteArray = new byte[64];
            myByteArray = BitConverter.GetBytes(i_uac);
            BitArray myBits = new BitArray(myByteArray);
            if (myBits[1])
                _disabled = true;
            else
                _disabled = false;
            //String userPrincipalName = GetProperty(directoryUser, ADProperties.USERPRINCIPALNAME);
            //if (!string.IsNullOrEmpty(userPrincipalName))
            //{
            //     domainAddress = userPrincipalName.Split('@')[1];
            //}
            //else
            //{
            //    domainAddress = String.Empty;
            //}

            //if (!string.IsNullOrEmpty(domainAddress))
            //{
            //    domainName = domainAddress.Split('.').First();
            //}
            //else
            //{
            //    domainName = String.Empty;
            //}
            //_loginNameWithDomain = String.Format(@"{0}\{1}", domainName, _loginName);
            //_streetAddress = GetProperty(directoryUser, ADProperties.STREETADDRESS);
            //_city = GetProperty(directoryUser, ADProperties.CITY);
            //_state = GetProperty(directoryUser, ADProperties.STATE);
            //_postalCode = GetProperty(directoryUser, ADProperties.POSTALCODE);
            //_country = GetProperty(directoryUser, ADProperties.COUNTRY);
            //_company = GetProperty(directoryUser, ADProperties.COMPANY);
            //_department = GetProperty(directoryUser, ADProperties.DEPARTMENT);
            //_homePhone = GetProperty(directoryUser, ADProperties.HOMEPHONE);
            //_extension = GetProperty(directoryUser, ADProperties.EXTENSION);
            //_mobile = GetProperty(directoryUser, ADProperties.MOBILE);
            //_fax = GetProperty(directoryUser, ADProperties.FAX);        
            //_title = GetProperty(directoryUser, ADProperties.TITLE);
            //_manager = GetProperty(directoryUser, ADProperties.MANAGER);
            //if (!String.IsNullOrEmpty(_manager))
            //{
            //    String[] managerArray = _manager.Split(',');
            //    _managerName = managerArray[0].Replace("CN=", "");
            //}
        }
 
        private static String GetProperty(DirectoryEntry userDetail, String propertyName)
        {
            if (userDetail.Properties.Contains(propertyName))
            {
                return userDetail.Properties[propertyName][0].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        public static ADUserDetail GetUser(DirectoryEntry directoryUser)
        {
            return new ADUserDetail(directoryUser);
        }

 
    #endregion
    }
}
