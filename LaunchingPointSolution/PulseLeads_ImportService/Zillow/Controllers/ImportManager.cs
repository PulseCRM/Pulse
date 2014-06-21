using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using PulseLeads.Zillow.Models;

namespace PulseLeads.Zillow.Controllers
{
    internal class ImportManager
    {
        /// <summary>
        /// Parses the Zillow XML into the ZillowAttributeCollection
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        internal static ZillowAttributeCollection ImportLeadFromZillowv5(DataSet ds)
        {
            try
            {
                // Initialize objects and import N/V pairs
                ZillowAttributeCollection collection = new ZillowAttributeCollection();
                foreach (DataTable dt in ds.Tables)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        foreach (DataColumn dc in dt.Columns)
                        {
                            collection.ZillowAttributes.Add(new ZillowAttribute()
                            {
                                GroupName = dt.TableName,
                                AttributeName = dc.ColumnName,
                                AttributeValue = dr[dc].ToString()
                            });
                        }
                    }
                }

                // Validate only Version 5 is supported
                IEnumerable<ZillowAttribute> query = from p in collection.ZillowAttributes
                                                     where p.GroupName.Equals("ZillowMortgageContactList", StringComparison.OrdinalIgnoreCase)
                                                         && p.AttributeName.Equals("version", StringComparison.OrdinalIgnoreCase)
                                                     select p;
                if (query.Any())
                {
                    if (!string.IsNullOrEmpty(query.First().AttributeValue))
                    {
                        if (!query.First().AttributeValue.Equals("5"))
                        {
                            ApplicationException appex = new ApplicationException("Application Error: only ZillowMortgageContactList version 5 is supported.");
                            NLogger.Error(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, appex.Message, appex);
                            throw appex;
                        }
                    }
                    else
                    {
                        ApplicationException appex = new ApplicationException("Application Error: unable to locate version in ZillowMortgageContactList.");
                        NLogger.Error(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, appex.Message, appex);
                        throw appex;
                    }
                }
                else
                {
                    ApplicationException appex = new ApplicationException("Application Error: unable to parse the Zillow XML.");
                    NLogger.Error(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, appex.Message, appex);
                    throw appex;
                }
                // update the collection's xml version
                collection.XmlVersion = query.First().AttributeValue;

                return collection;
            }
            catch (Exception ex)
            {
                NLogger.Error(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Saves the ZillowAttributeCollection to the Pulse database
        /// </summary>
        /// <param name="collection"></param>
        internal static void Save(ZillowAttributeCollection collection)
        {
            
        }
    }
}