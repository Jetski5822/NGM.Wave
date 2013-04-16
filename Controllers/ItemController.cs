using System.Web.Mvc;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Core.Contents;
using Orchard.Localization;
using Orchard.Mvc;
using Orchard.Themes;

namespace NGM.Wave.Controllers {
    [Themed]
    public class ItemController : Controller {
        private readonly IContentManager _contentManager;

        public ItemController(IContentManager contentManager, IOrchardServices services) {
            _contentManager = contentManager;
            Services = services;
            T = NullLocalizer.Instance;
        }

        public IOrchardServices Services { get; private set; }
        public Localizer T { get; set; }

        // /Contents/Item/DisplaySummary/72
        public ActionResult DisplaySummary(int id) {
            var contentItem = _contentManager.Get(id, VersionOptions.Published);

            if (contentItem == null)
                return HttpNotFound();

            if (!Services.Authorizer.Authorize(Permissions.ViewContent, contentItem, T("Cannot view content"))) {
                return new HttpUnauthorizedResult();
            }

            dynamic model = _contentManager.BuildDisplay(contentItem, "Summary");
            return new ShapeResult(this, model);
        }
    }
}