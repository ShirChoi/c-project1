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
            
            User user = choice == 1 ? Register(queryHandler) : LogIn(queryHandler);
            Console.WriteLine(user);  
        }

        
        public static User Register(SQLQueryHandler queryHandler) {
            bool registered = false;

            string userPhoneNumber = "ABOBA";
            PassportData userPassportData = new PassportData();
            UserType userType = UserType.Client;
            Gender userGender = Gender.Male;
            MaritalStatus userMartialStatus = MaritalStatus.Unmarried;
            Citizenship userCitizenship = Citizenship.Foreigner;
            User resultUser = null;

            while(!registered) {
                
                bool takenNumber = true; 
                
                while(takenNumber) {
                    Console.Write("Введите номер телефона: ");
                    userPhoneNumber = Console.ReadLine();
                
                    takenNumber = queryHandler.GetUserByNumber(userPhoneNumber) != null;

                    if(takenNumber){
                        Console.WriteLine("Пользователь с таким номером уже существует");
                    
                        continue;
                    }
                }

                Console.Write("Введите Имя: ");
                userPassportData.FirstName = Console.ReadLine();
                Console.Write("Введите Фамилию: ");
                userPassportData.LastName = Console.ReadLine();
                Console.Write("Введите Отчество: ");
                userPassportData.MiddleName = Console.ReadLine();
                
                bool validDate = false;
                while(!validDate){
                    Console.Write("Введите дату рождения\n");
                
                    Console.Write("день: ");
                    int day = int.Parse(Console.ReadLine());
                    Console.Write("месяц: ");
                    int month = int.Parse(Console.ReadLine());
                    Console.Write("год: ");
                    int year = int.Parse(Console.ReadLine());
                    
                    try {
                        userPassportData.BirthDate = new DateTime(
                            day:    day,
                            month:  month,
                            year:   year
                        );
                        
                        validDate = true;
                    } catch (Exception) {
                        Console.WriteLine("Неправильная дата");
                    }
                }
            
                
                bool validGender = false;
                while(!validGender) {
                    Console.WriteLine(
                        "Введите свои пол(М / Ж)\n" +
                        "М - Мужской\n" +
                        "Ж - Женский"
                    );

                    string genderString = Console.ReadLine();

                    if(genderString == "М") {
                        userGender = Gender.Male;
                        validGender = true;
                    } else if(genderString == "Ж") {
                        userGender = Gender.Female;
                        validGender = true;
                    } else {
                        Console.WriteLine("Неправильный формат ввода. Введите либо М, либо Ж");
                    }
                }

                bool validMStatus = false;
                while(!validMStatus) {
                    Console.WriteLine(
                        "Введите своё семейное положение:\n" +
                        "1. Не женат / Не замужем\n" +
                        "2. Женат / Замужем\n" +
                        "3. Разведён / Разведена\n" +
                        "4. Вдовец / Вдова"  
                    );
                   
                    int choice = 0;
                    
                    try {
                        choice = int.Parse(Console.ReadLine());
                    } catch (Exception) {
                        Console.WriteLine("выбор должен числом быть в промежутке 1-4");
                        continue;
                    }

                    if(!(choice >= 1 && choice <= 4)){
                        Console.WriteLine("выбор должен числом быть в промежутке 1-4");
                        continue;
                    }

                    validMStatus = true;
                    userMartialStatus = (MaritalStatus)choice;
                } 

                bool validCitizenship = false;
                while(!validCitizenship) {
                    Console.WriteLine(
                        "Введите гражданство:\n" +
                        "1. Таджикистанец\n" +
                        "2. Иностранец"
                    );
                    
                    int choice = 0;

                    try {
                        choice = int.Parse(Console.ReadLine());
                    } catch (Exception) {
                        Console.WriteLine("выбор должен числом быть в промежутке 1-2");
                        continue;
                    }

                    if(!(choice >= 1 && choice <= 2)){
                        Console.WriteLine("выбор должен числом быть в промежутке 1-2");
                        continue;
                    }

                    validCitizenship = true;

                    userCitizenship = choice == 1 ? Citizenship.Tajik : Citizenship.Foreigner;
                }
               
                resultUser = new User() {
                    PhoneNumber = userPhoneNumber,
                    Type = userType,
                    Gender = userGender,
                    MaritalStatus = userMartialStatus,
                    Citizenship = userCitizenship,
                    PassportData = userPassportData
                };

                registered = queryHandler.AddUser(resultUser);

                Console.WriteLine(registered ? "Пользователь успешно зарегестрирован" : "Не удалось добавить пользователя");
            }
            
            return resultUser;
        }

        public static User LogIn(SQLQueryHandler queryHandler) {
            Console.Write("Введите номер телефона пользователя: ");
            string userPhoneNumber = Console.ReadLine();

            bool existingNumber = false;
            User result = null;
            while(!existingNumber){
                result = queryHandler.GetUserByNumber(userPhoneNumber);
                existingNumber = result != null;
                

                if(!existingNumber) {
                    Console.WriteLine("Пользователя с таким номером не существует");

                    continue;
                }

                existingNumber = true;
            }

            return result;
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
