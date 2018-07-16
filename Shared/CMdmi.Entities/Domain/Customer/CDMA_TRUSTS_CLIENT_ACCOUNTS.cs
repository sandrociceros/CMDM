namespace CMdm.Entities.Domain.Customer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CDMA_TRUSTS_CLIENT_ACCOUNTS")]
    public partial class CDMA_TRUSTS_CLIENT_ACCOUNTS
    {

        [Key, Column(Order = 0)]
        [DisplayName("Customer No")]
        public string CUSTOMER_NO { get; set; }
        [DisplayName("Customer Business Address")]
        public string CUSTOMER_BUSINESS_ADDRESS { get; set; }
        [DisplayName("Customer Spouse DOB")]
        public DateTime? CUSTOMER_SPOUSE_DOB { get; set; }
        [DisplayName("Expected Annual Income From Other Sources")]
        public string OTHER_SOURCE_EXPECT_ANN_INC { get; set; }
        [DisplayName("Customer Business Name")]
        public string CUSTOMER_BUSINESS_NAME { get; set; }
        [DisplayName("Customer Spouse Name")]
        public string CUSTOMER_SPOUSE_NAME { get; set; }
        [DisplayName("Customer Spouse Occupation")]
        public string CUSTOMER_SPOUSE_OCCUPATION { get; set; }
        [DisplayName("Customer Business Type")]
        public string CUSTOMER_BUSINESS_TYPE { get; set; }
        [DisplayName("Primary Source Of Fund")]
        public string SOURCES_OF_FUND_TO_ACCOUNT { get; set; }
        [DisplayName("Branch")]
        public string BRANCH_CODE { get; set; }
        public int? QUEUE_STATUS { get; set; }

        public string LAST_MODIFIED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? LAST_MODIFIED_DATE { get; set; }
        [Key, Column(Order = 1)]
        public string AUTHORISED { get; set; }
        public string AUTHORISED_BY { get; set; }
        public DateTime? AUTHORISED_DATE { get; set; }
        public string IP_ADDRESS { get; set; }
       
    }
}
