using System;

static class UserQueryHandler {
    private const string adminCapability = 
        "1. Смена пользователя\n" +
        "2. Личный кабинет\n" +
        "3. Просмотр личного кабинета другого пользователя";
    private const string clientCapability = 
        "1. Смена пользователя\n" +
        "2. Личный кабинет\n" +
        "3. Просмотр личного кабинета другого пользователя";
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

        bool existingNumber = false;
        User result = null;
        while(!existingNumber){
            Console.Write("Введите номер телефона пользователя: ");
            string userPhoneNumber = Console.ReadLine();
            
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

    public static void LogOut() {}
    public static string UserCapabilities(User user) => user.Type == UserType.Client ? adminCapability : clientCapability;

    public static void PersonalCabinet(User user) {
        System.Console.WriteLine(user);
        string userTypeStr = user.Type == UserType.Client ? "Клиент" : "Администратор";
        System.Console.WriteLine($"Тип пользователя: {userTypeStr}");        
}
}