using System;

static class UserQueryHandler {
    private const string adminCapability = 
        "1. Смена пользователя\n" +
        "2. Личный кабинет\n" +
        "3. Просмотр личного кабинета другого пользователя";
    private const string clientCapability = 
        "1. Смена пользователя\n" +
        "2. Личный кабинет\n" +
        "3. Оформить кредит";
    public static User Logging(SQLQueryHandler queryHandler) {
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

        return choice == 1 ? UserQueryHandler.Register(queryHandler) : UserQueryHandler.LogIn(queryHandler);
    }
    private static User Register(SQLQueryHandler queryHandler) {
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
    
    private static User LogIn(SQLQueryHandler queryHandler) {

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
    public static Credit TakeCredit(User user, out int score) {
        int userIncome = -1;
        while(userIncome <= 0){
            Console.Write("Введите свою заработную плату:");
            bool validInput = int.TryParse(Console.ReadLine(), out userIncome);
            
            if(!validInput || userIncome <= 0) {
                Console.WriteLine("Заработная плата должна быть целым положительным числом");
            }
        }

        int creditAmount = -1;
        while(creditAmount <= 0){
            Console.Write("Введите сумму кредита:");
            bool validInput = int.TryParse(Console.ReadLine(), out creditAmount);
            
            if(!validInput || creditAmount <= 0) {
                Console.WriteLine("Сумма кредита должна быть целым положительным числом");
            }
        }

        int creditHistory = -1;
        while(creditHistory < 0){
            Console.Write("Введите количество погашенных кредитов:");
            bool validInput = int.TryParse(Console.ReadLine(), out creditHistory);
            
            if(!validInput || creditHistory < 0) {
                Console.WriteLine("количество погашенных кредитов должна быть целым неотрицательным числом");
            }
        }

        int delayedCreditHistory = -1;
        while(delayedCreditHistory < 0) {
            Console.Write("Введите количество просроков в кредитной истории:");
            bool validInput = int.TryParse(Console.ReadLine(), out delayedCreditHistory);
            
            if(!validInput || delayedCreditHistory < 0) {
                Console.WriteLine("число просроков должно быть целым неотрицательным числом");
            }
        }

        int creditPurposeInt = -1;
        while(creditPurposeInt < 1 || creditPurposeInt > 4) {
            Console.WriteLine(
                "Введите цель кредита:\n" +
                "1. Бытовая техника\n" +
                "2. Ремонт\n" +
                "3. Телефон\n" +
                "4. другое"
            );

            bool validInput = int.TryParse(Console.ReadLine(), out creditPurposeInt);
            
            if(!validInput || creditPurposeInt < 1 || creditPurposeInt > 4) {
                Console.WriteLine("выбор должен быть числом в промежутке 1-4");
            }
        }
        
        CreditPurpose creditPurpose = creditPurposeInt switch {
            1 => CreditPurpose.Appliances,
            2 => CreditPurpose.Renovation,
            3 => CreditPurpose.Phone,
            4 => CreditPurpose.Other,
            _ => throw new Exception("неверный выбор для причиный кредита")
        };
        
        int creditDuration = -1;

        while(creditDuration <= 0) {
            Console.Write("введите срок кредита:");
            bool validInput = int.TryParse(Console.ReadLine(), out creditDuration);
            
            if(!validInput || creditDuration < 0) {
                Console.WriteLine("срок кредита должен быть целым положительным");
            }
        }

        Credit credit = new Credit() {
            UserPhoneNumber         = user.PhoneNumber,
            CreditAmount            = creditAmount,
            UserIncome              = userIncome,
            CreditHistory           = creditHistory,
            DelayedCreditHistory    = delayedCreditHistory,
            CreditDuration          = creditDuration,
            CreditPurpose           = creditPurpose,
            ProcessingDate          = DateTime.Now,
        };
        score = credit.CalculateScore() + user.CalculateScore();
        return score > 11 ? credit : null;
    }
    public static string UserCapabilities(User user) => user.Type == UserType.Admin ? adminCapability : clientCapability;
    
    //для админов
    public static void CheckUserPersonalCabinet(SQLQueryHandler queryHandler) {
        User observedUser = UserQueryHandler.LogIn(queryHandler);
        System.Console.WriteLine(UserQueryHandler.PersonalCabinet(observedUser, queryHandler));
    }
    public static string PersonalCabinet(User user, SQLQueryHandler queryHandler) {
        string userTypeStr = user.Type == UserType.Client ? "Клиент" : "Администратор";
        string result = user.ToString() + '\n' +
                        $"Тип пользователя: {userTypeStr}";
                        
        if(user.Type == UserType.Admin)
            return result;

        var Credits = queryHandler.GetUserCreditHistory(user);

        result += "\n\nКредитная история:";

        if(Credits.Count == 0){
            result += "\n\nНет кредитной историй";

            return result;
        }

        foreach(Credit credit in Credits) {
            result += "\n\n" + credit.ToString();
        }

        return result;
    }
}