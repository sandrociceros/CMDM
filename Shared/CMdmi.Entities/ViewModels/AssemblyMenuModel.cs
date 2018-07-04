using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMdm.Entities.ViewModels
{
    public class AssemblyMenuModel
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        public string ReturnType { get; set; }
        public string Attributes { get; set; }
        public string Area { get; set; }
    }
}
