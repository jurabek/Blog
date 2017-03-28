using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Model.ViewModels
{
    public class SuccessViewModel
    {
        public SuccessViewModel()
        {
        }
        public SuccessViewModel(string title, string message)
        {
            Title = title;
            Message = message;
        }
        public string Title { get; set; }

        public string Message { get; set; }

    }
}
