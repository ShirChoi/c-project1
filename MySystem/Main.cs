using System;

namespace MySystem {
    class Program {
        static void Main(string[] args) {

            /*Console.WriteLine(
                "Добро пожаловать в Sarvar Bank\n" +
                "1. регистрация\n" +
                "2. авторизация"
            );*/

            User test = Register();
            System.Console.WriteLine(test);  
        }

        
        public static User Register() {
            bool registered = false;

            string userPhoneNumber = "ABOBA";
            PassportData userPassportData = new PassportData();
            UserType userType = UserType.Client;
            Gender userGender = Gender.Male;
            MaritalStatus userMartialStatus = MaritalStatus.Unmarried;
            Citizenship userCitizenship = Citizenship.Foreigner;

            while(!registered) {
                
                bool takenNumber = true; 
                
                while(takenNumber) {
                    Console.Write("Введите номер телефона: ");
                    userPhoneNumber  = Console.ReadLine();
                
                    takenNumber = false; //TODO: добвить проверку занят ли номера телефона
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
                
                    System.Console.Write("день: ");
                    int day = int.Parse(Console.ReadLine());
                    System.Console.Write("месяц: ");
                    int month = int.Parse(Console.ReadLine());
                    System.Console.Write("год: ");
                    int year = int.Parse(Console.ReadLine());
                    
                    try {
                        userPassportData.BirthDate = new DateTime(
                            day:    day,
                            month:  month,
                            year:   year
                        );
                        
                        validDate = true;
                    } catch (System.Exception) {
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
                    System.Console.WriteLine(
                        "Введите своё семейное положение:\n" +
                        "1. Не женат / Не замужем\n" +
                        "2. Женат / Замужем\n" +
                        "3. Разведён / Разведена\n" +
                        "4. Вдовец / Вдова"  
                    );
                   
                    int choice = 0;
                    
                    try {
                        choice = int.Parse(Console.ReadLine());
                    } catch (System.Exception) {
                        System.Console.WriteLine("выбор должен числом быть в промежутке 1-4");
                        continue;
                    }

                    if(!(choice >= 1 && choice <= 4)){
                        System.Console.WriteLine("выбор должен числом быть в промежутке 1-4");
                        continue;
                    }

                    validMStatus = true;
                    userMartialStatus = (MaritalStatus)choice;
                } 

                bool validCitizenship = false;

                while(!validCitizenship) {
                    System.Console.WriteLine(
                        "Введите гражданство:\n" +
                        "1. Таджикистанец\n" +
                        "2. Иностранец"
                    );
                    
                    int choice = 0;

                    try {
                        choice = int.Parse(Console.ReadLine());
                    } catch (System.Exception) {
                        System.Console.WriteLine("выбор должен числом быть в промежутке 1-2");
                        continue;
                    }

                    if(!(choice >= 1 && choice <= 2)){
                        System.Console.WriteLine("выбор должен числом быть в промежутке 1-2");
                        continue;
                    }

                    validCitizenship = true;

                    userCitizenship = choice == 1 ? Citizenship.Tajik : Citizenship.Foreigner;
                }
                registered = true;
            }
            //TODO: добавление в базу данных через SQL
            return new User() {
                PhoneNumber = userPhoneNumber,
                Type = userType,
                Gender = userGender,
                MaritalStatus = userMartialStatus,
                Citizenship = userCitizenship,
                PassportData = userPassportData
            };
        }
/*
        public User LogIn() {
        }*/
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

        
        

        System.Console.WriteLine(test);
        //System.Console.WriteLine(test.CalculateScore());
*/
