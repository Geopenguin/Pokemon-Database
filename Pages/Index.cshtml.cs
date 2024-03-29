using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Pokemon_Database.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Collection of ICards
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public IEnumerable<ICards>? Cards { get; set; }


        public void OnGet()
        {

        }
    }
}
