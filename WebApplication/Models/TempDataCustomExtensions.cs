using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Infrastructure;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace WebApplication.Models
{
    public static class TempDataCustomExtensions
    {
        private const string alertKey = "alert_message";

        public static void WriteAlertMessage(
            this ITempDataDictionary tempData, 
            string messageText,
            TempdataMessageType messageType = TempdataMessageType.Info)
        {
            tempData.WriteObject(
                key: alertKey,
                value: new PageAlertMessage 
                {
                    Text = messageText,
                    MessageType = messageType
                });
        }

        public static PageAlertMessage ReadAlertMessage(this ITempDataDictionary tempData)
        {
            return tempData.ReadObject<PageAlertMessage>(key: alertKey);
        }
    }
}
