using System;

namespace MySystem {
    class Program {
        static void Main(string[] args) {
            string connectionString = "Server=DESKTOP-CT0BSVJ\\DEV;" +
                                      "Database=Sarvar Bank;" +
                                      "Trusted_Connection=True;";
            
            SQLQueryHandler queryHandler = new SQLQueryHandler(connectionString);
            System.Console.WriteLine("Добро пожаловать в Sarvar Bank");
            
            int choice = 0;

            while(!(choice >= 1 && choice <= 2)) {
                Console.WriteLine(
                    "1. регистрация\n" +
                    "2. авторизация"
                );
                bool validInput = int.TryParse(Console.ReadLine(), out choice);
                if(!validInput || !(choice >= 1 && choice <= 2))
                    Console.WriteLine("выбор должен быть числом в промежутке 1-2");
            }
            
            User user = choice == 1 ? UserQueryHandler.Register(queryHandler) : UserQueryHandler.LogIn(queryHandler);
            UserQueryHandler.PersonalCabinet(user);
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
