using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;

namespace SORS.Pages
{
    public class ErrorlogModel : PageModel
    {
        private readonly string logFilePath = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName, "log.txt");

        public string LogContent { get; private set; }

        public void OnGet()
        {
            if (System.IO.File.Exists(logFilePath))
            {
                LogContent = System.IO.File.ReadAllText(logFilePath).Replace(Environment.NewLine, "<br/>");
            }
            else
            {
                LogContent = "No errors to display.";
            }
        }
    }
}

