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

    [Table("CDMA_INDIVIDUAL_IDENTIFICATION")]

    public partial class CDMA_INDIVIDUAL_IDENTIFICATION
    {

        [Key, Column(Order = 0)]
        [DisplayName("Customer NO")]
        public string CUSTOMER_NO { get; set; }
        [DisplayName("Identification Type")]
        public string IDENTIFICATION_TYPE { get; set; }
        [DisplayName("Reference NO")]
        public string ID_NO { get; set; }
        [DisplayName("Document Expiry Date")]
        public DateTime? ID_EXPIRY_DATE { get; set; }
        [DisplayName("Document Issue Date")]
        public DateTime? ID_ISSUE_DATE { get; set; }
        [DisplayName("Branch")]
        public string BRANCH_CODE { get; set; }
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
