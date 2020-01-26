using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace PokerOddsRazor.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty(SupportsGet = true)] //makes it POST only without supportsget
        public string DemoText { get; set; }
        [BindProperty(SupportsGet = true)]
        public string IndexCity { get; set; }

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            if (string.IsNullOrEmpty(DemoText))
            {
                DemoText = "Hi this text generated in Index.cshtml.cs. Put some text in query string to change" +
                    "localhost:5001/?demotext=\"testing\"";
            }

            if (string.IsNullOrEmpty(IndexCity))
            {
                IndexCity = "(set this city in add address)";
            }
        }
    }
}
