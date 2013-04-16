namespace NGM.Wave.AcceptanceTests.Helpers {
    public class ExampleUser {
        public static ExampleUser Admin {
            get {
                return new ExampleUser {
                    Username = "admin",
                    Password = "password"
                };
            }
        }

        public static ExampleUser MarkusMachado {
            get {
                return new ExampleUser {
                    Username = "MarkusMachado",
                    Password = "password",
                    EmailAddress = "Markus.Machado@supergroup.com",
                };
            }
        }

        public static ExampleUser VasundharaAraya {
            get { 
                return new ExampleUser {
                    Username = "VasundharaAraya",
                    Password = "password",
                    EmailAddress = "Vasundhara.Araya@DailyRockstars.com"
                }; 
            }
        }

        public string Username { get; private set; }
        public string EmailAddress { get; private set; }
        public string Password { get; private set; }
    }
}
