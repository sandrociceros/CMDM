using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMdm.Entities.Domain.CustomModule.Fcmb
{
    [Table("VW_ACCOUNTNO_CIFID")]
    public partial class CustAccount
    {
        [Key]
        public string FORACID { get; set; }
        public string CIF_ID { get; set; }
    }
}
