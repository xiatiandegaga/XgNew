using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xg.Cloud.Core.Cloud.Utilities
{
    public static class FormDataUtliity
    {
        public static string GetPostParams(IFormCollection form)
        {
            string param ="{";
            foreach (var key in form.Keys.ToList())
            {
                param += "\"" + key +"\""+":" + "\"" + form[key] + "\"" + ",";
            }
            return param.Substring(0,param.Length-1)+"}";
        }
    }
}
