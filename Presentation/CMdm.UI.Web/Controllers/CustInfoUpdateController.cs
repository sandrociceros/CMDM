using CMdm.Data;
using CMdm.Framework.Controllers;
using CMdm.Services.DqQue;
using CMdm.Services.Messaging;
using CMdm.UI.Web.Helpers.CrossCutting.Security;
using CMdm.UI.Web.Models.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMdm.UI.Web.Controllers
{
    public class CustInfoUpdateController : BaseController
    {
        private AppDbContext _db = new AppDbContext();
        private IDqQueService _dqQueService;
        private IMessagingService _messageService;

        public CustInfoUpdateController()
        {
            //bizrule = new DQQueBiz();
            _dqQueService = new DqQueService();
            _messageService = new MessagingService();
        }

        // GET: CustInfoUpdate
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
                return AccessDeniedView();
            var identity = ((CustomPrincipal)User).CustomIdentity;

            return View();
        }

        [HttpPost]
        [FormValueRequired("search")]
        [ValidateAntiForgeryToken]
        public ActionResult Index(CIUModel ciumodel)
        {
            var identity = ((CustomPrincipal)User).CustomIdentity;

            string customer_no = Request["CUSTOMER_NO"].ToString();

            int records = _db.CDMA_INDIVIDUAL_BIO_DATA.Count(o => o.CUSTOMER_NO == customer_no);

            
            if (records > 0)
            {
                return RedirectToAction("Edit", "IndividualCustomer", new { id = customer_no });
            }
            else
            {
                string errorMessage = string.Format("Customer with Id:{0} does not exist.", customer_no);
                ModelState.AddModelError("", errorMessage);
                return View();
            }
            
        }
    }
}