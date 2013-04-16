using System.Configuration;
using System.Diagnostics;
using System.IO;
using BoDi;
using NGM.Wave.AcceptanceTests.Helpers;
using TechTalk.SpecFlow;

namespace NGM.Wave.AcceptanceTests.Steps {
    [Binding]
    public class OrchardBrowserScenario {
        private readonly IObjectContainer _objectContainer;
        
        private Process _iisProcess;

        public OrchardBrowserScenario(IObjectContainer objectContainer) {
            _objectContainer = objectContainer;
        }

        [BeforeScenario]
        public void SetupBrowser() {

            SetupOrchardDatabase();
            SetupIIS();

            _objectContainer.RegisterInstanceAs<IOrchardClient>(new OrchardClient(), ExampleUser.MarkusMachado.Username);
            _objectContainer.RegisterInstanceAs<IOrchardClient>(new OrchardClient(), ExampleUser.VasundharaAraya.Username);
        }

        private void SetupIIS() {
            _iisProcess = Process.Start(new ProcessStartInfo() {
                FileName = @"C:\Program Files (x86)\IIS Express\iisexpress.exe",
                Arguments = @"/port:" + ConfigurationManager.AppSettings["OrchardPort"] +  @" /path:D:\Orchard\orchard_testbed\src\Orchard.Web",
                RedirectStandardOutput = false,
                UseShellExecute = true,
                CreateNoWindow = false,
                Verb = "runas"
            });
        }

        [AfterScenario]
        public void TearDownIIS() {
            _iisProcess.Kill();
        }

        private void SetupOrchardDatabase() {

            if (Directory.Exists(@"D:\Orchard\orchard_testbed\src\Orchard.Web\App_Data\Sites"))
                Directory.Delete(@"D:\Orchard\orchard_testbed\src\Orchard.Web\App_Data\Sites", true);

            if (File.Exists(@"D:\Orchard\orchard_testbed\src\Orchard.Web\App_Data\Cache.dat"))
                File.Delete(@"D:\Orchard\orchard_testbed\src\Orchard.Web\App_Data\Cache.dat");

            if (File.Exists(@"D:\Orchard\orchard_testbed\src\Orchard.Web\App_Data\hrestart.txt"))
                File.Delete(@"D:\Orchard\orchard_testbed\src\Orchard.Web\App_Data\hrestart.txt");

            DirectoryCopy(
                @"D:\Orchard\orchard_testbed\src\Orchard.Web\Modules\NGM.Wave\NGM.Wave.AcceptanceTests\Sites",
                @"D:\Orchard\orchard_testbed\src\Orchard.Web\App_Data\Sites", true);

        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs) {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists) {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            if (!Directory.Exists(destDirName)) {
                Directory.CreateDirectory(destDirName);
            }

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files) {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            if (copySubDirs) {
                foreach (DirectoryInfo subdir in dirs) {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
    }
}