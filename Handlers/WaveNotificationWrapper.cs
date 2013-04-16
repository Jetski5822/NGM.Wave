using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using NGM.Wave.Hubs;
using Orchard;

namespace NGM.Wave.Handlers {
    public interface IWaveNotificationWrapper : IDependency {
        void NotifyThreadRemoved(int forumId, int threadId);
        void NotifyThreadCreated(int forumId, int threadId, int postId, string location);
        void NotifyPostRemoved(int forumId, int threadId, int postId);
        void NotifyPostCreated(int groupId, int forumId, int threadId, int postId, int repliedOn, string location, string text);
    }

    public class WaveNotificationWrapper : IWaveNotificationWrapper {
        private const string ContentGroupNameFormat = "ContentItem-{0}";
        
        private readonly IWorkContextAccessor _workContextAccessor;

        public WaveNotificationWrapper(IWorkContextAccessor workContextAccessor) {
            _workContextAccessor = workContextAccessor;
        }

        private IHubContext Hub {
            get { return _workContextAccessor.GetContext().Resolve<IConnectionManager>().GetHubContext<WaveHub>(); }
        }

        public void NotifyThreadRemoved(int forumId, int threadId) {
            Hub.Clients.Group(ContentGroupName(forumId)).notifyThreadRemoved(forumId, threadId);
        }

        public void NotifyThreadCreated(int forumId, int threadId, int postId, string location) {
            Hub.Clients.Group(ContentGroupName(forumId)).notifyThreadCreated(forumId, threadId, postId, location);
        }

        public void NotifyPostCreated(int groupId, int forumId, int threadId, int postId, int repliedOn, string location, string text) {
            Hub.Clients.Group(ContentGroupName(groupId)).notifyPostCreated(forumId, threadId, postId, repliedOn, location, text);
        }

        public void NotifyPostRemoved(int forumId, int threadId, int postId) {
            Hub.Clients.Group(ContentGroupName(threadId)).notifyPostRemoved(forumId, threadId, postId);
        }

        private static string ContentGroupName(int contentItemId) {
            return string.Format(ContentGroupNameFormat, contentItemId);
        }
    }
}