using System.Web.Mvc;
using JetBrains.Annotations;
using NGM.Forum.Extensions;
using NGM.Forum.Models;
using Orchard;
using Orchard.ContentManagement.Handlers;

namespace NGM.Wave.Handlers {
    [UsedImplicitly]
    public class ForumPartHandler : ContentHandler {
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly IWaveNotificationWrapper _waveNotificationWrapper;

        public ForumPartHandler(IWorkContextAccessor workContextAccessor,
            IWaveNotificationWrapper waveNotificationWrapper) {
            _workContextAccessor = workContextAccessor;
            _waveNotificationWrapper = waveNotificationWrapper;

            OnPublished<PostPart>((context, postPart) => {
                if (!postPart.IsParentThread())
                    return;

                var threadPart = postPart.ThreadPart;
                var forumPart = threadPart.ForumPart;

                var workContext = _workContextAccessor.GetContext();
                var location = new UrlHelper(workContext.HttpContext.Request.RequestContext).ThreadView(threadPart);

                _waveNotificationWrapper.NotifyThreadCreated(forumPart.Id, threadPart.Id, postPart.Id, location);
            });

            OnRemoved<ThreadPart>((context, threadPart) => {
                var forumPart = threadPart.ForumPart;
                
                _waveNotificationWrapper.NotifyThreadRemoved(forumPart.Id, threadPart.Id);
            });
        }
    }
}