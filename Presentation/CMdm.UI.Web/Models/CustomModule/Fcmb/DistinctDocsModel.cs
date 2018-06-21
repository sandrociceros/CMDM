using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMdm.UI.Web.Models.CustomModule.Fcmb
{
    public class DistinctDocsModel
    {
        public DistinctDocsModel()
        {
            Branches = new List<SelectListItem>();
        }

        [DisplayName("Customer ID")]
        public string CIF_ID { get; set; }
        [DisplayName("Account Name")]
        public string ACCT_NAME { get; set; }
        [DisplayName("Branch Code")]
        public string SOL_ID { get; set; }
        [DisplayName("Due Date")]
        public DateTime? DUE_DATE { get; set; }
        [DisplayName("Reason")]
        public string FREZ_REASON_CODE { get; set; }
        [DisplayName("Account Officer Code")]
        public string ACCTOFFICER_CODE { get; set; }
        [DisplayName("Account Officer")]
        public string ACCTOFFICER_NAME { get; set; }
        [DisplayName("Branch Name")]
        public string BRANCH_NAME { get; set; }

        public int Id
        {
            get; set;
        }

        public IList<SelectListItem> Branches { get; set; }
    }
}