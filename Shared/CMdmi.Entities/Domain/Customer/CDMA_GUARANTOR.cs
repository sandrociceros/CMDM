using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMdm.Entities.Domain.Customer
{
    [Table("CDMA_GUARANTOR")]
    public partial class CDMA_GUARANTOR
    {
        [Key, Column(Order = 0)]
        [DisplayName("Customer NO")]
        public string CUSTOMER_NO { get; set; }
        [DisplayName("Last Name Of Guarantor")]
        public string LNAME_OF_GUARANTOR { get; set; }
        [DisplayName("First Name Of Guarantor")]
        public string FNAME_OF_GUARANTOR { get; set; }
        [DisplayName("TIN Of Guarantor")]
        public string TIN_OF_GUARANTOR { get; set; }
        [DisplayName("BVN Of Guarantor")]
        public string BVN_OF_GUARANTOR { get; set; }
        [DisplayName("Guaranteed Amount")]
        public decimal? GURANTEED_AMOUNT { get; set; }
        [DisplayName("Branch")]
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
