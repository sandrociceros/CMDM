using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using CMdm.Data;
using CMdm.WebService.Models;

namespace CMdm.WebService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "FetchCustomer" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select FetchCustomer.svc or FetchCustomer.svc.cs at the Solution Explorer and start debugging.
    public class FetchCustomer : IFetchCustomer
    {
        public string DoWork()
        {
            return "Hello World!";
        }

        WebServiceModel IFetchCustomer.GetCustomerDetails(string CUSTOMER_NO)
        {
            var model = (
            new WebServiceModel
            {
                CUSTOMER_NO = CUSTOMER_NO,
                TITLE = "",
                SURNAME = "",
                DATE_OF_BIRTH = DateTime.Now,
                FIRST_NAME = "",
                OTHER_NAME = "",
                NICKNAME_ALIAS = "",
                COUNTRY_OF_BIRTH = "",
                PLACE_OF_BIRTH = "",
                SEX = "",
                AGE = 0,
                MARITAL_STATUS = "",
                NATIONALITY = "",
                STATE_OF_ORIGIN = "",
                MOTHER_MAIDEN_NAME = "",
                DISABILITY = "",
                COMPLEXION = "",
                NUMBER_OF_CHILDREN = 0,
                RELIGION = "",
                BRANCH_CODE = "",
                RESIDENTIAL_ADDRESS = "",
                MOBILE_NO = "",
                EMAIL_ADDRESS = "",
                RETCODE = 0,
                RESPONSEDESCRIPTION = "Success"
                }
            );

                return model;
            }
    }
}
