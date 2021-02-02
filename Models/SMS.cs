using System.Collections.Generic;

namespace InfoTextSMSDashboard.BLL.Models
{
    public class SMS
    {
        public List<string> Recipients { get; set; }
        public string Message { get; set; }

        public string From { get; set; }
    }
}
