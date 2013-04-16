using Orchard.UI.Resources;

namespace NGM.Wave {
    public class ResourceManifest : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();
            manifest.DefineScript("WaveUI").SetUrl("ngm.wave.ui.js").SetDependencies("jQuery");

            manifest.DefineScript("Wave").SetUrl("ngm.wave.js").SetDependencies("WaveUI", "jQuery_SignalR_Hubs");
        }
    }
}