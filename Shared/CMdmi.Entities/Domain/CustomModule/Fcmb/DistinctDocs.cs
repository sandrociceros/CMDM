using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMdm.Entities.Domain.CustomModule.Fcmb
{
    [Table("VW_OUSTANDING_DOCUMENTATION1")]
    public partial class DistinctDocs
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public string CIF_ID { get; set; }

        public string ACCT_NAME { get; set; }
        
        public string SOL_ID { get; set; }

        public DateTime? DUE_DATE { get; set; }

        public string FREZ_REASON_CODE { get; set; }

        public string ACCTOFFICER_CODE { get; set; }

        public string ACCTOFFICER_NAME { get; set; }

        public string BRANCH_NAME { get; set; }        
        
    }
}
