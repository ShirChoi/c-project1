using System;

namespace MySystem {
    class Program {
        static void Main(string[] args) {
            string connectionString = "Server=DESKTOP-CT0BSVJ\\DEV;" +
                                      "Database=Sarvar Bank;" +
                                      "Trusted_Connection=True;";
            
            SQLQueryHandler queryHandler = new SQLQueryHandler(connectionString);
            Console.WriteLine("Добро пожаловать в Sarvar Bank");
            bool appShouldWork = true;
            
            while(appShouldWork) {
                User user = UserQueryHandler.Logging(queryHandler);

                bool userLoop = true;
                while(userLoop) {
                    int choice = 0;
                    while(!(choice >= 1 && choice <= 3)) {
                        Console.WriteLine(UserQueryHandler.UserCapabilities(user));
                        bool validInput = int.TryParse(Console.ReadLine(), out choice);
                        if(!validInput || !(choice >= 1 && choice <= 3))
                            Console.WriteLine("выбор должен быть числом в промежутке 1-3");
                    }
                    
                    switch(choice) {
                        case 1: {
                            userLoop = false;
                            continue;
                        }
                        case 2: {
                            Console.WriteLine(UserQueryHandler.PersonalCabinet(user));
                        } break;
                        case 3: {
                            if(user.Type == UserType.Admin)
                                UserQueryHandler.CheckUserPersonalCabinet(queryHandler);
                            else 
                                System.Console.WriteLine("fig tebe");
                        } break;
                        default: {
                            throw new Exception("неправильный выбор");
                        }
                    }
                }
                //appShouldWork = false;
            }
        }
    }
}

/*

        User test = new User(){
            PhoneNumber     = "935056800",
            Type            = UserType.Client,
            Gender          = Gender.Male,
            MaritalStatus   = MaritalStatus.Unmarried,
            Citizenship     = Citizenship.Tajik,
            
            PassportData    = new PassportData() {
                FirstName   = "Сарвар",
                LastName    = "Абдуллоев",
                MiddleName  = "Мамадамонович",
                BirthDate   = new DateTime(year: 2005, month: 06, day: 22)
            }
        };

        
        

        Console.WriteLine(test);
        //Console.WriteLine(test.CalculateScore());
*/
