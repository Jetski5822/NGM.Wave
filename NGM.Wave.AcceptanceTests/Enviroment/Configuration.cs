using System;
using System.Configuration;
using Coypu;
using Coypu.Drivers;

namespace NGM.Wave.AcceptanceTests.Enviroment {
    public static class Configuration {
        public static SessionConfiguration Orchard() {
            return new SessionConfiguration {
                AppHost = ConfigurationManager.AppSettings["OrchardUrl"],
                Port = int.Parse(ConfigurationManager.AppSettings["OrchardPort"]),
                Browser = Browser.Chrome,
                Timeout = TimeSpan.FromSeconds(5)
            };
        }
    }
}