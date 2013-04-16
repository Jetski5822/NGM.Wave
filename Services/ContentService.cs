using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard;
using Orchard.Security;

namespace NGM.Wave.Services {
    public interface IContentService : IDependency {
        void UpdateActivity(IUser user, string connectionId, string userAgent);
    }

    public class ContentService : IContentService {
        public void UpdateActivity(IUser user, string connectionId, string userAgent) {
            
        }
    }
}