using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using C_DentalClaimTracker.General;

namespace C_DentalClaimTracker.E_Claims.Processing
{
    public class UnsubmittedSearchResultList : SortableBindingList<UnsubmittedSearchResult>
    { }

    public class UnsubmittedSearchResult
    {
        private DateTime? _CreatedOn;
        private string _PatientName;
        private string _ClaimType;
        private string _CompanyName;
        private decimal _ClaimAmount;
        private string _Handling;
        private string _claimBatch;
        private string _ClaimID;
        private string _ClaimDB;
        private claim _ClaimObject;
        private string _providerInfo;


        public UnsubmittedSearchResult(DateTime? createdOn, string patientName, string providerInfo, string claimType, string companyName,
                        decimal claimAmount, string handling, string claimBatch, string claimID, string claimDB, claim claimObject)
        {
            _CreatedOn = createdOn;
            _PatientName = patientName;
            _ClaimType = claimType;
            _CompanyName = companyName;
            _ClaimAmount = claimAmount;
            _Handling = handling;
            _claimBatch = claimBatch;
            _ClaimID = claimID;
            _ClaimDB = claimDB;
            _ClaimObject = claimObject;
            _providerInfo = providerInfo;
        }

        public DateTime? CreatedOn
        {
            get { return _CreatedOn; }
            set { _CreatedOn = value; }
        }
        public string PatientName
        {
            get { return _PatientName; }
            set { _PatientName = value; }
        }
        public string ClaimType
        {
            get { return _ClaimType; }
            set { _ClaimType = value; }
        }
        public string CompanyName
        {
            get { return _CompanyName; }
            set { _CompanyName = value; }
        }
        public decimal ClaimAmount
        {
            get { return _ClaimAmount; }
            set { _ClaimAmount = value; }
        }
        public string Handling
        {
            get { return _Handling; }
            set { _Handling = value; }
        }
        public string ClaimID
        {
            get { return _ClaimID; }
            set { _ClaimID = value; }
        }
        public string ClaimBatch
        {
            get { return _claimBatch; }
            set { _claimBatch = value; }
        }
        public string ClaimDB
        {
            get { return _ClaimDB; }
            set { _ClaimDB = value; }
        }
        public claim ClaimObject
        {
            get { return _ClaimObject; }
            set { _ClaimObject = value; }
        }
        public string ProviderInfo
        {
            get { return _providerInfo; }
            set { _providerInfo = value; }
        }

    }
}
