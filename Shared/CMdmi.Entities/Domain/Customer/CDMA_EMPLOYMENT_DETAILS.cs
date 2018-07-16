namespace CMdm.Entities.Domain.Customer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    [Table("CDMA_EMPLOYMENT_DETAILS")]
    public partial class CDMA_EMPLOYMENT_DETAILS
    {
        [Key, Column(Order = 0)]
        [DisplayName("Customer No")]
        public string CUSTOMER_NO { get; set; }
        [DisplayName("Occupation")]
        public string OCCUPATION { get; set; }
        [DisplayName("Employment Status")]
        public string EMPLOYMENT_STATUS { get; set; }
        [DisplayName("Fax No")]
        public string FAX_NO { get; set; }
        [DisplayName("Date of Employment")]
        public DateTime? DATE_OF_EMPLOYMENT { get; set; }
        [DisplayName("Address No")]
        public int? EMP_ADDRESS_NO { get; set; }
        [DisplayName("Address LGA")]
        public int? EMPLOYMENT_ADD_LGA { get; set; }
        [DisplayName("Address City")]
        public string EMPLOYMENT_ADDRESS_CITY { get; set; }
        [DisplayName("Employer Name / Institution Name")]
        public string EMPLOYER_INSTITUTION_NAME { get; set; }      
        [DisplayName("Nature of Business")]
        public decimal? NATURE_OF_BUSINESS_OCCUPATION { get; set; }
        [DisplayName("Office Number")]
        public string OFFICE_NO_CUSTOMER { get; set; }
        [DisplayName("State")]
        public int? EMPLOYER_ADD_STATE { get; set; }
        [DisplayName("Street Name")]
        public string STREET_NAME { get; set; }
        [DisplayName("Annual Income")]
        public decimal? ANNUAL_INCOME { get; set; }
        [DisplayName("Bustop Landmark")]
        public string BUSTOP_LANDMARK_EMPLOYMENT_ADD { get; set; }
        [DisplayName("Branch")]
        public string BRANCH_CODE { get; set; }
        [DisplayName("Tier")]
        public int? TIER { get; set; }
        public int? QUEUE_STATUS { get; set; }

        public DateTime? CREATED_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? LAST_MODIFIED_DATE { get; set; }
        public string LAST_MODIFIED_BY { get; set; }
        [Key, Column(Order = 1)]
        public string AUTHORISED { get; set; }
        public string AUTHORISED_BY { get; set; }
        public DateTime? AUTHORISED_DATE { get; set; }
        public string IP_ADDRESS { get; set; }

        //public virtual CDMA_OCCUPATION_LIST Occupationtype { get; set; }
        //public virtual CDMA_INDUSTRY_SUBSECTOR Subsectortype { get; set; }
        //public virtual CDMA_NATURE_OF_BUSINESS Businessnature { get; set; }
        //public virtual CDMA_INDUSTRY_SEGMENT Indsegment { get; set; }
    }

 

}
