using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMdm.UI.Web.Models.Customer
{
    public class GuarantorModel
    {
        public GuarantorModel()
        {
            Branches = new List<SelectListItem>();
        }
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

        public List<SelectListItem> Branches { get; set; }

        public string ReadOnlyForm { get; set; }
        public string LastUpdatedby { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public string LastAuthdby { get; set; }
        public DateTime? LastAuthDate { get; set; }
        public int ExceptionId { get; internal set; }
        public string AuthoriserRemarks { get; internal set; }
    }
}