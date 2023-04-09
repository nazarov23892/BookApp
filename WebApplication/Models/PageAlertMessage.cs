using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class PageAlertMessage
    {
        public string Text { get; set; }
        public TempdataMessageType MessageType { get; set; } = TempdataMessageType.Info;
    }

    public enum TempdataMessageType
    {
        Info,
        Success,
        Danger
    }
}
