using System;
using NGM.Forum.Extensions;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using Orchard.Data.Migration;

namespace NGM.Wave {
    public class Migrations : DataMigrationImpl {
        private readonly IContentManager _contentManager;

        public Migrations(IContentManager contentManager) {
            _contentManager = contentManager;
        }

        public int Create() {
            //ContentDefinitionManager.AlterTypeDefinition("Thread", cfg => cfg.WithPart("ThreadHubPart"));

            // Every user has their own forum.
            // A forum is a a users view of 'Stuff'

            //ContentDefinitionManager.AlterTypeDefinition(Constants.ContentType.UserWave, cfg => cfg
            //    .WithPart("ForumPart")
            //    .WithPart("CommonPart")
            //    .WithPart("AutoroutePart", builder => builder
            //        .WithSetting("AutorouteSettings.AllowCustomPattern", "true")
            //        .WithSetting("AutorouteSettings.AutomaticAdjustmentOnEdit", "false")
            //        .WithSetting("AutorouteSettings.PatternDefinitions", "[{Name:'Title', Pattern: 'User/{User.Id}', Description: 'user-forum'}]")
            //        .WithSetting("AutorouteSettings.DefaultPatternIndex", "0"))
            //    .WithPart("TitlePart")
            //);


            //var users = _contentManager.Query<UserPart, UserPartRecord>().List();

            //foreach (var user in users) {
            //    var forum = _contentManager.New<ForumPart>(Constants.ContentType.UserWave);
            //    _contentManager.Create(forum, VersionOptions.Draft);
            //    forum.As<ForumPart>().Description = string.Format("Wave for {0}", user.UserName);
            //    forum.As<TitlePart>().Title = string.Format("Wave for {0}", user.UserName);
            //    forum.As<CommonPart>().Owner = user;
            //    _contentManager.Publish(forum.ContentItem);
            //}

            ContentDefinitionManager.AlterTypeDefinition(Constants.Parts.Forum, cfg => cfg.WithPart("WavePart"));
            ContentDefinitionManager.AlterTypeDefinition(Constants.Parts.Thread, cfg => cfg.WithPart("WavePart"));

            //SchemaBuilder.CreateTable("ContentClientRecord",
            //    table => table
            //        .Column<int>("Id", column => column.PrimaryKey().Identity())
            //        .Column<int>("UserId")
            //        .Column<string>("UserAgent")
            //        .Column<DateTime>("LastActivity")
            //        .Column<int>("UserKey")
            //    );

            return 1;
        }
    }
}