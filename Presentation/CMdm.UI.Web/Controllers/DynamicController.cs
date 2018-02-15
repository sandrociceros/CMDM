﻿using CMdm.Data;
using CMdm.Data.Rbac;
using CMdm.Entities.Domain.Customer;
using CMdm.Entities.ViewModels;
using CMdm.UI.Web.Helpers.CrossCutting.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
//
namespace CMdm.UI.Web.Controllers
{
    public class DynamicController : Controller
    {
        private AppDbContext db = new AppDbContext();
        public string changesComment = "";
        //public int[] index_id = new int[100];
        List<int> index_id = new List<int>();


        //List<int> termsList = new List<int>();
        // termsList.Add(value);
        // You can convert it back to an array if you would like to
        //int[] change_index = index_id.ToArray();

        public ActionResult loadlga(decimal state_id)
        {
            ViewBag.LGA = new SelectList(db.SRC_CDMA_LGA.Where(g => g.STATE_ID == state_id), "LGA_ID", "LGA_NAME");


            return PartialView("loadlga");
        }

        public CDMA_COUNTRIES countryById(decimal country_id) { 
            var countryDetails = db.CDMA_COUNTRIES.Where(a => a.COUNTRY_ID == country_id).FirstOrDefault();
            return countryDetails;
        }

        public CDMA_IDENTIFICATION_TYPE IdType(decimal id_type)
        {
            var idTypeDetails = db.CDMA_IDENTIFICATION_TYPE.Where(a => a.CODE == id_type).FirstOrDefault();
            return idTypeDetails;
        }

        public SRC_CDMA_LGA getloadlga(decimal lga_id)
        {
            SRC_CDMA_LGA lgaDetails = db.SRC_CDMA_LGA.Where(a => a.LGA_ID == lga_id).FirstOrDefault();
            return lgaDetails;
        }

        public SRC_CDMA_STATE getState(decimal state_id)
        {
            SRC_CDMA_STATE stateDetails = db.SRC_CDMA_STATE.Where(a => a.STATE_ID == state_id).FirstOrDefault();
            return stateDetails;
        }

        public CDMA_RELIGION getRel(decimal rel_id)
        {
            CDMA_RELIGION relDetails = db.CDMA_RELIGION.Where(a => a.CODE == rel_id).FirstOrDefault();
            return relDetails;
        }

