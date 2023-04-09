using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace WebApplication.Infrastructure
{
    public static class TempDataExtensions
    {
        public static T ReadObject<T>(this ITempDataDictionary tempData, string key) where T: class
        {
           string jsonStr = tempData[key] as string;
            return string.IsNullOrEmpty(jsonStr)
                ? null
                : JsonConvert.DeserializeObject<T>(jsonStr);
        }

        public static void WriteObject(this ITempDataDictionary tempData, string key, object value)
        {
            string jsonStr = JsonConvert.SerializeObject(value: value);
            tempData[key] = jsonStr;
        }
    }
}
