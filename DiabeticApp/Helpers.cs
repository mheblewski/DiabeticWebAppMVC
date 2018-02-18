using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiabeticApp
{
    public class Helpers
    {
        public IDictionary<string, string> ObjectToDictionary(object obj)
        {
            return obj
                .GetType()
                .GetProperties()
                .ToDictionary(p => p.Name, p => p.GetValue(obj).ToString());
        }
    }
}
