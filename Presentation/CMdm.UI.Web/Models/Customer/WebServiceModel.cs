using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMdm.UI.Web.Models.Customer
{
    public class WebServiceModel
    {
        public string CUSTOMER_NO { get; set; }
        public string SURNAME { get; set; }
        public string FIRST_NAME { get; set; }
        public string OTHER_NAME { get; set; }
        public string NICKNAME_ALIAS { get; set; }
        public DateTime? DATE_OF_BIRTH { get; set; }
        public string PLACE_OF_BIRTH { get; set; }
        public string COUNTRY_OF_BIRTH { get; set; }
        public string SEX { get; set; }
        public int? AGE { get; set; }
        public string MARITAL_STATUS { get; set; }
        public string NATIONALITY { get; set; }
        public string STATE_OF_ORIGIN { get; set; }
        public string MOTHER_MAIDEN_NAME { get; set; }
        public string DISABILITY { get; set; }
        public string COMPLEXION { get; set; }
        public int? NUMBER_OF_CHILDREN { get; set; }
        public string RELIGION { get; set; }
        public string BRANCH_CODE { get; set; }

        public string RESIDENTIAL_ADDRESS { get; set; }

        public string MOBILE_NO { get; set; }
        public string EMAIL_ADDRESS { get; set; }

        public int RETCODE { get; set; }
        public string RESPONSEDESCRIPTION { get; set; }
    }
}