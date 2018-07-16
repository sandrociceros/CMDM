using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMdm.Entities.Domain.Customer
{
    [Table("CDMA_INDIVIDUAL_NEXT_OF_KIN")]
    public partial class CDMA_INDIVIDUAL_NEXT_OF_KIN
    {
        [Key, Column(Order = 0)]
        [DisplayName("Customer No")]
        public string CUSTOMER_NO { get; set; }
        [DisplayName("Title")]
        public int? TITLE { get; set; }
        [DisplayName("Surname")]
        public string SURNAME { get; set; }
        [DisplayName("First Name")]
        public string FIRST_NAME { get; set; }
        [DisplayName("Middler Name")]
        public string OTHER_NAME { get; set; }
        [DisplayName("Date of Birth")]
        public DateTime? DATE_OF_BIRTH { get; set; }
        [DisplayName("Gender")]
        public string SEX { get; set; }
        [DisplayName("Next of Kin Relationship")]
        public int? RELATIONSHIP { get; set; }
        [DisplayName("Email")]
        public string EMAIL_ADDRESS { get; set; }
        [DisplayName("Residential Street")]
        public string NEXT_OF_KIN_RESIDENTIALSTREET { get; set; }
        [DisplayName("Address No")]
        public int? NOK_ADDRESS_NO { get; set; }
        [DisplayName("City")]
        public string CITY_TOWN { get; set; }
        [DisplayName("LGA")]
        public int? LGA { get; set; }
        [DisplayName("Nearest Bus Stop")]
        public string NEAREST_BUS_STOP_LANDMARK { get; set; }
        [DisplayName("State")]
        public int? STATE { get; set; }
        [DisplayName("Phone Number")]
        public string NEXT_OF_KIN_PHONE_NUMBER { get; set; }
        [DisplayName("Alternate Phone Number")]
        public string NEXT_OF_KIN_PHONE_NUMBER2 { get; set; }
        [DisplayName("Branch")]
        public string BRANCH_CODE { get; set; }
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
    }
}
