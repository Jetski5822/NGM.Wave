using System.Web.Routing;
using NGM.Wave.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.DisplayManagement.Descriptors;
using Orchard.UI.Admin;

namespace NGM.Wave {
    public class Shapes : IShapeTableProvider {
        private readonly IWorkContextAccessor _workContext;

        public Shapes(IWorkContextAccessor workContext) {
            _workContext = workContext;
        }

        public void Discover(ShapeTableBuilder builder) {
            builder.Describe("Content")
                   .OnDisplaying(displaying =>
                       {
                           if (AdminFilter.IsApplied(new RequestContext(_workContext.GetContext().HttpContext,new RouteData())))
                               return;

                           if (displaying.ShapeMetadata.DisplayType != "Detail")
                               return;

                           ContentItem contentItem = displaying.Shape.ContentItem;
                           if (contentItem != null) {
                               var wavePart = contentItem.As<WavePart>();

                               if (wavePart == null)
                                   return;

                               displaying.ShapeMetadata.Wrappers.Add("Wave_Wrapper");
                           }
                       });
        }
    }
}
