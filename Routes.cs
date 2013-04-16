using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using NGM.Wave.Extensions;
using Orchard.Mvc.Routes;

namespace NGM.Wave {
    public class Routes : IRouteProvider {
        public void GetRoutes(ICollection<RouteDescriptor> routes) {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }

        public IEnumerable<RouteDescriptor> GetRoutes() {
            return new[] {
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "Contents/Item/DisplaySummary/{id}",
                                                         new RouteValueDictionary {
                                                                                      {"area", Constants.LocalArea},
                                                                                      {"controller", "Item"},
                                                                                      {"action", "DisplaySummary"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", Constants.LocalArea}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                         };
        }
    }
}