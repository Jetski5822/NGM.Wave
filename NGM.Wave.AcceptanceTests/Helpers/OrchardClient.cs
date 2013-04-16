using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coypu;
using Coypu.Matchers;
using Coypu.Queries;
using NGM.Wave.AcceptanceTests.Enviroment;
using NUnit.Framework;

namespace NGM.Wave.AcceptanceTests.Helpers {
    public interface IOrchardClient : IDisposable {
        void CreateDiscussionsForum();
        void ViewDiscussionsForum();
        void Logon(ExampleUser user);
        void Logout();
        void CreateThread(string title, string body);
        void AssertViewingDetailViewOfThread(string threadTitle, string threadBody);
        void AssertThreadIsInForumThreadsList(string threadTitle, string threadBody);
        void DeleteThreadFromForumListView(string threadTitle);
        void NavigateToThread(string threadTitle);
        void ReplyToThread(string replyText);
        void AssertThatReplyAdded(string replyText);
        void AssertCurrentViewingUsers(string userNameText);
    }

    public class OrchardClient : IOrchardClient {
        private readonly Lazy<BrowserSession> _lazyBrowserSession = new Lazy<BrowserSession>(() => new BrowserSession(Configuration.Orchard()));
        private BrowserSession Browser { get { return _lazyBrowserSession.Value; } }

        public void CreateDiscussionsForum() {
            Browser.ClickLink("Dashboard");
            Browser.ClickLink("Forum");
            Browser.FillIn("Title").With("Discussions");
            Browser.FillIn("Description").With("This is a discussions Forum");
            Browser.FindCss("[data-id-prefix='ForumPart_Categories'] .expando-glyph").Click();
            Browser.Choose("General");
            Browser.ClickButton("Save");
        }

        public void ViewDiscussionsForum() {
            Browser.Visit("/Discussions");
        }

        public void Logon(ExampleUser user) {
            Browser.Visit("/Users/Account/LogOn");
            Browser.FillIn("Username").With(user.Username);
            Browser.FillIn("Password").With(user.Password);
            Browser.ClickButton("Sign In");
        }

        public void Logout() {
            var signOutLink = Browser.FindLink("Sign Out");

            if (signOutLink.Exists()) {
                signOutLink.Click();
            }
            else {
                Browser.ClickLink("Logout");
            }
        }

        public void CreateThread(string title, string body) {
            Browser.FindCss("a.create").Click();
            Browser.FillIn("Title").With(title);
            Browser.ExecuteScript("$(\"[data-id-prefix='ThreadPart_Categories'] .expando-glyph\").click()");
            Browser.Choose("General");
            Browser.FillIn("PostPart_Text").With(body);
            Browser.ClickButton("Save");
        }

        public void DeleteThreadFromForumListView(string threadTitle) {
            Browser.ClickLink(threadTitle.ToLowerInvariant());
            Browser.FindCss("article.thread a.remove").Click();
        }

        public void NavigateToThread(string threadTitle) {
            Browser.ClickLink(threadTitle.ToLowerInvariant());
        }

        public void ReplyToThread(string replyText) {
            Browser.FillIn("PostPart_Text").With(replyText);
            Browser.ClickButton("Save");
        }

        public void AssertThatReplyAdded(string replyText) {
            var postBody = Browser.FindAllCss(".post").Last();
            Assert.That(postBody.Text, Is.StringContaining(replyText).IgnoreCase);
            //Assert.That(postBody, Shows.Content(threadBody));
        }

        public void AssertCurrentViewingUsers(string userNameText) {
            Assert.That(Browser.FindCss(".current-users"), Shows.Content(userNameText));
        }

        public void AssertViewingDetailViewOfThread(string threadTitle, string threadBody) {
            var threadHeader = Browser.FindCss("article.thread h2");
            Assert.That(threadHeader.Text, Is.StringContaining(threadTitle).IgnoreCase);
            //Assert.That(threadHeader, Shows.Content(threadTitle));

            var postBody = Browser.FindCss(".post");
            Assert.That(postBody.Text, Is.StringContaining(threadBody).IgnoreCase);
            //Assert.That(postBody, Shows.Content(threadBody));
        }

        public void AssertThreadIsInForumThreadsList(string threadTitle, string threadBody) {
            var thread = Browser.FindCss("article.thread");
            Assert.That(thread.Text, Is.StringContaining(threadTitle).IgnoreCase);
            Assert.That(thread.Text, Is.StringContaining(threadBody).IgnoreCase);
            
            //Assert.That(thread, Shows.Content(threadTitle));
            //Assert.That(thread, Shows.Content(threadBody));
        }

        public void AssertThreadIsNotInForumThreadList(string threadTitle, string threadBody) {
            var thread = Browser.FindCss("article.thread");
            Assert.That(thread.Text, Is.Not.StringContaining(threadTitle).IgnoreCase);
            Assert.That(thread.Text, Is.Not.StringContaining(threadBody).IgnoreCase);

            //Assert.That(thread, Shows.No.Content(threadTitle));
            //Assert.That(thread, Shows.No.Content(threadBody));
        }

        public void Dispose() {
            Browser.Dispose();
        }
    }
}
