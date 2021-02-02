using System;
using System.Collections.Generic;
using System.Text;

namespace InfoTextSMSDashboard.BLL.Models
{
    public class OutputResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object SuccessResult { get; set; }
    }
}
