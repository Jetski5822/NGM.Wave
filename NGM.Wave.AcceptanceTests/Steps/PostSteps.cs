using System;
using BoDi;
using NGM.Wave.AcceptanceTests.Helpers;
using TechTalk.SpecFlow;

namespace NGM.Wave.AcceptanceTests.Steps {
    [Binding]
    public class PostSteps {
        private readonly Lazy<IOrchardClient> _lazyOrchardClientForMarkus;
        private readonly Lazy<IOrchardClient> _lazyOrchardClientForVasundhara;
        private IOrchardClient Markus { get { return _lazyOrchardClientForMarkus.Value; } }
        private IOrchardClient Vasundhara { get { return _lazyOrchardClientForVasundhara.Value; } }

        public PostSteps(IObjectContainer objectContainer) {
            _lazyOrchardClientForMarkus = new Lazy<IOrchardClient>(() =>objectContainer.Resolve<IOrchardClient>(ExampleUser.MarkusMachado.Username));
            _lazyOrchardClientForVasundhara = new Lazy<IOrchardClient>(() => objectContainer.Resolve<IOrchardClient>(ExampleUser.VasundharaAraya.Username));
        }

        [AfterScenario]
        public void TearDownBrowser() {
            Markus.Dispose();
            Vasundhara.Dispose();
        }

        [Given(@"Markus is currently viewing the Discussions Forum")]
        public void GivenMarkusIsCurrentlyViewingTheDiscussionsForum() {
            Markus.ViewDiscussionsForum();
        }

        [When(@"Vasundhara replies to Markus's question")]
        public void WhenVasundharaRepliesToMarkusSQuestion() {
            Vasundhara.Logon(ExampleUser.VasundharaAraya);
            Vasundhara.ViewDiscussionsForum();
            Vasundhara.NavigateToThread("I have a problem with my Jiggs.");
            Vasundhara.ReplyToThread("Have you thought about doing it a different way?");
        }

        [Then(@"Vasundhara see the reply added")]
        public void ThenVasundharaSeeTheReplyAdded() {
            Vasundhara.AssertThatReplyAdded("Have you thought about doing it a different way?");
        }

        [Then(@"Markus sees details on that Thread be updated in the Discussions Forum")]
        public void ThenMarkusSeesDetailsOnThatThreadBeUpdatedInTheDiscussionsForum() {
            Markus.AssertThreadIsInForumThreadsList("I have a problem with my Jiggs.", "Have you thought about doing it a different way?");
        }
    }
}
