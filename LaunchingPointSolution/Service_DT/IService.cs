using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using LP2.Service.Common;

namespace LP2.Service
{
    // NOTE: If you change the interface name "IService1" here, you must also update the reference to "IService1" in App.config.
    [ServiceContract]
    public interface ILP_Service
    {
        #region Operation Contracts for User Manager
        [OperationContract]
        StartUserImportResponse StartUserImportService(StartUserImportRequest req);

        [OperationContract]
        StopUserImportResponse StopUserImportService(StopUserImportRequest req);

        [OperationContract]
        ImportADUsersResponse ImportADUsers(ImportADUsersRequest req);

        [OperationContract]
        UpdateADUserResponse UpdateADUser(UpdateADUserRequest req);
        #endregion

        #region Operation Contracts for Point Manager
        [OperationContract]
        StartPointImportResponse StartPointImportService(StartPointImportRequest req);
        
        [OperationContract]
        StopPointImportResponse StopPointImportService(StopPointImportRequest req);

        [OperationContract]
        ImportAllLoansResponse ImportAllLoans(ImportAllLoansRequest req);

        [OperationContract]
        ImportLoansResponse ImportLoans(ImportLoansRequest req);

        [OperationContract]
        ImportLoanRepNamesResponse ImportLoanRepNames(ImportLoanRepNamesRequest req);

        [OperationContract]
        ImportCardexResponse ImportCardex(ImportCardexRequest req);

        [OperationContract]
        GetPointFileResponse GetPointFile(GetPointFileRequest req);
        #endregion
    }

   
}
