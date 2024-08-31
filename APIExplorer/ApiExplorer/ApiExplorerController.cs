using EPiServer.Shell.Modules;
using EPiServer.Shell.ViewComposition;
using EPiServer.Shell.Web.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIExplorer.ApiExplorer
{
    [Route("api/[controller]")]
    public class ApiExplorerController : Controller
    {
        private readonly IBootstrapper _bootstrapper;
        private readonly IViewManager _viewManager;
        private readonly ModuleTable _moduleTable;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiExplorerController(
            IBootstrapper bootstrapper,
            IViewManager viewManager,
            ModuleTable moduleTable,
            IHttpContextAccessor httpContextAccessor)
        {
            _bootstrapper = bootstrapper;
            _viewManager = viewManager;
            _moduleTable = moduleTable;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet("index")]
        public ActionResult Index()
        {
            var model = new ApiExplorerModel()
            {
                SwaggerUrl = "/swagger/index.html"
            };
            return View(nameof(Index), model);
        }

        protected static string BuildViewName(string methodName)
        {
            return $"APIExplorer.ApiExplorer/Views/ApiExplorer/{methodName}.cshtml";
        }

    }
}
