﻿using CMdm.Entities.Domain.CustomModule.Fcmb;
using CMdm.Services.ExportImport.Help;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CMdm.Services.ExportImport
{
    /// <summary>
    /// Export manager
    /// </summary>
    public partial class ExportManager : IExportManager
    {
        #region Ctor

        public ExportManager()
        {

        }
        #endregion
        protected virtual void SetCaptionStyle(ExcelStyle style)
        {
            style.Fill.PatternType = ExcelFillStyle.Solid;
            style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
            style.Font.Bold = true;
        }
        /// <summary>
        /// Export documents list to XLSX
        /// </summary>
        /// <param name="documents">documents</param>
        /// 
        public virtual byte[] ExportDocumentsToXlsx(IList<DistinctDocs> documents)
        {
            //property array
            var properties = new[]
            {
                new PropertyByName<DistinctDocs>("Account Name", p => p.ACCT_NAME),

                new PropertyByName<DistinctDocs>("Branch Code", p => p.SOL_ID),
                new PropertyByName<DistinctDocs>("Branch Name", p => p.BRANCH_NAME),

                new PropertyByName<DistinctDocs>("Due Date", p => p.DUE_DATE.ToString()),
                new PropertyByName<DistinctDocs>("Reason Code", p => p.FREZ_REASON_CODE),
                new PropertyByName<DistinctDocs>("CustomerId", p => p.CIF_ID),
                new PropertyByName<DistinctDocs>("Account Officer Code", p => p.ACCTOFFICER_CODE),
                new PropertyByName<DistinctDocs>("Account Officer Name", p => p.ACCTOFFICER_NAME),
                new PropertyByName<DistinctDocs>("Customer Type", p => p.CUSTOMERTYPE),
            };

            return ExportToXlsx(properties, documents);
        }

        public virtual byte[] ExportOutDocumentsToXlsx(IList<OutStandingDoc> documents)
        {
            //property array
            var properties = new[]
            {
                
                new PropertyByName<OutStandingDoc>("Account Number", p => p.FORACID),
                new PropertyByName<OutStandingDoc>("Account Name", p => p.ACCT_NAME),

                new PropertyByName<OutStandingDoc>("Branch Code", p => p.SOL_ID),
                new PropertyByName<OutStandingDoc>("Branch Name", p => p.BRANCH_NAME),
                new PropertyByName<OutStandingDoc>("Product Name", p => p.SCHM_DESC),
                new PropertyByName<OutStandingDoc>("Type", p => p.SCHM_TYPE),
                new PropertyByName<OutStandingDoc>("Product Description", p => p.REF_DESC),                
                new PropertyByName<OutStandingDoc>("Scheme Code", p => p.SCHM_CODE),             
                
                new PropertyByName<OutStandingDoc>("Due Date", p => p.DUE_DATE.ToString()),
                new PropertyByName<OutStandingDoc>("Reason", p => p.FREZ_REASON_CODE),
                new PropertyByName<OutStandingDoc>("Customer Id", p => p.CIF_ID),
                new PropertyByName<OutStandingDoc>("Account Officer Code", p => p.ACCTOFFICER_CODE),
                new PropertyByName<OutStandingDoc>("Account Officer Name", p => p.ACCTOFFICER_NAME),
                new PropertyByName<OutStandingDoc>("Customer Type", p => p.CUSTOMERTYPE),
            };

            return ExportToXlsx(properties, documents);
        }
        /// <summary>
        /// Export objects to XLSX
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="properties">Class access to the object through its properties</param>
        /// <param name="itemsToExport">The objects to export</param>
        /// <returns></returns>
        protected virtual byte[] ExportToXlsx<T>(PropertyByName<T>[] properties, IEnumerable<T> itemsToExport)
        {
            using (var stream = new MemoryStream())
            {
                // ok, we can run the real code of the sample now
                using (var xlPackage = new ExcelPackage(stream))
                {
                    // uncomment this line if you want the XML written out to the outputDir
                    //xlPackage.DebugMode = true; 

                    // get handles to the worksheets
                    var worksheet = xlPackage.Workbook.Worksheets.Add(typeof(T).Name);
                    var fWorksheet = xlPackage.Workbook.Worksheets.Add("DataForFilters");
                    fWorksheet.Hidden = eWorkSheetHidden.VeryHidden;

                    //create Headers and format them 
                    var manager = new PropertyManager<T>(properties.Where(p => !p.Ignore));
                    manager.WriteCaption(worksheet, SetCaptionStyle);

                    var row = 2;
                    foreach (var items in itemsToExport)
                    {
                        manager.CurrentObject = items;
                        manager.WriteToXlsx(worksheet, row++, false, fWorksheet: fWorksheet);
                    }

                    xlPackage.Save();
                }
                return stream.ToArray();
            }
        }
    }
}
