﻿using CMdm.Core;
using CMdm.Entities.Domain.CustomModule.Fcmb;
using CMdm.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMdm.Services.CustomModule.Fcmb
{
    public interface IMultipleRefCodeService
    {
        /// <summary>
        /// Gets a queitem by item identifiers
        /// </summary>
        /// <param name="recordId">recordId identifier</param>
        /// <returns>Vendor</returns>
        IList<MultipleRefCode> GetMultipleRefCodebyIds(int[] recordIds);
        /// <summary>
        /// Gets a Queitem by item reference identifier
        /// </summary>
        /// <param name="recordId">que identifier</param>
        /// <returns>Vendor</returns>
        MultipleRefCode GetMultRefCodebyId(int recordId);
        /// <summary>
        /// Gets all items
        /// </summary>
        /// <param name="name"> name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="sortExpression">A value indicating whether to show hidden records</param>
        /// <returns>Vendors</returns>
        IPagedList<MultipleRefCode> GetAllMultipleRefCodes(string name = "", string branchCode = "",
            int pageIndex = 0, int pageSize = int.MaxValue, string sortExpression = "");
    }
}
