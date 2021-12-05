using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SofTrustTask.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet() => Response.Redirect("index.html");
    }
}
