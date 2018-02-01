﻿using CMdm.UI.Web.ActionFilters;
using System.Web.Mvc;

namespace CMdm.UI.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorWithELMAHAttribute());
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ElmahHandledErrorLoggerFilter());
            filters.Add(new AuthorizeAttribute());
            //filters.Add(new AuthorizeAttribute());
        }
    }
}
