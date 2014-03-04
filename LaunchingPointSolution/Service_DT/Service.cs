using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using LP2.Service.Common;

namespace LP2.Service
{
    // NOTE: If you change the class name "Service1" here, you must also update the reference to "Service1" in App.config.
    public class LP_Service : ILP_Service
    {
         #region ILP_Service Members

        public StartUserImportResponse StartUserImportService(StartUserImportRequest req)
        {
            StartUserImportResponse resp = new StartUserImportResponse();
            resp.hdr = new RespHdr();
            resp.hdr.Successful = true;
            return resp;
        }

        public StopUserImportResponse StopUserImportService(StopUserImportRequest req)
        {
            StopUserImportResponse resp = new StopUserImportResponse();
            resp.hdr = new RespHdr();
            resp.hdr.Successful = true;

            return resp;
        }

        public ImportADUsersResponse ImportADUsers(ImportADUsersRequest req)
        {
            ImportADUsersResponse resp = new ImportADUsersResponse();
            resp.hdr = new RespHdr();
            resp.hdr.Successful = true;

            return resp;
        }

        public UpdateADUserResponse UpdateADUser(UpdateADUserRequest req)
        {
            UpdateADUserResponse resp = new UpdateADUserResponse();
            resp.hdr = new RespHdr();
            resp.hdr.Successful = true;
            return resp;
        }

        public StartPointImportResponse StartPointImportService(StartPointImportRequest req)
        {
            StartPointImportResponse resp = new StartPointImportResponse();
            resp.hdr = new RespHdr();
            resp.hdr.Successful = true;

            return resp;
        }

        public StopPointImportResponse StopPointImportService(StopPointImportRequest req)
        {
            StopPointImportResponse resp = new StopPointImportResponse();
            resp.hdr = new RespHdr();
            resp.hdr.Successful = true;

            return resp;
        }

        public ImportAllLoansResponse ImportAllLoans(ImportAllLoansRequest req)
        {
            ImportAllLoansResponse resp = new ImportAllLoansResponse();
            resp.hdr = new RespHdr();
            resp.hdr.Successful = true;

            return resp;
        }

        public ImportLoansResponse ImportLoans(ImportLoansRequest req)
        {
            ImportLoansResponse resp = new ImportLoansResponse();
            resp.hdr = new RespHdr();
            resp.hdr.Successful = true;

            return resp;
        }

        public ImportLoanRepNamesResponse ImportLoanRepNames(ImportLoanRepNamesRequest req)
        {
            ImportLoanRepNamesResponse resp = new ImportLoanRepNamesResponse();
            resp.hdr = new RespHdr();
            resp.hdr.Successful = true;

            return resp;
        }

        public ImportCardexResponse ImportCardex(ImportCardexRequest req)
        {
            ImportCardexResponse resp = new ImportCardexResponse();
            resp.hdr = new RespHdr();
            resp.hdr.Successful = true;

            return resp;
        }

        public GetPointFileResponse GetPointFile(GetPointFileRequest req)
        {
            GetPointFileResponse resp = new GetPointFileResponse();
            resp.hdr = new RespHdr();
            resp.hdr.Successful = true;
            return resp;
        }
        #endregion
    }
}
