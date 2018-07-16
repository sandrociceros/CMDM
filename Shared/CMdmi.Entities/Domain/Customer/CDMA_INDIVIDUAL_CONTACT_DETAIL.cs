

namespace CMdm.Entities.Domain.Customer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [Table("CDMA_INDIVIDUAL_CONTACT_DETAIL")]

    public partial class CDMA_INDIVIDUAL_CONTACT_DETAIL
    {
        [Key, Column(Order = 0)]
        [DisplayName("Customer NO")]
        public string CUSTOMER_NO { get; set; }
        [DisplayName("Phone NO")]
        public string MOBILE_NO { get; set; }
        [DisplayName("Email")]
        public string EMAIL_ADDRESS { get; set; }
        [DisplayName("Address")]
        public string MAILING_ADDRESS { get; set; }
        [DisplayName("Alternate Phone NO")]
        public string ALTERNATEPHONENO { get; set; }
        [DisplayName("Residence LGA")]
        public string RESIDENCE_LGA { get; set; }
        [DisplayName("Nearest Bustop Landmark")]
        public string NEAREST_BUSTOP_LANDMARK { get; set; }
        [DisplayName("Branch")]
        public string BRANCH_CODE { get; set; }
        [DisplayName("State")]
        public string STATE { get; set; }
        [DisplayName("City")]
        public string CITY { get; set; }
        [DisplayName("House No")]
        public string ADDRESS_NUMBER { get; set; }
        [DisplayName("Residential Street Name")]
        public string MAILING_ADDRESS_STREET { get; set; }
        public int? TIER { get; set; }

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
