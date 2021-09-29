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
                            Console.WriteLine(UserQueryHandler.PersonalCabinet(user, queryHandler));
                        } break;
                        case 3: {
                            if(user.Type == UserType.Admin)
                                UserQueryHandler.CheckUserPersonalCabinet(queryHandler);
                            else  {
                                Credit credit = UserQueryHandler.TakeCredit(user, out int score);

                                if(credit == null){
                                    System.Console.WriteLine(
                                        "Недостаточно балллов для взятия кредита\n" +
                                        $"У вас: {score} баллов\n" +
                                        "Минимально необходимое количество баллов для взятия кредита: 11"
                                    );
                                    System.Console.WriteLine();
                                    continue;
                                }
                                
                                credit.UserTakingCredit(queryHandler);
                                bool creditTaken = queryHandler.AddCredit(credit);
                                if(!creditTaken) {
                                    System.Console.WriteLine("Не удалось оформить кредит");
                                    continue;
                                }
                                System.Console.WriteLine("Кредит успешно оформлен");
                                System.Console.WriteLine(credit);
                            }
                        } break;
                        default: {
                            throw new Exception("неправильный выбор");
                        }
                    }
                    System.Console.WriteLine();
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
