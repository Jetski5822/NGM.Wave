using System.Web.Mvc;
using JetBrains.Annotations;
using NGM.Forum.Extensions;
using NGM.Forum.Models;
using Orchard;
using Orchard.ContentManagement.Handlers;

namespace NGM.Wave.Handlers {
    [UsedImplicitly]
    public class ThreadPartHandler : ContentHandler {
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly IWaveNotificationWrapper _waveNotificationWrapper;

        public ThreadPartHandler(IWorkContextAccessor workContextAccessor,
            IWaveNotificationWrapper waveNotificationWrapper) {
            _workContextAccessor = workContextAccessor;
            _waveNotificationWrapper = waveNotificationWrapper;

            OnPublished<PostPart>((context, postPart) => {
                if (postPart.IsParentThread())
                    return;

                var threadPart = postPart.ThreadPart;
                var forumPart = threadPart.ForumPart;

                var workContext = _workContextAccessor.GetContext();
                var location = new UrlHelper(workContext.HttpContext.Request.RequestContext).PostView(postPart);

                _waveNotificationWrapper.NotifyPostCreated(threadPart.Id, forumPart.Id, threadPart.Id, postPart.Id, postPart.RepliedOn.Value, location, postPart.Text);
                _waveNotificationWrapper.NotifyPostCreated(forumPart.Id, forumPart.Id, threadPart.Id, postPart.Id, postPart.RepliedOn.Value, location, postPart.Text);
            });
        }
    }
}