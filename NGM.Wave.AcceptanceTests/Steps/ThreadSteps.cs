using System;
using BoDi;
using NGM.Wave.AcceptanceTests.Helpers;
using TechTalk.SpecFlow;

namespace NGM.Wave.AcceptanceTests.Steps {
    [Binding]
    public class ThreadSteps {
        private readonly Lazy<IOrchardClient> _lazyOrchardClientForMarkus;
        private readonly Lazy<IOrchardClient> _lazyOrchardClientForVasundhara;

        private IOrchardClient Markus {
            get {
                if (!_lazyOrchardClientForMarkus.IsValueCreated) {
                    var val = _lazyOrchardClientForMarkus.Value;
                    val.Logon(ExampleUser.MarkusMachado);
                }
                return _lazyOrchardClientForMarkus.Value;
            }
        }

        private IOrchardClient Vasundhara {
            get {
                if (!_lazyOrchardClientForVasundhara.IsValueCreated) {
                    var val = _lazyOrchardClientForVasundhara.Value;
                    val.Logon(ExampleUser.VasundharaAraya);
                }
                return _lazyOrchardClientForVasundhara.Value;
            }
        }

        public ThreadSteps(IObjectContainer objectContainer) {
            _lazyOrchardClientForMarkus = new Lazy<IOrchardClient>(() =>objectContainer.Resolve<IOrchardClient>(ExampleUser.MarkusMachado.Username));
            _lazyOrchardClientForVasundhara = new Lazy<IOrchardClient>(() => objectContainer.Resolve<IOrchardClient>(ExampleUser.VasundharaAraya.Username));
        }

        [Given(@"Markus Machado has found the Discussions Forum that he would like the create a Thread on")]
        public void GivenMarkusMachadoHasFoundTheDiscussionsForumThatHeWouldLikeTheCreateAThreadOn() {
            using (var admin = new OrchardClient()) {
                admin.Logon(ExampleUser.Admin);
                admin.CreateDiscussionsForum();
                admin.Logout();
            }

            Markus.ViewDiscussionsForum();
        }

        [Given(@"Vasundhara Araya is also currently viewing the Discussions Forum")]
        public void GivenVasundharaArayaIsAlsoCurrentlyViewingTheDiscussionsForum() {
            Vasundhara.ViewDiscussionsForum();
        }

        [Given(@"Markus Machado has created a Thread on the Discussions Forum")]
        public void GivenMarkusMachadoHasCreatedAThreadOnTheDiscussionsForum() {
            GivenMarkusMachadoHasFoundTheDiscussionsForumThatHeWouldLikeTheCreateAThreadOn();
            WhenMarkusMachadoCreatesANewThread();
        }

        [Given(@"both Markus and Vasundhara are currently viewing the Discussions Forum")]
        public void GivenBothMarkusAndVasundharaAreCurrentlyViewingTheDiscussionsForum() {
            Markus.ViewDiscussionsForum();

            Vasundhara.ViewDiscussionsForum();
        }

        [When(@"Markus Machado creates a new Thread")]
        public void WhenMarkusMachadoCreatesANewThread() {
            Markus.CreateThread("I have a problem with my Jiggs.", "How do I solve it?");
        }

        [When(@"Markus Machado deletes the Thread")]
        public void WhenMarkusMachadoDeletesTheThread() {
            Markus.DeleteThreadFromForumListView("I have a problem with my Jiggs.");
        }

        [Then(@"he is taken to that Thread")]
        public void ThenHeIsTakenToThatThread() {
            Markus.AssertViewingDetailViewOfThread("I have a problem with my Jiggs.", "How do I solve it?");
        }

        [Then(@"Vasundhara view of the Discussions Forum is updated to include the new Thread")]
        public void ThenVasundharaViewOfTheDiscussionsForumIsUpdatedToIncludeTheNewThread() {
            Vasundhara.AssertThreadIsInForumThreadsList("I have a problem with my Jiggs.", "How do I solve it?");
        }

        [Then(@"the thread is removed from Markus Forum view")]
        public void ThenTheThreadIsRemovedFromMarkusForumView() {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the thread is removed from Vasundhara Forum view")]
        public void ThenTheThreadIsRemovedFromVasundharaForumView() {
            ScenarioContext.Current.Pending();
        }


        [When(@"Markus views the newly created Thread")]
        public void WhenMarkusViewsTheNewlyCreatedThread() {
            Markus.ViewDiscussionsForum();
            Markus.NavigateToThread("I have a problem with my Jiggs.");
        }

        [Then(@"Markus's name is represented with the word 'You'")]
        public void ThenMarkusNameIsRepresentedWithTheWordYou() {
            Markus.AssertCurrentViewingUsers("You");
        }

        [When(@"Vasundhara also views the newly created Thread at the same time as Markus")]
        public void WhenVasundharaAlsoViewsTheNewlyCreatedThreadAtTheSameTimeAsMarkus() {
            Vasundhara.ViewDiscussionsForum();
            Vasundhara.NavigateToThread("I have a problem with my Jiggs.");
        }

        [Then(@"(Vasundhara|Markus) sees only his name represented as \'You\' and (?:Vasundhara|Markus)'s name")]
        public void ThenVasundharaSeesOnlyHisNameRepresentedAsAndMarkusSName(string name) {
            Vasundhara.AssertCurrentViewingUsers("You, " + name);
        }

        [Given(@"both Markus and Vasundhara are viewing the same Thread")]
        public void GivenBothMarkusAndVasundharaAreViewingTheSameThread() {
            Markus.ViewDiscussionsForum();
            Markus.NavigateToThread("I have a problem with my Jiggs.");

            Vasundhara.ViewDiscussionsForum();
            Vasundhara.NavigateToThread("I have a problem with my Jiggs.");
        }

        [Given(@"Markus and Vasundhara are viewing the Discussions Forum")]
        public void GivenMarkusAndVasundharaAreViewingTheDiscussionsForum() {
            using (var admin = new OrchardClient()) {
                admin.Logon(ExampleUser.Admin);
                admin.CreateDiscussionsForum();
                admin.Logout();
            }

            Markus.ViewDiscussionsForum();
            Vasundhara.ViewDiscussionsForum();
        }

        [When(@"Markus navigates back to the discussions Forum")]
        public void WhenMarkusNavigatesBackToTheDiscussionsForum() {
            Markus.ViewDiscussionsForum();
        }

        [When(@"Markus refreshes the discussions Forum")]
        public void WhenMarkusRefreshesTheDiscussionsForum() {
            Markus.ViewDiscussionsForum();
        }

        [Then(@"Vasundhara should see Markus's name removed from the list of active users viewing that Thread")]
        public void ThenVasundharaShouldSeeMarkusSNameRemovedFromTheListOfActiveUsersViewingThatThread() {
            Vasundhara.AssertCurrentViewingUsers("You");
        }
    }
}
