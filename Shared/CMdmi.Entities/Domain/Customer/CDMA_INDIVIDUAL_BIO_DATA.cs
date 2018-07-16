namespace CMdm.Entities.Domain.Customer
{

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CDMA_INDIVIDUAL_BIO_DATA")]
    
    public partial class CDMA_INDIVIDUAL_BIO_DATA
    {
        [Key, Column(Order = 0)]
        [DisplayName("Customer NO")]
        public string CUSTOMER_NO { get; set; }
        [DisplayName("Title")]
        public string TITLE { get; set; }
        [DisplayName("Last Name")]
        public string SURNAME { get; set; }
        [DisplayName("First Name")]
        public string FIRST_NAME { get; set; }
        [DisplayName("Middle Name")]
        public string OTHER_NAME { get; set; }
        [DisplayName("Date of Birth")]
        public DateTime? DATE_OF_BIRTH { get; set; }
        [DisplayName("Gender")]
        public string SEX { get; set; }
        [DisplayName("Marital Status")]
        public string MARITAL_STATUS { get; set; }
        [DisplayName("LGA of Origin")]
        public string LGAOFORIGIN { get; set; }
        [DisplayName("Mother's Maiden Name")]
        public string MOTHER_MAIDEN_NAME { get; set; }
        [DisplayName("Country of Birth")]
        public string COUNTRY_OF_BIRTH { get; set; }
        [DisplayName("Residence Country")]
        public string RESIDENCE_COUNTRY { get; set; }
        [DisplayName("Nationality")]
        public string NATIONALITY { get; set; }
        [DisplayName("Religion")]
        public string RELIGION { get; set; }
        [DisplayName("State of Origin")]
        public string STATE_OF_ORIGIN { get; set; }
        [DisplayName("Tin")]
        public string TIN { get; set; }
        [DisplayName("BVN")]
        public string BVN { get; set; }
        [DisplayName("Residence Permit Issue Date")]
        public DateTime? RESIDENCEPERMIT_ISSDATE { get; set; }
        [DisplayName("Residence Permit Expiry Date")]
        public DateTime? RESIDENCEPERMIT_EXPDATE { get; set; }
        [DisplayName("Purpose of Account")]
        public string PURPOSEOFACCOUNT { get; set; }
        [DisplayName("Residence Permit Number")]
        public string RESIDENCEPERMITNO { get; set; }
        [DisplayName("Second Nationality")]
        public string SECONDNATIONALITY { get; set; }
        [DisplayName("Branch")]
        public string BRANCH_CODE { get; set; }
        public int? TIER { get; set; }

        [DisplayName("Last Modified By")]
        public string LAST_MODIFIED_BY { get; set; }        
        public DateTime? CREATED_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? LAST_MODIFIED_DATE { get; set; }
        [Key, Column(Order = 1)]
        public string AUTHORISED { get; set; }
        public string AUTHORISED_BY { get; set; }
        public DateTime? AUTHORISED_DATE { get; set; }
        public string IP_ADDRESS { get; set; }
        public int? QUEUE_STATUS { get; set; }
    }
}
