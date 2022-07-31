using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Models
{
    class ImportedObject : ImportedObjectBaseClass
    {
        public string schema;
        public string parentName;
        public string parentType { get; set; }
        public string dataType { get; set; }
        public string isNullable { get; set; }
        public double numberOfChildren;
    }
}
