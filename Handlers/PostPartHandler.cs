using JetBrains.Annotations;
using NGM.Forum.Models;
using Orchard.ContentManagement.Handlers;

namespace NGM.Wave.Handlers {
    [UsedImplicitly]
    public class PostPartHandler : ContentHandler {
        private readonly IWaveNotificationWrapper _waveNotificationWrapper;

        public PostPartHandler(IWaveNotificationWrapper waveNotificationWrapper) {
            _waveNotificationWrapper = waveNotificationWrapper;

            OnRemoved<PostPart>((context, postPart) => {
                if (postPart.IsParentThread())
                    return;

                var threadPart = postPart.ThreadPart;
                var forumPart = threadPart.ForumPart;

                _waveNotificationWrapper.NotifyPostRemoved(forumPart.Id, threadPart.Id, postPart.Id);
            });
        }
    }
}