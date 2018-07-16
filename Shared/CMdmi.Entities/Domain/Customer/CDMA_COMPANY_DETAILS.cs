using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMdm.Entities.Domain.Customer
{
    [Table("CDMA_COMPANY_DETAILS")]
    public partial class CDMA_COMPANY_DETAILS
    {
        [Key, Column(Order = 0)]
        [DisplayName("Customer NO")]
        public string CUSTOMER_NO { get; set; }
        [DisplayName("Operating Address")]
        public string BIZ_ADDRESS_REG_OFFICE_1 { get; set; }
        [DisplayName("Company Registration NO")]
        public string CERT_OF_INCORP_REG_NO { get; set; }
        [DisplayName("Expected Annual Turnover")]
        public decimal? EXPECTED_ANNUAL_TURNOVER { get; set; }
        [DisplayName("Operating Business 1")]
        public string OPERATING_BUSINESS_1 { get; set; }
        [DisplayName("Branch name")]
        public string BRANCH_CODE { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? LAST_MODIFIED_DATE { get; set; }
        public string LAST_MODIFIED_BY { get; set; }
        [Key, Column(Order = 1)]
        public string AUTHORISED { get; set; }
        public string AUTHORISED_BY { get; set; }
        public DateTime? AUTHORISED_DATE { get; set; }
        public string IP_ADDRESS { get; set; }
        public int? QUEUE_STATUS { get; set; }
    }
}
