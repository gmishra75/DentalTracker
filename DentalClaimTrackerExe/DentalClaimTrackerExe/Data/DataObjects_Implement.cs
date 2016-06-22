using System;
using System.Data;
using System.Collections;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;

namespace C_DentalClaimTracker
{
    public partial class procedure : DataObject
    {
        /// <summary>
        /// The default constructor for the procedures object, creates a new
        /// instance of this object without loading a default record.
        /// </summary>
        public procedure()
            : base("id", "procedures")
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        /// <summary>
        /// Constructor for the procedures object, creates a new
        /// instance of this object and loads a record using the passed ID.
        /// </summary>
        public procedure(int ID)
            : base("id", "procedures", ID)
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        private void thisRecordChanged(object sender, RecordChangedEventArgs e) { }

        /// <summary>
        /// Get/Set for the int id. Throws DataObjectException if the data is null.
        /// </summary>
        public int id
        {
            get
            {
                if (this["id"] is int)
                    return (int)this["id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - id)");
            }
            set
            {
                this["id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int externalid. Throws DataObjectException if the data is null.
        /// </summary>
        public int externalid
        {
            get
            {
                if (this["externalid"] is int)
                    return (int)this["externalid"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - externalid)");
            }
            set
            {
                this["externalid"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int claim_id. Throws DataObjectException if the data is null.
        /// </summary>
        public int claim_id
        {
            get
            {
                if (this["claim_id"] is int)
                    return (int)this["claim_id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - claim_id)");
            }
            set
            {
                this["claim_id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the datetime pl_date. Throws DataObjectException if the data is null.
        /// </summary>
        public DateTime? pl_date
        {
            get
            {
                return GetDateTimeNullable(this["pl_date"], false);
            }
            set
            {
                this["pl_date"] = DateTimeDBNull(value);
            }
        }
        /// <summary>
        /// Get/Set for the string ada_code. Returns "" if the data is null.
        /// </summary>
        public string ada_code
        {
            get
            {
                if (this["ada_code"] is string)
                    return (string)this["ada_code"];
                else
                    return "";
            }
            set
            {
                this["ada_code"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string description. Returns "" if the data is null.
        /// </summary>
        public string description
        {
            get
            {
                if (this["description"] is string)
                    return (string)this["description"];
                else
                    return "";
            }
            set
            {
                this["description"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int tooth_range_start. CUSTOM: Returns 0 if the data is null
        /// </summary>
        public int tooth_range_start
        {
            get
            {
                if (this["tooth_range_start"] is int)
                    return (int)this["tooth_range_start"];
                else
                    return 0;
            }
            set
            {
                this["tooth_range_start"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int tooth_range_end. CUSTOM: Returns 0 if the data is null
        /// </summary>
        public int tooth_range_end
        {
            get
            {
                if (this["tooth_range_end"] is int)
                    return (int)this["tooth_range_end"];
                else
                    return 0;
            }
            set
            {
                this["tooth_range_end"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string surf_string. Returns "" if the data is null.
        /// </summary>
        public string surf_string
        {
            get
            {
                if (this["surf_string"] is string)
                    return (string)this["surf_string"];
                else
                    return "";
            }
            set
            {
                this["surf_string"] = value;
            }
        }
        
    }



    public partial class call : DataObject, IComparable
    {
        private bool _readOnly;
        private claim _linkedClaim;

        /// <summary>
        /// The default constructor for the calls object, creates a new
        /// instance of this object without loading a default record.
        /// </summary>
        public call()
            : base("id", "calls")
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
            _readOnly = false;
        }
        /// <summary>
        /// Constructor for the calls object, creates a new
        /// instance of this object and loads a record using the passed ID.
        /// </summary>
        public call(int ID)
            : base("id", "calls", ID)
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
            _readOnly = false;
        }

        public bool ReadOnly
        {
            get { return _readOnly; }
            set { _readOnly = value; }
        }

        public claim LinkedClaim
        {
            get
            {
                if (_linkedClaim == null)
                {
                    if (this["claim_id"] is int)
                    {
                        _linkedClaim = new claim((int)this["claim_id"]);
                    }
                    else
                    {
                        throw new DataObjectException("Relationship is not initialized.");
                    }
                }
                return _linkedClaim;
            }
            set
            {
                _linkedClaim = value;
                this["claim_id"] = value.id;
            }
        }

        private void thisRecordChanged(object sender, RecordChangedEventArgs e)
        {
            _linkedClaim = null;
        }

        /// <summary>
        /// Get/Set for the int id. Throws DataObjectException if the data is null.
        /// </summary>
        public int id
        {
            get
            {
                if (this["id"] is int)
                    return (int)this["id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - id)");
            }
            set
            {
                this["id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string operator. Returns "" if the data is null.
        /// </summary>
        public string operatordata
        {
            get
            {
                if (this["operator"] is string)
                    return (string)this["operator"];
                else
                    return "";
            }
            set
            {
                this["operator"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the DateTime created_on. Throws DataObjectException if the data is null.
        /// </summary>
        public DateTime? created_on
        {
            get
            {
                return GetDateTimeNullable(this["created_on"], true);
            }
            set
            {
                this["created_on"] = DateTimeDBNull(value);
            }
        }
        /// <summary>
        /// Get/Set for the DateTime updated_on. Throws DataObjectException if the data is null.
        /// </summary>
        public DateTime? updated_on
        {
            get
            {
                return GetDateTimeNullable(this["updated_on"], true);
            }
            set
            {
                this["updated_on"] = DateTimeDBNull(value);
            }
        }
        /// <summary>
        /// Get/Set for the int claim_id. Throws DataObjectException if the data is null.
        /// </summary>
        public int claim_id
        {
            get
            {
                if (this["claim_id"] is int)
                    return (int)this["claim_id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - claim_id)");
            }
            set
            {
                this["claim_id"] = value;
            }
        }

        public int OnHoldSeconds
        {
            get
            {
                if (this["onhold_secs"] is int)
                    return (int)this["onhold_secs"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - onhold_secs)");
            }
            set
            {
                this["onhold_secs"] = value;
            }
        }

        public DateTime StartTime
        {
            get
            {
                return GetDateTime(this["start_time"]);
            }
            set
            {
                this["start_time"] = value;
            }
        }

        public int DurationSeconds
        {
            get
            {
                if (this["duration_secs"] is int)
                    return (int)this["duration_secs"];
                else
                    return 0;
            }
            set
            {
                this["duration_secs"] = value;
            }
        }

        public int call_status
        {
            get
            {
                if (this["call_status"] is int)
                    return (int)this["call_status"];
                else
                    return 0;
            }
            set
            {
                this["call_status"] = value;
            }
        }

        /// <summary>
        /// CUSTOM: Default return true
        /// </summary>
        public bool talked_to_human
        {
            get
            {
                if (this["talked_to_human"] is int)
                    return Convert.ToBoolean((int)this["talked_to_human"]);
                else
                    return true;
            }
            set
            {
                this["talked_to_human"] = Convert.ToInt32(value);
            }
        }

        /// <summary>
        /// Get/Set for the int parent_id. CUSTOM: Returns 0 if the value is null
        /// </summary>
        public int parent_id
        {
            get
            {
                if (this["parent_id"] is int)
                    return (int)this["parent_id"];
                else
                    return 0;
            }
            set
            {
                this["parent_id"] = value;
            }
        }

        #region IComparable Members
        public int CompareTo(object obj)
        {
            if (obj is call)
            {
                call temp = (call)obj;
                if ((this.created_on.HasValue) && (temp.created_on.HasValue))
                    return this.created_on.Value.CompareTo(temp.created_on.Value); // First choice compare created on date
                return this.id.CompareTo(temp.id); // Second choice compare ID
            }
            throw new ArgumentException("object is not a notes");
        }
        #endregion
    }

    public partial class choice : DataObject, IComparable
    {
        private question _linkedQuestion;
        public call _linkedCall;

        /// <summary>
        /// The default constructor for the choices object, creates a new
        /// instance of this object without loading a default record.
        /// </summary>
        public choice()
            : base("id", "choices")
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        /// <summary>
        /// Constructor for the choices object, creates a new
        /// instance of this object and loads a record using the passed ID.
        /// </summary>
        public choice(int ID)
            : base("id", "choices", ID)
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        private void thisRecordChanged(object sender, RecordChangedEventArgs e)
        {
            _linkedQuestion = null;
            _linkedCall = null;
        }

        public call LinkedCall
        {
            get
            {
                if (_linkedCall == null)
                {
                    if (this["call_id"] is int)
                    {
                        _linkedCall = new call((int)this["call_id"]);
                    }
                    else
                    {
                        throw new DataObjectException("Relationship is not initialized.");
                    }
                }
                return _linkedCall;
            }
            set
            {
                this["call_id"] = value.id;
                _linkedCall = value;
            }
        }


        public question LinkedQuestion
        {
            get
            {
                if (_linkedQuestion == null)
                {
                    if (this["question_id"] is int)
                    {
                        _linkedQuestion = new question((int)this["question_id"]);
                    }
                    else
                    {
                        throw new DataObjectException("Relationship is not initialized. (choice-question)");
                    }
                }
                return _linkedQuestion;
            }
            set
            {
                this["question_id"] = value.id;
                _linkedQuestion = value;
            }
        }

        /// <summary>
        /// Get/Set for the int id. Throws DataObjectException if the data is null.
        /// </summary>
        public int id
        {
            get
            {
                if (this["id"] is int)
                    return (int)this["id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - id)");
            }
            set
            {
                this["id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int call_id. Throws DataObjectException if the data is null.
        /// </summary>
        public int call_id
        {
            get
            {
                if (this["call_id"] is int)
                    return (int)this["call_id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - call_id)");
            }
            set
            {
                this["call_id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int choice_id. Throws DataObjectException if the data is null.
        /// </summary>
        public int choice_id
        {
            get
            {
                if (this["choice_id"] is int)
                    return (int)this["choice_id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - choice_id)");
            }
            set
            {
                this["choice_id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string operator. Returns "" if the data is null.
        /// </summary>
        public string operatordata
        {
            get
            {
                if (this["operator"] is string)
                    return (string)this["operator"];
                else
                    return "";
            }
            set
            {
                this["operator"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string question. Returns "" if the data is null.
        /// </summary>
        public string question
        {
            get
            {
                if (this["question"] is string)
                    return (string)this["question"];
                else
                    return "";
            }
            set
            {
                this["question"] = value;
            }
        }

        /// <summary>
        /// Get/Set for the int question. Throws DataObjectException if the data is null.
        /// </summary>
        public int question_id
        {
            get
            {
                if (this["question_id"] is int)
                    return (int)this["question_id"];
                throw new DataObjectException("Property value is empty or invalid. (int - question_id)");
            }
            set
            {
                this["question_id"] = value;
            }
        }

        /// <summary>
        /// Get/Set for the string answer. Returns "" if the data is null.
        /// </summary>
        public string answer
        {
            get
            {
                if (this["answer"] is string)
                    return (string)this["answer"];
                else
                    return "";
            }
            set
            {
                this["answer"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the DateTime created_at. Throws DataObjectException if the data is null.
        /// </summary>
        public DateTime? created_at
        {
            get
            {
                return GetDateTimeNullable(this["created_at"], true);
            }
            set
            {
                this["created_at"] = DateTimeDBNull(value);
            }
        }

        #region IComparable Members
        public int CompareTo(object obj)
        {
            if (obj is choice)
            {
                choice temp = (choice)obj;
                return this.LinkedQuestion.CompareTo(temp.LinkedQuestion);
            }
            throw new ArgumentException("object is not a choices");
        }
        #endregion
    }

    public partial class claim : DataObject
    {
        private company _companies;
        private clinic _clinic;
        private List<question> _questions;

        /// <summary>
        /// The default constructor for the claims object, creates a new
        /// instance of this object without loading a default record.
        /// </summary>
        public claim()
            : base("id", "claims")
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
            _questions = new List<question>();
        }
        /// <summary>
        /// Constructor for the claims object, creates a new
        /// instance of this object and loads a record using the passed ID.
        /// </summary>
        public claim(int ID)
            : base("id", "claims", ID)
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
            _questions = new List<question>();
        }


        public company LinkedCompany
        {
            get
            {
                if (_companies == null)
                {
                    if (this["company_id"] is int)
                    {
                        _companies = new company((int)this["company_id"]);
                    }
                    else
                    {
                        return new company();
                    }
                }
                return _companies;
            }
        }

        public clinic Clinic
        {
            get
            {
                if (_clinic == null)
                {
                    if (this["clinic_id"] is int)
                    {
                        if ((int) this["clinic_id"] > 0)
                        {
                        try
                        {
                            _clinic = new clinic((int)this["clinic_id"]);
                        }
                        catch
                        {
                            return null;
                        }
                            }
                        else
                            return null;
                    }
                    else
                    {
                        _clinic = new clinic();
                    }
                }
                return _clinic;
            }
        }

        private void thisRecordChanged(object sender, RecordChangedEventArgs e)
        {
            _companies = null;
        }

        /// <summary>
        /// Get/Set for the int id. Throws DataObjectException if the data is null.
        /// </summary>
        public int id
        {
            get
            {
                if (this["id"] is int)
                    return (int)this["id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - id)");
            }
            set
            {
                this["id"] = value;
            }
        }

        /// <summary>
        /// Get/Set for the int id. Throws DataObjectException if the data is null.
        /// </summary>
        public int company_address_id
        {
            get
            {
                if (this["company_address_id"] is int)
                    return (int)this["company_address_id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - company_address_id)");
            }
            set
            {
                this["company_address_id"] = value;
            }
        }

        /// <summary>
        /// Get/Set for the int company_id. Throws DataObjectException if the data is null.
        /// </summary>
        public int company_id
        {
            get
            {
                if (this["company_id"] is int)
                    return (int)this["company_id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - company_id)");
            }
            set
            {
                this["company_id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string created_by. Returns "" if the data is null.
        /// </summary>
        public string created_by
        {
            get
            {
                if (this["created_by"] is string)
                    return (string)this["created_by"];
                else
                    return "";
            }
            set
            {
                this["created_by"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the DateTime created_on. Throws DataObjectException if the data is null.
        /// </summary>
        public DateTime? created_on
        {
            get
            {
                return GetDateTimeNullable(this["created_on"], true);
            }
            set
            {
                this["created_on"] = DateTimeDBNull(value);
            }
        }
        /// <summary>
        /// Get/Set for the DateTime updated_on. Throws DataObjectException if the data is null.
        /// </summary>
        public DateTime? updated_on
        {
            get
            {
                return GetDateTimeNullable(this["updated_on"], true);
            }
            set
            {
                this["updated_on"] = DateTimeDBNull(value);
            }
        }
        /// <summary>
        /// Get/Set for the DateTime date_of_service. Throws DataObjectException if the data is null.
        /// </summary>
        public DateTime? date_of_service
        {
            get
            {
                return GetDateTimeNullable(this["date_of_service"], true);
            }
            set
            {
                this["date_of_service"] = DateTimeDBNull(value);
            }
        }

        /// <summary>
        /// Get/Set for the string service_description. Returns "" if the data is null.
        /// </summary>
        public string service_description
        {
            get
            {
                if (this["service_description"] is string)
                    return (string)this["service_description"];
                else
                    return "";
            }
            set
            {
                this["service_description"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string patient_first_name. Returns "" if the data is null.
        /// </summary>
        public string patient_first_name
        {
            get
            {
                if (this["patient_first_name"] is string)
                    return (string)this["patient_first_name"];
                else
                    return "";
            }
            set
            {
                this["patient_first_name"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the DateTime patient_dob. Throws DataObjectException if the data is null.
        /// </summary>
        public DateTime? patient_dob
        {
            get
            {
                return GetDateTimeNullable(this["patient_dob"], true);
            }
            set
            {
                this["patient_dob"] = DateTimeDBNull(value);
            }
        }

        /// <summary>
        /// Get/Set for the string patient_address. Returns "" if the data is null.
        /// </summary>
        public string patient_address
        {
            get
            {
                if (this["patient_address"] is string)
                    return CommonFunctions.FormatTextForMultiLine((string)this["patient_address"]);
                else
                    return "";
            }
            set
            {
                this["patient_address"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string doctor_first_name. Returns "" if the data is null.
        /// </summary>
        public string doctor_first_name
        {
            get
            {
                if (this["doctor_first_name"] is string)
                    return (string)this["doctor_first_name"];
                else
                    return "";
            }
            set
            {
                this["doctor_first_name"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string doctor_tax_number. Returns "" if the data is null.
        /// </summary>
        public string doctor_tax_number
        {
            get
            {
                if (this["doctor_tax_number"] is string)
                    return (string)this["doctor_tax_number"];
                else
                    return "";
            }
            set
            {
                this["doctor_tax_number"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string doctor_license_number. Returns "" if the data is null.
        /// </summary>
        public string doctor_license_number
        {
            get
            {
                if (this["doctor_license_number"] is string)
                    return (string)this["doctor_license_number"];
                else
                    return "";
            }
            set
            {
                this["doctor_license_number"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string doctor_bcbs_number. Returns "" if the data is null.
        /// </summary>
        public string doctor_bcbs_number
        {
            get
            {
                if (this["doctor_bcbs_number"] is string)
                    return (string)this["doctor_bcbs_number"];
                else
                    return "";
            }
            set
            {
                this["doctor_bcbs_number"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string doctor_phone_number. Returns "" if the data is null.
        /// </summary>
        public string doctor_phone_number
        {
            get
            {
                if (this["doctor_phone_number"] is string)
                    return (string)this["doctor_phone_number"];
                else
                    return "";
            }
            set
            {
                this["doctor_phone_number"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string doctor_fax_number. Returns "" if the data is null.
        /// </summary>
        public string doctor_fax_number
        {
            get
            {
                if (this["doctor_fax_number"] is string)
                    return (string)this["doctor_fax_number"];
                else
                    return "";
            }
            set
            {
                this["doctor_fax_number"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string doctor_address. Returns "" if the data is null.
        /// </summary>
        public string doctor_address
        {
            get
            {
                if (this["doctor_address"] is string)
                    return CommonFunctions.FormatTextForMultiLine((string)this["doctor_address"]);
                else
                    return "";
            }
            set
            {
                this["doctor_address"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string doctor_provider_id. Returns "" if the data is null.
        /// </summary>
        public string doctor_provider_id
        {
            get
            {
                if (this["doctor_provider_id"] is string)
                    return (string)this["doctor_provider_id"];
                else
                    return "";
            }
            set
            {
                this["doctor_provider_id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string doctor_dentrix_id. Returns "" if the data is null.
        /// </summary>
        public string doctor_dentrix_id
        {
            get
            {
                if (this["doctor_dentrix_id"] is string)
                    return (string)this["doctor_dentrix_id"];
                else
                    return "";
            }
            set
            {
                this["doctor_dentrix_id"] = value;
            }
        }

        /// <summary>
        /// Get/Set for the string subscriber_first_name. Returns "" if the data is null.
        /// </summary>
        public string subscriber_first_name
        {
            get
            {
                if (this["subscriber_first_name"] is string)
                    return (string)this["subscriber_first_name"];
                else
                    return "";
            }
            set
            {
                this["subscriber_first_name"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the DateTime subscriber_dob. Throws DataObjectException if the data is null.
        /// </summary>
        public DateTime? subscriber_dob
        {
            get
            {
                return GetDateTimeNullable(this["subscriber_dob"], true);
            }
            set
            {
                this["subscriber_dob"] = DateTimeDBNull(value);
            }
        }

        /// <summary>
        /// Get/Set for the string subscriber_number. Returns "" if the data is null.
        /// </summary>
        public string subscriber_number
        {
            get
            {
                if (this["subscriber_number"] is string)
                    return (string)this["subscriber_number"];
                else
                    return "";
            }
            set
            {
                this["subscriber_number"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string subscriber_alternate_number. Returns "" if the data is null.
        /// </summary>
        public string subscriber_alternate_number
        {
            get
            {
                if (this["subscriber_alternate_number"] is string)
                    return (string)this["subscriber_alternate_number"];
                else
                    return "";
            }
            set
            {
                this["subscriber_alternate_number"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string subscriber_group_name. Returns "" if the data is null.
        /// </summary>
        public string subscriber_group_name
        {
            get
            {
                if (this["subscriber_group_name"] is string)
                    return (string)this["subscriber_group_name"];
                else
                    return "";
            }
            set
            {
                this["subscriber_group_name"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string subscriber_group_number. Returns "" if the data is null.
        /// </summary>
        public string subscriber_group_number
        {
            get
            {
                if (this["subscriber_group_number"] is string)
                    return (string)this["subscriber_group_number"];
                else
                    return "";
            }
            set
            {
                this["subscriber_group_number"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string subscriber_address. Returns "" if the data is null.
        /// </summary>
        public string subscriber_address
        {
            get
            {
                if (this["subscriber_address"] is string)
                    return CommonFunctions.FormatTextForMultiLine((string)this["subscriber_address"]);
                else
                    return "";
            }
            set
            {
                this["subscriber_address"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string notes. Returns "" if the data is null.
        /// </summary>
        public string notes
        {
            get
            {
                if (this["notes"] is string)
                    return (string)this["notes"];
                else
                    return "";
            }
            set
            {
                this["notes"] = value;
            }
        }
        /// <summary>
        /// Specially coded to deal with TinyInt data type. Returns 1 (open) if data is nonnumeric
        /// </summary>
        public int open
        {
            get
            {
                if (CommonFunctions.IsNumeric(CommonFunctions.DBNullToString(this["open"])))
                    return System.Convert.ToInt32(this["open"]);
                else
                    return 1; // Special case, return that it's open
            }
            set
            {
                this["open"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string claimidnum. Returns "" if the data is null.
        /// </summary>
        public string claimidnum
        {
            get
            {
                if (this["claimidnum"] is string)
                    return (string)this["claimidnum"];
                else
                    return "";
            }
            set
            {
                this["claimidnum"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string claimdb. Returns "" if the data is null.
        /// </summary>
        public string claimdb
        {
            get
            {
                if (this["claimdb"] is string)
                    return (string)this["claimdb"];
                else
                    return "";
            }
            set
            {
                this["claimdb"] = value;
            }
        }

        /// <summary>
        /// Get/Set for the string patient_middle_initial. Returns "" if the data is null.
        /// </summary>
        public string patient_middle_initial
        {
            get
            {
                if (this["patient_middle_initial"] is string)
                    return (string)this["patient_middle_initial"];
                else
                    return "";
            }
            set
            {
                this["patient_middle_initial"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string patient_last_name. Returns "" if the data is null.
        /// </summary>
        public string patient_last_name
        {
            get
            {
                if (this["patient_last_name"] is string)
                    return (string)this["patient_last_name"];
                else
                    return "";
            }
            set
            {
                this["patient_last_name"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string doctor_middle_initial. Returns "" if the data is null.
        /// </summary>
        public string doctor_middle_initial
        {
            get
            {
                if (this["doctor_middle_initial"] is string)
                    return (string)this["doctor_middle_initial"];
                else
                    return "";
            }
            set
            {
                this["doctor_middle_initial"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string doctor_last_name. Returns "" if the data is null.
        /// </summary>
        public string doctor_last_name
        {
            get
            {
                if (this["doctor_last_name"] is string)
                    return (string)this["doctor_last_name"];
                else
                    return "";
            }
            set
            {
                this["doctor_last_name"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string patient_address2. Returns "" if the data is null.
        /// </summary>
        public string patient_address2
        {
            get
            {
                if (this["patient_address2"] is string)
                    return (string)this["patient_address2"];
                else
                    return "";
            }
            set
            {
                this["patient_address2"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string patient_city. Returns "" if the data is null.
        /// </summary>
        public string patient_city
        {
            get
            {
                if (this["patient_city"] is string)
                    return (string)this["patient_city"];
                else
                    return "";
            }
            set
            {
                this["patient_city"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string patient_state. Returns "" if the data is null.
        /// </summary>
        public string patient_state
        {
            get
            {
                if (this["patient_state"] is string)
                    return (string)this["patient_state"];
                else
                    return "";
            }
            set
            {
                this["patient_state"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string patient_zip. Returns "" if the data is null.
        /// </summary>
        public string patient_zip
        {
            get
            {
                if (this["patient_zip"] is string)
                    return (string)this["patient_zip"];
                else
                    return "";
            }
            set
            {
                this["patient_zip"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string doctor_address2. Returns "" if the data is null.
        /// </summary>
        public string doctor_address2
        {
            get
            {
                if (this["doctor_address2"] is string)
                    return (string)this["doctor_address2"];
                else
                    return "";
            }
            set
            {
                this["doctor_address2"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string doctor_city. Returns "" if the data is null.
        /// </summary>
        public string doctor_city
        {
            get
            {
                if (this["doctor_city"] is string)
                    return (string)this["doctor_city"];
                else
                    return "";
            }
            set
            {
                this["doctor_city"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string doctor_state. Returns "" if the data is null.
        /// </summary>
        public string doctor_state
        {
            get
            {
                if (this["doctor_state"] is string)
                    return (string)this["doctor_state"];
                else
                    return "";
            }
            set
            {
                this["doctor_state"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string doctor_zip. Returns "" if the data is null.
        /// </summary>
        public string doctor_zip
        {
            get
            {
                if (this["doctor_zip"] is string)
                    return (string)this["doctor_zip"];
                else
                    return "";
            }
            set
            {
                this["doctor_zip"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string subscriber_middle_initial. Returns "" if the data is null.
        /// </summary>
        public string subscriber_middle_initial
        {
            get
            {
                if (this["subscriber_middle_initial"] is string)
                    return (string)this["subscriber_middle_initial"];
                else
                    return "";
            }
            set
            {
                this["subscriber_middle_initial"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string subscriber_last_name. Returns "" if the data is null.
        /// </summary>
        public string subscriber_last_name
        {
            get
            {
                if (this["subscriber_last_name"] is string)
                    return (string)this["subscriber_last_name"];
                else
                    return "";
            }
            set
            {
                this["subscriber_last_name"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string subscriber_address2. Returns "" if the data is null.
        /// </summary>
        public string subscriber_address2
        {
            get
            {
                if (this["subscriber_address2"] is string)
                    return (string)this["subscriber_address2"];
                else
                    return "";
            }
            set
            {
                this["subscriber_address2"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string subscriber_city. Returns "" if the data is null.
        /// </summary>
        public string subscriber_city
        {
            get
            {
                if (this["subscriber_city"] is string)
                    return (string)this["subscriber_city"];
                else
                    return "";
            }
            set
            {
                this["subscriber_city"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string subscriber_state. Returns "" if the data is null.
        /// </summary>
        public string subscriber_state
        {
            get
            {
                if (this["subscriber_state"] is string)
                    return (string)this["subscriber_state"];
                else
                    return "";
            }
            set
            {
                this["subscriber_state"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string subscriber_zip. Returns "" if the data is null.
        /// </summary>
        public string subscriber_zip
        {
            get
            {
                if (this["subscriber_zip"] is string)
                    return (string)this["subscriber_zip"];
                else
                    return "";
            }
            set
            {
                this["subscriber_zip"] = value;
            }
        }


        /// <summary>
        /// Get/Set for the DateTime resent_date. Throws DataObjectException if the data is null.
        /// </summary>
        public DateTime? resent_date
        {
            get
            {
                return GetDateTimeNullable(this["resent_date"], true);
            }
            set
            {
                this["resent_date"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the DateTime tracer_date. Throws DataObjectException if the data is null.
        /// </summary>
        public DateTime? tracer_date
        {
            get
            {
                return GetDateTimeNullable(this["tracer_date"], true);
            }
            set
            {
                this["tracer_date"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the DateTime on_hold_date. Throws DataObjectException if the data is null.
        /// </summary>
        public DateTime? on_hold_date
        {
            get
            {
                return GetDateTimeNullable(this["on_hold_date"], true);
            }
            set
            {
                this["on_hold_date"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the DateTime sent_date. Throws DataObjectException if the data is null.
        /// </summary>
        public DateTime? sent_date
        {
            get
            {
                return GetDateTimeNullable(this["sent_date"], true);
            }
            set
            {
                this["sent_date"] = value;
            }
        }

        /// <summary>
        /// Get/Set for the DateTime sent_date. Throws DataObjectException if the data is null.
        /// </summary>
        public DateTime? revisit_date
        {
            get
            {
                DateTime? toReturn = GetDateTimeNullable(this["revisit_date"], true);

                return toReturn;
            }
            set
            {
                this["revisit_date"] = value;
            }
        }

        /// <summary>
        /// Get/Set for the string handling. UNIQUE: Returns "Unclassified" if the data is null.
        /// </summary>
        public string handling
        {
            get
            {
                if (this["handling"] is string)
                    return (string)this["handling"];
                else
                    return "Unclassified";
            }
            set
            {
                this["handling"] = value;
            }
        }

        public string primary_claimid
        {
            get
            {
                if (this["primary_claimid"] is string)
                    return (string)this["primary_claimid"];
                else
                    return "";
            }
            set
            {
                this["primary_claimid"] = value;
            }
        }

        public string primary_claimdb
        {
            get
            {
                if (this["primary_claimdb"] is string)
                    return (string)this["primary_claimdb"];
                else
                    return "";
            }
            set
            {
                this["primary_claimdb"] = value;
            }
        }

        /// <summary>
        /// CUSTOM: returns -1 if null
        /// </summary>
        public int status_id
        {
            get
            {
                if (this["status_id"] is int)
                    return (int)this["status_id"];
                else
                    return -1;
            }
            set
            {
                this["status_id"] = value;
            }
        }


        /// <summary>
        /// CUSTOM: returns -1 if null
        /// </summary>
        public int status_last_user_id
        {
            get
            {
                if (this["status_last_user_id"] is int)
                    return (int)this["status_last_user_id"];
                else
                    return -1;
            }
            set
            {
                this["status_last_user_id"] = value;
            }
        }

        /// <summary>
        /// Get/Set for the DateTime sent_date. Returns null if the data is null.
        /// </summary>
        public DateTime? status_last_date
        {
            get
            {
                return GetDateTimeNullable(this["status_last_date"], true);
            }
            set
            {
                this["status_last_date"] = value;
            }
        }


        /// <summary>
        /// Get/Set for the int apex_override_default_message. Returns false if the data is null
        /// </summary>
        public bool apex_override_default_message
        {
            get
            {
                if (CommonFunctions.IsNumeric(this["apex_override_default_message"].ToString()))
                    return Convert.ToBoolean(Convert.ToInt32(this["apex_override_default_message"]));
                else
                    return false;
            }
            set
            {
                this["apex_override_default_message"] = Convert.ToInt32(value);
            }
        }
        /// <summary>
        /// Get/Set for the string apex_message_text. Returns "" if the data is null.
        /// </summary>
        public string apex_message_text
        {
            get
            {
                if (this["apex_message_text"] is string)
                    return (string)this["apex_message_text"];
                else
                    return "";
            }
            set
            {
                this["apex_message_text"] = value;
            }
        }

        /// <summary>
        /// Get/Set for the string nea_number. Returns "" if the data is null.
        /// </summary>
        public string nea_number
        {
            get
            {
                if (this["nea_number"] is string)
                    return (string)this["nea_number"];
                else
                    return "";
            }
            set
            {
                this["nea_number"] = value;
            }
        }

        /// <summary>
        /// Get/Set for the string override_address. Returns "" if the data is null
        /// </summary>
        public string override_address_provider
        {
            get
            {
                if (this["override_address_provider"] is string)
                    return (string)this["override_address_provider"];
                else
                    return "";
            }
            set
            {
                this["override_address_provider"] = value;
            }
        }

        /// <summary>
        /// Returns "" if the payerid is null
        /// </summary>
        public string payer_id
        {
            get
            {
                if (this["payer_id"] is string)
                    return (string)this["payer_id"];
                else
                    return "";
            }
            set
            {
                this["payer_id"] = value;
            }
        }

        public string payer_name
        {
            get
            {
                if (this["payer_name"] is string)
                    return (string)this["payer_name"];
                else
                    return "";
            }
            set
            {
                this["payer_name"] = value;
            }
        }
    }

    public partial class company : DataObject
    {
        /// <summary>
        /// The default constructor for the companies object, creates a new
        /// instance of this object without loading a default record.
        /// </summary>
        public company()
            : base("id", "companies")
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        /// <summary>
        /// Constructor for the companies object, creates a new
        /// instance of this object and loads a record using the passed ID.
        /// </summary>
        public company(int ID)
            : base("id", "companies", ID)
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }

        /// <summary>
        /// Constructor for the companies object, creates a new
        /// instance of this object and loads a record using the passed datarow.
        /// </summary>
        public company(DataRow toLoad)
            : base("id", "companies")
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
            Load(toLoad);
        }

        private void thisRecordChanged(object sender, RecordChangedEventArgs e) { }

        public bool IsUnique()
        {
            string sql = "SELECT * FROM companies WHERE name = '" + name + "'";

            if (this["id"] is int)
            {
                sql += " AND id != " + id;
            }

            DataTable dt = Search(sql);

            return dt.Rows.Count == 0;
        }

        /// <summary>
        /// Get/Set for the int id. Throws DataObjectException if the data is null.
        /// </summary>
        public int id
        {
            get
            {
                if (this["id"] is int)
                    return (int)this["id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - id)");
            }
            set
            {
                this["id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the DateTime created_on. Throws DataObjectException if the data is null.
        /// </summary>
        public DateTime? created_on
        {
            get
            {
                return GetDateTimeNullable(this["created_on"], true);
            }
            set
            {
                this["created_on"] = DateTimeDBNull(value);
            }
        }
        /// <summary>
        /// Get/Set for the DateTime updated_on. Throws DataObjectException if the data is null.
        /// </summary>
        public DateTime? updated_on
        {
            get
            {
                return GetDateTimeNullable(this["updated_on"], true);
            }
            set
            {
                this["updated_on"] = DateTimeDBNull(value);
            }
        }
        /// <summary>
        /// Get/Set for the string name. Returns "" if the data is null.
        /// </summary>
        public string name
        {
            get
            {
                if (this["name"] is string)
                    return (string)this["name"];
                else
                    return "";
            }
            set
            {
                this["name"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string address. Returns "" if the data is null.
        /// </summary>
        public string address
        {
            get
            {
                if (this["address"] is string)
                    return CommonFunctions.FormatTextForMultiLine((string)this["address"]);
                else
                    return "";
            }
            set
            {
                this["address"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string group_name. Returns "" if the data is null.
        /// </summary>
        public string group_name
        {
            get
            {
                if (this["group_name"] is string)
                    return (string)this["group_name"];
                else
                    return "";
            }
            set
            {
                this["group_name"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string group_num. Returns "" if the data is null.
        /// </summary>
        public string group_num
        {
            get
            {
                if (this["group_num"] is string)
                    return (string)this["group_num"];
                else
                    return "";
            }
            set
            {
                this["group_num"] = value;
            }
        }
    }

    public class schema_migrations : DataObject
    {
        /// <summary>
        /// The default constructor for the schema_migrations object, creates a new
        /// instance of this object without loading a default record.
        /// </summary>
        public schema_migrations()
            : base("version", "schema_migrations")
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        /// <summary>
        /// Constructor for the schema_migrations object, creates a new
        /// instance of this object and loads a record using the passed ID.
        /// </summary>
        public schema_migrations(int ID)
            : base("version", "schema_migrations", ID)
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        private void thisRecordChanged(object sender, RecordChangedEventArgs e) { }

        /// <summary>
        /// Get/Set for the string version. Returns "" if the data is null.
        /// </summary>
        public string version
        {
            get
            {
                if (this["version"] is string)
                    return (string)this["version"];
                else
                    return "";
            }
            set
            {
                this["version"] = value;
            }
        }
    }

    public class notes : DataObject, IComparable
    {
        public notes()
            : base("note_id", "notes")
        {

        }

        public notes(int noteId)
            : base("note_id", "notes", noteId)
        { }

        public int NoteId
        {
            get
            {
                if (this["note_id"] is int)
                    return (int)this["note_id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - note_id)");
            }
            set
            {
                this["note_id"] = value;
            }
        }

        public int CallId
        {
            get
            {
                if (this["call_id"] is int)
                    return (int)this["call_id"];
                else if (this["call_id"] == DBNull.Value)
                {
                    return 0;
                }
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - call_id)");
            }
            set
            {
                this["call_id"] = value;
            }
        }

        public int claim_id
        {
            get
            {
                if (this["claim_id"] is int)
                    return (int)this["claim_id"];
                else if (this["claim_id"] == DBNull.Value)
                {
                    return 0;
                }
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - claim_id)");
            }
            set
            {
                this["claim_id"] = value;
            }
        }


        /// <summary>
        /// Get/Set for the string operator. Returns "" if the data is null.
        /// </summary>
        public string operatorId
        {
            get
            {
                if (this["operator_id"] is string)
                    return (string)this["operator_id"];
                else
                    return "";
            }
            set
            {
                this["operator_id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the DateTime created_on. Throws DataObjectException if the data is null.
        /// </summary>
        public DateTime? created_on
        {
            get
            {
                return GetDateTimeNullable(this["created_on"], true);
            }
            set
            {
                this["created_on"] = DateTimeDBNull(value);
            }
        }
        /// <summary>
        /// Get/Set for the DateTime updated_on. Throws DataObjectException if the data is null.
        /// </summary>
        public DateTime? updated_on
        {
            get
            {
                return GetDateTimeNullable(this["updated_on"], true);
            }
            set
            {
                this["updated_on"] = DateTimeDBNull(value);
            }
        }

        /// <summary>
        /// Get/Set for the string answer. Returns "" if the data is null.
        /// </summary>
        public string Note
        {
            get
            {
                if (this["note_text"] is string)
                    return (string)this["note_text"];
                else
                    return "";
            }
            set
            {
                this["note_text"] = value;
            }
        }

        #region IComparable Members
        public int CompareTo(object obj)
        {
            if (obj is notes)
            {
                notes temp = (notes)obj;
                if ((this.created_on.HasValue) && (temp.created_on.HasValue))
                {
                    return this.created_on.Value.CompareTo(temp.created_on.Value);
                }
                else
                {
                    return 0;
                }
            }
            throw new ArgumentException("object is not a notes");
        }
        #endregion
    }


    public partial class question : DataObject, IComparable
    {
        private List<question> _subQuestions;
        private List<multiple_choice_answer> _multipleChoiceAnswers;

        /// <summary>
        /// The default constructor for the questions object, creates a new
        /// instance of this object without loading a default record.
        /// </summary>
        public question()
            : base("id", "questions")
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
            _subQuestions = new List<question>();
            _multipleChoiceAnswers = new List<multiple_choice_answer>();
        }
        /// <summary>
        /// Constructor for the questions object, creates a new
        /// instance of this object and loads a record using the passed ID.
        /// </summary>
        public question(int ID)
            : base("id", "questions", ID)
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
            _subQuestions = new List<question>();
            _multipleChoiceAnswers = new List<multiple_choice_answer>();
        }
        private void thisRecordChanged(object sender, RecordChangedEventArgs e)
        { }

        #region IComparable Members
        public int CompareTo(object obj)
        {
            if (obj is question)
            {
                question temp = (question)obj;
                return this.order_id.CompareTo(temp.order_id);
            }
            throw new ArgumentException("object is not a questions");
        }
        #endregion

        public List<question> SubQuestions
        {
            get { return _subQuestions; }
        }

        public List<multiple_choice_answer> MultipleChoiceAnswers
        {
            get { return _multipleChoiceAnswers; }
        }

        /// <summary>
        /// Get/Set for the int id. Throws DataObjectException if the data is null.
        /// </summary>
        public int id
        {
            get
            {
                if (this["id"] is int)
                    return (int)this["id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - id)");
            }
            set
            {
                this["id"] = value;
            }
        }

        public int order_id
        {
            get
            {
                if (this["order_id"] is int)
                    return (int)this["order_id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - id)");
            }
            set
            {
                this["order_id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string text. Returns "" if the data is null.
        /// </summary>
        public string text
        {
            get
            {
                if (this["text"] is string)
                    return (string)this["text"];
                else
                    return "";
            }
            set
            {
                this["text"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int parent. Throws DataObjectException if the data is null.
        /// </summary>
        public int parent
        {
            get
            {
                if (this["parent"] is int)
                    return (int)this["parent"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - parent)");
            }
            set
            {
                this["parent"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string required_answer. Returns "" if the data is null.
        /// </summary>
        public string required_answer
        {
            get
            {
                if (this["required_answer"] is string)
                    return (string)this["required_answer"];
                else
                    return "";
            }
            set
            {
                this["required_answer"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int type. Throws DataObjectException if the data is null.
        /// </summary>
        public QuestionTypes type
        {
            get
            {
                if (this["type"] is int)
                    return (QuestionTypes)this["type"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - type)");
            }
            set
            {
                this["type"] = (int)value;
            }
        }
        /// <summary>
        /// Get/Set for the int is_fork. Throws DataObjectException if the data is null.
        /// </summary>
        public bool is_fork
        {
            get
            {
                if (this["is_fork"] is int)
                    return System.Convert.ToBoolean((int)this["is_fork"]);
                else
                    return false;
            }
            set
            {
                this["is_fork"] = System.Convert.ToInt32(value);
            }
        }
        /// <summary>
        /// Get/Set for the int is_classification. Throws DataObjectException if the data is null.
        /// </summary>
        public bool is_classification
        {
            get
            {
                if (this["is_classification"] is int)
                    return System.Convert.ToBoolean((int)this["is_classification"]);
                else
                    return false;
            }
            set
            {
                this["is_classification"] = System.Convert.ToInt32(value);
            }
        }

        /// <summary>
        /// Get/Set for the string required_answer. Returns "" if the data is null.
        /// </summary>
        public string popup_question_text
        {
            get
            {
                if (this["popup_question_text"] is string)
                    return (string)this["popup_question_text"];
                else
                    return "";
            }
            set
            {
                this["popup_question_text"] = value;
            }
        }

        /// <summary>
        /// Get/Set for the int default_revisit_date. Returns 0
        /// </summary>
        public int default_revisit_date
        {
            get
            {
                if (this["default_revisit_date"] is int)
                    return (int)this["default_revisit_date"];
                else
                    return 0;
            }
            set
            {
                this["default_revisit_date"] = value;
            }
        }

        /// <summary>
        /// Get/Set for the int linked_status. Returns 0 if the data is null
        /// </summary>
        public int linked_status
        {
            get
            {
                if (this["linked_status"] is int)
                    return (int)this["linked_status"];
                else
                    return 0;
            }
            set
            {
                this["linked_status"] = value;
            }
        }

        /// <summary>
        /// Get/Set for the int set_revisit_date. Returns -1 (not set) if the data is null
        /// </summary>
        //public frmEditQuestionTree.SetRevisitDateOptions set_revisit_date
        //{
        //    get
        //    {
        //        if (this["set_revisit_date"] is int)
        //            return (frmEditQuestionTree.SetRevisitDateOptions)this["set_revisit_date"];
        //        else
        //            return frmEditQuestionTree.SetRevisitDateOptions.NotSet;
        //    }
        //    set
        //    {
        //        this["set_revisit_date"] = (int) value;
        //    }
        //}
    }

    public class multiple_choice_answer : DataObject, IComparable
    {
        private question _linkedQuestion;

        /// <summary>
        /// The default constructor for the multiple_choice_answers object, creates a new
        /// instance of this object without loading a default record.
        /// </summary>
        public multiple_choice_answer()
            : base("question_id", "multiple_choice_answers")
        {
            PrimaryKeys.Add("order_id");
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        /// <summary>
        /// Constructor for the multiple_choice_answers object, creates a new
        /// instance of this object and loads a record using the passed ID.
        /// </summary>
        public multiple_choice_answer(int ID)
            : base("question_id", "multiple_choice_answers", ID)
        {
            PrimaryKeys.Add("order_id");
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        private void thisRecordChanged(object sender, RecordChangedEventArgs e)
        {
            _linkedQuestion = null;
        }

        #region IComparable Members
        public int CompareTo(object obj)
        {
            if (obj is multiple_choice_answer)
            {
                multiple_choice_answer temp = (multiple_choice_answer)obj;
                return this.order_id.CompareTo(temp.order_id);
            }
            throw new ArgumentException("object is not a multiple_choice_answers");
        }
        #endregion

        /// <summary>
        /// Returns the linked question. Throws an exception if relationship doesn't exist
        /// </summary>
        public question LinkedQuestion
        {
            get
            {
                if (_linkedQuestion == null)
                {
                    if (this["question_id"] is int)
                    {
                        _linkedQuestion = new question((int)this["question_id"]);
                    }
                    else
                    {
                        throw new DataObjectException("Relationship is not initialized (multiple_choice_answers - questions)");
                    }
                }
                return _linkedQuestion;
            }
        }

        /// <summary>
        /// Get/Set for the int question_id. Throws DataObjectException if the data is null.
        /// </summary>
        public int question_id
        {
            get
            {
                if (this["question_id"] is int)
                    return (int)this["question_id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - question_id)");
            }
            set
            {
                this["question_id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int order_id. Throws DataObjectException if the data is null.
        /// </summary>
        public int order_id
        {
            get
            {
                if (this["order_id"] is int)
                    return (int)this["order_id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - order_id)");
            }
            set
            {
                this["order_id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string answer_text. Returns "" if the data is null.
        /// </summary>
        public string answer_text
        {
            get
            {
                if (this["answer_text"] is string)
                    return (string)this["answer_text"];
                else
                    return "";
            }
            set
            {
                this["answer_text"] = value;
            }
        }

        public override string ToString()
        {
            return answer_text;
        }
    }

    public class data_mapping_field : DataObject
    {
        /// <summary>
        /// The default constructor for the data_mapping_fields object, creates a new
        /// instance of this object without loading a default record.
        /// </summary>
        public data_mapping_field()
            : base("id", "data_mapping_fields")
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        /// <summary>
        /// Constructor for the data_mapping_fields object, creates a new
        /// instance of this object and loads a record using the passed ID.
        /// </summary>
        public data_mapping_field(int ID)
            : base("id", "data_mapping_fields", ID)
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        private void thisRecordChanged(object sender, RecordChangedEventArgs e) { }

        /// <summary>
        /// Get/Set for the int id. Throws DataObjectException if the data is null.
        /// </summary>
        public int id
        {
            get
            {
                if (this["id"] is int)
                    return (int)this["id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - id)");
            }
            set
            {
                this["id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string field_name. Returns "" if the data is null.
        /// </summary>
        public string field_name
        {
            get
            {
                if (this["field_name"] is string)
                    return (string)this["field_name"];
                else
                    return "";
            }
            set
            {
                this["field_name"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string field_type. Returns "" if the data is null.
        /// </summary>
        public string field_type
        {
            get
            {
                if (this["field_type"] is string)
                    return (string)this["field_type"];
                else
                    return "";
            }
            set
            {
                this["field_type"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string table_name. Returns "" if the data is null.
        /// </summary>
        public string table_name
        {
            get
            {
                if (this["table_name"] is string)
                    return (string)this["table_name"];
                else
                    return "";
            }
            set
            {
                this["table_name"] = value;
            }
        }
    }

    public partial class data_mapping_schema : DataObject
    {
        /// <summary>
        /// The default constructor for the data_mapping_schemas object, creates a new
        /// instance of this object without loading a default record.
        /// </summary>
        public data_mapping_schema()
            : base("id", "data_mapping_schemas")
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        /// <summary>
        /// Constructor for the data_mapping_schemas object, creates a new
        /// instance of this object and loads a record using the passed ID.
        /// </summary>
        public data_mapping_schema(int ID)
            : base("id", "data_mapping_schemas", ID)
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        private void thisRecordChanged(object sender, RecordChangedEventArgs e) { }

        /// <summary>
        /// Get/Set for the int id. Throws DataObjectException if the data is null.
        /// </summary>
        public int id
        {
            get
            {
                if (this["id"] is int)
                    return (int)this["id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - id)");
            }
            set
            {
                this["id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string schema_name. Returns "" if the data is null.
        /// </summary>
        public string schema_name
        {
            get
            {
                if (this["schema_name"] is string)
                    return (string)this["schema_name"];
                else
                    return "";
            }
            set
            {
                this["schema_name"] = value;
            }
        }

        /// <summary>
        /// Get/Set for the int data_type. Throws DataObjectException if the data is null.
        /// </summary>
        public DataMappingConnectionTypes data_type
        {
            get
            {
                if (this["data_type"] is int)
                    return (DataMappingConnectionTypes)this["data_type"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - data_type)");
            }
            set
            {
                this["data_type"] = (int)value;
            }
        }
        /// <summary>
        /// Get/Set for the string sql. Returns "" if the data is null.
        /// </summary>
        public string sqlstatement
        {
            get
            {
                if (this["sqlstatement"] is string)
                    return (string)this["sqlstatement"];
                else
                    return "";
            }
            set
            {
                this["sqlstatement"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string sql. Returns "" if the data is null.
        /// </summary>
        public string sqlstatementsecondaries
        {
            get
            {
                if (this["sqlstatementsecondaries"] is string)
                    return (string)this["sqlstatementsecondaries"];
                else
                    return "";
            }
            set
            {
                this["sqlstatementsecondaries"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string sql. Returns "" if the data is null.
        /// </summary>
        public string sqlstatementpredeterms
        {
            get
            {
                if (this["sqlstatementpredeterms"] is string)
                    return (string)this["sqlstatementpredeterms"];
                else
                    return "";
            }
            set
            {
                this["sqlstatementpredeterms"] = value;
            }
        }
        public string sqlstatementsecondarypredeterms 
        {
            get
            {
                if (this["sqlstatementsecondarypredeterms"] is string)
                    return (string)this["sqlstatementsecondarypredeterms"];
                else
                    return "";
            }
            set
            {
                this["sqlstatementsecondarypredeterms"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string server_name. Returns "" if the data is null.
        /// SPECIAL: Properly handles "\" in string due to MySQL truncation.
        /// </summary>
        public string server_name
        {
            get
            {
                if (this["server_name"] is string)
                    return (string)this["server_name"];
                else
                    return "";
            }
            set
            {
                this["server_name"] = value.Replace("\\", "\\\\");
            }
        }
        /// <summary>
        /// Get/Set for the string database_name. Returns "" if the data is null.
        /// </summary>
        public string database_name
        {
            get
            {
                if (this["database_name"] is string)
                    return (string)this["database_name"];
                else
                    return "";
            }
            set
            {
                this["database_name"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string user_name. Returns "" if the data is null.
        /// </summary>
        public string user_name
        {
            get
            {
                if (this["user_name"] is string)
                    return (string)this["user_name"];
                else
                    return "";
            }
            set
            {
                this["user_name"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string password. Returns "" if the data is null.
        /// </summary>
        public string pw
        {
            get
            {
                if (this["pw"] is string)
                    return (string)this["pw"];
                else
                    return "";
            }
            set
            {
                this["pw"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int allow_password_save. Throws DataObjectException if the data is null.
        /// </summary>
        public bool allow_password_save
        {
            get
            {
                if (this["allow_password_save"] is Int16)
                    return System.Convert.ToBoolean(this["allow_password_save"]);
                else
                    return false;
            }
            set
            {
                this["allow_password_save"] = value;
            }
        }

        /// <summary>
        /// Get/Set for the string claim_id_column. Returns "" if the data is null.
        /// </summary>
        public string claim_id_column
        {
            get
            {
                if (this["claim_id_column"] is string)
                    return (string)this["claim_id_column"];
                else
                    return "";
            }
            set
            {
                this["claim_id_column"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string claim_id_column. Returns "" if the data is null.
        /// </summary>
        public string claim_db_column
        {
            get
            {
                if (this["claim_db_column"] is string)
                    return (string)this["claim_db_column"];
                else
                    return "";
            }
            set
            {
                this["claim_db_column"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string company_namecolumn. Returns "" if the data is null.
        /// </summary>
        public string company_namecolumn
        {
            get
            {
                if (this["company_namecolumn"] is string)
                    return (string)this["company_namecolumn"];
                else
                    return "";
            }
            set
            {
                this["company_namecolumn"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string date_column. Returns "" if the data is null.
        /// </summary>
        public string date_column
        {
            get
            {
                if (this["date_column"] is string)
                    return (string)this["date_column"];
                else
                    return "";
            }
            set
            {
                this["date_column"] = value;
            }
        }
    }

    public class data_mapping_schema_data : DataObject
    {
        private data_mapping_schema _linkedSchema;
        private data_mapping_field _linkedField;

        /// <summary>
        /// The default constructor for the data_mapping_schema_data object, creates a new
        /// instance of this object without loading a default record.
        /// </summary>
        public data_mapping_schema_data()
            : base("schema_id", "data_mapping_schema_data")
        {
            PrimaryKeys.Add("mapping_field_id");
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        /// <summary>
        /// Constructor for the data_mapping_schema_data object, creates a new
        /// instance of this object and loads a record using the passed ID.
        /// </summary>
        public data_mapping_schema_data(int ID)
            : base("schema_id", "data_mapping_schema_data", ID)
        {
            PrimaryKeys.Add("mapping_field_id");
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        private void thisRecordChanged(object sender, RecordChangedEventArgs e)
        {
            _linkedField = null;
            _linkedSchema = null;
        }

        public data_mapping_schema LinkedSchema
        {
            get
            {
                if (this["schema_id"] is int)
                    return new data_mapping_schema((int)this["schema_id"]);
                else
                    throw new Exception("Relationship is not initialized (data_mapping_schema_data - data_mapping_schemas)");
            }
        }

        public data_mapping_field LinkedField
        {
            get
            {
                if (this["mapping_field_id"] is int)
                    return new data_mapping_field((int)this["mapping_field_id"]);
                else
                    throw new Exception("Relationship is not initialized (data_mapping_schema_data - data_mapping_fields)");
            }
        }

        /// <summary>
        /// Get/Set for the int schema_id. Throws DataObjectException if the data is null.
        /// </summary>
        public int schema_id
        {
            get
            {
                if (this["schema_id"] is int)
                    return (int)this["schema_id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - schema_id)");
            }
            set
            {
                this["schema_id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int mapping_field_id. Throws DataObjectException if the data is null.
        /// </summary>
        public int mapping_field_id
        {
            get
            {
                if (this["mapping_field_id"] is int)
                    return (int)this["mapping_field_id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - mapping_field_id)");
            }
            set
            {
                this["mapping_field_id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string mapped_to_text. Returns "" if the data is null.
        /// </summary>
        public string mapped_to_text
        {
            get
            {
                if (this["mapped_to_text"] is string)
                    return (string)this["mapped_to_text"];
                else
                    return "";
            }
            set
            {
                this["mapped_to_text"] = value;
            }
        }
    }

    public partial class insurance_company_group : DataObject
    {
        /// <summary>
        /// The default constructor for the insurance_company_groups object, creates a new
        /// instance of this object without loading a default record.
        /// </summary>
        public insurance_company_group()
            : base("id", "insurance_company_groups")
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        /// <summary>
        /// Constructor for the insurance_company_groups object, creates a new
        /// instance of this object and loads a record using the passed ID.
        /// </summary>
        public insurance_company_group(int ID)
            : base("id", "insurance_company_groups", ID)
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }

        /// <summary>
        /// Constructor for the insurance_company_groups object, creates a new
        /// instance of this object and loads a record using the passed datarow.
        /// </summary>
        public insurance_company_group(DataRow toLoad)
            : base("id", "insurance_company_groups")
        {
            
            RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
            Load(toLoad);
        }
        

        /// <summary>
        /// Get/Set for the int id. Throws DataObjectException if the data is null.
        /// </summary>
        public int id
        {
            get
            {
                if (this["id"] is int)
                    return (int)this["id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - id)");
            }
            set
            {
                this["id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string name. Returns "" if the data is null.
        /// </summary>
        public string name
        {
            get
            {
                if (this["name"] is string)
                    return (string)this["name"];
                else
                    return "";
            }
            set
            {
                this["name"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string description. Returns "" if the data is null.
        /// </summary>
        public string description
        {
            get
            {
                if (this["description"] is string)
                    return (string)this["description"];
                else
                    return "";
            }
            set
            {
                this["description"] = value;
            }
        }

        /// <summary>
        /// Get/Set for the int primary_provider. 
        /// CUSTOM: Returns 0 if the data is null
        /// </summary>
        public int primary_provider
        {
            get
            {
                if (this["primary_provider"] is int)
                    return (int)this["primary_provider"];
                else
                    return 0;
            }
            set
            {
                this["primary_provider"] = value;
            }
        }
    }

    public class insurance_company_groups_filters : DataObject
    {
        /// <summary>
        /// The default constructor for the insurance_company_groups_filters object, creates a new
        /// instance of this object without loading a default record.
        /// </summary>
        public insurance_company_groups_filters()
            : base("icg_id", "insurance_company_groups_filters")
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        /// <summary>
        /// Constructor for the insurance_company_groups_filters object, creates a new
        /// instance of this object and loads a record using the passed ID.
        /// </summary>
        public insurance_company_groups_filters(int ID)
            : base("icg_id", "insurance_company_groups_filters", ID)
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }

        /// <summary>
        /// Constructor for the insurance_company_groups_filters object, creates a new
        /// instance of this object and loads a record using the passed datarow.
        /// </summary>
        public insurance_company_groups_filters(DataRow toLoad)
            : base("icg_id", "insurance_company_groups_filters")
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
            Load(toLoad);
        }
        private void thisRecordChanged(object sender, RecordChangedEventArgs e) { }

        /// <summary>
        /// Get/Set for the int icg_id. Throws DataObjectException if the data is null.
        /// </summary>
        public int icg_id
        {
            get
            {
                if (this["icg_id"] is int)
                    return (int)this["icg_id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - icg_id)");
            }
            set
            {
                this["icg_id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string filter_text. Returns "" if the data is null.
        /// </summary>
        public string filter_text
        {
            get
            {
                if (this["filter_text"] is string)
                    return (string)this["filter_text"];
                else
                    return "";
            }
            set
            {
                this["filter_text"] = value;
            }
        }
    }





    public partial class company_contact_info : DataObject
    {
        company _linkedCompany;

        /// <summary>
        /// The default constructor for the company_contact_info object, creates a new
        /// instance of this object without loading a default record.
        /// </summary>
        public company_contact_info()
            : base("order_id", "company_contact_info")
        {
            PrimaryKeys.Add("company_id");
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        /// <summary>
        /// Constructor for the company_contact_info object, creates a new
        /// instance of this object and loads a record using the passed ID.
        /// </summary>
        public company_contact_info(int ID)
            : base("order_id", "company_contact_info", ID)
        {
            PrimaryKeys.Add("company_id");
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        private void thisRecordChanged(object sender, RecordChangedEventArgs e) {
            _linkedCompany = null;
        }

        public company LinkedCompany
        {
            get
            {
                if (this["company_id"] is int)
                {
                    if (_linkedCompany == null)
                    {
                        _linkedCompany = new company(company_id);
                    }

                    return _linkedCompany;
                    
                }
                else
                    throw new Exception("Relationship is not initialized (Company_contact_info - company)");
            }
        }

        /// <summary>
        /// Get/Set for the int company_id. Throws DataObjectException if the data is null.
        /// </summary>
        public int order_id
        {
            get
            {
                if (this["order_id"] is int)
                    return (int)this["order_id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - order_id)");
            }
            set
            {
                this["order_id"] = value;
            }
        }

        /// <summary>
        /// Get/Set for the int company_id. Throws DataObjectException if the data is null.
        /// </summary>
        public int company_id
        {
            get
            {
                if (this["company_id"] is int)
                    return (int)this["company_id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - company_id)");
            }
            set
            {
                this["company_id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string address. Returns "" if the data is null.
        /// </summary>
        public string address
        {
            get
            {
                if (this["address"] is string)
                    return (string)this["address"];
                else
                    return "";
            }
            set
            {
                this["address"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string address2. Returns "" if the data is null.
        /// </summary>
        public string address2
        {
            get
            {
                if (this["address2"] is string)
                    return (string)this["address2"];
                else
                    return "";
            }
            set
            {
                this["address2"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string city. Returns "" if the data is null.
        /// </summary>
        public string city
        {
            get
            {
                if (this["city"] is string)
                    return (string)this["city"];
                else
                    return "";
            }
            set
            {
                this["city"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string state. Returns "" if the data is null.
        /// </summary>
        public string state
        {
            get
            {
                if (this["state"] is string)
                    return (string)this["state"];
                else
                    return "";
            }
            set
            {
                this["state"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string zip. Returns "" if the data is null.
        /// </summary>
        public string zip
        {
            get
            {
                if (this["zip"] is string)
                    return (string)this["zip"];
                else
                    return "";
            }
            set
            {
                this["zip"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string phone. Returns "" if the data is null.
        /// </summary>
        public string phone
        {
            get
            {
                if (this["phone"] is string)
                    return (string)this["phone"];
                else
                    return "";
            }
            set
            {
                this["phone"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string email. Returns "" if the data is null.
        /// </summary>
        public string email
        {
            get
            {
                if (this["email"] is string)
                    return (string)this["email"];
                else
                    return "";
            }
            set
            {
                this["email"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string extension. Returns "" if the data is null.
        /// </summary>
        public string extension
        {
            get
            {
                if (this["extension"] is string)
                    return (string)this["extension"];
                else
                    return "";
            }
            set
            {
                this["extension"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string notes. Returns "" if the data is null.
        /// </summary>
        public string notes
        {
            get
            {
                if (this["notes"] is string)
                    return (string)this["notes"];
                else
                    return "";
            }
            set
            {
                this["notes"] = value;
            }
        }
    }

    public class claim_change_log : DataObject
    {
        /// <summary>
        /// The default constructor for the claim_change_log object, creates a new
        /// instance of this object without loading a default record.
        /// </summary>
        public claim_change_log()
            : base("id", "claim_change_log")
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        /// <summary>
        /// Constructor for the claim_change_log object, creates a new
        /// instance of this object and loads a record using the passed ID.
        /// </summary>
        public claim_change_log(int ID)
            : base("id", "claim_change_log", ID)
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        private void thisRecordChanged(object sender, RecordChangedEventArgs e) { }

        /// <summary>
        /// Get/Set for the int id. Throws DataObjectException if the data is null.
        /// </summary>
        public int id
        {
            get
            {
                if (this["id"] is int)
                    return (int)this["id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - id)");
            }
            set
            {
                this["id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string claim_id. Returns "" if the data is null.
        /// </summary>
        public int claim_id
        {
            get
            {
                if (this["claim_id"] is int)
                    return (int)this["claim_id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - id)");
            }
            set
            {
                this["claim_id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string field_info. Returns "" if the data is null.
        /// </summary>
        public string field_info
        {
            get
            {
                if (this["field_info"] is string)
                    return (string)this["field_info"];
                else
                    return "";
            }
            set
            {
                this["field_info"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string old_data. Returns "" if the data is null.
        /// </summary>
        public string old_data
        {
            get
            {
                if (this["old_data"] is string)
                    return (string)this["old_data"];
                else
                    return "";
            }
            set
            {
                this["old_data"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string new_data. Returns "" if the data is null.
        /// </summary>
        public string new_data
        {
            get
            {
                if (this["new_data"] is string)
                    return (string)this["new_data"];
                else
                    return "";
            }
            set
            {
                this["new_data"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the datetime change_date. Throws DataObjectException if the data is null.
        /// </summary>
        public DateTime? change_date
        {
            get
            {
                return GetDateTimeNullable(this["change_date"], false);
                
            }
            set
            {
                this["change_date"] = DateTimeDBNull(value);
            }
        }
    }

    public class splitter_codes : DataObject
    {
        /// <summary>
        /// The default constructor for the splitter_codes object, creates a new
        /// instance of this object without loading a default record.
        /// </summary>
        public splitter_codes()
            : base("Procedure_Code", "splitter_codes")
        {
            PrimaryKeys.Add("Carrier");
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        /// <summary>
        /// Constructor for the splitter_codes object, creates a new
        /// instance of this object and loads a record using the passed ID.
        /// </summary>
        public splitter_codes(int[] ID)
            : base(new string[] { "Procedure_Code", "Carrier" }, "splitter_codes", ID)
        {
            PrimaryKeys.Add("Carrier");
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        private void thisRecordChanged(object sender, RecordChangedEventArgs e) { }

        /// <summary>
        /// Get/Set for the string Procedure_Code. Returns "" if the data is null.
        /// </summary>
        public string Procedure_Code
        {
            get
            {
                if (this["Procedure_Code"] is string)
                    return (string)this["Procedure_Code"];
                else
                    return "";
            }
            set
            {
                this["Procedure_Code"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string Carrier. Returns "" if the data is null.
        /// </summary>
        public string Carrier
        {
            get
            {
                if (this["Carrier"] is string)
                    return (string)this["Carrier"];
                else
                    return "";
            }
            set
            {
                this["Carrier"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int Priority. Throws DataObjectException if the data is null.
        /// </summary>
        public int Priority
        {
            get
            {
                if (this["Priority"] is int)
                    return (int)this["Priority"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - Priority)");
            }
            set
            {
                this["Priority"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int Split_Rule. Throws DataObjectException if the data is null.
        /// </summary>
        public int Split_Rule
        {
            get
            {
                if (this["Split_Rule"] is int)
                    return (int)this["Split_Rule"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - Split_Rule)");
            }
            set
            {
                this["Split_Rule"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string Notes. Returns "" if the data is null.
        /// </summary>
        public string Notes
        {
            get
            {
                if (this["Notes"] is string)
                    return (string)this["Notes"];
                else
                    return "";
            }
            set
            {
                this["Notes"] = value;
            }
        }
    }

    public class splitter_rules : DataObject
    {
        /// <summary>
        /// The default constructor for the splitter_rules object, creates a new
        /// instance of this object without loading a default record.
        /// </summary>
        public splitter_rules()
            : base("Split_Rule", "splitter_rules")
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        /// <summary>
        /// Constructor for the splitter_rules object, creates a new
        /// instance of this object and loads a record using the passed ID.
        /// </summary>
        public splitter_rules(int ID)
            : base("Split_Rule", "splitter_rules", ID)
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        private void thisRecordChanged(object sender, RecordChangedEventArgs e) { }

        /// <summary>
        /// Get/Set for the int Split_Rule. Throws DataObjectException if the data is null.
        /// </summary>
        public int Split_Rule
        {
            get
            {
                if (this["Split_Rule"] is int)
                    return (int)this["Split_Rule"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - Split_Rule)");
            }
            set
            {
                this["Split_Rule"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the char Rule_Type. Throws DataObjectException if the data is null.
        /// </summary>
        public char Rule_Type
        {
            get
            {
                if (this["Rule_Type"] is char)
                    return (char)this["Rule_Type"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (char - Rule_Type)");
            }
            set
            {
                this["Rule_Type"] = value;
            }
        }
    }

    public class splitter_rule_details : DataObject
    {
        /// <summary>
        /// The default constructor for the splitter_rule_details object, creates a new
        /// instance of this object without loading a default record.
        /// </summary>
        public splitter_rule_details()
            : base("split_rule", "splitter_rule_details")
        {
            PrimaryKeys.Add("Procedure_Code");
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        /// <summary>
        /// Constructor for the splitter_rule_details object, creates a new
        /// instance of this object and loads a record using the passed ID.
        /// </summary>
        public splitter_rule_details(int[] ID)
            : base(new string[] { "split_rule", "Procedure_Code" }, "splitter_rule_details", ID)
        {
            PrimaryKeys.Add("Procedure_Code");
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        private void thisRecordChanged(object sender, RecordChangedEventArgs e) { }

        /// <summary>
        /// Get/Set for the int Split_Rule. Throws DataObjectException if the data is null.
        /// </summary>
        public int Split_Rule
        {
            get
            {
                if (this["Split_Rule"] is int)
                    return (int)this["Split_Rule"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - Split_Rule)");
            }
            set
            {
                this["Split_Rule"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string Procedure_Code. Returns "" if the data is null.
        /// </summary>
        public string Procedure_Code
        {
            get
            {
                if (this["Procedure_Code"] is string)
                    return (string)this["Procedure_Code"];
                else
                    return "";
            }
            set
            {
                this["Procedure_Code"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int Priority. Throws DataObjectException if the data is null.
        /// </summary>
        public int Priority
        {
            get
            {
                if (this["Priority"] is int)
                    return (int)this["Priority"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - Priority)");
            }
            set
            {
                this["Priority"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int Rule_value. Throws DataObjectException if the data is null.
        /// </summary>
        public int Rule_value
        {
            get
            {
                if (this["Rule_value"] is int)
                    return (int)this["Rule_value"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - Rule_value)");
            }
            set
            {
                this["Rule_value"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string Description. Returns "" if the data is null.
        /// </summary>
        public string Description
        {
            get
            {
                if (this["Description"] is string)
                    return (string)this["Description"];
                else
                    return "";
            }
            set
            { this["Description"] = value; }
        }
    }

    public class batch_claim_list : DataObject
    {
        /// <summary>
        /// The default constructor for the batch_claim_list object, creates a new
        /// instance of this object without loading a default record.
        /// </summary>
        public batch_claim_list()
            : base("batch_id", "batch_claim_list")
        {
            PrimaryKeys.Add("claim_id");
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        /// <summary>
        /// Constructor for the batch_claim_list object, creates a new
        /// instance of this object and loads a record using the passed ID.
        /// </summary>
        public batch_claim_list(int[] ID)
            : base(new string[] { "batch_id", "claim_id" }, "batch_claim_list", ID)
        {
            PrimaryKeys.Add("claim_id");
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        private void thisRecordChanged(object sender, RecordChangedEventArgs e) { }

        /// <summary>
        /// Get/Set for the int batch_id. Throws DataObjectException if the data is null.
        /// </summary>
        public int batch_id
        {
            get
            {
                if (this["batch_id"] is int)
                    return (int)this["batch_id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - batch_id)");
            }
            set
            {
                this["batch_id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int claim_id. Throws DataObjectException if the data is null.
        /// </summary>
        public int claim_id
        {
            get
            {
                if (this["claim_id"] is int)
                    return (int)this["claim_id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - claim_id)");
            }
            set
            {
                this["claim_id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int still_in_batch. Throws DataObjectException if the data is null.
        /// </summary>
        public bool still_in_batch
        {
            get
            {
                if (this["still_in_batch"] is bool)
                    return (bool)this["still_in_batch"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (bool - still_in_batch)");
            }
            set
            {
                this["still_in_batch"] = value;
            }
        }
    }

    public partial class claim_batch : DataObject
    {
        /// <summary>
        /// The default constructor for the claim_batch object, creates a new
        /// instance of this object without loading a default record.
        /// </summary>
        public claim_batch()
            : base("id", "claim_batch")
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        /// <summary>
        /// Constructor for the claim_batch object, creates a new
        /// instance of this object and loads a record using the passed ID.
        /// </summary>
        public claim_batch(int ID)
            : base("id", "claim_batch", ID)
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        private void thisRecordChanged(object sender, RecordChangedEventArgs e) { }

        /// <summary>
        /// Get/Set for the int batch_id. Throws DataObjectException if the data is null.
        /// </summary>
        public int id
        {
            get
            {
                if (this["id"] is int)
                    return (int)this["id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - batch_id)");
            }
            set
            {
                this["id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the datetime batch_date. Throws DataObjectException if the data is null.
        /// </summary>
        public DateTime batch_date
        {
            get
            {
                return GetDateTime(this["batch_date"]);
            }
            set
            {
                this["batch_date"] = value;
            }
        }
        

        /// <summary>
        /// Get/Set for the int source. UNIQUE: 0 = local, 1 = outside
        /// </summary>
        public int source
        {
            get
            {
                if (this["source"] is int)
                    return (int)this["source"];
                else
                    return 0;
            }
            set
            {
                this["source"] = value;
            }
        }

        /// <summary>
        /// Get/Set for the string handling. CUSTOM: Returns "NO INFO" if the data is null or empty.
        /// Valid values: NO INFO, Standard Paper Batch, Apex Batch, Resend Daily Summary, Mercury Batch
        /// </summary>
        public string batch_info
        {
            get
            {
                
                string defaultValue = "NO INFO";

                if (this["batch_info"] is string)
                    if ((string)this["batch_info"] == "")
                        return defaultValue;
                    else
                        return (string)this["batch_info"];
                else
                    return defaultValue;
            }
            set
            {
                this["batch_info"] = value;
            }
        }

        /// <summary>
        /// Get/Set for the string server_name. CUSTOM: Returns "" if the data is null or empty.
        /// </summary>
        public string server_name
        {
            get
            {
                string defaultValue = "";

                if (this["server_name"] is string)
                    return (string)this["server_name"];
                else
                    return defaultValue;
            }
            set
            {
                this["server_name"] = value;
            }
        }
    }

    public partial class bad_payment_claims : DataObject
    {
        /// <summary>
        /// The default constructor for the bad_payment_claims object, creates a new
        /// instance of this object without loading a default record.
        /// </summary>
        public bad_payment_claims()
            : base("claimid", "bad_payment_claims")
        {
            PrimaryKeys.Add("claimdb");
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        /// <summary>
        /// Constructor for the bad_payment_claims object, creates a new
        /// instance of this object and loads a record using the passed ID.
        /// </summary>
        public bad_payment_claims(int[] ID)
            : base(new string[] { "claimid", "claimdb" }, "bad_payment_claims", ID)
        {
            PrimaryKeys.Add("claimdb");
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        private void thisRecordChanged(object sender, RecordChangedEventArgs e) { }

        public claim LinkedClaim
        {
            get
            {
                DataTable aMatch = Search("SELECT * FROM claims WHERE claimidnum = '" + claimid + 
                    "' AND claimdb = '" + claimdb + "'");
                claim toReturn;

                if (aMatch.Rows.Count > 0)
                {
                    toReturn = new claim();
                    toReturn.Load(aMatch.Rows[0]);
                    return toReturn;
                }
                else
                    return null;

                
            }
        }

        /// <summary>
        /// Get/Set for the string claimid. Returns "" if the data is null.
        /// </summary>
        public string claimid
        {
            get
            {
                if (this["claimid"] is string)
                    return (string)this["claimid"];
                else
                    return "";
            }
            set
            {
                this["claimid"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string claimdb. Returns "" if the data is null.
        /// </summary>
        public string claimdb
        {
            get
            {
                if (this["claimdb"] is string)
                    return (string)this["claimdb"];
                else
                    return "";
            }
            set
            {
                this["claimdb"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int is_verified. returns False on null
        /// </summary>
        public bool is_verified
        {
            get
            {
                if (this["is_verified"] is System.Int16)
                    return System.Convert.ToBoolean(this["is_verified"]);
                else
                    return false;
            }
            set
            {
                this["is_verified"] = System.Convert.ToInt16(value);
            }
        }
        /// <summary>
        /// Get/Set for the string notes. Returns "" if the data is null.
        /// </summary>
        public string notes
        {
            get
            {
                if (this["notes"] is string)
                    return (string)this["notes"];
                else
                    return "";
            }
            set
            {
                this["notes"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the datetime last_update. Throws DataObjectException if the data is null.
        /// </summary>
        public DateTime last_update
        {
            get
            {
                return GetDateTime(this["last_update"]);
            }
            set
            {
                this["last_update"] = value;
            }
        }

        /// <summary>
        /// Get/Set for the string notes. Returns "" if the data is null.
        /// </summary>
        public string verified_by
        {
            get
            {
                if (this["verified_by"] is string)
                    return (string)this["verified_by"];
                else
                    return "";
            }
            set
            {
                this["verified_by"] = value;
            }
        }
    }


    public partial class user : DataObject
    {
        private user_preferences _userData = null;

        /// <summary>
        /// The default constructor for the users object, creates a new
        /// instance of this object without loading a default record.
        /// </summary>
        public user()
            : base("id", "users")
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        /// <summary>
        /// Constructor for the users object, creates a new
        /// instance of this object and loads a record using the passed ID.
        /// </summary>
        public user(int ID)
            : base("id", "users", ID)
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        private void thisRecordChanged(object sender, RecordChangedEventArgs e) {
            _userData = null;
        }

        /// <summary>
        /// Get/Set for the int id. Throws DataObjectException if the data is null.
        /// </summary>
        public int id
        {
            get
            {
                if (this["id"] is int)
                    return (int)this["id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - id)");
            }
            set
            {
                this["id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string username. Returns "" if the data is null.
        /// </summary>
        public string username
        {
            get
            {
                if (this["username"] is string)
                    return (string)this["username"];
                else
                    return "";
            }
            set
            {
                this["username"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string password. Returns "" if the data is null.
        /// </summary>
        public string password
        {
            get
            {
                if (this["password"] is string)
                    return (string)this["password"];
                else
                    return "";
            }
            set
            {
                this["password"] = value;
            }
        }
        
        /// <summary>
        /// Get/Set for the int is_active. CUSTOM: Acts as bool. Returns false if the value is null.
        /// </summary>
        public bool is_active
        {
            get
            {
                if (this["is_active"] is Int16)
                    return Convert.ToBoolean((Int16)this["is_active"]);
                else
                    return false;
            }
            set
            {
                this["is_active"] = System.Convert.ToInt16(value);
            }
        }

        /// <summary>
        /// Get/Set for the int is_admin. CUSTOM: Acts as bool. Returns false if the value is null.
        /// </summary>
        public bool is_admin
        {
            get
            {
                if (this["is_admin"] is Int16)
                    return Convert.ToBoolean((Int16)this["is_admin"]);
                else
                    return false;
            }
            set
            {
                this["is_admin"] = System.Convert.ToInt16(value);
            }
        }
    }

    public class clinic : DataObject
    {
        /// <summary>
        /// The default constructor for the clinics object, creates a new
        /// instance of this object without loading a default record.
        /// </summary>
        public clinic()
            : base("id", "clinics")
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        /// <summary>
        /// Constructor for the clinics object, creates a new
        /// instance of this object and loads a record using the passed ID.
        /// </summary>
        public clinic(int ID)
            : base("id", "clinics", ID)
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        private void thisRecordChanged(object sender, RecordChangedEventArgs e) { }

        public override string ToString()
        {
            return name;
        }

        /// <summary>
        /// Get/Set for the int id. Throws DataObjectException if the data is null.
        /// </summary>
        public int id
        {
            get
            {
                if (this["id"] is int)
                    return (int)this["id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - id)");
            }
            set
            {
                this["id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string name. Returns "" if the data is null.
        /// </summary>
        public string name
        {
            get
            {
                if (this["name"] is string)
                    return (string)this["name"];
                else
                    return "";
            }
            set
            {
                this["name"] = value;
            }
        }
    }

    public class claim_sent_history : DataObject
    {
        /// <summary>
        /// The default constructor for the claim_sent_history object, creates a new
        /// instance of this object without loading a default record.
        /// </summary>
        public claim_sent_history()
            : base("id", "claim_sent_history")
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        /// <summary>
        /// Constructor for the claim_sent_history object, creates a new
        /// instance of this object and loads a record using the passed ID.
        /// </summary>
        public claim_sent_history(int ID)
            : base("id", "claim_sent_history", ID)
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        private void thisRecordChanged(object sender, RecordChangedEventArgs e) { }

        /// <summary>
        /// Get/Set for the int claim_id. Throws DataObjectException if the data is null.
        /// </summary>
        public int id
        {
            get
            {
                if (this["id"] is int)
                    return (int)this["id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - id)");
            }
            set
            {
                this["id"] = value;
            }
        }

        /// <summary>
        /// Get/Set for the int claim_id. Throws DataObjectException if the data is null.
        /// </summary>
        public int claim_id
        {
            get
            {
                if (this["claim_id"] is int)
                    return (int)this["claim_id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - claim_id)");
            }
            set
            {
                this["claim_id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the DateTime sent_date. Throws DataObjectException if the data is null.
        /// </summary>
        public DateTime sent_date
        {
            get
            {
                return GetDateTime(this["sent_date"]);
            }
            set
            {
                this["sent_date"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int send_type. Throws DataObjectException if the data is null.
        /// </summary>
        public clsClaimEnums.SentMethods send_type
        {
            get
            {
                if (this["send_type"] is int)
                    return (clsClaimEnums.SentMethods)this["send_type"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - send_type)");
            }
            set
            {
                this["send_type"] = (int) value;
            }
        }
        /// <summary>
        /// Get/Set for the int is_resend. Throws DataObjectException if the data is null.
        /// </summary>
        public bool is_resend
        {
            get
            {
                if (this["is_resend"] is Int16)
                    return Convert.ToBoolean((Int16)this["is_resend"]);
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - is_resend)");
            }
            set
            {
                this["is_resend"] = Convert.ToInt16(value);
            }
        }
    }



    public partial class claim_status : DataObject
    {
        /// <summary>
        /// The default constructor for the claimstatus object, creates a new
        /// instance of this object without loading a default record.
        /// </summary>
        public claim_status()
            : base("id", "claimstatus")
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        /// <summary>
        /// Constructor for the claimstatus object, creates a new
        /// instance of this object and loads a record using the passed ID.
        /// </summary>
        public claim_status(int ID)
            : base("id", "claimstatus", ID)
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        private void thisRecordChanged(object sender, RecordChangedEventArgs e) { }

        /// <summary>
        /// Get/Set for the int id. Throws DataObjectException if the data is null.
        /// </summary>
        public int id
        {
            get
            {
                if (this["id"] is int)
                    return (int)this["id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - id)");
            }
            set
            {
                this["id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string name. Returns "" if the data is null.
        /// </summary>
        public string name
        {
            get
            {
                if (this["name"] is string)
                    return (string)this["name"];
                else
                    return "";
            }
            set
            {
                this["name"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string description. Returns "" if the data is null.
        /// </summary>
        public string description
        {
            get
            {
                if (this["description"] is string)
                    return (string)this["description"];
                else
                    return "";
            }
            set
            {
                this["description"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int order. Throws DataObjectException if the data is null.
        /// </summary>
        public int orderid
        {
            get
            {
                if (this["orderid"] is int)
                    return (int)this["orderid"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - order)");
            }
            set
            {
                this["orderid"] = value;
            }
        }

        /// <summary>
        /// Get/Set for the int user_visible. Throws DataObjectException if the data is null.
        /// </summary>
        public bool user_visible
        {
            get
            {
                if (this["user_visible"] is Int16)
                    return Convert.ToBoolean((Int16)this["user_visible"]);
                else
                    return true;
            }
            set
            {
                this["user_visible"] = Convert.ToInt16(value);
            }
        }
    }

    public partial class user_preferences : DataObject
    {
        /// <summary>
        /// The default constructor for the user_preferences object, creates a new
        /// instance of this object without loading a default record.
        /// </summary>
        public user_preferences()
            : base("user_id", "user_preferences")
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        /// <summary>
        /// Constructor for the user_preferences object, creates a new
        /// instance of this object and loads a record using the passed ID.
        /// </summary>
        public user_preferences(int ID)
            : base("user_id", "user_preferences", ID)
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        private void thisRecordChanged(object sender, RecordChangedEventArgs e) { }

        /// <summary>
        /// Get/Set for the int user_id. Throws DataObjectException if the data is null.
        /// </summary>
        public int user_id
        {
            get
            {
                if (this["user_id"] is int)
                    return (int)this["user_id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - user_id)");
            }
            set
            {
                this["user_id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int search_form_sent_date. Throws DataObjectException if the data is null.
        /// </summary>
        public int search_form_sent_date
        {
            get
            {
                if (this["search_form_sent_date"] is int)
                    return (int)this["search_form_sent_date"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - search_form_sent_date)");
            }
            set
            {
                this["search_form_sent_date"] = value;
            }
        }

        /// <summary>
        /// Get/Set for the int open_search_form. CUSTOM: Acts as bool. Returns false if the value is null.
        /// </summary>
        public bool open_search_form
        {
            get
            {
                if (this["open_search_form"] is Int16)
                    return Convert.ToBoolean((Int16)this["open_search_form"]);
                else
                    return false;
            }
            set
            {
                this["open_search_form"] = System.Convert.ToInt16(value);
            }
        }
        /// <summary>
        /// Get/Set for the int open_eclaims_form. CUSTOM: Acts as bool. Returns false if the value is null.
        /// </summary>
        public bool open_eclaims_form
        {
            get
            {
                if (this["open_eclaims_form"] is Int16)
                    return Convert.ToBoolean((Int16)this["open_eclaims_form"]);
                else
                    return false;
            }
            set
            {
                this["open_eclaims_form"] = System.Convert.ToInt16(value);
            }
        }


        /// <summary>
        /// Get/Set for the int claim_form_maximized. CUSTOM: Acts as bool. Returns false if the value is null.
        /// </summary>
        public bool claim_form_maximized
        {
            get
            {
                if (this["claim_form_maximized"] is Int16)
                    return Convert.ToBoolean((Int16)this["claim_form_maximized"]);
                else
                    return false;
            }
            set
            {
                this["claim_form_maximized"] = System.Convert.ToInt16(value);
            }
        }

        /// <summary>
        /// Get/Set for the int claim_form_left. Custom: Returns -1 if the data is null.
        /// </summary>
        public int claim_form_left
        {
            get
            {
                if (this["claim_form_left"] is int)
                    return (int)this["claim_form_left"];
                else
                    return -1;
            }
            set
            {
                this["claim_form_left"] = value;
            }
        }

        /// <summary>
        /// Get/Set for the int claim_form_top. CUSTOM: Returns -1 if the data is null
        /// </summary>
        public int claim_form_top
        {
            get
            {
                if (this["claim_form_top"] is int)
                    return (int)this["claim_form_top"];
                else
                    return -1;
            }
            set
            {
                this["claim_form_top"] = value;
            }
        }

        /// <summary>
        /// Get/Set for the int claim_form_width. CUSTOM: Returns -1 if the data is null
        /// </summary>
        public int claim_form_width
        {
            get
            {
                if (this["claim_form_width"] is int)
                    return (int)this["claim_form_width"];
                else
                    return -1;
            }
            set
            {
                this["claim_form_width"] = value;
            }
        }

        /// <summary>
        /// Get/Set for the int claim_form_height. CUSTOM: Returns -1 if the data is null
        /// </summary>
        public int claim_form_height
        {
            get
            {
                if (this["claim_form_height"] is int)
                    return (int)this["claim_form_height"];
                else
                    return -1;
            }
            set
            {
                this["claim_form_height"] = value;
            }
        }

        /// <summary>
        /// Get/Set for the int show_my_claims_first. CUSTOM: Acts as bool. Returns true if the value is null.
        /// </summary>
        public bool show_my_claims_first
        {
            get
            {
                if (this["show_my_claims_first"] is Int16)
                    return Convert.ToBoolean((Int16)this["show_my_claims_first"]);
                else
                    return true;
            }
            set
            {
                this["show_my_claims_first"] = System.Convert.ToInt16(value);
            }
        }
    }

    public partial class unusual_payment_rules : DataObject
    {
        /// <summary>
        /// The default constructor for the unusual_payment_rules object, creates a new
        /// instance of this object without loading a default record.
        /// </summary>
        public unusual_payment_rules()
            : base("id", "unusual_payment_rules")
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        /// <summary>
        /// Constructor for the unusual_payment_rules object, creates a new
        /// instance of this object and loads a record using the passed ID.
        /// </summary>
        public unusual_payment_rules(int ID)
            : base("id", "unusual_payment_rules", ID)
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        private void thisRecordChanged(object sender, RecordChangedEventArgs e) { }

        /// <summary>
        /// Get/Set for the int id. Throws DataObjectException if the data is null.
        /// </summary>
        public int id
        {
            get
            {
                if (this["id"] is int)
                    return (int)this["id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - id)");
            }
            set
            {
                this["id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string process_code. Returns "" if the data is null.
        /// </summary>
        public string process_code
        {
            get
            {
                if (this["process_code"] is string)
                    return (string)this["process_code"];
                else
                    return "";
            }
            set
            {
                this["process_code"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int operator_code. Throws DataObjectException if the data is null.
        /// </summary>
        public OperatorCodes operator_code
        {
            get
            {
                if (this["operator_code"] is int)
                    return (OperatorCodes)this["operator_code"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - operator_code)");
            }
            set
            {
                this["operator_code"] = (int)value;
            }
        }
        /// <summary>
        /// Get/Set for the decimal amount. Throws DataObjectException if the data is null.
        /// </summary>
        public decimal amount
        {
            get
            {
                if (this["amount"] is decimal)
                    return (decimal)this["amount"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (decimal - amount)");
            }
            set
            {
                this["amount"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int amount_type_code. Throws DataObjectException if the data is null.
        /// </summary>
        public AmountTypeCodes amount_type_code
        {
            get
            {
                if (this["amount_type_code"] is int)
                    return (AmountTypeCodes)this["amount_type_code"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - amount_type_code)");
            }
            set
            {
                this["amount_type_code"] = (AmountTypeCodes)value;
            }
        }

        public int order_id
        {
            get
            {
                if (this["order_id"] is int)
                    return (int)this["order_id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - order_id)");
            }
            set
            {
                this["order_id"] = value;
            }
        }
    }

    public partial class claim_status_history : DataObject
    {
        /// <summary>
        /// The default constructor for the claim_status_history object, creates a new
        /// instance of this object without loading a default record.
        /// </summary>
        public claim_status_history()
            : base("claim_id", "claim_status_history")
        {
            PrimaryKeys.Add("order_id");
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        /// <summary>
        /// Constructor for the claim_status_history object, creates a new
        /// instance of this object and loads a record using the passed ID.
        /// </summary>
        public claim_status_history(int[] IDs)
            : base(new string[] { "claim_id", "order_id" }, "claim_status_history", IDs)
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        private void thisRecordChanged(object sender, RecordChangedEventArgs e) { }

        /// <summary>
        /// Get/Set for the int claim_id. Throws DataObjectException if the data is null.
        /// </summary>
        public int claim_id
        {
            get
            {
                if (this["claim_id"] is int)
                    return (int)this["claim_id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - claim_id)");
            }
            set
            {
                this["claim_id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int order_id. Throws DataObjectException if the data is null.
        /// </summary>
        public int order_id
        {
            get
            {
                if (this["order_id"] is int)
                    return (int)this["order_id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - order_id)");
            }
            set
            {
                this["order_id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int user_id. Throws DataObjectException if the data is null.
        /// </summary>
        public int user_id
        {
            get
            {
                if (this["user_id"] is int)
                    return (int)this["user_id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - user_id)");
            }
            set
            {
                this["user_id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the datetime date_of_change. Throws DataObjectException if the data is null.
        /// </summary>
        public DateTime? date_of_change
        {
            get
            {
                return GetDateTimeNullable(this["date_of_change"], true);
            }
            set
            {
                if (value.HasValue)
                    this["date_of_change"] = value.Value;
                else
                    this["date_of_change"] = DBNull.Value;
            }
        }
        /// <summary>
        /// Get/Set for the int old_status_id. Throws DataObjectException if the data is null.
        /// </summary>
        public int old_status_id
        {
            get
            {
                if (this["old_status_id"] is int)
                    return (int)this["old_status_id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - old_status_id)");
            }
            set
            {
                this["old_status_id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int new_status_id. Throws DataObjectException if the data is null.
        /// </summary>
        public int new_status_id
        {
            get
            {
                if (this["new_status_id"] is int)
                    return (int)this["new_status_id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - new_status_id)");
            }
            set
            {
                this["new_status_id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the datetime old_revisit_date.
        /// </summary>
        public DateTime? old_revisit_date
        {
            get
            {
                return GetDateTimeNullable(this["old_revisit_date"], true);
            }
            set
            {
                if (value.HasValue)
                    this["old_revisit_date"] = value.Value;
                else
                    this["old_revisit_date"] = DBNull.Value;

            }
        }
        /// <summary>
        /// Get/Set for the datetime new_revisit_date. Throws DataObjectException if the data is null.
        /// </summary>
        public DateTime? new_revisit_date
        {
            get
            {
                return GetDateTimeNullable(this["new_revisit_date"], true);
            }
            set
            {
                if (value.HasValue)
                    this["new_revisit_date"] = value.Value;
                else
                    this["new_revisit_date"] = DBNull.Value;
            }
        }
        /// <summary>
        /// Get/Set for the int still_in_batch. Throws DataObjectException if the data is null.
        /// </summary>
        public bool as_is_flag
        {
            get
            {
                try
                {
                    return System.Convert.ToBoolean(this["as_is_flag"]);
                }
                catch
                {
                    throw new DataObjectException("Property value is empty or invalid. (bool - as_is_flag)");
                }
            }
            set
            {
                this["as_is_flag"] = value;
            }
        }
    }


    public partial class user_action_log : DataObject
    {
        /// <summary>
        /// The default constructor for the user_action_log object, creates a new
        /// instance of this object without loading a default record.
        /// </summary>
        public user_action_log()
            : base("user_id", "user_action_log")
        {
            PrimaryKeys.Add("order_id");
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        /// <summary>
        /// Constructor for the user_action_log object, creates a new
        /// instance of this object and loads a record using the passed ID.
        /// </summary>
        public user_action_log(int[] IDs)
            : base(new string[] { "user_id", "order_id" } , "user_action_log", IDs)
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        private void thisRecordChanged(object sender, RecordChangedEventArgs e) { }

        /// <summary>
        /// Get/Set for the int user_id. Throws DataObjectException if the data is null.
        /// </summary>
        public int user_id
        {
            get
            {
                if (this["user_id"] is int)
                    return (int)this["user_id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - user_id)");
            }
            set
            {
                this["user_id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int order_id. Throws DataObjectException if the data is null.
        /// </summary>
        public int order_id
        {
            get
            {
                if (this["order_id"] is int)
                    return (int)this["order_id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - order_id)");
            }
            set
            {
                this["order_id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int action_id. Throws DataObjectException if the data is null.
        /// </summary>
        public int action_id
        {
            get
            {
                if (this["action_id"] is int)
                    return (int)this["action_id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - action_id)");
            }
            set
            {
                this["action_id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the datetime action_taken_time. Throws DataObjectException if the data is null.
        /// </summary>
        public DateTime action_taken_time
        {
            get
            {
                return GetDateTime(this["action_taken_time"]);
            }
            set
            {
                this["action_taken_time"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int claim_id. Throws DataObjectException if the data is null.
        /// </summary>
        public int claim_id
        {
            get
            {
                if (this["claim_id"] is int)
                    return (int)this["claim_id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - claim_id)");
            }
            set
            {
                this["claim_id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int call_id. Throws DataObjectException if the data is null.
        /// </summary>
        public int call_id
        {
            get
            {
                if (this["call_id"] is int)
                    return (int)this["call_id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - call_id)");
            }
            set
            {
                this["call_id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string additional_notes. Returns "" if the data is null.
        /// </summary>
        public string additional_notes
        {
            get
            {
                if (this["additional_notes"] is string)
                    return (string)this["additional_notes"];
                else
                    return "";
            }
            set
            {
                this["additional_notes"] = value;
            }
        }
    }

    public class user_logging_rules : DataObject
    {
        /// <summary>
        /// The default constructor for the user_logging_rules object, creates a new
        /// instance of this object without loading a default record.
        /// </summary>
        public user_logging_rules()
            : base("id", "user_logging_rules")
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        /// <summary>
        /// Constructor for the user_logging_rules object, creates a new
        /// instance of this object and loads a record using the passed ID.
        /// </summary>
        public user_logging_rules(int ID)
            : base("id", "user_logging_rules", ID)
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        private void thisRecordChanged(object sender, RecordChangedEventArgs e) { }

        /// <summary>
        /// Get/Set for the int id. Throws DataObjectException if the data is null.
        /// </summary>
        public int id
        {
            get
            {
                if (this["id"] is int)
                    return (int)this["id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - id)");
            }
            set
            {
                this["id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string name. Returns "" if the data is null.
        /// </summary>
        public string name
        {
            get
            {
                if (this["name"] is string)
                    return (string)this["name"];
                else
                    return "";
            }
            set
            {
                this["name"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string description. Returns "" if the data is null.
        /// </summary>
        public string description
        {
            get
            {
                if (this["description"] is string)
                    return (string)this["description"];
                else
                    return "";
            }
            set
            {
                this["description"] = value;
            }
        }
    }

    public class provider_eligibility_companies : DataObject
    {
        /// <summary>
        /// The default constructor for the provider_eligibility_companies object, creates a new
        /// instance of this object without loading a default record.
        /// </summary>
        public provider_eligibility_companies()
            : base("per_id", "provider_eligibility_companies")
        {
            PrimaryKeys.Add("restriction_text");
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        /// <summary>
        /// Constructor for the provider_eligibility_companies object, creates a new
        /// instance of this object and loads a record using the passed ID.
        /// </summary>
        public provider_eligibility_companies(int ID)
            : base("per_id", "provider_eligibility_companies", ID)
        {
            PrimaryKeys.Add("restriction_text");
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        private void thisRecordChanged(object sender, RecordChangedEventArgs e) { }

        /// <summary>
        /// Get/Set for the int per_id. Throws DataObjectException if the data is null.
        /// </summary>
        public int per_id
        {
            get
            {
                if (this["per_id"] is int)
                    return (int)this["per_id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - per_id)");
            }
            set
            {
                this["per_id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string restriction_text. Returns "" if the data is null.
        /// </summary>
        public string restriction_text
        {
            get
            {
                if (this["restriction_text"] is string)
                    return (string)this["restriction_text"];
                else
                    return "";
            }
            set
            {
                this["restriction_text"] = value;
            }
        }
    }



    public partial class provider_eligibility_restrictions : DataObject
    {
        /// <summary>
        /// The default constructor for the provider_eligibility_restrictions object, creates a new
        /// instance of this object without loading a default record.
        /// </summary>
        public provider_eligibility_restrictions()
            : base("id", "provider_eligibility_restrictions")
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        /// <summary>
        /// Constructor for the provider_eligibility_restrictions object, creates a new
        /// instance of this object and loads a record using the passed ID.
        /// </summary>
        public provider_eligibility_restrictions(int ID)
            : base("id", "provider_eligibility_restrictions", ID)
        {   
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        
        /// <summary>
        /// Get/Set for the int id. Throws DataObjectException if the data is null.
        /// </summary>
        public int id
        {
            get
            {
                if (this["id"] is int)
                    return (int)this["id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - id)");
            }
            set
            {
                this["id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string provider_id. Returns "" if the data is null.
        /// </summary>
        public string provider_id
        {
            get
            {
                if (this["provider_id"] is string)
                    return (string)this["provider_id"];
                else
                    return "";
            }
            set
            {
                this["provider_id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string switch_to_provider_id. Returns "" if the data is null.
        /// </summary>
        public string switch_to_provider_id
        {
            get
            {
                if (this["switch_to_provider_id"] is string)
                    return (string)this["switch_to_provider_id"];
                else
                    return "";
            }
            set
            {
                this["switch_to_provider_id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int insurance_id. Throws DataObjectException if the data is null.
        /// </summary>
        public int insurance_id
        {
            get
            {
                if (this["insurance_id"] is int)
                    return (int)this["insurance_id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - insurance_id)");
            }
            set
            {
                this["insurance_id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the datetime start_date. Throws DataObjectException if the data is null.
        /// </summary>
        public DateTime? start_date
        {
            
            get
            {
                return GetDateTimeNullable(this["start_date"], true);
            }
            set
            {
                this["start_date"] = DateTimeDBNull(value);
            }
        }

        
        /// <summary>
        /// Get/Set for the datetime end_date. Throws DataObjectException if the data is null.
        /// </summary>
        public DateTime? end_date
        {
            get
            {
                return GetDateTimeNullable(this["end_date"], true);
            }
            set
            {
                this["end_date"] = DateTimeDBNull(value);
            }
        }

        /// <summary>
        /// Get/Set for the int insurance_group_id. Throws DataObjectException if the data is null.
        /// </summary>
        public int insurance_group_id
        {
            get
            {
                if (this["insurance_group_id"] is int)
                    return (int)this["insurance_group_id"];
                else
                    return 0;
            }
            set
            {
                this["insurance_group_id"] = value;
            }
        }

        /// <summary>
        /// Get/set for is_advanced (flags whether or not we're using an insurance_group_id)
        /// If this is true insurance_group_id should also be 0.
        /// </summary>
        public bool is_advanced
        {
            get
            {
                if (this["is_advanced"] is int)
                    return Convert.ToBoolean((int)this["is_advanced"]);
                else
                    return true;
            }
            set
            {
                this["is_advanced"] = Convert.ToInt32(value);
            }
        }

        /// <summary>
        /// Get/set for is_advanced (flags whether or not we're using an insurance_group_id)
        /// If this is true insurance_group_id should also be 0.
        /// </summary>
        public bool is_enabled
        {
            get
            {
                if (this["is_enabled"] is int)
                    return Convert.ToBoolean((int)this["is_enabled"]);
                else
                    return true;
            }
            set
            {
                this["is_enabled"] = Convert.ToInt32(value);
            }
        }
    }

    public class messages : DataObject
    {
        /// <summary>
        /// The default constructor for the messages object, creates a new
        /// instance of this object without loading a default record.
        /// </summary>
        public messages()
            : base("id", "messages")
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        /// <summary>
        /// Constructor for the messages object, creates a new
        /// instance of this object and loads a record using the passed ID.
        /// </summary>
        public messages(int ID)
            : base("id", "messages", ID)
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        private void thisRecordChanged(object sender, RecordChangedEventArgs e) { }

        /// <summary>
        /// Get/Set for the int id. Throws DataObjectException if the data is null.
        /// </summary>
        public int id
        {
            get
            {
                if (this["id"] is int)
                    return (int)this["id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - id)");
            }
            set
            {
                this["id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int from_user_id. Throws DataObjectException if the data is null.
        /// </summary>
        public int from_user_id
        {
            get
            {
                if (this["from_user_id"] is int)
                    return (int)this["from_user_id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - from_user_id)");
            }
            set
            {
                this["from_user_id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int claim_id. Throws DataObjectException if the data is null.
        /// </summary>
        public int claim_id
        {
            get
            {
                if (this["claim_id"] is int)
                    return (int)this["claim_id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - claim_id)");
            }
            set
            {
                this["claim_id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string message_text. Throws DataObjectException if the data is null.
        /// </summary>
        public string message_text
        {
            get
            {
                if (this["message_text"] is string)
                    return (string)this["message_text"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (mediumtext - message_text)");
            }
            set
            {
                this["message_text"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int message_status_id. Throws DataObjectException if the data is null.
        /// </summary>
        public int message_status_id
        {
            get
            {
                if (this["message_status_id"] is int)
                    return (int)this["message_status_id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - message_status_id)");
            }
            set
            {
                this["message_status_id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the datetime message_send_date. Throws DataObjectException if the data is null.
        /// </summary>
        public DateTime? message_send_date
        {
            get
            {
                return GetDateTimeNullable(this["message_send_date"], true);
            }
            set
            {
                this["message_send_date"] = value;
            }
        }
    }

    public class message_recipients : DataObject
    {
        /// <summary>
        /// The default constructor for the message_recipients object, creates a new
        /// instance of this object without loading a default record.
        /// </summary>
        public message_recipients()
            : base("message_id", "message_recipients")
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        /// <summary>
        /// Constructor for the message_recipients object, creates a new
        /// instance of this object and loads a record using the passed ID.
        /// </summary>
        public message_recipients(int ID)
            : base("message_id", "message_recipients", ID)
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        private void thisRecordChanged(object sender, RecordChangedEventArgs e) { }

        /// <summary>
        /// Get/Set for the int message_id. Throws DataObjectException if the data is null.
        /// </summary>
        public int message_id
        {
            get
            {
                if (this["message_id"] is int)
                    return (int)this["message_id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - message_id)");
            }
            set
            {
                this["message_id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int user_id. Throws DataObjectException if the data is null.
        /// </summary>
        public int user_id
        {
            get
            {
                if (this["user_id"] is int)
                    return (int)this["user_id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - user_id)");
            }
            set
            {
                this["user_id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int read. Throws DataObjectException if the data is null.
        /// </summary>
        public int read
        {
            get
            {
                if (this["read"] is int)
                    return (int)this["read"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - read)");
            }
            set
            {
                this["read"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the datetime read_date.Returns null.
        /// </summary>
        public DateTime? read_date
        {
            get
            {
                return GetDateTimeNullable(this["read_date"], true);
                
            }
            set
            {
                this["read_date"] = value;
            }
        }
    }

    public class apex_rules_companies : DataObject
    {
        /// <summary>
        /// The default constructor for the apex_rules_companies object, creates a new
        /// instance of this object without loading a default record.
        /// </summary>
        public apex_rules_companies()
            : base("rule_id", "apex_rules_companies")
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        /// <summary>
        /// Constructor for the apex_rules_companies object, creates a new
        /// instance of this object and loads a record using the passed ID.
        /// </summary>
        public apex_rules_companies(int ID)
            : base("rule_id", "apex_rules_companies", ID)
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        private void thisRecordChanged(object sender, RecordChangedEventArgs e) { }

        /// <summary>
        /// Get/Set for the int rule_id. Throws DataObjectException if the data is null.
        /// </summary>
        public int rule_id
        {
            get
            {
                if (this["rule_id"] is int)
                    return (int)this["rule_id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - rule_id)");
            }
            set
            {
                this["rule_id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string company_info. Returns "" if the data is null.
        /// </summary>
        public string company_info
        {
            get
            {
                if (this["company_info"] is string)
                    return (string)this["company_info"];
                else
                    return "";
            }
            set
            {
                this["company_info"] = value;
            }
        }
    }

    public class apex_rules_procedure_codes : DataObject
    {
        /// <summary>
        /// The default constructor for the apex_rules_procedure_codes object, creates a new
        /// instance of this object without loading a default record.
        /// </summary>
        public apex_rules_procedure_codes()
            : base("rule_id", "apex_rules_procedure_codes")
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        /// <summary>
        /// Constructor for the apex_rules_procedure_codes object, creates a new
        /// instance of this object and loads a record using the passed ID.
        /// </summary>
        public apex_rules_procedure_codes(int ID)
            : base("rule_id", "apex_rules_procedure_codes", ID)
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        private void thisRecordChanged(object sender, RecordChangedEventArgs e) { }

        /// <summary>
        /// Get/Set for the int rule_id. Throws DataObjectException if the data is null.
        /// </summary>
        public int rule_id
        {
            get
            {
                if (this["rule_id"] is int)
                    return (int)this["rule_id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - rule_id)");
            }
            set
            {
                this["rule_id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string procedure_code. Returns "" if the data is null.
        /// </summary>
        public string procedure_code
        {
            get
            {
                if (this["procedure_code"] is string)
                    return (string)this["procedure_code"];
                else
                    return "";
            }
            set
            {
                this["procedure_code"] = value;
            }
        }
    }

    public partial class apex_rules_rulelist : DataObject
    {
        /// <summary>
        /// The default constructor for the apex_rules_rulelist object, creates a new
        /// instance of this object without loading a default record.
        /// </summary>
        public apex_rules_rulelist()
            : base("id", "apex_rules_rulelist")
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        /// <summary>
        /// Constructor for the apex_rules_rulelist object, creates a new
        /// instance of this object and loads a record using the passed ID.
        /// </summary>
        public apex_rules_rulelist(int ID)
            : base("id", "apex_rules_rulelist", ID)
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        private void thisRecordChanged(object sender, RecordChangedEventArgs e) { }

        /// <summary>
        /// Get/Set for the int id. Throws DataObjectException if the data is null.
        /// </summary>
        public int id
        {
            get
            {
                if (this["id"] is int)
                    return (int)this["id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - id)");
            }
            set
            {
                this["id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int apply_primary. Throws DataObjectException if the data is null.
        /// </summary>
        public bool apply_primary
        {
            get
            {
                if (CommonFunctions.IsNumeric(this["apply_primary"].ToString()))
                    return Convert.ToBoolean(Convert.ToInt32(this["apply_primary"]));
                else
                    return false;
            }
            set
            {
                this["apply_primary"] = Convert.ToInt32(value);
            }
        }
        /// <summary>
        /// Get/Set for the int apply_secondary. Throws DataObjectException if the data is null.
        /// </summary>
        public bool apply_secondary
        {
            get
            {
                if (CommonFunctions.IsNumeric(this["apply_secondary"].ToString()))
                    return Convert.ToBoolean(Convert.ToInt32(this["apply_secondary"]));
                else
                    return false;
            }
            set
            {
                this["apply_secondary"] = Convert.ToInt32(value);
            }
        }
        /// <summary>
        /// Get/Set for the int apply_predeterm. Throws DataObjectException if the data is null.
        /// </summary>
        public bool apply_predeterm
        {
            get
            {
                if (CommonFunctions.IsNumeric(this["apply_predeterm"].ToString()))
                    return Convert.ToBoolean(Convert.ToInt32(this["apply_predeterm"]));
                else
                    return false;
            }
            set
            {
                this["apply_predeterm"] = Convert.ToInt32(value);
            }
        }
        /// <summary>
        /// Get/Set for the int apply_to_all. Returns false
        /// </summary>
        public bool apply_to_all
        {
            get
            {
                if (CommonFunctions.IsNumeric(this["apply_to_all"].ToString()))
                    return Convert.ToBoolean(Convert.ToInt32(this["apply_to_all"]));
                else
                    return false;
            }
            set
            {
                this["apply_to_all"] = Convert.ToInt32(value);
            }
        }
        /// <summary>
        /// Get/Set for the string name. Returns "" if the data is null.
        /// </summary>
        public string name
        {
            get
            {
                if (this["name"] is string)
                    return (string)this["name"];
                else
                    return "";
            }
            set
            {
                this["name"] = value;
            }
        }

        /// <summary>
        /// Get/Set for the string rule_text. Returns "" if the data is null.
        /// </summary>
        public string rule_text
        {
            get
            {
                if (this["rule_text"] is string)
                    return (string)this["rule_text"];
                else
                    return "";
            }
            set
            {
                this["rule_text"] = value;
            }
        }

        /// <summary>
        /// Get/Set for the int priority. Returns 10,000 if the data is null
        /// </summary>
        public int priority
        {
            get
            {
                if (this["priority"] is int)
                    return (int)this["priority"];
                else
                    return 10000;
            }
            set
            {
                this["priority"] = value;
            }
        }
    }

    public class mercury_payer_alias : DataObject
    {
        /// <summary>
        /// The default constructor for the mercury_payer_alias object, creates a new
        /// instance of this object without loading a default record.
        /// </summary>
        public mercury_payer_alias()
            : base("id", "mercury_payer_alias")
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        /// <summary>
        /// Constructor for the mercury_payer_alias object, creates a new
        /// instance of this object and loads a record using the passed ID.
        /// </summary>
        public mercury_payer_alias(int ID)
            : base("id", "mercury_payer_alias", ID)
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        private void thisRecordChanged(object sender, RecordChangedEventArgs e) { }

        /// <summary>
        /// Get/Set for the int id. Throws DataObjectException if the data is null.
        /// </summary>
        public int id
        {
            get
            {
                if (this["id"] is int)
                    return (int)this["id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - id)");
            }
            set
            {
                this["id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int mercury_id. Throws DataObjectException if the data is null.
        /// </summary>
        public int mercury_id
        {
            get
            {
                if (this["mercury_id"] is int)
                    return (int)this["mercury_id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - mercury_id)");
            }
            set
            {
                this["mercury_id"] = value;
            }
        }

        /// <summary>
        /// Get/Set for the string alias. Returns "" if the data is null.
        /// </summary>
        public string alias
        {
            get
            {
                if (this["alias"] is string)
                    return (string)this["alias"];
                else
                    return "";
            }
            set
            {
                this["alias"] = value;
            }
        }
    }

    public partial class mercury_payer_list : DataObject
    {
        /// <summary>
        /// The default constructor for the mercury_payer_list object, creates a new
        /// instance of this object without loading a default record.
        /// </summary>
        public mercury_payer_list()
            : base("id", "mercury_payer_list")
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        /// <summary>
        /// Constructor for the mercury_payer_list object, creates a new
        /// instance of this object and loads a record using the passed ID.
        /// </summary>
        public mercury_payer_list(int ID)
            : base("id", "mercury_payer_list", ID)
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        private void thisRecordChanged(object sender, RecordChangedEventArgs e) { }

        /// <summary>
        /// Get/Set for the smallint id. Throws DataObjectException if the data is null.
        /// </summary>
        public int id
        {
            get
            {
                if (this["id"] is int)
                    return (int)this["id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (smallint - id)");
            }
            set
            {
                this["id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string Name. Returns "" if the data is null.
        /// </summary>
        public string name
        {
            get
            {
                if (this["Name"] is string)
                    return (string)this["Name"];
                else
                    return "";
            }
            set
            {
                this["Name"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string payer_id. Returns "" if the data is null.
        /// </summary>
        public string payer_id
        {
            get
            {
                if (this["payer_id"] is string)
                    return (string)this["payer_id"];
                else
                    return "";
            }
            set
            {
                this["payer_id"] = value;
            }
        }
    }

    public class eligibility_data : DataObject
    {
        /// <summary>
        /// The default constructor for the eligibility_data object, creates a new
        /// instance of this object without loading a default record.
        /// </summary>
        public eligibility_data()
            : base("id", "eligibility_data")
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        /// <summary>
        /// Constructor for the eligibility_data object, creates a new
        /// instance of this object and loads a record using the passed ID.
        /// </summary>
        public eligibility_data(int ID)
            : base("id", "eligibility_data", ID)
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        private void thisRecordChanged(object sender, RecordChangedEventArgs e) { }

        /// <summary>
        /// Get/Set for the int id. Throws DataObjectException if the data is null.
        /// </summary>
        public int id
        {
            get
            {
                if (this["id"] is int)
                    return (int)this["id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - id)");
            }
            set
            {
                this["id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int dentrix_id. Throws DataObjectException if the data is null.
        /// </summary>
        public int dentrix_id
        {
            get
            {
                if (this["dentrix_id"] is int)
                    return (int)this["dentrix_id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - dentrix_id)");
            }
            set
            {
                this["dentrix_id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int dentrix_db. Throws DataObjectException if the data is null.
        /// </summary>
        public int dentrix_db
        {
            get
            {
                if (this["dentrix_db"] is int)
                    return (int)this["dentrix_db"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - dentrix_db)");
            }
            set
            {
                this["dentrix_db"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string patient_name. Returns "" if the data is null.
        /// </summary>
        public string patient_name
        {
            get
            {
                if (this["patient_name"] is string)
                    return (string)this["patient_name"];
                else
                    return "";
            }
            set
            {
                this["patient_name"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the datetime appt_date. Throws DataObjectException if the data is null.
        /// </summary>
        public DateTime appt_date
        {
            get
            {
                if (this["appt_date"] is DateTime)
                    return (DateTime)this["appt_date"];
                else
                {
                    try
                    {
                        DateTime toReturn = Convert.ToDateTime(this["appt_date"]);
                        return toReturn;
                    }
                    catch
                    {
                        throw new DataObjectException("Property value is empty or invalid. (datetime - appt_date)");
                    }
                }
            }
            set
            {
                this["appt_date"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int appt_length. Throws DataObjectException if the data is null.
        /// </summary>
        public int appt_length
        {
            get
            {
                if (this["appt_length"] is int)
                    return (int)this["appt_length"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - appt_length)");
            }
            set
            {
                this["appt_length"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string procedure_info. Returns "" if the data is null.
        /// </summary>
        public string procedure_info
        {
            get
            {
                if (this["procedure_info"] is string)
                    return (string)this["procedure_info"];
                else
                    return "";
            }
            set
            {
                this["procedure_info"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the datetime last_check. Throws DataObjectException if the data is null.
        /// </summary>
        public DateTime? last_check
        {
            get
            {
                return GetDateTimeNullable(this["last_check"], false);
            }
            set
            {
                this["last_check"] = DateTimeDBNull(value);
            }
        }
        /// <summary>
        /// Get/Set for the int last_status. Throws DataObjectException if the data is null.
        /// </summary>
        public int last_status
        {
            get
            {
                if (this["last_status"] is int)
                    return (int)this["last_status"];
                else
                    return 0;
            }
            set
            {
                this["last_status"] = value;
            }
        }
        
    }

    public partial class appointment_audit : DataObject
    {
        /// <summary>
        /// The default constructor for the appointment_audit object, creates a new
        /// instance of this object without loading a default record.
        /// </summary>
        public appointment_audit()
            : base("id", "appointment_audit")
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        /// <summary>
        /// Constructor for the appointment_audit object, creates a new
        /// instance of this object and loads a record using the passed ID.
        /// </summary>
        public appointment_audit(int ID)
            : base("id", "appointment_audit", ID)
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        private void thisRecordChanged(object sender, RecordChangedEventArgs e) { }

        /// <summary>
        /// Get/Set for the int id. Throws exception if the data is null.
        /// </summary>
        public int id
        {
            get
            {
                if (this["id"] is int)
                    return (int)this["id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - id)");
            }
            set
            {
                this["id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string USER_CHANGED. Returns "" if the data is null.
        /// </summary>
        public string USER_CHANGED
        {
            get
            {
                if (this["USER_CHANGED"] is string)
                    return (string)this["USER_CHANGED"];
                else
                    return "";
            }
            set
            {
                this["USER_CHANGED"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the DateTime DATE_CHANGED. Throws exception if the data is null.
        /// </summary>
        public DateTime DATE_CHANGED
        {
            get
            {
                if (this["DATE_CHANGED"] is DateTime)
                    return (DateTime)this["DATE_CHANGED"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (DateTime - DATE_CHANGED)");
            }
            set
            {
                this["DATE_CHANGED"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int APPTID. Returns 0 if null
        /// </summary>
        public int APPTID
        {
            get
            {
                if (this["APPTID"] is int)
                    return (int)this["APPTID"];
                else
                    return 0;
            }
            set
            {
                this["APPTID"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int APPTDB. Throws exception if the data is null.
        /// </summary>
        public int APPTDB
        {
            get
            {
                if (this["APPTDB"] is int)
                    return (int)this["APPTDB"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - APPTDB)");
            }
            set
            {
                this["APPTDB"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int PATID. Throws exception if the data is null.
        /// </summary>
        public int PATID
        {
            get
            {
                if (this["PATID"] is int)
                    return (int)this["PATID"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - PATID)");
            }
            set
            {
                this["PATID"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int PATDB. Throws exception if the data is null.
        /// </summary>
        public int PATDB
        {
            get
            {
                if (this["PATDB"] is int)
                    return (int)this["PATDB"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - PATDB)");
            }
            set
            {
                this["PATDB"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int OPID. Throws exception if the data is null.
        /// </summary>
        public int OPID
        {
            get
            {
                if (this["OPID"] is int)
                    return (int)this["OPID"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - OPID)");
            }
            set
            {
                this["OPID"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int OPDB. Throws exception if the data is null.
        /// </summary>
        public int OPDB
        {
            get
            {
                if (this["OPDB"] is int)
                    return (int)this["OPDB"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - OPDB)");
            }
            set
            {
                this["OPDB"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int PRPROVID. Throws exception if the data is null.
        /// </summary>
        public int PRPROVID
        {
            get
            {
                if (this["PRPROVID"] is int)
                    return (int)this["PRPROVID"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - PRPROVID)");
            }
            set
            {
                this["PRPROVID"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int PRPROVDB. Throws exception if the data is null.
        /// </summary>
        public int PRPROVDB
        {
            get
            {
                if (this["PRPROVDB"] is int)
                    return (int)this["PRPROVDB"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - PRPROVDB)");
            }
            set
            {
                this["PRPROVDB"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int SCPROVID. Throws exception if the data is null.
        /// </summary>
        public int SCPROVID
        {
            get
            {
                if (this["SCPROVID"] is int)
                    return (int)this["SCPROVID"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - SCPROVID)");
            }
            set
            {
                this["SCPROVID"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int SCPROVDB. Throws exception if the data is null.
        /// </summary>
        public int SCPROVDB
        {
            get
            {
                if (this["SCPROVDB"] is int)
                    return (int)this["SCPROVDB"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - SCPROVDB)");
            }
            set
            {
                this["SCPROVDB"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int NEWPATADDRESSID. Throws exception if the data is null.
        /// </summary>
        public int NEWPATADDRESSID
        {
            get
            {
                if (this["NEWPATADDRESSID"] is int)
                    return (int)this["NEWPATADDRESSID"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - NEWPATADDRESSID)");
            }
            set
            {
                this["NEWPATADDRESSID"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int NEWPATADDRESSDB. Throws exception if the data is null.
        /// </summary>
        public int NEWPATADDRESSDB
        {
            get
            {
                if (this["NEWPATADDRESSDB"] is int)
                    return (int)this["NEWPATADDRESSDB"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - NEWPATADDRESSDB)");
            }
            set
            {
                this["NEWPATADDRESSDB"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the DateTime APPTDATE. Throws exception if the data is null.
        /// </summary>
        public DateTime APPTDATE
        {
            get
            {
                if (this["APPTDATE"] is DateTime)
                    return (DateTime)this["APPTDATE"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (DateTime - APPTDATE)");
            }
            set
            {
                this["APPTDATE"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int SMALLAPPTID. Throws exception if the data is null.
        /// </summary>
        public int SMALLAPPTID
        {
            get
            {
                if (this["SMALLAPPTID"] is int)
                    return (int)this["SMALLAPPTID"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - SMALLAPPTID)");
            }
            set
            {
                this["SMALLAPPTID"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int CLASS. Throws exception if the data is null.
        /// </summary>
        public int CLASS
        {
            get
            {
                if (this["CLASS"] is int)
                    return (int)this["CLASS"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - CLASS)");
            }
            set
            {
                this["CLASS"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string PATNAME. Returns "" if the data is null.
        /// </summary>
        public string PATNAME
        {
            get
            {
                if (this["PATNAME"] is string)
                    return (string)this["PATNAME"];
                else
                    return "";
            }
            set
            {
                this["PATNAME"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int APPTLEN. Throws exception if the data is null.
        /// </summary>
        public int APPTLEN
        {
            get
            {
                if (this["APPTLEN"] is int)
                    return (int)this["APPTLEN"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - APPTLEN)");
            }
            set
            {
                this["APPTLEN"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int AMOUNT. Throws exception if the data is null.
        /// </summary>
        public int AMOUNT
        {
            get
            {
                if (this["AMOUNT"] is int)
                    return (int)this["AMOUNT"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - AMOUNT)");
            }
            set
            {
                this["AMOUNT"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the DateTime BROKENDATE. Throws exception if the data is null.
        /// </summary>
        public DateTime BROKENDATE
        {
            get
            {
                if (this["BROKENDATE"] is DateTime)
                    return (DateTime)this["BROKENDATE"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (DateTime - BROKENDATE)");
            }
            set
            {
                this["BROKENDATE"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int FLAG. Throws exception if the data is null.
        /// </summary>
        public int FLAG
        {
            get
            {
                if (this["FLAG"] is int)
                    return (int)this["FLAG"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - FLAG)");
            }
            set
            {
                this["FLAG"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string PHONE. Returns "" if the data is null.
        /// </summary>
        public string PHONE
        {
            get
            {
                if (this["PHONE"] is string)
                    return (string)this["PHONE"];
                else
                    return "";
            }
            set
            {
                this["PHONE"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string APPTREASON. Returns "" if the data is null.
        /// </summary>
        public string APPTREASON
        {
            get
            {
                if (this["APPTREASON"] is string)
                    return (string)this["APPTREASON"];
                else
                    return "";
            }
            set
            {
                this["APPTREASON"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int TIME_HOUR. Throws exception if the data is null.
        /// </summary>
        public int TIME_HOUR
        {
            get
            {
                if (this["TIME_HOUR"] is int)
                    return (int)this["TIME_HOUR"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - TIME_HOUR)");
            }
            set
            {
                this["TIME_HOUR"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int TIME_MINUTE. Throws exception if the data is null.
        /// </summary>
        public int TIME_MINUTE
        {
            get
            {
                if (this["TIME_MINUTE"] is int)
                    return (int)this["TIME_MINUTE"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - TIME_MINUTE)");
            }
            set
            {
                this["TIME_MINUTE"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int STATUS. Throws exception if the data is null.
        /// </summary>
        public int STATUS
        {
            get
            {
                if (this["STATUS"] is int)
                    return (int)this["STATUS"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - STATUS)");
            }
            set
            {
                this["STATUS"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int APPTTYPE. Throws exception if the data is null.
        /// </summary>
        public int APPTTYPE
        {
            get
            {
                if (this["APPTTYPE"] is int)
                    return (int)this["APPTTYPE"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - APPTTYPE)");
            }
            set
            {
                this["APPTTYPE"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int TIMEBLOCK. Throws exception if the data is null.
        /// </summary>
        public int TIMEBLOCK
        {
            get
            {
                if (this["TIMEBLOCK"] is int)
                    return (int)this["TIMEBLOCK"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - TIMEBLOCK)");
            }
            set
            {
                this["TIMEBLOCK"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int FOLLOWUP. Throws exception if the data is null.
        /// </summary>
        public int FOLLOWUP
        {
            get
            {
                if (this["FOLLOWUP"] is int)
                    return (int)this["FOLLOWUP"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - FOLLOWUP)");
            }
            set
            {
                this["FOLLOWUP"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int APPTCLASS. Throws exception if the data is null.
        /// </summary>
        public int APPTCLASS
        {
            get
            {
                if (this["APPTCLASS"] is int)
                    return (int)this["APPTCLASS"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - APPTCLASS)");
            }
            set
            {
                this["APPTCLASS"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the DateTime CREATEDATE. Throws exception if the data is null.
        /// </summary>
        public DateTime CREATEDATE
        {
            get
            {
                if (this["CREATEDATE"] is DateTime)
                    return (DateTime)this["CREATEDATE"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (DateTime - CREATEDATE)");
            }
            set
            {
                this["CREATEDATE"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int OPTYPE. Throws exception if the data is null.
        /// </summary>
        public int OPTYPE
        {
            get
            {
                if (this["OPTYPE"] is int)
                    return (int)this["OPTYPE"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - OPTYPE)");
            }
            set
            {
                this["OPTYPE"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int PRPROVTYPE. Throws exception if the data is null.
        /// </summary>
        public int PRPROVTYPE
        {
            get
            {
                if (this["PRPROVTYPE"] is int)
                    return (int)this["PRPROVTYPE"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - PRPROVTYPE)");
            }
            set
            {
                this["PRPROVTYPE"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int SCPROVTYPE. Throws exception if the data is null.
        /// </summary>
        public int SCPROVTYPE
        {
            get
            {
                if (this["SCPROVTYPE"] is int)
                    return (int)this["SCPROVTYPE"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - SCPROVTYPE)");
            }
            set
            {
                this["SCPROVTYPE"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string PATTERN. Returns "" if the data is null.
        /// </summary>
        public string PATTERN
        {
            get
            {
                if (this["PATTERN"] is string)
                    return (string)this["PATTERN"];
                else
                    return "";
            }
            set
            {
                this["PATTERN"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string PROCCODEID. Returns "" if the data is null.
        /// </summary>
        public string PROCCODEID
        {
            get
            {
                if (this["PROCCODEID"] is string)
                    return (string)this["PROCCODEID"];
                else
                    return "";
            }
            set
            {
                this["PROCCODEID"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string PROCCODEDB. Returns "" if the data is null.
        /// </summary>
        public string PROCCODEDB
        {
            get
            {
                if (this["PROCCODEDB"] is string)
                    return (string)this["PROCCODEDB"];
                else
                    return "";
            }
            set
            {
                this["PROCCODEDB"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string PROCCODETYPE. Returns "" if the data is null.
        /// </summary>
        public string PROCCODETYPE
        {
            get
            {
                if (this["PROCCODETYPE"] is string)
                    return (string)this["PROCCODETYPE"];
                else
                    return "";
            }
            set
            {
                this["PROCCODETYPE"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string PREFNAME. Returns "" if the data is null.
        /// </summary>
        public string PREFNAME
        {
            get
            {
                if (this["PREFNAME"] is string)
                    return (string)this["PREFNAME"];
                else
                    return "";
            }
            set
            {
                this["PREFNAME"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string TITLE. Returns "" if the data is null.
        /// </summary>
        public string TITLE
        {
            get
            {
                if (this["TITLE"] is string)
                    return (string)this["TITLE"];
                else
                    return "";
            }
            set
            {
                this["TITLE"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string CHART. Returns "" if the data is null.
        /// </summary>
        public string CHART
        {
            get
            {
                if (this["CHART"] is string)
                    return (string)this["CHART"];
                else
                    return "";
            }
            set
            {
                this["CHART"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string WKPHONE. Returns "" if the data is null.
        /// </summary>
        public string WKPHONE
        {
            get
            {
                if (this["WKPHONE"] is string)
                    return (string)this["WKPHONE"];
                else
                    return "";
            }
            set
            {
                this["WKPHONE"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int HASAPPTNOTE. Throws exception if the data is null.
        /// </summary>
        public int HASAPPTNOTE
        {
            get
            {
                if (this["HASAPPTNOTE"] is int)
                    return (int)this["HASAPPTNOTE"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - HASAPPTNOTE)");
            }
            set
            {
                this["HASAPPTNOTE"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int HASMEDALERT. Throws exception if the data is null.
        /// </summary>
        public int HASMEDALERT
        {
            get
            {
                if (this["HASMEDALERT"] is int)
                    return (int)this["HASMEDALERT"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - HASMEDALERT)");
            }
            set
            {
                this["HASMEDALERT"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int PatAlert. Throws exception if the data is null.
        /// </summary>
        public int PatAlert
        {
            get
            {
                if (this["PatAlert"] is int)
                    return (int)this["PatAlert"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - PatAlert)");
            }
            set
            {
                this["PatAlert"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int OPERATORID. Throws exception if the data is null.
        /// </summary>
        public int OPERATORID
        {
            get
            {
                if (this["OPERATORID"] is int)
                    return (int)this["OPERATORID"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - OPERATORID)");
            }
            set
            {
                this["OPERATORID"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int OPERATORDB. Throws exception if the data is null.
        /// </summary>
        public int OPERATORDB
        {
            get
            {
                if (this["OPERATORDB"] is int)
                    return (int)this["OPERATORDB"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - OPERATORDB)");
            }
            set
            {
                this["OPERATORDB"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the DateTime DDB_LAST_MOD. Throws exception if the data is null.
        /// </summary>
        public DateTime DDB_LAST_MOD
        {
            get
            {
                if (this["DDB_LAST_MOD"] is DateTime)
                    return (DateTime)this["DDB_LAST_MOD"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (DateTime - DDB_LAST_MOD)");
            }
            set
            {
                this["DDB_LAST_MOD"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the DateTime LAST_VERIFIED_PRI. Throws exception if the data is null.
        /// </summary>
        public DateTime LAST_VERIFIED_PRI
        {
            get
            {
                if (this["LAST_VERIFIED_PRI"] is DateTime)
                    return (DateTime)this["LAST_VERIFIED_PRI"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (DateTime - LAST_VERIFIED_PRI)");
            }
            set
            {
                this["LAST_VERIFIED_PRI"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the DateTime LAST_VERIFIED_SEC. Throws exception if the data is null.
        /// </summary>
        public DateTime LAST_VERIFIED_SEC
        {
            get
            {
                if (this["LAST_VERIFIED_SEC"] is DateTime)
                    return (DateTime)this["LAST_VERIFIED_SEC"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (DateTime - LAST_VERIFIED_SEC)");
            }
            set
            {
                this["LAST_VERIFIED_SEC"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int n_APPTID. Throws exception if the data is null.
        /// </summary>
        public int n_APPTID
        {
            get
            {
                if (this["n_APPTID"] is int)
                    return (int)this["n_APPTID"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - n_APPTID)");
            }
            set
            {
                this["n_APPTID"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int n_APPTDB. Throws exception if the data is null.
        /// </summary>
        public int n_APPTDB
        {
            get
            {
                if (this["n_APPTDB"] is int)
                    return (int)this["n_APPTDB"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - n_APPTDB)");
            }
            set
            {
                this["n_APPTDB"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int n_PATID. Throws exception if the data is null.
        /// </summary>
        public int n_PATID
        {
            get
            {
                if (this["n_PATID"] is int)
                    return (int)this["n_PATID"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - n_PATID)");
            }
            set
            {
                this["n_PATID"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int n_PATDB. Throws exception if the data is null.
        /// </summary>
        public int n_PATDB
        {
            get
            {
                if (this["n_PATDB"] is int)
                    return (int)this["n_PATDB"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - n_PATDB)");
            }
            set
            {
                this["n_PATDB"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int n_OPID. Throws exception if the data is null.
        /// </summary>
        public int n_OPID
        {
            get
            {
                if (this["n_OPID"] is int)
                    return (int)this["n_OPID"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - n_OPID)");
            }
            set
            {
                this["n_OPID"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int n_OPDB. Throws exception if the data is null.
        /// </summary>
        public int n_OPDB
        {
            get
            {
                if (this["n_OPDB"] is int)
                    return (int)this["n_OPDB"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - n_OPDB)");
            }
            set
            {
                this["n_OPDB"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int n_PRPROVID. Throws exception if the data is null.
        /// </summary>
        public int n_PRPROVID
        {
            get
            {
                if (this["n_PRPROVID"] is int)
                    return (int)this["n_PRPROVID"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - n_PRPROVID)");
            }
            set
            {
                this["n_PRPROVID"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int n_PRPROVDB. Throws exception if the data is null.
        /// </summary>
        public int n_PRPROVDB
        {
            get
            {
                if (this["n_PRPROVDB"] is int)
                    return (int)this["n_PRPROVDB"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - n_PRPROVDB)");
            }
            set
            {
                this["n_PRPROVDB"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int n_SCPROVID. Throws exception if the data is null.
        /// </summary>
        public int n_SCPROVID
        {
            get
            {
                if (this["n_SCPROVID"] is int)
                    return (int)this["n_SCPROVID"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - n_SCPROVID)");
            }
            set
            {
                this["n_SCPROVID"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int n_SCPROVDB. Throws exception if the data is null.
        /// </summary>
        public int n_SCPROVDB
        {
            get
            {
                if (this["n_SCPROVDB"] is int)
                    return (int)this["n_SCPROVDB"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - n_SCPROVDB)");
            }
            set
            {
                this["n_SCPROVDB"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int n_NEWPATADDRESSID. Throws exception if the data is null.
        /// </summary>
        public int n_NEWPATADDRESSID
        {
            get
            {
                if (this["n_NEWPATADDRESSID"] is int)
                    return (int)this["n_NEWPATADDRESSID"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - n_NEWPATADDRESSID)");
            }
            set
            {
                this["n_NEWPATADDRESSID"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int n_NEWPATADDRESSDB. Throws exception if the data is null.
        /// </summary>
        public int n_NEWPATADDRESSDB
        {
            get
            {
                if (this["n_NEWPATADDRESSDB"] is int)
                    return (int)this["n_NEWPATADDRESSDB"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - n_NEWPATADDRESSDB)");
            }
            set
            {
                this["n_NEWPATADDRESSDB"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the DateTime n_APPTDATE. Throws exception if the data is null.
        /// </summary>
        public DateTime n_APPTDATE
        {
            get
            {
                if (this["n_APPTDATE"] is DateTime)
                    return (DateTime)this["n_APPTDATE"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (DateTime - n_APPTDATE)");
            }
            set
            {
                this["n_APPTDATE"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int n_SMALLAPPTID. Throws exception if the data is null.
        /// </summary>
        public int n_SMALLAPPTID
        {
            get
            {
                if (this["n_SMALLAPPTID"] is int)
                    return (int)this["n_SMALLAPPTID"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - n_SMALLAPPTID)");
            }
            set
            {
                this["n_SMALLAPPTID"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int n_CLASS. Throws exception if the data is null.
        /// </summary>
        public int n_CLASS
        {
            get
            {
                if (this["n_CLASS"] is int)
                    return (int)this["n_CLASS"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - n_CLASS)");
            }
            set
            {
                this["n_CLASS"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string n_PATNAME. Returns "" if the data is null.
        /// </summary>
        public string n_PATNAME
        {
            get
            {
                if (this["n_PATNAME"] is string)
                    return (string)this["n_PATNAME"];
                else
                    return "";
            }
            set
            {
                this["n_PATNAME"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int n_APPTLEN. Throws exception if the data is null.
        /// </summary>
        public int n_APPTLEN
        {
            get
            {
                if (this["n_APPTLEN"] is int)
                    return (int)this["n_APPTLEN"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - n_APPTLEN)");
            }
            set
            {
                this["n_APPTLEN"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int n_AMOUNT. Throws exception if the data is null.
        /// </summary>
        public int n_AMOUNT
        {
            get
            {
                if (this["n_AMOUNT"] is int)
                    return (int)this["n_AMOUNT"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - n_AMOUNT)");
            }
            set
            {
                this["n_AMOUNT"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the DateTime n_BROKENDATE. Throws exception if the data is null.
        /// </summary>
        public DateTime n_BROKENDATE
        {
            get
            {
                if (this["n_BROKENDATE"] is DateTime)
                    return (DateTime)this["n_BROKENDATE"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (DateTime - n_BROKENDATE)");
            }
            set
            {
                this["n_BROKENDATE"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int n_FLAG. Throws exception if the data is null.
        /// </summary>
        public int n_FLAG
        {
            get
            {
                if (this["n_FLAG"] is int)
                    return (int)this["n_FLAG"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - n_FLAG)");
            }
            set
            {
                this["n_FLAG"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string n_PHONE. Returns "" if the data is null.
        /// </summary>
        public string n_PHONE
        {
            get
            {
                if (this["n_PHONE"] is string)
                    return (string)this["n_PHONE"];
                else
                    return "";
            }
            set
            {
                this["n_PHONE"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string n_APPTREASON. Returns "" if the data is null.
        /// </summary>
        public string n_APPTREASON
        {
            get
            {
                if (this["n_APPTREASON"] is string)
                    return (string)this["n_APPTREASON"];
                else
                    return "";
            }
            set
            {
                this["n_APPTREASON"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int n_TIME_HOUR. Throws exception if the data is null.
        /// </summary>
        public int n_TIME_HOUR
        {
            get
            {
                if (this["n_TIME_HOUR"] is int)
                    return (int)this["n_TIME_HOUR"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - n_TIME_HOUR)");
            }
            set
            {
                this["n_TIME_HOUR"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int n_TIME_MINUTE. Throws exception if the data is null.
        /// </summary>
        public int n_TIME_MINUTE
        {
            get
            {
                if (this["n_TIME_MINUTE"] is int)
                    return (int)this["n_TIME_MINUTE"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - n_TIME_MINUTE)");
            }
            set
            {
                this["n_TIME_MINUTE"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int n_STATUS. Throws exception if the data is null.
        /// </summary>
        public int n_STATUS
        {
            get
            {
                if (this["n_STATUS"] is int)
                    return (int)this["n_STATUS"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - n_STATUS)");
            }
            set
            {
                this["n_STATUS"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int n_APPTTYPE. Throws exception if the data is null.
        /// </summary>
        public int n_APPTTYPE
        {
            get
            {
                if (this["n_APPTTYPE"] is int)
                    return (int)this["n_APPTTYPE"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - n_APPTTYPE)");
            }
            set
            {
                this["n_APPTTYPE"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int n_TIMEBLOCK. Throws exception if the data is null.
        /// </summary>
        public int n_TIMEBLOCK
        {
            get
            {
                if (this["n_TIMEBLOCK"] is int)
                    return (int)this["n_TIMEBLOCK"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - n_TIMEBLOCK)");
            }
            set
            {
                this["n_TIMEBLOCK"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int n_FOLLOWUP. Throws exception if the data is null.
        /// </summary>
        public int n_FOLLOWUP
        {
            get
            {
                if (this["n_FOLLOWUP"] is int)
                    return (int)this["n_FOLLOWUP"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - n_FOLLOWUP)");
            }
            set
            {
                this["n_FOLLOWUP"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int n_APPTCLASS. Throws exception if the data is null.
        /// </summary>
        public int n_APPTCLASS
        {
            get
            {
                if (this["n_APPTCLASS"] is int)
                    return (int)this["n_APPTCLASS"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - n_APPTCLASS)");
            }
            set
            {
                this["n_APPTCLASS"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the DateTime n_CREATEDATE. Throws exception if the data is null.
        /// </summary>
        public DateTime n_CREATEDATE
        {
            get
            {
                if (this["n_CREATEDATE"] is DateTime)
                    return (DateTime)this["n_CREATEDATE"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (DateTime - n_CREATEDATE)");
            }
            set
            {
                this["n_CREATEDATE"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int n_OPTYPE. Throws exception if the data is null.
        /// </summary>
        public int n_OPTYPE
        {
            get
            {
                if (this["n_OPTYPE"] is int)
                    return (int)this["n_OPTYPE"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - n_OPTYPE)");
            }
            set
            {
                this["n_OPTYPE"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int n_PRPROVTYPE. Throws exception if the data is null.
        /// </summary>
        public int n_PRPROVTYPE
        {
            get
            {
                if (this["n_PRPROVTYPE"] is int)
                    return (int)this["n_PRPROVTYPE"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - n_PRPROVTYPE)");
            }
            set
            {
                this["n_PRPROVTYPE"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int n_SCPROVTYPE. Throws exception if the data is null.
        /// </summary>
        public int n_SCPROVTYPE
        {
            get
            {
                if (this["n_SCPROVTYPE"] is int)
                    return (int)this["n_SCPROVTYPE"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - n_SCPROVTYPE)");
            }
            set
            {
                this["n_SCPROVTYPE"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string n_PATTERN. Returns "" if the data is null.
        /// </summary>
        public string n_PATTERN
        {
            get
            {
                if (this["n_PATTERN"] is string)
                    return (string)this["n_PATTERN"];
                else
                    return "";
            }
            set
            {
                this["n_PATTERN"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string n_PROCCODEID. Returns "" if the data is null.
        /// </summary>
        public string n_PROCCODEID
        {
            get
            {
                if (this["n_PROCCODEID"] is string)
                    return (string)this["n_PROCCODEID"];
                else
                    return "";
            }
            set
            {
                this["n_PROCCODEID"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string n_PROCCODEDB. Returns "" if the data is null.
        /// </summary>
        public string n_PROCCODEDB
        {
            get
            {
                if (this["n_PROCCODEDB"] is string)
                    return (string)this["n_PROCCODEDB"];
                else
                    return "";
            }
            set
            {
                this["n_PROCCODEDB"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string n_PROCCODETYPE. Returns "" if the data is null.
        /// </summary>
        public string n_PROCCODETYPE
        {
            get
            {
                if (this["n_PROCCODETYPE"] is string)
                    return (string)this["n_PROCCODETYPE"];
                else
                    return "";
            }
            set
            {
                this["n_PROCCODETYPE"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string n_PREFNAME. Returns "" if the data is null.
        /// </summary>
        public string n_PREFNAME
        {
            get
            {
                if (this["n_PREFNAME"] is string)
                    return (string)this["n_PREFNAME"];
                else
                    return "";
            }
            set
            {
                this["n_PREFNAME"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string n_TITLE. Returns "" if the data is null.
        /// </summary>
        public string n_TITLE
        {
            get
            {
                if (this["n_TITLE"] is string)
                    return (string)this["n_TITLE"];
                else
                    return "";
            }
            set
            {
                this["n_TITLE"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string n_CHART. Returns "" if the data is null.
        /// </summary>
        public string n_CHART
        {
            get
            {
                if (this["n_CHART"] is string)
                    return (string)this["n_CHART"];
                else
                    return "";
            }
            set
            {
                this["n_CHART"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string n_WKPHONE. Returns "" if the data is null.
        /// </summary>
        public string n_WKPHONE
        {
            get
            {
                if (this["n_WKPHONE"] is string)
                    return (string)this["n_WKPHONE"];
                else
                    return "";
            }
            set
            {
                this["n_WKPHONE"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int n_HASAPPTNOTE. Throws exception if the data is null.
        /// </summary>
        public int n_HASAPPTNOTE
        {
            get
            {
                if (this["n_HASAPPTNOTE"] is int)
                    return (int)this["n_HASAPPTNOTE"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - n_HASAPPTNOTE)");
            }
            set
            {
                this["n_HASAPPTNOTE"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int n_HASMEDALERT. Throws exception if the data is null.
        /// </summary>
        public int n_HASMEDALERT
        {
            get
            {
                if (this["n_HASMEDALERT"] is int)
                    return (int)this["n_HASMEDALERT"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - n_HASMEDALERT)");
            }
            set
            {
                this["n_HASMEDALERT"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int n_PatAlert. Throws exception if the data is null.
        /// </summary>
        public int n_PatAlert
        {
            get
            {
                if (this["n_PatAlert"] is int)
                    return (int)this["n_PatAlert"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - n_PatAlert)");
            }
            set
            {
                this["n_PatAlert"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int n_OPERATORID. Throws exception if the data is null.
        /// </summary>
        public int n_OPERATORID
        {
            get
            {
                if (this["n_OPERATORID"] is int)
                    return (int)this["n_OPERATORID"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - n_OPERATORID)");
            }
            set
            {
                this["n_OPERATORID"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int n_OPERATORDB. Throws exception if the data is null.
        /// </summary>
        public int n_OPERATORDB
        {
            get
            {
                if (this["n_OPERATORDB"] is int)
                    return (int)this["n_OPERATORDB"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - n_OPERATORDB)");
            }
            set
            {
                this["n_OPERATORDB"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the DateTime n_DDB_LAST_MOD. Throws exception if the data is null.
        /// </summary>
        public DateTime n_DDB_LAST_MOD
        {
            get
            {
                if (this["n_DDB_LAST_MOD"] is DateTime)
                    return (DateTime)this["n_DDB_LAST_MOD"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (DateTime - n_DDB_LAST_MOD)");
            }
            set
            {
                this["n_DDB_LAST_MOD"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the DateTime n_LAST_VERIFIED_PRI. Throws exception if the data is null.
        /// </summary>
        public DateTime n_LAST_VERIFIED_PRI
        {
            get
            {
                if (this["n_LAST_VERIFIED_PRI"] is DateTime)
                    return (DateTime)this["n_LAST_VERIFIED_PRI"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (DateTime - n_LAST_VERIFIED_PRI)");
            }
            set
            {
                this["n_LAST_VERIFIED_PRI"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the DateTime n_LAST_VERIFIED_SEC. Throws exception if the data is null.
        /// </summary>
        public DateTime n_LAST_VERIFIED_SEC
        {
            get
            {
                if (this["n_LAST_VERIFIED_SEC"] is DateTime)
                    return (DateTime)this["n_LAST_VERIFIED_SEC"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (DateTime - n_LAST_VERIFIED_SEC)");
            }
            set
            {
                this["n_LAST_VERIFIED_SEC"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int change_type. Throws exception if the data is null.
        /// </summary>
        public ChangeTypes change_type
        {
            get
            {
                if (this["change_type"] is int)
                    return (ChangeTypes)this["change_type"];
                else
                    return ChangeTypes.Other;
            }
            set
            {
                this["change_type"] = (int) value;
            }
        }
    }

    public partial class provider_eligibility_test_rules : DataObject
    {
        /// <summary>
        /// The default constructor for the provider_eligibility_test_rules object, creates a new
        /// instance of this object without loading a default record.
        /// </summary>
        public provider_eligibility_test_rules()
            : base("id", "provider_eligibility_test_rules")
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        /// <summary>
        /// Constructor for the provider_eligibility_test_rules object, creates a new
        /// instance of this object and loads a record using the passed ID.
        /// </summary>
        public provider_eligibility_test_rules(int ID)
            : base("id", "provider_eligibility_test_rules", ID)
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        private void thisRecordChanged(object sender, RecordChangedEventArgs e) { }

        /// <summary>
        /// Get/Set for the int id. Throws exception if the data is null.
        /// </summary>
        public int id
        {
            get
            {
                if (this["id"] is int)
                    return (int)this["id"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - id)");
            }
            set
            {
                this["id"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string provider. Returns "" if the data is null.
        /// </summary>
        public string provider
        {
            get
            {
                if (this["provider"] is string)
                    return (string)this["provider"];
                else
                    return "";
            }
            set
            {
                this["provider"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string insurance. Returns "" if the data is null.
        /// </summary>
        public string insurance
        {
            get
            {
                if (this["insurance"] is string)
                    return (string)this["insurance"];
                else
                    return "";
            }
            set
            {
                this["insurance"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the DateTime date_of_service. Throws exception if the data is null.
        /// </summary>
        public DateTime date_of_service
        {
            get
            {
                if (this["date_of_service"] is DateTime)
                    return (DateTime)this["date_of_service"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (DateTime - date_of_service)");
            }
            set
            {
                this["date_of_service"] = value;
            }
        }
    }


    public partial class dentrix_providers : DataObject
    {
        /// <summary>
        /// The default constructor for the Dentrix_Providers object, creates a new
        /// instance of this object without loading a default record.
        /// </summary>
        public dentrix_providers()
            : base("URSCID", "Dentrix_Providers")
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        /// <summary>
        /// Constructor for the Dentrix_Providers object, creates a new
        /// instance of this object and loads a record using the passed ID.
        /// </summary>
        public dentrix_providers(int ID)
            : base("URSCID", "Dentrix_Providers", ID)
        {
            this.RecordChanged += new DataObject.RecordChangedEventHandler(thisRecordChanged);
        }
        private void thisRecordChanged(object sender, RecordChangedEventArgs e) { }

        /// <summary>
        /// Get/Set for the int URSCID. Throws exception if the data is null.
        /// </summary>
        public int URSCID
        {
            get
            {
                if (this["URSCID"] is int)
                    return (int)this["URSCID"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - URSCID)");
            }
            set
            {
                this["URSCID"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int RSCDB. Throws exception if the data is null.
        /// </summary>
        public int RSCDB
        {
            get
            {
                if (this["RSCDB"] is int)
                    return (int)this["RSCDB"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - RSCDB)");
            }
            set
            {
                this["RSCDB"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string RSCID. Returns "" if the data is null.
        /// </summary>
        public string RSCID
        {
            get
            {
                if (this["RSCID"] is string)
                    return (string)this["RSCID"];
                else
                    return "";
            }
            set
            {
                this["RSCID"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int IDNUM. Throws exception if the data is null.
        /// </summary>
        public int IDNUM
        {
            get
            {
                if (this["IDNUM"] is int)
                    return (int)this["IDNUM"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - IDNUM)");
            }
            set
            {
                this["IDNUM"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string NAME_LAST. Returns "" if the data is null.
        /// </summary>
        public string NAME_LAST
        {
            get
            {
                if (this["NAME_LAST"] is string)
                    return (string)this["NAME_LAST"];
                else
                    return "";
            }
            set
            {
                this["NAME_LAST"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string NAME_FIRST. Returns "" if the data is null.
        /// </summary>
        public string NAME_FIRST
        {
            get
            {
                if (this["NAME_FIRST"] is string)
                    return (string)this["NAME_FIRST"];
                else
                    return "";
            }
            set
            {
                this["NAME_FIRST"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string NAME_INITIAL. Returns "" if the data is null.
        /// </summary>
        public string NAME_INITIAL
        {
            get
            {
                if (this["NAME_INITIAL"] is string)
                    return (string)this["NAME_INITIAL"];
                else
                    return "";
            }
            set
            {
                this["NAME_INITIAL"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string NAME_TITLE. Returns "" if the data is null.
        /// </summary>
        public string NAME_TITLE
        {
            get
            {
                if (this["NAME_TITLE"] is string)
                    return (string)this["NAME_TITLE"];
                else
                    return "";
            }
            set
            {
                this["NAME_TITLE"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string PRACTITLE. Returns "" if the data is null.
        /// </summary>
        public string PRACTITLE
        {
            get
            {
                if (this["PRACTITLE"] is string)
                    return (string)this["PRACTITLE"];
                else
                    return "";
            }
            set
            {
                this["PRACTITLE"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string STREET. Returns "" if the data is null.
        /// </summary>
        public string STREET
        {
            get
            {
                if (this["STREET"] is string)
                    return (string)this["STREET"];
                else
                    return "";
            }
            set
            {
                this["STREET"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string STREET2. Returns "" if the data is null.
        /// </summary>
        public string STREET2
        {
            get
            {
                if (this["STREET2"] is string)
                    return (string)this["STREET2"];
                else
                    return "";
            }
            set
            {
                this["STREET2"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string CITY. Returns "" if the data is null.
        /// </summary>
        public string CITY
        {
            get
            {
                if (this["CITY"] is string)
                    return (string)this["CITY"];
                else
                    return "";
            }
            set
            {
                this["CITY"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string STATE. Returns "" if the data is null.
        /// </summary>
        public string STATE
        {
            get
            {
                if (this["STATE"] is string)
                    return (string)this["STATE"];
                else
                    return "";
            }
            set
            {
                this["STATE"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string ZIP. Returns "" if the data is null.
        /// </summary>
        public string ZIP
        {
            get
            {
                if (this["ZIP"] is string)
                    return (string)this["ZIP"];
                else
                    return "";
            }
            set
            {
                this["ZIP"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string COUNTRY. Returns "" if the data is null.
        /// </summary>
        public string COUNTRY
        {
            get
            {
                if (this["COUNTRY"] is string)
                    return (string)this["COUNTRY"];
                else
                    return "";
            }
            set
            {
                this["COUNTRY"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string PHONE. Returns "" if the data is null.
        /// </summary>
        public string PHONE
        {
            get
            {
                if (this["PHONE"] is string)
                    return (string)this["PHONE"];
                else
                    return "";
            }
            set
            {
                this["PHONE"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string EXT. Returns "" if the data is null.
        /// </summary>
        public string EXT
        {
            get
            {
                if (this["EXT"] is string)
                    return (string)this["EXT"];
                else
                    return "";
            }
            set
            {
                this["EXT"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string SS. Returns "" if the data is null.
        /// </summary>
        public string SS
        {
            get
            {
                if (this["SS"] is string)
                    return (string)this["SS"];
                else
                    return "";
            }
            set
            {
                this["SS"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string STATEID. Returns "" if the data is null.
        /// </summary>
        public string STATEID
        {
            get
            {
                if (this["STATEID"] is string)
                    return (string)this["STATEID"];
                else
                    return "";
            }
            set
            {
                this["STATEID"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string ADA. Returns "" if the data is null.
        /// </summary>
        public string ADA
        {
            get
            {
                if (this["ADA"] is string)
                    return (string)this["ADA"];
                else
                    return "";
            }
            set
            {
                this["ADA"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string MEDICAID. Returns "" if the data is null.
        /// </summary>
        public string MEDICAID
        {
            get
            {
                if (this["MEDICAID"] is string)
                    return (string)this["MEDICAID"];
                else
                    return "";
            }
            set
            {
                this["MEDICAID"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string DRUGID. Returns "" if the data is null.
        /// </summary>
        public string DRUGID
        {
            get
            {
                if (this["DRUGID"] is string)
                    return (string)this["DRUGID"];
                else
                    return "";
            }
            set
            {
                this["DRUGID"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int CLASS. Throws exception if the data is null.
        /// </summary>
        public int CLASS
        {
            get
            {
                if (this["CLASS"] is int)
                    return (int)this["CLASS"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - CLASS)");
            }
            set
            {
                this["CLASS"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int ECFUSER. Throws exception if the data is null.
        /// </summary>
        public int ECFUSER
        {
            get
            {
                if (this["ECFUSER"] is int)
                    return (int)this["ECFUSER"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - ECFUSER)");
            }
            set
            {
                this["ECFUSER"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int CLAIMSIGN. Throws exception if the data is null.
        /// </summary>
        public int CLAIMSIGN
        {
            get
            {
                if (this["CLAIMSIGN"] is int)
                    return (int)this["CLAIMSIGN"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - CLAIMSIGN)");
            }
            set
            {
                this["CLAIMSIGN"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int STATEMENTINFO. Throws exception if the data is null.
        /// </summary>
        public int STATEMENTINFO
        {
            get
            {
                if (this["STATEMENTINFO"] is int)
                    return (int)this["STATEMENTINFO"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - STATEMENTINFO)");
            }
            set
            {
                this["STATEMENTINFO"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int Unused1. Throws exception if the data is null.
        /// </summary>
        public int Unused1
        {
            get
            {
                if (this["Unused1"] is int)
                    return (int)this["Unused1"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - Unused1)");
            }
            set
            {
                this["Unused1"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string DEPOSITSLIP. Returns "" if the data is null.
        /// </summary>
        public string DEPOSITSLIP
        {
            get
            {
                if (this["DEPOSITSLIP"] is string)
                    return (string)this["DEPOSITSLIP"];
                else
                    return "";
            }
            set
            {
                this["DEPOSITSLIP"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int CDAFEE1. Throws exception if the data is null.
        /// </summary>
        public int CDAFEE1
        {
            get
            {
                if (this["CDAFEE1"] is int)
                    return (int)this["CDAFEE1"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - CDAFEE1)");
            }
            set
            {
                this["CDAFEE1"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int CDAFEE2. Throws exception if the data is null.
        /// </summary>
        public int CDAFEE2
        {
            get
            {
                if (this["CDAFEE2"] is int)
                    return (int)this["CDAFEE2"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - CDAFEE2)");
            }
            set
            {
                this["CDAFEE2"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int CDAFEE3. Throws exception if the data is null.
        /// </summary>
        public int CDAFEE3
        {
            get
            {
                if (this["CDAFEE3"] is int)
                    return (int)this["CDAFEE3"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - CDAFEE3)");
            }
            set
            {
                this["CDAFEE3"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int CDAFEE4. Throws exception if the data is null.
        /// </summary>
        public int CDAFEE4
        {
            get
            {
                if (this["CDAFEE4"] is int)
                    return (int)this["CDAFEE4"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - CDAFEE4)");
            }
            set
            {
                this["CDAFEE4"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int CDAFEE5. Throws exception if the data is null.
        /// </summary>
        public int CDAFEE5
        {
            get
            {
                if (this["CDAFEE5"] is int)
                    return (int)this["CDAFEE5"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - CDAFEE5)");
            }
            set
            {
                this["CDAFEE5"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string ID1. Returns "" if the data is null.
        /// </summary>
        public string ID1
        {
            get
            {
                if (this["ID1"] is string)
                    return (string)this["ID1"];
                else
                    return "";
            }
            set
            {
                this["ID1"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string ID2. Returns "" if the data is null.
        /// </summary>
        public string ID2
        {
            get
            {
                if (this["ID2"] is string)
                    return (string)this["ID2"];
                else
                    return "";
            }
            set
            {
                this["ID2"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string ID3. Returns "" if the data is null.
        /// </summary>
        public string ID3
        {
            get
            {
                if (this["ID3"] is string)
                    return (string)this["ID3"];
                else
                    return "";
            }
            set
            {
                this["ID3"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string PROVIDERNUM. Returns "" if the data is null.
        /// </summary>
        public string PROVIDERNUM
        {
            get
            {
                if (this["PROVIDERNUM"] is string)
                    return (string)this["PROVIDERNUM"];
                else
                    return "";
            }
            set
            {
                this["PROVIDERNUM"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string OFFICENUM. Returns "" if the data is null.
        /// </summary>
        public string OFFICENUM
        {
            get
            {
                if (this["OFFICENUM"] is string)
                    return (string)this["OFFICENUM"];
                else
                    return "";
            }
            set
            {
                this["OFFICENUM"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int FEESCHID. Throws exception if the data is null.
        /// </summary>
        public int FEESCHID
        {
            get
            {
                if (this["FEESCHID"] is int)
                    return (int)this["FEESCHID"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - FEESCHID)");
            }
            set
            {
                this["FEESCHID"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int FEESCHDB. Throws exception if the data is null.
        /// </summary>
        public int FEESCHDB
        {
            get
            {
                if (this["FEESCHDB"] is int)
                    return (int)this["FEESCHDB"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - FEESCHDB)");
            }
            set
            {
                this["FEESCHDB"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string RSCINFO. Returns "" if the data is null.
        /// </summary>
        public string RSCINFO
        {
            get
            {
                if (this["RSCINFO"] is string)
                    return (string)this["RSCINFO"];
                else
                    return "";
            }
            set
            {
                this["RSCINFO"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string ASSIGNRSC. Returns "" if the data is null.
        /// </summary>
        public string ASSIGNRSC
        {
            get
            {
                if (this["ASSIGNRSC"] is string)
                    return (string)this["ASSIGNRSC"];
                else
                    return "";
            }
            set
            {
                this["ASSIGNRSC"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string AsgnRscDB. Returns "" if the data is null.
        /// </summary>
        public string AsgnRscDB
        {
            get
            {
                if (this["AsgnRscDB"] is string)
                    return (string)this["AsgnRscDB"];
                else
                    return "";
            }
            set
            {
                this["AsgnRscDB"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int DefInstructorID. Throws exception if the data is null.
        /// </summary>
        public int DefInstructorID
        {
            get
            {
                if (this["DefInstructorID"] is int)
                    return (int)this["DefInstructorID"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - DefInstructorID)");
            }
            set
            {
                this["DefInstructorID"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int DefInstructorDB. Throws exception if the data is null.
        /// </summary>
        public int DefInstructorDB
        {
            get
            {
                if (this["DefInstructorDB"] is int)
                    return (int)this["DefInstructorDB"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - DefInstructorDB)");
            }
            set
            {
                this["DefInstructorDB"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int StudentFlag. Throws exception if the data is null.
        /// </summary>
        public int StudentFlag
        {
            get
            {
                if (this["StudentFlag"] is int)
                    return (int)this["StudentFlag"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - StudentFlag)");
            }
            set
            {
                this["StudentFlag"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int DefaultClinic. Throws exception if the data is null.
        /// </summary>
        public int DefaultClinic
        {
            get
            {
                if (this["DefaultClinic"] is int)
                    return (int)this["DefaultClinic"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - DefaultClinic)");
            }
            set
            {
                this["DefaultClinic"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string UserLoginName. Returns "" if the data is null.
        /// </summary>
        public string UserLoginName
        {
            get
            {
                if (this["UserLoginName"] is string)
                    return (string)this["UserLoginName"];
                else
                    return "";
            }
            set
            {
                this["UserLoginName"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int RVUID. Throws exception if the data is null.
        /// </summary>
        public int RVUID
        {
            get
            {
                if (this["RVUID"] is int)
                    return (int)this["RVUID"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - RVUID)");
            }
            set
            {
                this["RVUID"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int RVUDB. Throws exception if the data is null.
        /// </summary>
        public int RVUDB
        {
            get
            {
                if (this["RVUDB"] is int)
                    return (int)this["RVUDB"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - RVUDB)");
            }
            set
            {
                this["RVUDB"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the string FinancialLocCode. Returns "" if the data is null.
        /// </summary>
        public string FinancialLocCode
        {
            get
            {
                if (this["FinancialLocCode"] is string)
                    return (string)this["FinancialLocCode"];
                else
                    return "";
            }
            set
            {
                this["FinancialLocCode"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the DateTime DDB_LAST_MOD. Throws exception if the data is null.
        /// </summary>
        public DateTime DDB_LAST_MOD
        {
            get
            {
                if (this["DDB_LAST_MOD"] is DateTime)
                    return (DateTime)this["DDB_LAST_MOD"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (DateTime - DDB_LAST_MOD)");
            }
            set
            {
                this["DDB_LAST_MOD"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int BILLINGPROVID. Throws exception if the data is null.
        /// </summary>
        public int BILLINGPROVID
        {
            get
            {
                if (this["BILLINGPROVID"] is int)
                    return (int)this["BILLINGPROVID"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - BILLINGPROVID)");
            }
            set
            {
                this["BILLINGPROVID"] = value;
            }
        }
        /// <summary>
        /// Get/Set for the int BILLINGPROVDB. Throws exception if the data is null.
        /// </summary>
        public int BILLINGPROVDB
        {
            get
            {
                if (this["BILLINGPROVDB"] is int)
                    return (int)this["BILLINGPROVDB"];
                else
                    throw new DataObjectException("Property value is empty or invalid. (int - BILLINGPROVDB)");
            }
            set
            {
                this["BILLINGPROVDB"] = value;
            }
        }
    }


}