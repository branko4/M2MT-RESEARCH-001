using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XSLT_CLI
{
    public class SDK
    {
        private readonly IDManager IDManager = new IDManager();
        public string GetID(string name)
        {
            return this.IDManager.GetID(name);
        }

        public string GetID(string puic, string objectTypeName)
        {
            return GetID($"{puic},{objectTypeName}");
        }
    }
}
