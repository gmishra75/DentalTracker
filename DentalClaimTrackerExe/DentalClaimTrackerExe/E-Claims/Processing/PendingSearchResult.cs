using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using C_DentalClaimTracker.General;

namespace C_DentalClaimTracker.E_Claims.Processing
{
    public class PendingSearchResultList : SortableBindingList<PendingSearchResult>
    { }

    public class PendingSearchResult
    {
        // number, date, batch_info, handling, server, paid claims, data, claim list
        private string _num;
        private string _batchDate;
        private string _info;
        private string _handling;
        private string _server;
        private string _paidClaims;
        private claim_batch _data;
        private object _claimList;
        private decimal _amount;

        public PendingSearchResult(string number, string batchDate, decimal amount, string info, string handling,
            string server, string paidClaims, claim_batch data, object claimList)
        {
            _num = number;
            _batchDate = batchDate;
            _amount = amount;
            _info = info;
            _handling = handling;
            _server = server;
            _paidClaims = paidClaims;
            _data = data;
            _claimList = claimList;
        }

        public string Num
        {
            get { return _num; }
            set { _num = value; }
        }
        public string BatchDate
        {
            get { return _batchDate; }
            set { _batchDate = value; }
        }
        public decimal Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }
        public string Info
        {
            get { return _info; }
            set { _info = value; }
        }
        public string Handling
        {
            get { return _handling; }
            set { _handling = value; }
        }
        public string Server
        {
            get { return _server; }
            set { _server = value; }
        }
        public string PaidClaims
        {
            get { return _paidClaims; }
            set { _paidClaims = value; }
        }
        public claim_batch Data
        {
            get { return _data; }
            set { _data = value; }
        }
        public object ClaimList
        {
            get { return _claimList; }
            set { _claimList = value; }
        }

    }
}