        public CM_USER_PROFILE getUserDetails(decimal user_id)
        {
            CM_USER_PROFILE detail = db.CM_USER_PROFILE.Where(a => a.PROFILE_ID == user_id).FirstOrDefault();
             
            return detail;
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult validatedata(DynamicViewModel DynamicModel)
        {
            var identity = ((CustomPrincipal)User).CustomIdentity;
            var dateAndTime = DateTime.Now;
            var c_id = DynamicModel.BioData.CUSTOMER_NO;

            //create a log before update
            string tied = DateTime.Now.ToString("hhmmssffffff");

 


            CDMA_INDIVIDUAL_BIO_DATA cDMA_INDIVIDUAL_BIO_DATA = db.CDMA_INDIVIDUAL_BIO_DATA.Find(c_id);
            CDMA_INDIVIDUAL_CONTACT_DETAIL cDMA_INDIVIDUAL_CONTACT_DETAIL = db.CDMA_INDIVIDUAL_CONTACT_DETAIL.Find(c_id);
            CDMA_INDIVIDUAL_ADDRESS_DETAIL cDMA_INDIVIDUAL_ADDRESS_DETAIL = db.CDMA_INDIVIDUAL_ADDRESS_DETAIL.Find(c_id);
            CDMA_INDIVIDUAL_IDENTIFICATION cDMA_INDIVIDUAL_IDENTIFICATION = db.CDMA_INDIVIDUAL_IDENTIFICATION.Find(c_id);
            CDMA_INDIVIDUAL_OTHER_DETAILS cDMA_INDIVIDUAL_OTHER_DETAILS = db.CDMA_INDIVIDUAL_OTHER_DETAILS.Find(c_id);

            //Address details
            //  var authrizer = DynamicModel.AddressDetails.AUTHORISED;
            // var AUTHORISED_BY = DynamicModel.AddressDetails.AUTHORISED_BY;
            //   var AUTHORISED_DATE = DynamicModel.AddressDetails.AUTHORISED_DATE;
            // checked if address record does not exist 
            if (cDMA_INDIVIDUAL_ADDRESS_DETAIL == null)
            {
                CDMA_INDIVIDUAL_ADDRESS_DETAIL cDMA_INDIVIDUAL_ADDRESS_DETAIL_SAVE = new CDMA_INDIVIDUAL_ADDRESS_DETAIL();
                cDMA_INDIVIDUAL_ADDRESS_DETAIL_SAVE.CREATED_BY = identity.ProfileId.ToString();
                cDMA_INDIVIDUAL_ADDRESS_DETAIL_SAVE.CREATED_DATE = dateAndTime;
                cDMA_INDIVIDUAL_ADDRESS_DETAIL_SAVE.CUSTOMER_NO = DynamicModel.BioData.CUSTOMER_NO;
                cDMA_INDIVIDUAL_ADDRESS_DETAIL_SAVE.LAST_MODIFIED_BY = identity.ProfileId.ToString();
                cDMA_INDIVIDUAL_ADDRESS_DETAIL_SAVE.LAST_MODIFIED_DATE = dateAndTime;                 

                db.CDMA_INDIVIDUAL_ADDRESS_DETAIL.Add(cDMA_INDIVIDUAL_ADDRESS_DETAIL_SAVE);
                db.SaveChanges();
            }
            else
            {
                cDMA_INDIVIDUAL_ADDRESS_DETAIL.CITY_TOWN_OF_RESIDENCE = DynamicModel.AddressDetails.CITY_TOWN_OF_RESIDENCE;
                cDMA_INDIVIDUAL_ADDRESS_DETAIL.COUNTRY_OF_RESIDENCE = Request["COUNTRY_OF_RESIDENCE"]; // DynamicModel.AddressDetails.COUNTRY_OF_RESIDENCE;
                cDMA_INDIVIDUAL_ADDRESS_DETAIL.CUSTOMER_NO = DynamicModel.BioData.CUSTOMER_NO;
                cDMA_INDIVIDUAL_ADDRESS_DETAIL.IP_ADDRESS = this.Request.ServerVariables["REMOTE_ADDR"];
                cDMA_INDIVIDUAL_ADDRESS_DETAIL.LAST_MODIFIED_BY = identity.ProfileId.ToString();
                cDMA_INDIVIDUAL_ADDRESS_DETAIL.LAST_MODIFIED_DATE = dateAndTime;
                cDMA_INDIVIDUAL_ADDRESS_DETAIL.LGA_OF_RESIDENCE = Request["LGA"]; // DynamicModel.AddressDetails.LGA_OF_RESIDENCE;
                cDMA_INDIVIDUAL_ADDRESS_DETAIL.NEAREST_BUS_STOP_LANDMARK = DynamicModel.AddressDetails.NEAREST_BUS_STOP_LANDMARK;
                cDMA_INDIVIDUAL_ADDRESS_DETAIL.RESIDENCE_OWNED_OR_RENT = Request["RESIDENCE_OWNED_OR_RENT"];
                cDMA_INDIVIDUAL_ADDRESS_DETAIL.RESIDENTIAL_ADDRESS = DynamicModel.AddressDetails.RESIDENTIAL_ADDRESS;
                cDMA_INDIVIDUAL_ADDRESS_DETAIL.STATE_OF_RESIDENCE = Request["STATES_RES"];  //DynamicModel.AddressDetails.STATE_OF_RESIDENCE;
                cDMA_INDIVIDUAL_ADDRESS_DETAIL.ZIP_POSTAL_CODE = DynamicModel.AddressDetails.ZIP_POSTAL_CODE;
                cDMA_INDIVIDUAL_ADDRESS_DETAIL.RESIDENCE_OWNED_OR_RENT = DynamicModel.AddressDetails.RESIDENCE_OWNED_OR_RENT;
                db.SaveChanges();


                var address = (CDMA_INDIVIDUAL_ADDRESS_DETAIL)this.Session["cDMA_INDIVIDUAL_ADDRESS_DETAIL"];
                // compare the Old record with the new one and get comment
                var address_changes = compareAddress(cDMA_INDIVIDUAL_ADDRESS_DETAIL, address);
                changesComment = changesComment + address_changes;
                // log address details
                CDMA_INDIVIDUAL_ADDRESS_DETAIL_LOG address_log = ConvertToAddressLog(address);
                address_log.TIED = tied;
                db.CDMA_INDIVIDUAL_ADDRESS_DETAIL_LOG.Add(address_log);
                db.SaveChanges();


            }

            // Biodata            
            cDMA_INDIVIDUAL_BIO_DATA.AGE = DynamicModel.BioData.AGE;
            //var AUTHORISED = DynamicModel.BioData.AUTHORISED;
            //var BIO_AUTHORISED_BY = DynamicModel.BioData.AUTHORISED_BY;
            // var BIO_AUTHORISED_DATE = DynamicModel.BioData.AUTHORISED_DATE;
            cDMA_INDIVIDUAL_BIO_DATA.BRANCH_CODE = Request["BRANCH"];  //DynamicModel.BioData.BRANCH_CODE;
            cDMA_INDIVIDUAL_BIO_DATA.COMPLEXION = DynamicModel.BioData.COMPLEXION;
            var COUNTRY_OF_BIRTH = Request["COUNTRY_OF_BIRTH"]; // DynamicModel.BioData.COUNTRY_OF_BIRTH;
                                                                // var BIO_CREATED_BY = DynamicModel.BioData.CREATED_BY;
                                                                // var BIO_CREATED_DATE = DynamicModel.BioData.CREATED_DATE;
            cDMA_INDIVIDUAL_BIO_DATA.CUSTOMER_NO = DynamicModel.BioData.CUSTOMER_NO;
            cDMA_INDIVIDUAL_BIO_DATA.DATE_OF_BIRTH = DynamicModel.BioData.DATE_OF_BIRTH;
            cDMA_INDIVIDUAL_BIO_DATA.DISABILITY = DynamicModel.BioData.DISABILITY;
            cDMA_INDIVIDUAL_BIO_DATA.FIRST_NAME = DynamicModel.BioData.FIRST_NAME;
            cDMA_INDIVIDUAL_BIO_DATA.IP_ADDRESS = this.Request.ServerVariables["REMOTE_ADDR"];
            cDMA_INDIVIDUAL_BIO_DATA.LAST_MODIFIED_DATE = dateAndTime;//DynamicModel.BioData.LAST_MODIFIED_DATE;
            cDMA_INDIVIDUAL_BIO_DATA.MARITAL_STATUS = DynamicModel.BioData.MARITAL_STATUS;
            cDMA_INDIVIDUAL_BIO_DATA.MOTHER_MAIDEN_NAME = DynamicModel.BioData.MOTHER_MAIDEN_NAME;
            cDMA_INDIVIDUAL_BIO_DATA.NATIONALITY = Request["BioData_NATIONALITY"];  //// DynamicModel.BioData.NATIONALITY;
            cDMA_INDIVIDUAL_BIO_DATA.NICKNAME_ALIAS = DynamicModel.BioData.NICKNAME_ALIAS;
            cDMA_INDIVIDUAL_BIO_DATA.NUMBER_OF_CHILDREN = DynamicModel.BioData.NUMBER_OF_CHILDREN;
            cDMA_INDIVIDUAL_BIO_DATA.OTHER_NAME = DynamicModel.BioData.OTHER_NAME;
            cDMA_INDIVIDUAL_BIO_DATA.PLACE_OF_BIRTH = DynamicModel.BioData.PLACE_OF_BIRTH;
            cDMA_INDIVIDUAL_BIO_DATA.RELIGION = DynamicModel.BioData.RELIGION;
            cDMA_INDIVIDUAL_BIO_DATA.SEX = DynamicModel.BioData.SEX;
            cDMA_INDIVIDUAL_BIO_DATA.STATE_OF_ORIGIN = DynamicModel.BioData.STATE_OF_ORIGIN;
            cDMA_INDIVIDUAL_BIO_DATA.SURNAME = DynamicModel.BioData.SURNAME;
            cDMA_INDIVIDUAL_BIO_DATA.TITLE = DynamicModel.BioData.TITLE;
            cDMA_INDIVIDUAL_BIO_DATA.RELIGION = Request["RELIGION"];
            cDMA_INDIVIDUAL_BIO_DATA.COUNTRY_OF_BIRTH = Request["COUNTRY_OF_BIRTH"];
            db.SaveChanges();

            CDMA_INDIVIDUAL_BIO_DATA bio_data = (CDMA_INDIVIDUAL_BIO_DATA)this.Session["cDMA_INDIVIDUAL_BIO_DATA"];
            // compare old bio record with the new bio record
            var biodata_changes = compareBioRecord(cDMA_INDIVIDUAL_BIO_DATA, bio_data);
            changesComment = changesComment + biodata_changes;


            CDMA_INDIVIDUAL_BIO_LOG biodata_log = ConvertToBioDataLog(bio_data);
            biodata_log.TIED = tied;
            db.CDMA_INDIVIDUAL_BIO_LOG.Add(biodata_log);
            db.SaveChanges();


            //contact	 
            //var CONTACT_AUTHORISED =  DynamicModel.contact.AUTHORISED;
            // var CONTACT_AUTHORISED_BY = DynamicModel.contact.AUTHORISED_BY;
            //  var CONTACT_AUTHORISED_DATE = DynamicModel.contact.AUTHORISED_DATE;
            //  var CONTACT_CREATED_BY = DynamicModel.contact.CREATED_BY;
            //  var CONTACT_CREATED_DATE = DynamicModel.contact.CREATED_DATE;
            //  var CONTACT_CUSTOMER_NO = DynamicModel.contact.CUSTOMER_NO;
            if (cDMA_INDIVIDUAL_CONTACT_DETAIL == null)
            {
                CDMA_INDIVIDUAL_CONTACT_DETAIL cDMA_INDIVIDUAL_CONTACT_DETAIL_SAVE = new CDMA_INDIVIDUAL_CONTACT_DETAIL();
                cDMA_INDIVIDUAL_CONTACT_DETAIL_SAVE.CUSTOMER_NO = DynamicModel.BioData.CUSTOMER_NO;
                cDMA_INDIVIDUAL_CONTACT_DETAIL_SAVE.CREATED_BY = identity.ProfileId.ToString();
                cDMA_INDIVIDUAL_CONTACT_DETAIL_SAVE.CREATED_DATE = dateAndTime;
                cDMA_INDIVIDUAL_CONTACT_DETAIL_SAVE.MAILING_ADDRESS = DynamicModel.contact.MAILING_ADDRESS;
                cDMA_INDIVIDUAL_CONTACT_DETAIL_SAVE.MOBILE_NO = DynamicModel.contact.MOBILE_NO;
                cDMA_INDIVIDUAL_CONTACT_DETAIL_SAVE.EMAIL_ADDRESS = DynamicModel.contact.EMAIL_ADDRESS;
                cDMA_INDIVIDUAL_CONTACT_DETAIL_SAVE.IP_ADDRESS = this.Request.ServerVariables["REMOTE_ADDR"];
                cDMA_INDIVIDUAL_CONTACT_DETAIL_SAVE.LAST_MODIFIED_BY = identity.ProfileId.ToString();
                db.CDMA_INDIVIDUAL_CONTACT_DETAIL.Add(cDMA_INDIVIDUAL_CONTACT_DETAIL_SAVE);
                db.SaveChanges();

            }
            else
            {
                cDMA_INDIVIDUAL_CONTACT_DETAIL.LAST_MODIFIED_BY = identity.ProfileId.ToString();
                cDMA_INDIVIDUAL_CONTACT_DETAIL.MAILING_ADDRESS = DynamicModel.contact.MAILING_ADDRESS;
                cDMA_INDIVIDUAL_CONTACT_DETAIL.MOBILE_NO = DynamicModel.contact.MOBILE_NO;
                cDMA_INDIVIDUAL_CONTACT_DETAIL.EMAIL_ADDRESS = DynamicModel.contact.EMAIL_ADDRESS;
                cDMA_INDIVIDUAL_CONTACT_DETAIL.IP_ADDRESS = this.Request.ServerVariables["REMOTE_ADDR"];
                cDMA_INDIVIDUAL_CONTACT_DETAIL.LAST_MODIFIED_BY = identity.ProfileId.ToString();
                cDMA_INDIVIDUAL_CONTACT_DETAIL.LAST_MODIFIED_DATE = dateAndTime;
                db.SaveChanges();
                CDMA_INDIVIDUAL_CONTACT_DETAIL contact_data = (CDMA_INDIVIDUAL_CONTACT_DETAIL)this.Session["cDMA_INDIVIDUAL_CONTACT_DETAIL"];

                var contact_changes = compareContactRecord(cDMA_INDIVIDUAL_CONTACT_DETAIL, contact_data);
                changesComment = changesComment + contact_changes;

                CDMA_INDIVIDUAL_CONTACT_LOG contact_log = ConvertToContactDataLog(contact_data);
                contact_log.TIED = tied;
                db.CDMA_INDIVIDUAL_CONTACT_LOG.Add(contact_log);
                db.SaveChanges();
            }



            // identification 
            if (cDMA_INDIVIDUAL_IDENTIFICATION == null)
            {
                CDMA_INDIVIDUAL_IDENTIFICATION cDMA_INDIVIDUAL_IDENTIFICATION_SAVE = new CDMA_INDIVIDUAL_IDENTIFICATION();
                cDMA_INDIVIDUAL_IDENTIFICATION_SAVE.CREATED_BY = identity.ProfileId.ToString();
                cDMA_INDIVIDUAL_IDENTIFICATION_SAVE.CREATED_DATE = dateAndTime;
                cDMA_INDIVIDUAL_IDENTIFICATION_SAVE.CUSTOMER_NO = DynamicModel.BioData.CUSTOMER_NO;
                cDMA_INDIVIDUAL_IDENTIFICATION_SAVE.IDENTIFICATION_TYPE = Request["CDMA_IDENTIFICATION_TYPE"];
                cDMA_INDIVIDUAL_IDENTIFICATION_SAVE.ID_EXPIRY_DATE = DynamicModel.identification.ID_EXPIRY_DATE;
                cDMA_INDIVIDUAL_IDENTIFICATION_SAVE.ID_ISSUE_DATE = DynamicModel.identification.ID_ISSUE_DATE;
                cDMA_INDIVIDUAL_IDENTIFICATION_SAVE.ID_NO = DynamicModel.identification.ID_NO;
                cDMA_INDIVIDUAL_IDENTIFICATION_SAVE.IP_ADDRESS = this.Request.ServerVariables["REMOTE_ADDR"];
                cDMA_INDIVIDUAL_IDENTIFICATION_SAVE.LAST_MODIFIED_BY = DynamicModel.identification.LAST_MODIFIED_BY;
                cDMA_INDIVIDUAL_IDENTIFICATION_SAVE.LAST_MODIFIED_DATE = dateAndTime;
                cDMA_INDIVIDUAL_IDENTIFICATION_SAVE.PLACE_OF_ISSUANCE = DynamicModel.identification.PLACE_OF_ISSUANCE;
                db.CDMA_INDIVIDUAL_IDENTIFICATION.Add(cDMA_INDIVIDUAL_IDENTIFICATION_SAVE);
                db.SaveChanges();

            }
            else
            {
                //var identification_AUTHORISED =  DynamicModel.identification.AUTHORISED;
                //var identification_AUTHORISED_BY = DynamicModel.identification.AUTHORISED_BY;
                // var identification_AUTHORISED_DATE = DynamicModel.identification.AUTHORISED_DATE;
                //var identification_CREATED_BY = DynamicModel.identification.CREATED_BY;
                // var identification_CREATED_DATE = DynamicModel.identification.CREATED_DATE;
                cDMA_INDIVIDUAL_IDENTIFICATION.CUSTOMER_NO = DynamicModel.BioData.CUSTOMER_NO;
                cDMA_INDIVIDUAL_IDENTIFICATION.IDENTIFICATION_TYPE = Request["CDMA_IDENTIFICATION_TYPE"];
                cDMA_INDIVIDUAL_IDENTIFICATION.ID_EXPIRY_DATE = DynamicModel.identification.ID_EXPIRY_DATE;
                cDMA_INDIVIDUAL_IDENTIFICATION.ID_ISSUE_DATE = DynamicModel.identification.ID_ISSUE_DATE;
                cDMA_INDIVIDUAL_IDENTIFICATION.ID_NO = DynamicModel.identification.ID_NO;
                cDMA_INDIVIDUAL_IDENTIFICATION.IP_ADDRESS = this.Request.ServerVariables["REMOTE_ADDR"];
                cDMA_INDIVIDUAL_IDENTIFICATION.LAST_MODIFIED_BY = DynamicModel.identification.LAST_MODIFIED_BY;
                cDMA_INDIVIDUAL_IDENTIFICATION.LAST_MODIFIED_DATE = dateAndTime;
                cDMA_INDIVIDUAL_IDENTIFICATION.PLACE_OF_ISSUANCE = DynamicModel.identification.PLACE_OF_ISSUANCE;
                CDMA_INDIVIDUAL_IDENTIFICATION identification_data = (CDMA_INDIVIDUAL_IDENTIFICATION)this.Session["cDMA_INDIVIDUAL_IDENTIFICATION"];

                var identification_changes = compareIdentificationRecord(cDMA_INDIVIDUAL_IDENTIFICATION, identification_data);
                changesComment = changesComment + identification_changes;
                db.SaveChanges();

                CDMA_INDIVIDUAL_IDENTIFICATION_LOG identification_log = ConvertToIdentificationLog(identification_data);
                identification_log.TIED = tied;
                db.CDMA_INDIVIDUAL_IDENTIFICATION_LOG.Add(identification_log);
                db.SaveChanges();



            }



            //otherdetails	 

            if (cDMA_INDIVIDUAL_OTHER_DETAILS == null)
            {
                CDMA_INDIVIDUAL_OTHER_DETAILS cDMA_INDIVIDUAL_OTHER_DETAILS_SAVE = new CDMA_INDIVIDUAL_OTHER_DETAILS();
                cDMA_INDIVIDUAL_OTHER_DETAILS_SAVE.CREATED_BY = identity.ProfileId.ToString();
                cDMA_INDIVIDUAL_OTHER_DETAILS_SAVE.CREATED_DATE = dateAndTime;
                cDMA_INDIVIDUAL_OTHER_DETAILS_SAVE.CUSTOMER_NO = DynamicModel.BioData.CUSTOMER_NO;
                cDMA_INDIVIDUAL_OTHER_DETAILS_SAVE.IP_ADDRESS = this.Request.ServerVariables["REMOTE_ADDR"];
                cDMA_INDIVIDUAL_OTHER_DETAILS_SAVE.LAST_MODIFIED_DATE = dateAndTime;
                cDMA_INDIVIDUAL_OTHER_DETAILS_SAVE.TIN_NO = DynamicModel.otherdetails.TIN_NO;
                db.CDMA_INDIVIDUAL_OTHER_DETAILS.Add(cDMA_INDIVIDUAL_OTHER_DETAILS_SAVE);
                db.SaveChanges();
            }
            else
            {
                //  var otherdetails_PLACE_OF_ISSUANCE = DynamicModel.otherdetails.AUTHORISED;
                // var otherdetails_PLACE_OF_AUTHORISED_BY = DynamicModel.otherdetails.AUTHORISED_BY;
                // var otherdetails_PLACE_OF_AUTHORISED_DATE = DynamicModel.otherdetails.AUTHORISED_DATE;
                //  var otherdetails_PLACE_OF_CREATED_BY = DynamicModel.otherdetails.CREATED_BY;
                // var otherdetails_PLACE_OF_CREATED_DATE = DynamicModel.otherdetails.CREATED_DATE;
                cDMA_INDIVIDUAL_OTHER_DETAILS.CUSTOMER_NO = DynamicModel.BioData.CUSTOMER_NO;
                cDMA_INDIVIDUAL_OTHER_DETAILS.IP_ADDRESS = this.Request.ServerVariables["REMOTE_ADDR"];
                cDMA_INDIVIDUAL_OTHER_DETAILS.LAST_MODIFIED_DATE = dateAndTime;
                cDMA_INDIVIDUAL_OTHER_DETAILS.TIN_NO = DynamicModel.otherdetails.TIN_NO;
                db.SaveChanges();
                CDMA_INDIVIDUAL_OTHER_DETAILS other_data = (CDMA_INDIVIDUAL_OTHER_DETAILS)this.Session["cDMA_INDIVIDUAL_OTHER_DETAILS"];

                var identification_changes = compareOtherRecord(cDMA_INDIVIDUAL_OTHER_DETAILS, other_data);
                changesComment = changesComment + identification_changes;

                CDMA_INDIVIDUAL_OTHER_DETAILS_LOG other_log = ConvertToOtherLog(other_data);
                other_log.TIED = tied;
                db.CDMA_INDIVIDUAL_OTHER_DETAILS_LOG.Add(other_log);
                db.SaveChanges();



            }



            CDMA_INDIVIDUAL_PROFILE_LOG SAVE_BIODATA_LOG = new CDMA_INDIVIDUAL_PROFILE_LOG();
            SAVE_BIODATA_LOG.AFFECTED_CATEGORY = this.Session["category"].ToString();
            SAVE_BIODATA_LOG.CUSTOMER_NO = c_id;
            SAVE_BIODATA_LOG.LOGGED_BY = Convert.ToDecimal(identity.ProfileId);
            SAVE_BIODATA_LOG.LOGGED_DATE = dateAndTime;
            SAVE_BIODATA_LOG.TIED = tied;
            SAVE_BIODATA_LOG.COMMENTS = changesComment;
            string change_index = String.Join(", ", index_id);
            //int[] change_index = index_id.ToArray();
            SAVE_BIODATA_LOG.CHANGE_INDEX = change_index.ToString();
            db.CDMA_INDIVIDUAL_PROFILE_LOG.Add(SAVE_BIODATA_LOG);
            db.SaveChanges();




            return PartialView("SaveBioData", DynamicModel);


        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApproveBioData(DynamicViewModel DynamicModel)
        {
            var identity = ((CustomPrincipal)User).CustomIdentity;
            var dateAndTime = DateTime.Now;
            var c_id = DynamicModel.BioData.CUSTOMER_NO;

            //create a log before update
            string tied = DateTime.Now.ToString("hhmmssffffff");

 

            CDMA_INDIVIDUAL_BIO_DATA cDMA_INDIVIDUAL_BIO_DATA = db.CDMA_INDIVIDUAL_BIO_DATA.Find(c_id);
            CDMA_INDIVIDUAL_CONTACT_DETAIL cDMA_INDIVIDUAL_CONTACT_DETAIL = db.CDMA_INDIVIDUAL_CONTACT_DETAIL.Find(c_id);
            CDMA_INDIVIDUAL_ADDRESS_DETAIL cDMA_INDIVIDUAL_ADDRESS_DETAIL = db.CDMA_INDIVIDUAL_ADDRESS_DETAIL.Find(c_id);
            CDMA_INDIVIDUAL_IDENTIFICATION cDMA_INDIVIDUAL_IDENTIFICATION = db.CDMA_INDIVIDUAL_IDENTIFICATION.Find(c_id);
            CDMA_INDIVIDUAL_OTHER_DETAILS cDMA_INDIVIDUAL_OTHER_DETAILS = db.CDMA_INDIVIDUAL_OTHER_DETAILS.Find(c_id);

            //Address details
            //  var authrizer = DynamicModel.AddressDetails.AUTHORISED;
            // var AUTHORISED_BY = DynamicModel.AddressDetails.AUTHORISED_BY;
            //   var AUTHORISED_DATE = DynamicModel.AddressDetails.AUTHORISED_DATE;
            // checked if address record does not exist 
            if (cDMA_INDIVIDUAL_ADDRESS_DETAIL == null)
            {
                CDMA_INDIVIDUAL_ADDRESS_DETAIL cDMA_INDIVIDUAL_ADDRESS_DETAIL_SAVE = new CDMA_INDIVIDUAL_ADDRESS_DETAIL();
                cDMA_INDIVIDUAL_ADDRESS_DETAIL_SAVE.CREATED_BY = identity.ProfileId.ToString();
                cDMA_INDIVIDUAL_ADDRESS_DETAIL_SAVE.CREATED_DATE = dateAndTime;
                 cDMA_INDIVIDUAL_ADDRESS_DETAIL_SAVE.CUSTOMER_NO = DynamicModel.BioData.CUSTOMER_NO;
                 cDMA_INDIVIDUAL_ADDRESS_DETAIL_SAVE.LAST_MODIFIED_BY = identity.ProfileId.ToString();
                cDMA_INDIVIDUAL_ADDRESS_DETAIL_SAVE.LAST_MODIFIED_DATE = dateAndTime;
                
                db.CDMA_INDIVIDUAL_ADDRESS_DETAIL.Add(cDMA_INDIVIDUAL_ADDRESS_DETAIL_SAVE);
                db.SaveChanges();
            }
            else
            {
                cDMA_INDIVIDUAL_ADDRESS_DETAIL.AUTHORISED_BY = identity.ProfileId.ToString();
                cDMA_INDIVIDUAL_ADDRESS_DETAIL.AUTHORISED_DATE = dateAndTime;
                cDMA_INDIVIDUAL_ADDRESS_DETAIL.CUSTOMER_NO = DynamicModel.BioData.CUSTOMER_NO;
                cDMA_INDIVIDUAL_ADDRESS_DETAIL.IP_ADDRESS = this.Request.ServerVariables["REMOTE_ADDR"];
                cDMA_INDIVIDUAL_ADDRESS_DETAIL.LAST_MODIFIED_BY = identity.ProfileId.ToString();
                cDMA_INDIVIDUAL_ADDRESS_DETAIL.LAST_MODIFIED_DATE = dateAndTime;
                cDMA_INDIVIDUAL_ADDRESS_DETAIL.NEAREST_BUS_STOP_LANDMARK = DynamicModel.AddressDetails.NEAREST_BUS_STOP_LANDMARK;
                db.SaveChanges(); 

            }

            // Biodata            
            cDMA_INDIVIDUAL_BIO_DATA.AGE = DynamicModel.BioData.AGE;
            cDMA_INDIVIDUAL_BIO_DATA.AUTHORISED = identity.ProfileId.ToString();
            cDMA_INDIVIDUAL_BIO_DATA.AUTHORISED_BY = identity.ProfileId.ToString();
            
            cDMA_INDIVIDUAL_BIO_DATA.AUTHORISED_DATE = dateAndTime;
            cDMA_INDIVIDUAL_BIO_DATA.LAST_MODIFIED_DATE = dateAndTime;             
            db.SaveChanges();

            /*

            //contact	 
            //var CONTACT_AUTHORISED =  DynamicModel.contact.AUTHORISED;
            // var CONTACT_AUTHORISED_BY = DynamicModel.contact.AUTHORISED_BY;
            //  var CONTACT_AUTHORISED_DATE = DynamicModel.contact.AUTHORISED_DATE;
            //  var CONTACT_CREATED_BY = DynamicModel.contact.CREATED_BY;
            //  var CONTACT_CREATED_DATE = DynamicModel.contact.CREATED_DATE;
            //  var CONTACT_CUSTOMER_NO = DynamicModel.contact.CUSTOMER_NO;
            if (cDMA_INDIVIDUAL_CONTACT_DETAIL == null)
            {
                CDMA_INDIVIDUAL_CONTACT_DETAIL cDMA_INDIVIDUAL_CONTACT_DETAIL_SAVE = new CDMA_INDIVIDUAL_CONTACT_DETAIL();
                cDMA_INDIVIDUAL_CONTACT_DETAIL_SAVE.CUSTOMER_NO = DynamicModel.BioData.CUSTOMER_NO;
                cDMA_INDIVIDUAL_CONTACT_DETAIL_SAVE.CREATED_BY = identity.ProfileId.ToString();
                cDMA_INDIVIDUAL_CONTACT_DETAIL_SAVE.CREATED_DATE = dateAndTime;
                cDMA_INDIVIDUAL_CONTACT_DETAIL_SAVE.MAILING_ADDRESS = DynamicModel.contact.MAILING_ADDRESS;
                cDMA_INDIVIDUAL_CONTACT_DETAIL_SAVE.MOBILE_NO = DynamicModel.contact.MOBILE_NO;
                cDMA_INDIVIDUAL_CONTACT_DETAIL_SAVE.EMAIL_ADDRESS = DynamicModel.contact.EMAIL_ADDRESS;
                cDMA_INDIVIDUAL_CONTACT_DETAIL_SAVE.IP_ADDRESS = this.Request.ServerVariables["REMOTE_ADDR"];
                cDMA_INDIVIDUAL_CONTACT_DETAIL_SAVE.LAST_MODIFIED_BY = identity.ProfileId.ToString();
                db.CDMA_INDIVIDUAL_CONTACT_DETAIL.Add(cDMA_INDIVIDUAL_CONTACT_DETAIL_SAVE);
                db.SaveChanges();

            }
            else
            {
                cDMA_INDIVIDUAL_CONTACT_DETAIL.LAST_MODIFIED_BY = identity.ProfileId.ToString();
                cDMA_INDIVIDUAL_CONTACT_DETAIL.MAILING_ADDRESS = DynamicModel.contact.MAILING_ADDRESS;
                cDMA_INDIVIDUAL_CONTACT_DETAIL.MOBILE_NO = DynamicModel.contact.MOBILE_NO;
                cDMA_INDIVIDUAL_CONTACT_DETAIL.EMAIL_ADDRESS = DynamicModel.contact.EMAIL_ADDRESS;
                cDMA_INDIVIDUAL_CONTACT_DETAIL.IP_ADDRESS = this.Request.ServerVariables["REMOTE_ADDR"];
                cDMA_INDIVIDUAL_CONTACT_DETAIL.LAST_MODIFIED_BY = identity.ProfileId.ToString();
                cDMA_INDIVIDUAL_CONTACT_DETAIL.LAST_MODIFIED_DATE = dateAndTime;
                db.SaveChanges();
                CDMA_INDIVIDUAL_CONTACT_DETAIL contact_data = (CDMA_INDIVIDUAL_CONTACT_DETAIL)this.Session["cDMA_INDIVIDUAL_CONTACT_DETAIL"];

                var contact_changes = compareContactRecord(cDMA_INDIVIDUAL_CONTACT_DETAIL, contact_data);
                changesComment = changesComment + contact_changes;

                CDMA_INDIVIDUAL_CONTACT_LOG contact_log = ConvertToContactDataLog(contact_data);
                contact_log.TIED = tied;
                db.CDMA_INDIVIDUAL_CONTACT_LOG.Add(contact_log);
                db.SaveChanges();
            }



            // identification 
            if (cDMA_INDIVIDUAL_IDENTIFICATION == null)
            {
                CDMA_INDIVIDUAL_IDENTIFICATION cDMA_INDIVIDUAL_IDENTIFICATION_SAVE = new CDMA_INDIVIDUAL_IDENTIFICATION();
                cDMA_INDIVIDUAL_IDENTIFICATION_SAVE.CREATED_BY = identity.ProfileId.ToString();
                cDMA_INDIVIDUAL_IDENTIFICATION_SAVE.CREATED_DATE = dateAndTime;
                cDMA_INDIVIDUAL_IDENTIFICATION_SAVE.CUSTOMER_NO = DynamicModel.BioData.CUSTOMER_NO;
                cDMA_INDIVIDUAL_IDENTIFICATION_SAVE.IDENTIFICATION_TYPE = Request["CDMA_IDENTIFICATION_TYPE"];
                cDMA_INDIVIDUAL_IDENTIFICATION_SAVE.ID_EXPIRY_DATE = DynamicModel.identification.ID_EXPIRY_DATE;
                cDMA_INDIVIDUAL_IDENTIFICATION_SAVE.ID_ISSUE_DATE = DynamicModel.identification.ID_ISSUE_DATE;
                cDMA_INDIVIDUAL_IDENTIFICATION_SAVE.ID_NO = DynamicModel.identification.ID_NO;
                cDMA_INDIVIDUAL_IDENTIFICATION_SAVE.IP_ADDRESS = this.Request.ServerVariables["REMOTE_ADDR"];
                cDMA_INDIVIDUAL_IDENTIFICATION_SAVE.LAST_MODIFIED_BY = DynamicModel.identification.LAST_MODIFIED_BY;
                cDMA_INDIVIDUAL_IDENTIFICATION_SAVE.LAST_MODIFIED_DATE = dateAndTime;
                cDMA_INDIVIDUAL_IDENTIFICATION_SAVE.PLACE_OF_ISSUANCE = DynamicModel.identification.PLACE_OF_ISSUANCE;
                db.CDMA_INDIVIDUAL_IDENTIFICATION.Add(cDMA_INDIVIDUAL_IDENTIFICATION_SAVE);
                db.SaveChanges();

            }
            else
            {
                //var identification_AUTHORISED =  DynamicModel.identification.AUTHORISED;
                //var identification_AUTHORISED_BY = DynamicModel.identification.AUTHORISED_BY;
                // var identification_AUTHORISED_DATE = DynamicModel.identification.AUTHORISED_DATE;
                //var identification_CREATED_BY = DynamicModel.identification.CREATED_BY;
                // var identification_CREATED_DATE = DynamicModel.identification.CREATED_DATE;
                cDMA_INDIVIDUAL_IDENTIFICATION.CUSTOMER_NO = DynamicModel.BioData.CUSTOMER_NO;
                cDMA_INDIVIDUAL_IDENTIFICATION.IDENTIFICATION_TYPE = Request["CDMA_IDENTIFICATION_TYPE"];
                cDMA_INDIVIDUAL_IDENTIFICATION.ID_EXPIRY_DATE = DynamicModel.identification.ID_EXPIRY_DATE;
                cDMA_INDIVIDUAL_IDENTIFICATION.ID_ISSUE_DATE = DynamicModel.identification.ID_ISSUE_DATE;
                cDMA_INDIVIDUAL_IDENTIFICATION.ID_NO = DynamicModel.identification.ID_NO;
                cDMA_INDIVIDUAL_IDENTIFICATION.IP_ADDRESS = this.Request.ServerVariables["REMOTE_ADDR"];
                cDMA_INDIVIDUAL_IDENTIFICATION.LAST_MODIFIED_BY = DynamicModel.identification.LAST_MODIFIED_BY;
                cDMA_INDIVIDUAL_IDENTIFICATION.LAST_MODIFIED_DATE = dateAndTime;
                cDMA_INDIVIDUAL_IDENTIFICATION.PLACE_OF_ISSUANCE = DynamicModel.identification.PLACE_OF_ISSUANCE;
                CDMA_INDIVIDUAL_IDENTIFICATION identification_data = (CDMA_INDIVIDUAL_IDENTIFICATION)this.Session["cDMA_INDIVIDUAL_IDENTIFICATION"];

                var identification_changes = compareIdentificationRecord(cDMA_INDIVIDUAL_IDENTIFICATION, identification_data);
                changesComment = changesComment + identification_changes;
                db.SaveChanges();

                CDMA_INDIVIDUAL_IDENTIFICATION_LOG identification_log = ConvertToIdentificationLog(identification_data);
                identification_log.TIED = tied;
                db.CDMA_INDIVIDUAL_IDENTIFICATION_LOG.Add(identification_log);
                db.SaveChanges();



            }



            //otherdetails	 

            if (cDMA_INDIVIDUAL_OTHER_DETAILS == null)
            {
                CDMA_INDIVIDUAL_OTHER_DETAILS cDMA_INDIVIDUAL_OTHER_DETAILS_SAVE = new CDMA_INDIVIDUAL_OTHER_DETAILS();
                cDMA_INDIVIDUAL_OTHER_DETAILS_SAVE.CREATED_BY = identity.ProfileId.ToString();
                cDMA_INDIVIDUAL_OTHER_DETAILS_SAVE.CREATED_DATE = dateAndTime;
                cDMA_INDIVIDUAL_OTHER_DETAILS_SAVE.CUSTOMER_NO = DynamicModel.BioData.CUSTOMER_NO;
                cDMA_INDIVIDUAL_OTHER_DETAILS_SAVE.IP_ADDRESS = this.Request.ServerVariables["REMOTE_ADDR"];
                cDMA_INDIVIDUAL_OTHER_DETAILS_SAVE.LAST_MODIFIED_DATE = dateAndTime;
                cDMA_INDIVIDUAL_OTHER_DETAILS_SAVE.TIN_NO = DynamicModel.otherdetails.TIN_NO;
                db.CDMA_INDIVIDUAL_OTHER_DETAILS.Add(cDMA_INDIVIDUAL_OTHER_DETAILS_SAVE);
                db.SaveChanges();
            }
            else
            {
                //  var otherdetails_PLACE_OF_ISSUANCE = DynamicModel.otherdetails.AUTHORISED;
                // var otherdetails_PLACE_OF_AUTHORISED_BY = DynamicModel.otherdetails.AUTHORISED_BY;
                // var otherdetails_PLACE_OF_AUTHORISED_DATE = DynamicModel.otherdetails.AUTHORISED_DATE;
                //  var otherdetails_PLACE_OF_CREATED_BY = DynamicModel.otherdetails.CREATED_BY;
                // var otherdetails_PLACE_OF_CREATED_DATE = DynamicModel.otherdetails.CREATED_DATE;
                cDMA_INDIVIDUAL_OTHER_DETAILS.CUSTOMER_NO = DynamicModel.BioData.CUSTOMER_NO;
                cDMA_INDIVIDUAL_OTHER_DETAILS.IP_ADDRESS = this.Request.ServerVariables["REMOTE_ADDR"];
                cDMA_INDIVIDUAL_OTHER_DETAILS.LAST_MODIFIED_DATE = dateAndTime;
                cDMA_INDIVIDUAL_OTHER_DETAILS.TIN_NO = DynamicModel.otherdetails.TIN_NO;
                db.SaveChanges();
                CDMA_INDIVIDUAL_OTHER_DETAILS other_data = (CDMA_INDIVIDUAL_OTHER_DETAILS)this.Session["cDMA_INDIVIDUAL_OTHER_DETAILS"];

                var identification_changes = compareOtherRecord(cDMA_INDIVIDUAL_OTHER_DETAILS, other_data);
                changesComment = changesComment + identification_changes;

                CDMA_INDIVIDUAL_OTHER_DETAILS_LOG other_log = ConvertToOtherLog(other_data);
                other_log.TIED = tied;
                db.CDMA_INDIVIDUAL_OTHER_DETAILS_LOG.Add(other_log);
                db.SaveChanges();

            

            }



            CDMA_INDIVIDUAL_PROFILE_LOG SAVE_BIODATA_LOG = new CDMA_INDIVIDUAL_PROFILE_LOG();
            SAVE_BIODATA_LOG.AFFECTED_CATEGORY = this.Session["category"].ToString();
            SAVE_BIODATA_LOG.CUSTOMER_NO = c_id;
            SAVE_BIODATA_LOG.LOGGED_BY = Convert.ToDecimal(identity.ProfileId);
            SAVE_BIODATA_LOG.LOGGED_DATE = dateAndTime;
            SAVE_BIODATA_LOG.TIED = tied;
            SAVE_BIODATA_LOG.COMMENTS = changesComment;
            string change_index = String.Join(", ", index_id);
            //int[] change_index = index_id.ToArray();
            SAVE_BIODATA_LOG.CHANGE_INDEX = change_index.ToString();
            db.CDMA_INDIVIDUAL_PROFILE_LOG.Add(SAVE_BIODATA_LOG);
            db.SaveChanges();


            */

            return PartialView("SaveBioData", DynamicModel);


        }








        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveBioData(DynamicViewModel DynamicModel) {
            var identity = ((CustomPrincipal)User).CustomIdentity;
            var dateAndTime = DateTime.Now;
            var c_id = DynamicModel.BioData.CUSTOMER_NO;

            //create a log before update
            string tied = DateTime.Now.ToString("hhmmssffffff");
           

            /*
            
           // log Individual Address Details
            var address = (CDMA_INDIVIDUAL_ADDRESS_DETAIL) this.Session["cDMA_INDIVIDUAL_ADDRESS_DETAIL"];
            CDMA_INDIVIDUAL_ADDRESS_DETAIL_LOG address_log = ConvertToAddressLog(address);
            address_log.TIED = tied;
            db.CDMA_INDIVIDUAL_ADDRESS_DETAIL_LOG.Add(address_log);
            db.SaveChanges();

            // log Biodata Details
            CDMA_INDIVIDUAL_BIO_DATA bio_data = (CDMA_INDIVIDUAL_BIO_DATA)this.Session["cDMA_INDIVIDUAL_BIO_DATA"];
            CDMA_INDIVIDUAL_BIO_LOG biodata_log = ConvertToBioDataLog(bio_data);
            biodata_log.TIED = tied;
            db.CDMA_INDIVIDUAL_BIO_LOG.Add(biodata_log);
            db.SaveChanges();

            // log contact Details
            CDMA_INDIVIDUAL_CONTACT_DETAIL contact_data = (CDMA_INDIVIDUAL_CONTACT_DETAIL)this.Session["cDMA_INDIVIDUAL_CONTACT_DETAIL"];
            CDMA_INDIVIDUAL_CONTACT_LOG contact_log = ConvertToContactDataLog(contact_data);
            contact_log.TIED = tied;
            db.CDMA_INDIVIDUAL_CONTACT_LOG.Add(contact_log);
            db.SaveChanges();

            // identification
            CDMA_INDIVIDUAL_IDENTIFICATION identification_data = (CDMA_INDIVIDUAL_IDENTIFICATION)this.Session["cDMA_INDIVIDUAL_IDENTIFICATION"];
            CDMA_INDIVIDUAL_IDENTIFICATION_LOG identification_log = ConvertToIdentificationLog(identification_data);
            identification_log.TIED = tied;
            db.CDMA_INDIVIDUAL_IDENTIFICATION_LOG.Add(identification_log);
            db.SaveChanges();

            // OTHER DETAILS
            CDMA_INDIVIDUAL_OTHER_DETAILS other_data = (CDMA_INDIVIDUAL_OTHER_DETAILS)this.Session["cDMA_INDIVIDUAL_OTHER_DETAILS"];
            CDMA_INDIVIDUAL_OTHER_DETAILS_LOG other_log = ConvertToOtherLog(other_data);
            other_log.TIED = tied;
            db.CDMA_INDIVIDUAL_OTHER_DETAILS_LOG.Add(other_log);
            db.SaveChanges();
            */


            CDMA_INDIVIDUAL_BIO_DATA cDMA_INDIVIDUAL_BIO_DATA = db.CDMA_INDIVIDUAL_BIO_DATA.Find(c_id);
            CDMA_INDIVIDUAL_CONTACT_DETAIL cDMA_INDIVIDUAL_CONTACT_DETAIL = db.CDMA_INDIVIDUAL_CONTACT_DETAIL.Find(c_id);
            CDMA_INDIVIDUAL_ADDRESS_DETAIL cDMA_INDIVIDUAL_ADDRESS_DETAIL = db.CDMA_INDIVIDUAL_ADDRESS_DETAIL.Find(c_id);
            CDMA_INDIVIDUAL_IDENTIFICATION cDMA_INDIVIDUAL_IDENTIFICATION = db.CDMA_INDIVIDUAL_IDENTIFICATION.Find(c_id);
            CDMA_INDIVIDUAL_OTHER_DETAILS cDMA_INDIVIDUAL_OTHER_DETAILS = db.CDMA_INDIVIDUAL_OTHER_DETAILS.Find(c_id);

            //Address details
            //  var authrizer = DynamicModel.AddressDetails.AUTHORISED;
            // var AUTHORISED_BY = DynamicModel.AddressDetails.AUTHORISED_BY;
            //   var AUTHORISED_DATE = DynamicModel.AddressDetails.AUTHORISED_DATE;
           // checked if address record does not exist 
            if (cDMA_INDIVIDUAL_ADDRESS_DETAIL == null) {
                CDMA_INDIVIDUAL_ADDRESS_DETAIL cDMA_INDIVIDUAL_ADDRESS_DETAIL_SAVE = new CDMA_INDIVIDUAL_ADDRESS_DETAIL();
                cDMA_INDIVIDUAL_ADDRESS_DETAIL_SAVE.CREATED_BY = identity.ProfileId.ToString();
                cDMA_INDIVIDUAL_ADDRESS_DETAIL_SAVE.CREATED_DATE = dateAndTime;
                cDMA_INDIVIDUAL_ADDRESS_DETAIL_SAVE.CITY_TOWN_OF_RESIDENCE = DynamicModel.AddressDetails.CITY_TOWN_OF_RESIDENCE;
                cDMA_INDIVIDUAL_ADDRESS_DETAIL_SAVE.COUNTRY_OF_RESIDENCE = Request["COUNTRY_OF_RESIDENCE"]; // DynamicModel.AddressDetails.COUNTRY_OF_RESIDENCE;
                cDMA_INDIVIDUAL_ADDRESS_DETAIL_SAVE.CUSTOMER_NO = DynamicModel.BioData.CUSTOMER_NO;
                cDMA_INDIVIDUAL_ADDRESS_DETAIL_SAVE.IP_ADDRESS = this.Request.ServerVariables["REMOTE_ADDR"];
                cDMA_INDIVIDUAL_ADDRESS_DETAIL_SAVE.LAST_MODIFIED_BY = identity.ProfileId.ToString();
                cDMA_INDIVIDUAL_ADDRESS_DETAIL_SAVE.LAST_MODIFIED_DATE = dateAndTime;
                cDMA_INDIVIDUAL_ADDRESS_DETAIL_SAVE.LGA_OF_RESIDENCE = Request["LGA"]; // DynamicModel.AddressDetails.LGA_OF_RESIDENCE;
                cDMA_INDIVIDUAL_ADDRESS_DETAIL_SAVE.NEAREST_BUS_STOP_LANDMARK = DynamicModel.AddressDetails.NEAREST_BUS_STOP_LANDMARK;
                cDMA_INDIVIDUAL_ADDRESS_DETAIL_SAVE.RESIDENCE_OWNED_OR_RENT = DynamicModel.AddressDetails.RESIDENCE_OWNED_OR_RENT;//Request["RESIDENCE_OWNED_OR_RENT"]; 
                cDMA_INDIVIDUAL_ADDRESS_DETAIL_SAVE.RESIDENTIAL_ADDRESS = DynamicModel.AddressDetails.CITY_TOWN_OF_RESIDENCE;
                cDMA_INDIVIDUAL_ADDRESS_DETAIL_SAVE.STATE_OF_RESIDENCE = Request["STATES"];  //DynamicModel.AddressDetails.STATE_OF_RESIDENCE;
                cDMA_INDIVIDUAL_ADDRESS_DETAIL_SAVE.ZIP_POSTAL_CODE = DynamicModel.AddressDetails.ZIP_POSTAL_CODE;
                
                db.CDMA_INDIVIDUAL_ADDRESS_DETAIL.Add(cDMA_INDIVIDUAL_ADDRESS_DETAIL_SAVE);
                db.SaveChanges();
            }
            else
            { 
            cDMA_INDIVIDUAL_ADDRESS_DETAIL.CITY_TOWN_OF_RESIDENCE = DynamicModel.AddressDetails.CITY_TOWN_OF_RESIDENCE;
            cDMA_INDIVIDUAL_ADDRESS_DETAIL.COUNTRY_OF_RESIDENCE = Request["COUNTRY_OF_RESIDENCE"]; // DynamicModel.AddressDetails.COUNTRY_OF_RESIDENCE;
            cDMA_INDIVIDUAL_ADDRESS_DETAIL.CUSTOMER_NO = DynamicModel.BioData.CUSTOMER_NO;
            cDMA_INDIVIDUAL_ADDRESS_DETAIL.IP_ADDRESS = this.Request.ServerVariables["REMOTE_ADDR"];
            cDMA_INDIVIDUAL_ADDRESS_DETAIL.LAST_MODIFIED_BY = identity.ProfileId.ToString();
            cDMA_INDIVIDUAL_ADDRESS_DETAIL.LAST_MODIFIED_DATE = dateAndTime;
            cDMA_INDIVIDUAL_ADDRESS_DETAIL.LGA_OF_RESIDENCE = Request["LGA"]; // DynamicModel.AddressDetails.LGA_OF_RESIDENCE;
            cDMA_INDIVIDUAL_ADDRESS_DETAIL.NEAREST_BUS_STOP_LANDMARK = DynamicModel.AddressDetails.NEAREST_BUS_STOP_LANDMARK;
            cDMA_INDIVIDUAL_ADDRESS_DETAIL.RESIDENCE_OWNED_OR_RENT = Request["RESIDENCE_OWNED_OR_RENT"];
            cDMA_INDIVIDUAL_ADDRESS_DETAIL.RESIDENTIAL_ADDRESS = DynamicModel.AddressDetails.RESIDENTIAL_ADDRESS;
            cDMA_INDIVIDUAL_ADDRESS_DETAIL.STATE_OF_RESIDENCE = Request["STATES_RES"];  //DynamicModel.AddressDetails.STATE_OF_RESIDENCE;
            cDMA_INDIVIDUAL_ADDRESS_DETAIL.ZIP_POSTAL_CODE = DynamicModel.AddressDetails.ZIP_POSTAL_CODE;
            cDMA_INDIVIDUAL_ADDRESS_DETAIL.RESIDENCE_OWNED_OR_RENT = DynamicModel.AddressDetails.RESIDENCE_OWNED_OR_RENT;
            db.SaveChanges();

               
            var address = (CDMA_INDIVIDUAL_ADDRESS_DETAIL)this.Session["cDMA_INDIVIDUAL_ADDRESS_DETAIL"];
            // compare the Old record with the new one and get comment
            var address_changes  = compareAddress(cDMA_INDIVIDUAL_ADDRESS_DETAIL, address);
                changesComment = changesComment + address_changes;
            // log address details
            CDMA_INDIVIDUAL_ADDRESS_DETAIL_LOG address_log = ConvertToAddressLog(address);
            address_log.TIED = tied;
            db.CDMA_INDIVIDUAL_ADDRESS_DETAIL_LOG.Add(address_log);
            db.SaveChanges();


            }

            // Biodata            
            cDMA_INDIVIDUAL_BIO_DATA.AGE = DynamicModel.BioData.AGE;
           //var AUTHORISED = DynamicModel.BioData.AUTHORISED;
           //var BIO_AUTHORISED_BY = DynamicModel.BioData.AUTHORISED_BY;
           // var BIO_AUTHORISED_DATE = DynamicModel.BioData.AUTHORISED_DATE;
           cDMA_INDIVIDUAL_BIO_DATA.BRANCH_CODE = Request["BRANCH"];  //DynamicModel.BioData.BRANCH_CODE;
           cDMA_INDIVIDUAL_BIO_DATA.COMPLEXION = DynamicModel.BioData.COMPLEXION;
           var COUNTRY_OF_BIRTH = Request["COUNTRY_OF_BIRTH"]; // DynamicModel.BioData.COUNTRY_OF_BIRTH;
                                                               // var BIO_CREATED_BY = DynamicModel.BioData.CREATED_BY;
                                                               // var BIO_CREATED_DATE = DynamicModel.BioData.CREATED_DATE;
           cDMA_INDIVIDUAL_BIO_DATA.CUSTOMER_NO = DynamicModel.BioData.CUSTOMER_NO;
           cDMA_INDIVIDUAL_BIO_DATA.DATE_OF_BIRTH = DynamicModel.BioData.DATE_OF_BIRTH;
           cDMA_INDIVIDUAL_BIO_DATA.DISABILITY = DynamicModel.BioData.DISABILITY;
           cDMA_INDIVIDUAL_BIO_DATA.FIRST_NAME = DynamicModel.BioData.FIRST_NAME;
           cDMA_INDIVIDUAL_BIO_DATA.IP_ADDRESS = this.Request.ServerVariables["REMOTE_ADDR"];
           cDMA_INDIVIDUAL_BIO_DATA.LAST_MODIFIED_DATE = dateAndTime;//DynamicModel.BioData.LAST_MODIFIED_DATE;
           cDMA_INDIVIDUAL_BIO_DATA.MARITAL_STATUS = DynamicModel.BioData.MARITAL_STATUS;
           cDMA_INDIVIDUAL_BIO_DATA.MOTHER_MAIDEN_NAME = DynamicModel.BioData.MOTHER_MAIDEN_NAME;
           cDMA_INDIVIDUAL_BIO_DATA.NATIONALITY = Request["BioData_NATIONALITY"];  //// DynamicModel.BioData.NATIONALITY;
           cDMA_INDIVIDUAL_BIO_DATA.NICKNAME_ALIAS = DynamicModel.BioData.NICKNAME_ALIAS;
           cDMA_INDIVIDUAL_BIO_DATA.NUMBER_OF_CHILDREN = DynamicModel.BioData.NUMBER_OF_CHILDREN;
           cDMA_INDIVIDUAL_BIO_DATA.OTHER_NAME = DynamicModel.BioData.OTHER_NAME;
           cDMA_INDIVIDUAL_BIO_DATA.PLACE_OF_BIRTH = DynamicModel.BioData.PLACE_OF_BIRTH;
           cDMA_INDIVIDUAL_BIO_DATA.RELIGION = DynamicModel.BioData.RELIGION;
           cDMA_INDIVIDUAL_BIO_DATA.SEX = DynamicModel.BioData.SEX;
           cDMA_INDIVIDUAL_BIO_DATA.STATE_OF_ORIGIN = DynamicModel.BioData.STATE_OF_ORIGIN;
           cDMA_INDIVIDUAL_BIO_DATA.SURNAME = DynamicModel.BioData.SURNAME;
           cDMA_INDIVIDUAL_BIO_DATA.TITLE = DynamicModel.BioData.TITLE;
           cDMA_INDIVIDUAL_BIO_DATA.RELIGION = Request["RELIGION"];
           cDMA_INDIVIDUAL_BIO_DATA.COUNTRY_OF_BIRTH = Request["COUNTRY_OF_BIRTH"];           
           db.SaveChanges();

           CDMA_INDIVIDUAL_BIO_DATA bio_data = (CDMA_INDIVIDUAL_BIO_DATA)this.Session["cDMA_INDIVIDUAL_BIO_DATA"];
           // compare old bio record with the new bio record
           var biodata_changes = compareBioRecord(cDMA_INDIVIDUAL_BIO_DATA, bio_data);
           changesComment = changesComment + biodata_changes;


            CDMA_INDIVIDUAL_BIO_LOG biodata_log = ConvertToBioDataLog(bio_data);
            biodata_log.TIED = tied;
            db.CDMA_INDIVIDUAL_BIO_LOG.Add(biodata_log);
            db.SaveChanges();


            //contact	 
            //var CONTACT_AUTHORISED =  DynamicModel.contact.AUTHORISED;
            // var CONTACT_AUTHORISED_BY = DynamicModel.contact.AUTHORISED_BY;
            //  var CONTACT_AUTHORISED_DATE = DynamicModel.contact.AUTHORISED_DATE;
            //  var CONTACT_CREATED_BY = DynamicModel.contact.CREATED_BY;
            //  var CONTACT_CREATED_DATE = DynamicModel.contact.CREATED_DATE;
            //  var CONTACT_CUSTOMER_NO = DynamicModel.contact.CUSTOMER_NO;
            if (cDMA_INDIVIDUAL_CONTACT_DETAIL == null)
            {
                CDMA_INDIVIDUAL_CONTACT_DETAIL cDMA_INDIVIDUAL_CONTACT_DETAIL_SAVE = new CDMA_INDIVIDUAL_CONTACT_DETAIL();
                cDMA_INDIVIDUAL_CONTACT_DETAIL_SAVE.CUSTOMER_NO = DynamicModel.BioData.CUSTOMER_NO;
                cDMA_INDIVIDUAL_CONTACT_DETAIL_SAVE.CREATED_BY = identity.ProfileId.ToString();
                cDMA_INDIVIDUAL_CONTACT_DETAIL_SAVE.CREATED_DATE = dateAndTime;
                cDMA_INDIVIDUAL_CONTACT_DETAIL_SAVE.MAILING_ADDRESS = DynamicModel.contact.MAILING_ADDRESS;
                cDMA_INDIVIDUAL_CONTACT_DETAIL_SAVE.MOBILE_NO = DynamicModel.contact.MOBILE_NO;
                cDMA_INDIVIDUAL_CONTACT_DETAIL_SAVE.EMAIL_ADDRESS = DynamicModel.contact.EMAIL_ADDRESS;
                cDMA_INDIVIDUAL_CONTACT_DETAIL_SAVE.IP_ADDRESS = this.Request.ServerVariables["REMOTE_ADDR"];
                cDMA_INDIVIDUAL_CONTACT_DETAIL_SAVE.LAST_MODIFIED_BY = identity.ProfileId.ToString();
                db.CDMA_INDIVIDUAL_CONTACT_DETAIL.Add(cDMA_INDIVIDUAL_CONTACT_DETAIL_SAVE);
                db.SaveChanges();

            }else {
              cDMA_INDIVIDUAL_CONTACT_DETAIL.LAST_MODIFIED_BY = identity.ProfileId.ToString();
              cDMA_INDIVIDUAL_CONTACT_DETAIL.MAILING_ADDRESS = DynamicModel.contact.MAILING_ADDRESS;
              cDMA_INDIVIDUAL_CONTACT_DETAIL.MOBILE_NO = DynamicModel.contact.MOBILE_NO;
              cDMA_INDIVIDUAL_CONTACT_DETAIL.EMAIL_ADDRESS = DynamicModel.contact.EMAIL_ADDRESS;
              cDMA_INDIVIDUAL_CONTACT_DETAIL.IP_ADDRESS = this.Request.ServerVariables["REMOTE_ADDR"];
              cDMA_INDIVIDUAL_CONTACT_DETAIL.LAST_MODIFIED_BY = identity.ProfileId.ToString();
              cDMA_INDIVIDUAL_CONTACT_DETAIL.LAST_MODIFIED_DATE = dateAndTime;
              db.SaveChanges();
              CDMA_INDIVIDUAL_CONTACT_DETAIL contact_data = (CDMA_INDIVIDUAL_CONTACT_DETAIL)this.Session["cDMA_INDIVIDUAL_CONTACT_DETAIL"];

              var contact_changes = compareContactRecord(cDMA_INDIVIDUAL_CONTACT_DETAIL, contact_data);
              changesComment = changesComment + contact_changes;

              CDMA_INDIVIDUAL_CONTACT_LOG contact_log = ConvertToContactDataLog(contact_data);
              contact_log.TIED = tied;
              db.CDMA_INDIVIDUAL_CONTACT_LOG.Add(contact_log);
              db.SaveChanges();
            }



            // identification 
            if (cDMA_INDIVIDUAL_IDENTIFICATION == null)
            {
                CDMA_INDIVIDUAL_IDENTIFICATION cDMA_INDIVIDUAL_IDENTIFICATION_SAVE= new CDMA_INDIVIDUAL_IDENTIFICATION();
                cDMA_INDIVIDUAL_IDENTIFICATION_SAVE.CREATED_BY = identity.ProfileId.ToString();
                cDMA_INDIVIDUAL_IDENTIFICATION_SAVE.CREATED_DATE = dateAndTime;
                cDMA_INDIVIDUAL_IDENTIFICATION_SAVE.CUSTOMER_NO = DynamicModel.BioData.CUSTOMER_NO;
                cDMA_INDIVIDUAL_IDENTIFICATION_SAVE.IDENTIFICATION_TYPE = Request["CDMA_IDENTIFICATION_TYPE"];
                cDMA_INDIVIDUAL_IDENTIFICATION_SAVE.ID_EXPIRY_DATE = DynamicModel.identification.ID_EXPIRY_DATE;
                cDMA_INDIVIDUAL_IDENTIFICATION_SAVE.ID_ISSUE_DATE = DynamicModel.identification.ID_ISSUE_DATE;
                cDMA_INDIVIDUAL_IDENTIFICATION_SAVE.ID_NO = DynamicModel.identification.ID_NO;
                cDMA_INDIVIDUAL_IDENTIFICATION_SAVE.IP_ADDRESS = this.Request.ServerVariables["REMOTE_ADDR"];
                cDMA_INDIVIDUAL_IDENTIFICATION_SAVE.LAST_MODIFIED_BY = DynamicModel.identification.LAST_MODIFIED_BY;
                cDMA_INDIVIDUAL_IDENTIFICATION_SAVE.LAST_MODIFIED_DATE = dateAndTime;
                cDMA_INDIVIDUAL_IDENTIFICATION_SAVE.PLACE_OF_ISSUANCE = DynamicModel.identification.PLACE_OF_ISSUANCE;
                db.CDMA_INDIVIDUAL_IDENTIFICATION.Add(cDMA_INDIVIDUAL_IDENTIFICATION_SAVE);
                db.SaveChanges();

            }else { 
            //var identification_AUTHORISED =  DynamicModel.identification.AUTHORISED;
            //var identification_AUTHORISED_BY = DynamicModel.identification.AUTHORISED_BY;
            // var identification_AUTHORISED_DATE = DynamicModel.identification.AUTHORISED_DATE;
            //var identification_CREATED_BY = DynamicModel.identification.CREATED_BY;
            // var identification_CREATED_DATE = DynamicModel.identification.CREATED_DATE;
                  cDMA_INDIVIDUAL_IDENTIFICATION.CUSTOMER_NO = DynamicModel.BioData.CUSTOMER_NO;
                  cDMA_INDIVIDUAL_IDENTIFICATION.IDENTIFICATION_TYPE = Request["CDMA_IDENTIFICATION_TYPE"];
                  cDMA_INDIVIDUAL_IDENTIFICATION.ID_EXPIRY_DATE = DynamicModel.identification.ID_EXPIRY_DATE;
                  cDMA_INDIVIDUAL_IDENTIFICATION.ID_ISSUE_DATE = DynamicModel.identification.ID_ISSUE_DATE;
                  cDMA_INDIVIDUAL_IDENTIFICATION.ID_NO = DynamicModel.identification.ID_NO;
                  cDMA_INDIVIDUAL_IDENTIFICATION.IP_ADDRESS = this.Request.ServerVariables["REMOTE_ADDR"];
                  cDMA_INDIVIDUAL_IDENTIFICATION.LAST_MODIFIED_BY = DynamicModel.identification.LAST_MODIFIED_BY;
                  cDMA_INDIVIDUAL_IDENTIFICATION.LAST_MODIFIED_DATE = dateAndTime;
                  cDMA_INDIVIDUAL_IDENTIFICATION.PLACE_OF_ISSUANCE = DynamicModel.identification.PLACE_OF_ISSUANCE;
                CDMA_INDIVIDUAL_IDENTIFICATION identification_data = (CDMA_INDIVIDUAL_IDENTIFICATION)this.Session["cDMA_INDIVIDUAL_IDENTIFICATION"];

                var identification_changes = compareIdentificationRecord(cDMA_INDIVIDUAL_IDENTIFICATION, identification_data);
                changesComment = changesComment + identification_changes;
                db.SaveChanges();

                CDMA_INDIVIDUAL_IDENTIFICATION_LOG identification_log = ConvertToIdentificationLog(identification_data);
                identification_log.TIED = tied;
                db.CDMA_INDIVIDUAL_IDENTIFICATION_LOG.Add(identification_log);
                db.SaveChanges();

               
                
            }



            //otherdetails	 

            if (cDMA_INDIVIDUAL_OTHER_DETAILS == null)
            {
                CDMA_INDIVIDUAL_OTHER_DETAILS cDMA_INDIVIDUAL_OTHER_DETAILS_SAVE = new CDMA_INDIVIDUAL_OTHER_DETAILS();
                cDMA_INDIVIDUAL_OTHER_DETAILS_SAVE.CREATED_BY = identity.ProfileId.ToString();
                cDMA_INDIVIDUAL_OTHER_DETAILS_SAVE.CREATED_DATE = dateAndTime;
                cDMA_INDIVIDUAL_OTHER_DETAILS_SAVE.CUSTOMER_NO = DynamicModel.BioData.CUSTOMER_NO;
                cDMA_INDIVIDUAL_OTHER_DETAILS_SAVE.IP_ADDRESS = this.Request.ServerVariables["REMOTE_ADDR"];
                cDMA_INDIVIDUAL_OTHER_DETAILS_SAVE.LAST_MODIFIED_DATE = dateAndTime;
                cDMA_INDIVIDUAL_OTHER_DETAILS_SAVE.TIN_NO = DynamicModel.otherdetails.TIN_NO;
                db.CDMA_INDIVIDUAL_OTHER_DETAILS.Add(cDMA_INDIVIDUAL_OTHER_DETAILS_SAVE);
                db.SaveChanges();
            }
            else
            {
            //  var otherdetails_PLACE_OF_ISSUANCE = DynamicModel.otherdetails.AUTHORISED;
            // var otherdetails_PLACE_OF_AUTHORISED_BY = DynamicModel.otherdetails.AUTHORISED_BY;
            // var otherdetails_PLACE_OF_AUTHORISED_DATE = DynamicModel.otherdetails.AUTHORISED_DATE;
            //  var otherdetails_PLACE_OF_CREATED_BY = DynamicModel.otherdetails.CREATED_BY;
            // var otherdetails_PLACE_OF_CREATED_DATE = DynamicModel.otherdetails.CREATED_DATE;
              cDMA_INDIVIDUAL_OTHER_DETAILS.CUSTOMER_NO = DynamicModel.BioData.CUSTOMER_NO;
              cDMA_INDIVIDUAL_OTHER_DETAILS.IP_ADDRESS = this.Request.ServerVariables["REMOTE_ADDR"];
              cDMA_INDIVIDUAL_OTHER_DETAILS.LAST_MODIFIED_DATE = dateAndTime;
              cDMA_INDIVIDUAL_OTHER_DETAILS.TIN_NO = DynamicModel.otherdetails.TIN_NO;
              db.SaveChanges();
              CDMA_INDIVIDUAL_OTHER_DETAILS other_data = (CDMA_INDIVIDUAL_OTHER_DETAILS)this.Session["cDMA_INDIVIDUAL_OTHER_DETAILS"];

              var identification_changes = compareOtherRecord(cDMA_INDIVIDUAL_OTHER_DETAILS, other_data);
              changesComment = changesComment + identification_changes;

              CDMA_INDIVIDUAL_OTHER_DETAILS_LOG other_log = ConvertToOtherLog(other_data);
              other_log.TIED = tied;
              db.CDMA_INDIVIDUAL_OTHER_DETAILS_LOG.Add(other_log);
              db.SaveChanges();

              
                
            }



            CDMA_INDIVIDUAL_PROFILE_LOG SAVE_BIODATA_LOG = new CDMA_INDIVIDUAL_PROFILE_LOG();
            SAVE_BIODATA_LOG.AFFECTED_CATEGORY = this.Session["category"].ToString();
            SAVE_BIODATA_LOG.CUSTOMER_NO = c_id;
            SAVE_BIODATA_LOG.LOGGED_BY = Convert.ToDecimal(identity.ProfileId);
            SAVE_BIODATA_LOG.LOGGED_DATE = dateAndTime;
            SAVE_BIODATA_LOG.TIED = tied;
            SAVE_BIODATA_LOG.COMMENTS = changesComment;
            string change_index = String.Join(", ", index_id);
            //int[] change_index = index_id.ToArray();
            SAVE_BIODATA_LOG.CHANGE_INDEX = change_index.ToString();
            db.CDMA_INDIVIDUAL_PROFILE_LOG.Add(SAVE_BIODATA_LOG);
            db.SaveChanges();




            return PartialView("SaveBioData", DynamicModel);


        }




        public ActionResult DataValidation(string c_id, decimal branch, decimal rule, string table)
        {
            this.Session["table"] = table;
            this.Session["category"] = "biodata";

            switch (table)
            {
                case "CDMA_INDIVIDUAL_BIO_DATA":
                    if (c_id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    CDMA_INDIVIDUAL_BIO_DATA cDMA_INDIVIDUAL_BIO_DATA = db.CDMA_INDIVIDUAL_BIO_DATA.SingleOrDefault(c => c.CUSTOMER_NO == c_id);
                    CDMA_INDIVIDUAL_CONTACT_DETAIL cDMA_INDIVIDUAL_CONTACT_DETAIL = db.CDMA_INDIVIDUAL_CONTACT_DETAIL.SingleOrDefault(c => c.CUSTOMER_NO == c_id);
                    CDMA_INDIVIDUAL_ADDRESS_DETAIL cDMA_INDIVIDUAL_ADDRESS_DETAIL = db.CDMA_INDIVIDUAL_ADDRESS_DETAIL.SingleOrDefault(c => c.CUSTOMER_NO == c_id);
                    CDMA_INDIVIDUAL_IDENTIFICATION cDMA_INDIVIDUAL_IDENTIFICATION = db.CDMA_INDIVIDUAL_IDENTIFICATION.SingleOrDefault(c => c.CUSTOMER_NO == c_id);
                    CDMA_INDIVIDUAL_OTHER_DETAILS cDMA_INDIVIDUAL_OTHER_DETAILS = db.CDMA_INDIVIDUAL_OTHER_DETAILS.SingleOrDefault(c => c.CUSTOMER_NO == c_id);

                    //get all customer changes log  
                    ViewBag.profile_log = db.CDMA_INDIVIDUAL_PROFILE_LOG.Where(b => b.CUSTOMER_NO == c_id).ToList().Where(b => b.AFFECTED_CATEGORY == "biodata").OrderBy(i => i.LOG_ID).ToList();

                    var lastUpdate = db.CDMA_INDIVIDUAL_PROFILE_LOG.Where(b => b.CUSTOMER_NO == c_id).ToList().Where(b => b.AFFECTED_CATEGORY == "biodata").OrderByDescending(i => i.LOG_ID).Take(1);
                    var changeIndexSingle = lastUpdate.SingleOrDefault();
                    var ChangeIndex = changeIndexSingle.CHANGE_INDEX;
                    int[] nums = Array.ConvertAll(ChangeIndex.Split(','), int.Parse);
                    ViewBag.changeIndex = nums;

                    ViewBag.record = cDMA_INDIVIDUAL_BIO_DATA;
                    ViewBag.COUNTRY_OF_BIRTH = new SelectList(db.CDMA_COUNTRIES, "COUNTRY_ID", "COUNTRY_NAME", cDMA_INDIVIDUAL_BIO_DATA.COUNTRY_OF_BIRTH);
                    ViewBag.BioData_NATIONALITY = new SelectList(db.CDMA_COUNTRIES, "COUNTRY_ABBREVIATION", "COUNTRY_NAME", cDMA_INDIVIDUAL_BIO_DATA.NATIONALITY);
                    ViewBag.RELIGION = new SelectList(db.CDMA_RELIGION, "CODE", "RELIGION", cDMA_INDIVIDUAL_BIO_DATA.RELIGION);
                    ViewBag.BRANCH = new SelectList(db.CM_BRANCH, "BRANCH_ID", "BRANCH_NAME", cDMA_INDIVIDUAL_BIO_DATA.BRANCH_CODE);
                    ViewBag.STATES = new SelectList(db.SRC_CDMA_STATE, "STATE_ID", "STATE_NAME", cDMA_INDIVIDUAL_BIO_DATA.STATE_OF_ORIGIN);

                    ViewBag.STATES_RES = new SelectList(db.SRC_CDMA_STATE, "STATE_ID", "STATE_NAME", cDMA_INDIVIDUAL_BIO_DATA.STATE_OF_ORIGIN);

                    if (cDMA_INDIVIDUAL_ADDRESS_DETAIL == null)
                    {
                        ViewBag.COUNTRY_OF_RESIDENCE = new SelectList(db.CDMA_COUNTRIES, "COUNTRY_ID", "COUNTRY_NAME");
                    }
                    else
                    {
                        ViewBag.COUNTRY_OF_RESIDENCE = new SelectList(db.CDMA_COUNTRIES, "COUNTRY_ID", "COUNTRY_NAME", cDMA_INDIVIDUAL_ADDRESS_DETAIL.COUNTRY_OF_RESIDENCE);
                    }


                    if (cDMA_INDIVIDUAL_IDENTIFICATION == null)
                    {
                        ViewBag.CDMA_IDENTIFICATION_TYPE = new SelectList(db.CDMA_IDENTIFICATION_TYPE, "CODE", "ID_TYPE");

                    }
                    else
                    {
                        ViewBag.CDMA_IDENTIFICATION_TYPE = new SelectList(db.CDMA_IDENTIFICATION_TYPE, "CODE", "ID_TYPE", cDMA_INDIVIDUAL_IDENTIFICATION.IDENTIFICATION_TYPE);

                    }
                    // decimal user_lga = Convert.ToDecimal(cDMA_INDIVIDUAL_ADDRESS_DETAIL.LGA_OF_RESIDENCE);

                    if (cDMA_INDIVIDUAL_ADDRESS_DETAIL == null)
                    {
                        ViewBag.LGA = new[] { new SelectListItem { } };
                    }
                    else
                    {
                        decimal user_lga = Convert.ToDecimal(cDMA_INDIVIDUAL_ADDRESS_DETAIL.LGA_OF_RESIDENCE);
                        decimal user_res_state = Convert.ToDecimal(cDMA_INDIVIDUAL_ADDRESS_DETAIL.STATE_OF_RESIDENCE);
                        ViewBag.LGA = new SelectList(db.SRC_CDMA_LGA.Where(g => g.LGA_ID == user_lga), "LGA_ID", "LGA_NAME", user_lga);
                        ViewBag.STATES_RES = new SelectList(db.SRC_CDMA_STATE, "STATE_ID", "STATE_NAME", user_res_state);

                    }

                    //save current data into session variable for future usage
                    //set the customer value incase of empty record 

                    this.Session["cDMA_INDIVIDUAL_BIO_DATA"] = cDMA_INDIVIDUAL_BIO_DATA;
                    this.Session["cDMA_INDIVIDUAL_ADDRESS_DETAIL"] = cDMA_INDIVIDUAL_ADDRESS_DETAIL;
                    this.Session["cDMA_INDIVIDUAL_CONTACT_DETAIL"] = cDMA_INDIVIDUAL_CONTACT_DETAIL;
                    this.Session["cDMA_INDIVIDUAL_IDENTIFICATION"] = cDMA_INDIVIDUAL_IDENTIFICATION;
                    this.Session["cDMA_INDIVIDUAL_OTHER_DETAILS"] = cDMA_INDIVIDUAL_OTHER_DETAILS;

                    if (cDMA_INDIVIDUAL_BIO_DATA == null)
                    {
                        return HttpNotFound();
                    }

                    var viewModel = new DynamicViewModel(); //Create an instance of the above view model
                    viewModel.BioData = cDMA_INDIVIDUAL_BIO_DATA;
                    viewModel.AddressDetails = cDMA_INDIVIDUAL_ADDRESS_DETAIL;
                    viewModel.contact = cDMA_INDIVIDUAL_CONTACT_DETAIL;
                    viewModel.identification = cDMA_INDIVIDUAL_IDENTIFICATION;
                    viewModel.otherdetails = cDMA_INDIVIDUAL_OTHER_DETAILS;
                    //return PartialView("EditCustomer", viewModel);
                    return PartialView("validateData", viewModel);
                case "Beet Armyworm":
                case "Indian Meal Moth":
                case "Ash Pug":
                case "Latticed Heath":
                case "Ribald Wave":
                case "The Streak":
                    return JavaScript("alert('No more images');");
                default:
                    return JavaScript("alert('No more images');");
            }





        }











        public ActionResult Dataquality(string c_id, decimal branch, decimal rule,string table)
        {
            this.Session["table"] = table;
            this.Session["category"] = "biodata";

            switch (table)
            {
                case "CDMA_INDIVIDUAL_BIO_DATA":
                    if (c_id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    CDMA_INDIVIDUAL_BIO_DATA cDMA_INDIVIDUAL_BIO_DATA = db.CDMA_INDIVIDUAL_BIO_DATA.SingleOrDefault(c => c.CUSTOMER_NO == c_id);
                    CDMA_INDIVIDUAL_CONTACT_DETAIL cDMA_INDIVIDUAL_CONTACT_DETAIL = db.CDMA_INDIVIDUAL_CONTACT_DETAIL.SingleOrDefault(c => c.CUSTOMER_NO == c_id);
                    CDMA_INDIVIDUAL_ADDRESS_DETAIL cDMA_INDIVIDUAL_ADDRESS_DETAIL = db.CDMA_INDIVIDUAL_ADDRESS_DETAIL.SingleOrDefault(c => c.CUSTOMER_NO == c_id);
                    CDMA_INDIVIDUAL_IDENTIFICATION cDMA_INDIVIDUAL_IDENTIFICATION = db.CDMA_INDIVIDUAL_IDENTIFICATION.SingleOrDefault(c => c.CUSTOMER_NO == c_id);
                    CDMA_INDIVIDUAL_OTHER_DETAILS cDMA_INDIVIDUAL_OTHER_DETAILS = db.CDMA_INDIVIDUAL_OTHER_DETAILS.SingleOrDefault(c => c.CUSTOMER_NO == c_id);

                    //get all customer changes log  
                    ViewBag.profile_log =  db.CDMA_INDIVIDUAL_PROFILE_LOG.Where(b => b.CUSTOMER_NO == c_id).ToList().Where(b => b.AFFECTED_CATEGORY == "biodata").OrderBy(i => i.LOG_ID).ToList();

                    ViewBag.logs = (from ep in db.CDMA_INDIVIDUAL_PROFILE_LOG
                                      join e in db.CM_USER_PROFILE on ep.LOGGED_BY equals e.PROFILE_ID
                                      where ep.CUSTOMER_NO == c_id
                                      where ep.AFFECTED_CATEGORY == "biodata"
                                      select new
                                      {
                                          NAME = e.DISPLAY_NAME,
                                          COMMENT = ep.COMMENTS,
                                          DATE = ep.LOGGED_DATE,
                                          TIED = ep.TIED,
                                          INDEX = ep.CHANGE_INDEX
                                      }).ToList();


                    ViewBag.record = cDMA_INDIVIDUAL_BIO_DATA;
                    ViewBag.COUNTRY_OF_BIRTH = new SelectList(db.CDMA_COUNTRIES, "COUNTRY_ID", "COUNTRY_NAME", cDMA_INDIVIDUAL_BIO_DATA.COUNTRY_OF_BIRTH);
                    ViewBag.BioData_NATIONALITY = new SelectList(db.CDMA_COUNTRIES, "COUNTRY_ABBREVIATION", "COUNTRY_NAME", cDMA_INDIVIDUAL_BIO_DATA.NATIONALITY);
                    ViewBag.RELIGION = new SelectList(db.CDMA_RELIGION, "CODE", "RELIGION", cDMA_INDIVIDUAL_BIO_DATA.RELIGION);
                    ViewBag.BRANCH = new SelectList(db.CM_BRANCH, "BRANCH_ID", "BRANCH_NAME", cDMA_INDIVIDUAL_BIO_DATA.BRANCH_CODE);
                    ViewBag.STATES = new SelectList(db.SRC_CDMA_STATE, "STATE_ID", "STATE_NAME", cDMA_INDIVIDUAL_BIO_DATA.STATE_OF_ORIGIN);

                    ViewBag.STATES_RES = new SelectList(db.SRC_CDMA_STATE, "STATE_ID", "STATE_NAME", cDMA_INDIVIDUAL_BIO_DATA.STATE_OF_ORIGIN);

                    if (cDMA_INDIVIDUAL_ADDRESS_DETAIL == null)
                    {
                        ViewBag.COUNTRY_OF_RESIDENCE = new SelectList(db.CDMA_COUNTRIES, "COUNTRY_ID", "COUNTRY_NAME");
                    }
                    else
                    {
                        ViewBag.COUNTRY_OF_RESIDENCE = new SelectList(db.CDMA_COUNTRIES, "COUNTRY_ID", "COUNTRY_NAME", cDMA_INDIVIDUAL_ADDRESS_DETAIL.COUNTRY_OF_RESIDENCE);
                    }


                    if (cDMA_INDIVIDUAL_IDENTIFICATION == null)
                    {
                        ViewBag.CDMA_IDENTIFICATION_TYPE = new SelectList(db.CDMA_IDENTIFICATION_TYPE, "CODE", "ID_TYPE");

                    }
                    else
                    {
                        ViewBag.CDMA_IDENTIFICATION_TYPE = new SelectList(db.CDMA_IDENTIFICATION_TYPE, "CODE", "ID_TYPE", cDMA_INDIVIDUAL_IDENTIFICATION.IDENTIFICATION_TYPE);

                    }
                    // decimal user_lga = Convert.ToDecimal(cDMA_INDIVIDUAL_ADDRESS_DETAIL.LGA_OF_RESIDENCE);

                    if (cDMA_INDIVIDUAL_ADDRESS_DETAIL == null)
                    {
                        ViewBag.LGA = new[] { new SelectListItem { } }; 
                    }
                    else
                    {
                        decimal user_lga = Convert.ToDecimal(cDMA_INDIVIDUAL_ADDRESS_DETAIL.LGA_OF_RESIDENCE);
                        decimal user_res_state = Convert.ToDecimal(cDMA_INDIVIDUAL_ADDRESS_DETAIL.STATE_OF_RESIDENCE);
                        ViewBag.LGA = new SelectList(db.SRC_CDMA_LGA.Where(g => g.LGA_ID == user_lga) , "LGA_ID", "LGA_NAME", user_lga);
                        ViewBag.STATES_RES = new SelectList(db.SRC_CDMA_STATE, "STATE_ID", "STATE_NAME", user_res_state);

                    }
 
                    //save current data into session variable for future usage
                    //set the customer value incase of empty record 

                    this.Session["cDMA_INDIVIDUAL_BIO_DATA"] = cDMA_INDIVIDUAL_BIO_DATA;
                    this.Session["cDMA_INDIVIDUAL_ADDRESS_DETAIL"] = cDMA_INDIVIDUAL_ADDRESS_DETAIL;
                    this.Session["cDMA_INDIVIDUAL_CONTACT_DETAIL"] = cDMA_INDIVIDUAL_CONTACT_DETAIL;
                    this.Session["cDMA_INDIVIDUAL_IDENTIFICATION"] = cDMA_INDIVIDUAL_IDENTIFICATION;
                    this.Session["cDMA_INDIVIDUAL_OTHER_DETAILS"] = cDMA_INDIVIDUAL_OTHER_DETAILS;

                    if (cDMA_INDIVIDUAL_BIO_DATA == null)
                    {
                        return HttpNotFound();
                    }

                    var viewModel = new DynamicViewModel(); //Create an instance of the above view model
                    viewModel.BioData = cDMA_INDIVIDUAL_BIO_DATA;  
                    viewModel.AddressDetails = cDMA_INDIVIDUAL_ADDRESS_DETAIL;
                    viewModel.contact = cDMA_INDIVIDUAL_CONTACT_DETAIL;
                    viewModel.identification = cDMA_INDIVIDUAL_IDENTIFICATION;
                    viewModel.otherdetails = cDMA_INDIVIDUAL_OTHER_DETAILS;
                    //return PartialView("EditCustomer", viewModel);
                    return PartialView("EditCustomer", viewModel);
                case "Beet Armyworm":
                case "Indian Meal Moth":
                case "Ash Pug":
                case "Latticed Heath":
                case "Ribald Wave":
                case "The Streak":
                    return JavaScript("alert('No more images');");
                default:
                    return JavaScript("alert('No more images');");
            }





        }

        public string compareOtherRecord(CDMA_INDIVIDUAL_OTHER_DETAILS current, CDMA_INDIVIDUAL_OTHER_DETAILS previous) {
            string result = "";
           // cDMA_INDIVIDUAL_OTHER_DETAILS.TIN_NO = DynamicModel.otherdetails.TIN_NO;
            if (!(current.TIN_NO.Equals(previous.TIN_NO)))
            {
                index_id.Add(1);
                result = "<li>" + result + "Identification Type changed from <strong>"
                                + previous.TIN_NO + "</strong> To "
                                + current.TIN_NO + "</li>";
            }

            return result;


        }
        public string compareIdentificationRecord(CDMA_INDIVIDUAL_IDENTIFICATION current, CDMA_INDIVIDUAL_IDENTIFICATION previous)
        {
            string result = "";
            if (!(current.IDENTIFICATION_TYPE.Equals(previous.IDENTIFICATION_TYPE)))
            {
                var previous_idtype = IdType(Convert.ToDecimal(previous.IDENTIFICATION_TYPE));
                var current_idtype = IdType(Convert.ToDecimal(current.IDENTIFICATION_TYPE));
                index_id.Add(2);

                result = "<li>" + result + "Identification Type changed from <strong>"
                                + previous_idtype.ID_TYPE + "</strong> To "
                                + current_idtype.ID_TYPE + "</li>";
            }
            if (!(current.ID_EXPIRY_DATE.Equals(previous.ID_EXPIRY_DATE)))
            {
                index_id.Add(3);
                result = "<li>" +  result + "Expiring date changed from <strong>"
                                + previous.ID_EXPIRY_DATE + "</strong> To "
                                + current.ID_EXPIRY_DATE + "</li>";
            }

            if (!(current.ID_ISSUE_DATE.Equals(previous.ID_ISSUE_DATE)))
            {
                index_id.Add(4);
                result = "<li>" + result + "ID Issued Date changed from  <strong>"
                                + previous.ID_ISSUE_DATE + "</strong> To "
                                + current.ID_ISSUE_DATE + "</li>";
            }

            if (!(current.ID_NO.Equals(previous.ID_NO)))
            {
                index_id.Add(5);
                result = "<li>" + result + "ID Number changed from  <strong>"
                                + previous.ID_NO + "</strong> To "
                                + current.ID_NO + "</li>";
            }

            if (!(current.PLACE_OF_ISSUANCE.Equals(previous.PLACE_OF_ISSUANCE)))
            {
                index_id.Add(6);
                result = "<li>" + result + "ID Place of Issueance changed from  <strong>"
                                + previous.PLACE_OF_ISSUANCE + "</strong> To "
                                + current.PLACE_OF_ISSUANCE + "</li>";
            }


            return result;

        }

        public string compareContactRecord(CDMA_INDIVIDUAL_CONTACT_DETAIL current, CDMA_INDIVIDUAL_CONTACT_DETAIL previous) {

            string result = "";
            if (!(current.MAILING_ADDRESS.Equals(previous.MAILING_ADDRESS)))
            {
                index_id.Add(7);
                result = "<li>" + result + "Mailing Address changed from <strong>"
                                + previous.MAILING_ADDRESS + "</strong> To "
                                + current.MAILING_ADDRESS + "</li>";
            }
            if (!(current.MOBILE_NO.Equals(previous.MOBILE_NO)))
            {
                index_id.Add(8);
                result = "<li>" + result + "Mobile number changed from <strong>"
                                + previous.MOBILE_NO + "</strong> To "
                                + current.MOBILE_NO + "</li>";
            }

            if (!(current.EMAIL_ADDRESS.Equals(previous.EMAIL_ADDRESS)))
            {
                index_id.Add(9);
                result = "<li>" + result + "Email changed from <strong>"
                                + previous.EMAIL_ADDRESS + "</strong> To "
                                + current.EMAIL_ADDRESS + "</li>";
            }


            return result;
        }


        public string compareBioRecord(CDMA_INDIVIDUAL_BIO_DATA current, CDMA_INDIVIDUAL_BIO_DATA previous)
        {
            string result = "";

            if (!(current.DATE_OF_BIRTH.Equals(previous.DATE_OF_BIRTH)))
            {
                index_id.Add(10);
                result = "<li>" + result + "Date of birth changed from <strong>"
                                + previous.DATE_OF_BIRTH + "</strong> To "
                                + current.DATE_OF_BIRTH + "</li>";
            }
            if (!(current.DISABILITY.Equals(previous.DISABILITY)))
            {
                index_id.Add(11);
                result = "<li>" + result + "Disability changed from <strong>"
                                + previous.DISABILITY + "</strong> To "
                                + current.DISABILITY + "</li>";
            }

            if (!(current.DISABILITY.Equals(previous.DISABILITY)))
            {
                index_id.Add(12);
                result = "<li>" + result + "Disability changed from <strong>"
                                + previous.DISABILITY + "</strong> To "
                                + current.DISABILITY + "</li>";
            }

            if (!(current.FIRST_NAME.Equals(previous.FIRST_NAME)))
            {
                index_id.Add(13);
                result = "<li>" + result + "First Name changed from <strong>"
                                + previous.FIRST_NAME + "</strong> To "
                                + current.FIRST_NAME + "</li>";
            }

            if (!(current.MARITAL_STATUS.Equals(previous.MARITAL_STATUS)))
            {
                index_id.Add(14);
                result = "<li>" + result + "Marital Status changed from <strong>"
                                + previous.MARITAL_STATUS + "</strong> To "
                                + current.MARITAL_STATUS + "</li>";
            }

            if (!(current.MOTHER_MAIDEN_NAME.Equals(previous.MOTHER_MAIDEN_NAME)))
            {
                index_id.Add(15);
                result = "<li>" + result + "Mother maiden name changed<strong>"
                                + previous.MOTHER_MAIDEN_NAME + "</strong> To "
                                + current.MOTHER_MAIDEN_NAME + "</li>";
            }

            if (!(current.NATIONALITY.Equals(previous.NATIONALITY)))
            {
                index_id.Add(16);
                var previous_nationality = countryById(Convert.ToDecimal(previous.NATIONALITY));
                var current_nationality = countryById(Convert.ToDecimal(current.NATIONALITY));
                result = "<li>" + result + "Nationality changed from <strong>"
                                + previous_nationality.COUNTRY_NAME + "</strong> To "
                                + previous_nationality.COUNTRY_NAME + "</li>";
            }

            if (!(current.NICKNAME_ALIAS.Equals(previous.NICKNAME_ALIAS)))
            {
                index_id.Add(17);
                result = "<li>" + result + "Nickname Changed from<strong>"
                                + previous.NICKNAME_ALIAS + "</strong> To "
                                + current.NICKNAME_ALIAS + "</li>";
            }

            if (!(current.NUMBER_OF_CHILDREN.Equals(previous.NUMBER_OF_CHILDREN)))
            {
                index_id.Add(18);
                result = "<li>" + result + "Number of children changed from<strong>"
                                + previous.NUMBER_OF_CHILDREN + "</strong> To "
                                + current.NUMBER_OF_CHILDREN + "</li>";
            }

            if (!(current.OTHER_NAME.Equals(previous.OTHER_NAME)))
            {
                index_id.Add(19);
                result = "<li>" + result + "Other Name changed from<strong>"
                                + previous.OTHER_NAME + "</strong> To "
                                + current.OTHER_NAME + "</li>";
            }
            if (!(current.PLACE_OF_BIRTH.Equals(previous.PLACE_OF_BIRTH)))
            {
                index_id.Add(20);
                result = "<li>" + result + "Place of birth changed from<strong>"
                                + previous.PLACE_OF_BIRTH + "</strong> To "
                                + current.PLACE_OF_BIRTH + "</li>";
            }

            if (!(current.RELIGION.Equals(previous.RELIGION)))
            {
                index_id.Add(21);
                result = "<li>" + result + "Religion changed from<strong>"
                                + previous.RELIGION + "</strong> To "
                                + current.RELIGION + "</li>";
            }

            if (!(current.SEX.Equals(previous.SEX)))
            {
                index_id.Add(22);
                result = "<li>" + result + "Sex changed from<strong>"
                                + previous.RELIGION + "</strong> To "
                                + current.RELIGION + "</li>";
            }

            if (!(current.STATE_OF_ORIGIN.Equals(previous.STATE_OF_ORIGIN)))
            {
                index_id.Add(23);
                var previous_state_of_origin = getState(Convert.ToDecimal(previous.STATE_OF_ORIGIN));
                var current_state_of_origin = getState(Convert.ToDecimal(current.STATE_OF_ORIGIN));
                result = "<li>" + result + "Satte of Origin changed from <strong>"
                                + previous_state_of_origin.STATE_NAME + "</strong> To "
                                + current_state_of_origin.STATE_NAME + "</li>";
            }

            if (!(current.SURNAME.Equals(previous.SURNAME)))
            {
                index_id.Add(24);
                result = "<li>" + result + "Surname chnaged from <strong>"
                                + previous.SURNAME + "</strong> To "
                                + current.SURNAME + "</li>";
            }

            if (!(current.TITLE.Equals(previous.TITLE)))
            {
                index_id.Add(25);
                result = "<li>" + result + "Title changed from <strong>"
                                + previous.TITLE + "</strong> To "
                                + current.TITLE + "</li>";
            }

            if (!(current.RELIGION.Equals(previous.RELIGION)))
            {
                index_id.Add(26);
                var previous_rel = getRel(Convert.ToDecimal(previous.RELIGION));
                var current_rel = getRel(Convert.ToDecimal(current.RELIGION));
                result = "<li>" + result + "Religion changed from <strong>"
                                + previous_rel.RELIGION + "</strong> To "
                                + current_rel.RELIGION + "</li>";
            }

            if (!(current.COUNTRY_OF_BIRTH.Equals(previous.COUNTRY_OF_BIRTH)))
            {
                index_id.Add(27);
                var previous_country = countryById(Convert.ToDecimal(previous.COUNTRY_OF_BIRTH));
                var current_country = countryById(Convert.ToDecimal(current.COUNTRY_OF_BIRTH));
                result = "<li>" + result + "Title changed from <strong>"
                                + previous_country.COUNTRY_NAME + "</strong> To "
                                + previous_country.COUNTRY_NAME + "</li>";
            }
 
            return result;

        }

        public string compareAddress(CDMA_INDIVIDUAL_ADDRESS_DETAIL current, CDMA_INDIVIDUAL_ADDRESS_DETAIL previous)
        {

            string result = "";
            if (!(current.CITY_TOWN_OF_RESIDENCE.Equals(previous.CITY_TOWN_OF_RESIDENCE))) {
                index_id.Add(28);
                result = "<li>" + result + "City Town of Residence changed from <strong>" 
                                + previous.CITY_TOWN_OF_RESIDENCE + "</strong> To "
                                + current.CITY_TOWN_OF_RESIDENCE + "</li>";
            }
            if (!(current.COUNTRY_OF_RESIDENCE.Equals(previous.COUNTRY_OF_RESIDENCE)))
            {
                index_id.Add(29);
                var previous_country = countryById(Convert.ToDecimal(previous.COUNTRY_OF_RESIDENCE));
                var current_country = countryById(Convert.ToDecimal(current.COUNTRY_OF_RESIDENCE));
                result = "<li>" + result + "Country of residence changed from <strong>"
                                + previous_country.COUNTRY_NAME + "</strong> To "
                                + current_country.COUNTRY_NAME + "</li>";
            }

            //if (!(current.COUNTRY_OF_RESIDENCE.Equals(previous.COUNTRY_OF_RESIDENCE)))
            //{
            //    index_id.Add(30);
            //    var previous_country = countryById(Convert.ToDecimal(previous.COUNTRY_OF_RESIDENCE));
            //    var current_country = countryById(Convert.ToDecimal(current.COUNTRY_OF_RESIDENCE));
            //    result = result + "Country of residence changed from <strong>"
            //                    + previous_country.COUNTRY_NAME + "</strong> To "
            //                    + current_country.COUNTRY_NAME;
            //}

            if (!(current.LGA_OF_RESIDENCE.Equals(previous.LGA_OF_RESIDENCE)))
            {
                index_id.Add(30);
                var previous_lga_of_residence = getloadlga(Convert.ToDecimal(previous.LGA_OF_RESIDENCE));
                var current_lga_of_residence = getloadlga(Convert.ToDecimal(current.LGA_OF_RESIDENCE));
                result = "<li>" + result + "LGA of residence changed from <strong>"
                                + previous_lga_of_residence.LGA_NAME + "</strong> To "
                                + current_lga_of_residence.LGA_NAME + "</li>";
            }

            if (!(current.NEAREST_BUS_STOP_LANDMARK.Equals(previous.NEAREST_BUS_STOP_LANDMARK)))
            {
                index_id.Add(31);
                result = "<li>" + result + "Nearest bus stop changed from <strong>"
                                + previous.NEAREST_BUS_STOP_LANDMARK + "</strong> To "
                                + current.NEAREST_BUS_STOP_LANDMARK + "</li>";
            }

            if (!(current.RESIDENCE_OWNED_OR_RENT.Equals(previous.RESIDENCE_OWNED_OR_RENT)))
            {
                index_id.Add(32);
                result = "<li>" + result + "Residence status changed from <strong>"
                                + previous.RESIDENCE_OWNED_OR_RENT + "</strong> To "
                                + current.RESIDENCE_OWNED_OR_RENT + "</li>";
            }

            if (!(current.RESIDENTIAL_ADDRESS.Equals(previous.RESIDENTIAL_ADDRESS)))
            {
                index_id.Add(33);
                result = "<li>" + result + "Residential address changed from <strong>"
                                + previous.RESIDENTIAL_ADDRESS + "</strong> To "
                                + current.RESIDENTIAL_ADDRESS + "</li>";
            }

            if (!(current.STATE_OF_RESIDENCE.Equals(previous.STATE_OF_RESIDENCE)))
            {
                index_id.Add(34);
                var previous_state_of_residence = getState(Convert.ToDecimal(previous.STATE_OF_RESIDENCE));
                var previous_of_residence = getState(Convert.ToDecimal(current.STATE_OF_RESIDENCE));
                result = "<li>" + result + "State of Residence changed from <strong>"
                                + previous_state_of_residence.STATE_NAME + "</strong> To "
                                + previous_of_residence.STATE_NAME + "</li>";
            }

            if (!(current.ZIP_POSTAL_CODE.Equals(previous.ZIP_POSTAL_CODE)))
            {
                index_id.Add(35);
                result = "<li>" + result + "State of Residence changed from <strong>"
                                + previous.ZIP_POSTAL_CODE + "</strong> To "
                                + current.ZIP_POSTAL_CODE + "</li>";
            }
            

            return result;
        }

        public CDMA_INDIVIDUAL_ADDRESS_DETAIL_LOG ConvertToAddressLog(CDMA_INDIVIDUAL_ADDRESS_DETAIL address)
        {
            var dateAndTime = DateTime.Now;
            var identity = ((CustomPrincipal)User).CustomIdentity;
            CDMA_INDIVIDUAL_ADDRESS_DETAIL_LOG cDMA_INDIVIDUAL_ADDRESS_DETAIL_LOG = new CDMA_INDIVIDUAL_ADDRESS_DETAIL_LOG();
            cDMA_INDIVIDUAL_ADDRESS_DETAIL_LOG.CREATED_BY = identity.ProfileId.ToString();
            cDMA_INDIVIDUAL_ADDRESS_DETAIL_LOG.CREATED_DATE = dateAndTime;
            cDMA_INDIVIDUAL_ADDRESS_DETAIL_LOG.CITY_TOWN_OF_RESIDENCE = address.CITY_TOWN_OF_RESIDENCE;
            cDMA_INDIVIDUAL_ADDRESS_DETAIL_LOG.COUNTRY_OF_RESIDENCE = address.COUNTRY_OF_RESIDENCE; 
            cDMA_INDIVIDUAL_ADDRESS_DETAIL_LOG.CUSTOMER_NO = address.CUSTOMER_NO;
            cDMA_INDIVIDUAL_ADDRESS_DETAIL_LOG.IP_ADDRESS = address.IP_ADDRESS;
            cDMA_INDIVIDUAL_ADDRESS_DETAIL_LOG.LAST_MODIFIED_BY = identity.ProfileId.ToString();
            cDMA_INDIVIDUAL_ADDRESS_DETAIL_LOG.LAST_MODIFIED_DATE = dateAndTime;
            cDMA_INDIVIDUAL_ADDRESS_DETAIL_LOG.LGA_OF_RESIDENCE = address.LGA_OF_RESIDENCE;
            cDMA_INDIVIDUAL_ADDRESS_DETAIL_LOG.NEAREST_BUS_STOP_LANDMARK = address.NEAREST_BUS_STOP_LANDMARK;
            cDMA_INDIVIDUAL_ADDRESS_DETAIL_LOG.RESIDENCE_OWNED_OR_RENT = address.RESIDENCE_OWNED_OR_RENT;//Request["RESIDENCE_OWNED_OR_RENT"]; 
            cDMA_INDIVIDUAL_ADDRESS_DETAIL_LOG.RESIDENTIAL_ADDRESS = address.CITY_TOWN_OF_RESIDENCE;
            cDMA_INDIVIDUAL_ADDRESS_DETAIL_LOG.STATE_OF_RESIDENCE = address.STATE_OF_RESIDENCE;
            cDMA_INDIVIDUAL_ADDRESS_DETAIL_LOG.ZIP_POSTAL_CODE = address.ZIP_POSTAL_CODE;

            return  cDMA_INDIVIDUAL_ADDRESS_DETAIL_LOG;

        }

        public CDMA_INDIVIDUAL_CONTACT_LOG ConvertToContactDataLog(CDMA_INDIVIDUAL_CONTACT_DETAIL contact_data) {

            CDMA_INDIVIDUAL_CONTACT_LOG cDMA_INDIVIDUAL_CONTACT_log = new CDMA_INDIVIDUAL_CONTACT_LOG();
            cDMA_INDIVIDUAL_CONTACT_log.CUSTOMER_NO = contact_data.CUSTOMER_NO;
            cDMA_INDIVIDUAL_CONTACT_log.CREATED_BY = contact_data.CREATED_DATE.ToString();
            cDMA_INDIVIDUAL_CONTACT_log.CREATED_DATE = contact_data.CREATED_DATE;
            cDMA_INDIVIDUAL_CONTACT_log.MAILING_ADDRESS = contact_data.MAILING_ADDRESS;
            cDMA_INDIVIDUAL_CONTACT_log.MOBILE_NO = contact_data.MOBILE_NO;
            cDMA_INDIVIDUAL_CONTACT_log.EMAIL_ADDRESS = contact_data.EMAIL_ADDRESS;
            cDMA_INDIVIDUAL_CONTACT_log.IP_ADDRESS = contact_data.IP_ADDRESS;
            cDMA_INDIVIDUAL_CONTACT_log.LAST_MODIFIED_BY = contact_data.LAST_MODIFIED_BY;

            return cDMA_INDIVIDUAL_CONTACT_log;


        }

        public CDMA_INDIVIDUAL_OTHER_DETAILS_LOG ConvertToOtherLog(CDMA_INDIVIDUAL_OTHER_DETAILS other_data)
        {
            CDMA_INDIVIDUAL_OTHER_DETAILS_LOG cDMA_INDIVIDUAL_OTHER_DETAILS_LOG = new CDMA_INDIVIDUAL_OTHER_DETAILS_LOG();
            cDMA_INDIVIDUAL_OTHER_DETAILS_LOG.CREATED_BY = other_data.CREATED_BY;
            cDMA_INDIVIDUAL_OTHER_DETAILS_LOG.CREATED_DATE = other_data.CREATED_DATE;
            cDMA_INDIVIDUAL_OTHER_DETAILS_LOG.CUSTOMER_NO = other_data.CUSTOMER_NO;
            cDMA_INDIVIDUAL_OTHER_DETAILS_LOG.IP_ADDRESS = other_data.IP_ADDRESS;
            cDMA_INDIVIDUAL_OTHER_DETAILS_LOG.LAST_MODIFIED_DATE = other_data.LAST_MODIFIED_DATE;
            cDMA_INDIVIDUAL_OTHER_DETAILS_LOG.TIN_NO = other_data.TIN_NO;

            return cDMA_INDIVIDUAL_OTHER_DETAILS_LOG;

        }

        public CDMA_INDIVIDUAL_IDENTIFICATION_LOG ConvertToIdentificationLog(CDMA_INDIVIDUAL_IDENTIFICATION identification_data)
        {
            CDMA_INDIVIDUAL_IDENTIFICATION_LOG cDMA_INDIVIDUAL_IDENTIFICATION_LOG = new CDMA_INDIVIDUAL_IDENTIFICATION_LOG();
            cDMA_INDIVIDUAL_IDENTIFICATION_LOG.AUTHORISED = identification_data.AUTHORISED;
            cDMA_INDIVIDUAL_IDENTIFICATION_LOG.AUTHORISED_BY = identification_data.AUTHORISED_BY;
            cDMA_INDIVIDUAL_IDENTIFICATION_LOG.AUTHORISED_DATE = identification_data.AUTHORISED_DATE;
            cDMA_INDIVIDUAL_IDENTIFICATION_LOG.CREATED_BY = identification_data.CREATED_BY;
            cDMA_INDIVIDUAL_IDENTIFICATION_LOG.CREATED_DATE = identification_data.CREATED_DATE;
            cDMA_INDIVIDUAL_IDENTIFICATION_LOG.CUSTOMER_NO = identification_data.CUSTOMER_NO;
            cDMA_INDIVIDUAL_IDENTIFICATION_LOG.IDENTIFICATION_TYPE = identification_data.IDENTIFICATION_TYPE;// Request["CDMA_IDENTIFICATION_TYPE"];
            cDMA_INDIVIDUAL_IDENTIFICATION_LOG.ID_EXPIRY_DATE = identification_data.ID_EXPIRY_DATE;
            cDMA_INDIVIDUAL_IDENTIFICATION_LOG.ID_ISSUE_DATE = identification_data.ID_ISSUE_DATE;
            cDMA_INDIVIDUAL_IDENTIFICATION_LOG.ID_NO = identification_data.ID_NO;
            cDMA_INDIVIDUAL_IDENTIFICATION_LOG.IP_ADDRESS = identification_data.IP_ADDRESS;//.ServerVariables["REMOTE_ADDR"];
            cDMA_INDIVIDUAL_IDENTIFICATION_LOG.LAST_MODIFIED_BY = identification_data.LAST_MODIFIED_BY;
            cDMA_INDIVIDUAL_IDENTIFICATION_LOG.LAST_MODIFIED_DATE = identification_data.LAST_MODIFIED_DATE;
            cDMA_INDIVIDUAL_IDENTIFICATION_LOG.PLACE_OF_ISSUANCE = identification_data.PLACE_OF_ISSUANCE;
            cDMA_INDIVIDUAL_IDENTIFICATION_LOG.LAST_MODIFIED_BY = identification_data.LAST_MODIFIED_BY;
             
            return cDMA_INDIVIDUAL_IDENTIFICATION_LOG;

        }

        public CDMA_INDIVIDUAL_BIO_LOG ConvertToBioDataLog(CDMA_INDIVIDUAL_BIO_DATA bio_data)
        {
            var dateAndTime = DateTime.Now;
            var identity = ((CustomPrincipal)User).CustomIdentity;
            CDMA_INDIVIDUAL_BIO_LOG cDMA_INDIVIDUAL_BIO_DATA_LOG = new CDMA_INDIVIDUAL_BIO_LOG();

            cDMA_INDIVIDUAL_BIO_DATA_LOG.AGE = bio_data.AGE;
            cDMA_INDIVIDUAL_BIO_DATA_LOG.AUTHORISED = bio_data.AUTHORISED;
            cDMA_INDIVIDUAL_BIO_DATA_LOG.AUTHORISED_BY = bio_data.AUTHORISED_BY;
            cDMA_INDIVIDUAL_BIO_DATA_LOG.AUTHORISED_DATE = bio_data.AUTHORISED_DATE;
            cDMA_INDIVIDUAL_BIO_DATA_LOG.BRANCH_CODE = bio_data.BRANCH_CODE;
            cDMA_INDIVIDUAL_BIO_DATA_LOG.COMPLEXION = bio_data.COMPLEXION;
            cDMA_INDIVIDUAL_BIO_DATA_LOG.COUNTRY_OF_BIRTH = bio_data.COUNTRY_OF_BIRTH; // DynamicModel.BioData.COUNTRY_OF_BIRTH;
            cDMA_INDIVIDUAL_BIO_DATA_LOG.CREATED_BY = bio_data.CREATED_BY;
            cDMA_INDIVIDUAL_BIO_DATA_LOG.CREATED_DATE = bio_data.CREATED_DATE;
            cDMA_INDIVIDUAL_BIO_DATA_LOG.CUSTOMER_NO = bio_data.CUSTOMER_NO;
            cDMA_INDIVIDUAL_BIO_DATA_LOG.DATE_OF_BIRTH = bio_data.DATE_OF_BIRTH;
            cDMA_INDIVIDUAL_BIO_DATA_LOG.DISABILITY = bio_data.DISABILITY;
            cDMA_INDIVIDUAL_BIO_DATA_LOG.FIRST_NAME = bio_data.FIRST_NAME;
            cDMA_INDIVIDUAL_BIO_DATA_LOG.IP_ADDRESS = bio_data.IP_ADDRESS;
            cDMA_INDIVIDUAL_BIO_DATA_LOG.LAST_MODIFIED_DATE = bio_data.LAST_MODIFIED_DATE;
            cDMA_INDIVIDUAL_BIO_DATA_LOG.MARITAL_STATUS = bio_data.MARITAL_STATUS;
            cDMA_INDIVIDUAL_BIO_DATA_LOG.MOTHER_MAIDEN_NAME = bio_data.MOTHER_MAIDEN_NAME;
            cDMA_INDIVIDUAL_BIO_DATA_LOG.NATIONALITY = bio_data.NATIONALITY;
            cDMA_INDIVIDUAL_BIO_DATA_LOG.NICKNAME_ALIAS = bio_data.NICKNAME_ALIAS;
            cDMA_INDIVIDUAL_BIO_DATA_LOG.NUMBER_OF_CHILDREN = bio_data.NUMBER_OF_CHILDREN;
            cDMA_INDIVIDUAL_BIO_DATA_LOG.OTHER_NAME = bio_data.OTHER_NAME;
            cDMA_INDIVIDUAL_BIO_DATA_LOG.PLACE_OF_BIRTH = bio_data.PLACE_OF_BIRTH;
            cDMA_INDIVIDUAL_BIO_DATA_LOG.RELIGION = bio_data.RELIGION;
            cDMA_INDIVIDUAL_BIO_DATA_LOG.SEX = bio_data.SEX;
            cDMA_INDIVIDUAL_BIO_DATA_LOG.STATE_OF_ORIGIN = bio_data.STATE_OF_ORIGIN;
            cDMA_INDIVIDUAL_BIO_DATA_LOG.SURNAME = bio_data.SURNAME;
            cDMA_INDIVIDUAL_BIO_DATA_LOG.TITLE = bio_data.TITLE;
            cDMA_INDIVIDUAL_BIO_DATA_LOG.RELIGION = bio_data.RELIGION;
            cDMA_INDIVIDUAL_BIO_DATA_LOG.COUNTRY_OF_BIRTH = bio_data.COUNTRY_OF_BIRTH;

            return cDMA_INDIVIDUAL_BIO_DATA_LOG;



        }

    }
    }
