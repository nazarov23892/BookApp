using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class ViewAlertMessage
    {
        public string Text { get; set; }
        public ViewAlertMessageType MessageType { get; set; } = ViewAlertMessageType.Info;
    }

    public enum ViewAlertMessageType
    {
        Info,
        Success,
        Danger
    }
}
