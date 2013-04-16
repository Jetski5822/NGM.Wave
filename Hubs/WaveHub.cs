using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using NGM.Wave.Models;
using NGM.Wave.Services;
using Orchard.Caching;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.Environment;
using Orchard.Localization;
using Orchard.Security;

namespace NGM.Wave.Hubs {
    [HubName("waveHub")]
    public class WaveHub : Hub {
        private readonly Work<IContentManager> _workContentManager;
        private readonly Work<IAuthenticationService> _workAuthenticationService;
        private readonly Work<IContentService> _workContentService;
        private readonly ICacheManager _cacheManager;

        private const string ContentGroupNameFormat = "ContentItem-{0}";

        public WaveHub(Work<IContentManager> workContentManager,
                       Work<IAuthenticationService> workAuthenticationService,
                       Work<IContentService> workContentService,
                       ICacheManager cacheManager) {
            _workContentManager = workContentManager;
            _workAuthenticationService = workAuthenticationService;
            _workContentService = workContentService;
            _cacheManager = cacheManager;
        }

        private IContentManager _contentManager {
            get { return _workContentManager.Value; }
        }

        private IAuthenticationService _authenticationService {
            get { return _workAuthenticationService.Value; }
        }

        private IContentService _contentService {
            get { return _workContentService.Value; }
        }

        private string UserAgent {
            get {
                if (Context.Headers != null) {
                    return Context.Headers["User-Agent"];
                }
                return null;
            }
        }

        public override Task OnConnected() {
            return Clients.All.joined(Context.ConnectionId, DateTime.Now.ToString());
        }

        public override Task OnDisconnected() {
            return Clients.All.leave(Context.ConnectionId, DateTime.Now.ToString());
        }

        public override Task OnReconnected() {
            return Clients.All.rejoined(Context.ConnectionId, DateTime.Now.ToString());
        }

        public void Join(int contentItemId) {
            Join(false, contentItemId);
        }

        public void Join(bool reconnecting, int contentItemId) {
            
            IUser user = _authenticationService.GetAuthenticatedUser() ?? new UnauthenticatedUser(Context.ConnectionId);

            if (!reconnecting) {
                _contentService.UpdateActivity(user, Context.ConnectionId, UserAgent);
            }

            var clientState = new ClientState{ActiveContent = contentItemId};

            OnUserInitialize(clientState, user, reconnecting);
        }

        private void OnUserInitialize(ClientState clientState, IUser user, bool reconnecting) {
            Clients.Caller.activeContent = clientState.ActiveContent;

            if (!reconnecting) {
                Clients.Caller.id = user.Id;
                Clients.Caller.username = user.UserName;
                Clients.Caller.email = user.Email;
            }

            var userViewModel = new UserViewModel(user);

            var isOwner = _contentManager.Get(clientState.ActiveContent).As<ICommonPart>().Owner == user;;

            var contentGroupName = ContentGroupName(clientState.ActiveContent);

            // Tell the people who are editing this content that you are viewing it and cmaybe editing it.
            Clients.Group(contentGroupName).addUser(userViewModel, contentGroupName, isOwner).Wait();

            // Add the caller to the group so they receive content updates
            Groups.Add(Context.ConnectionId, contentGroupName);
        }

        private static string ContentGroupName(int contentItemId) {
            return string.Format(ContentGroupNameFormat, contentItemId);
        }
    }

    public class UserViewModel {
        public UserViewModel(IUser user) {
            UserName = user.UserName;
        }

        public string UserName { get; private set; }
    }

    public class UnauthenticatedUser : IUser {
        public UnauthenticatedUser(string conntentionId) {
            ConntentionId = conntentionId;
        }

        public string ConntentionId { get; private set; }

        public ContentItem ContentItem { get; private set; }
        public int Id { get; private set; }
        public string UserName { get; private set; }
        public string Email { get; private set; }
    }

    public class HubUpdateModel : IUpdateModel {
        private readonly string _data;

        public HubUpdateModel(string data) {
            _data = data;
        }

        public bool TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, string[] excludeProperties) where TModel : class {
            return false;
        }

        public void AddModelError(string key, LocalizedString errorMessage) {
        }
    }
}