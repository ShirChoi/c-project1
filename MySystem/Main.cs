using System;

namespace MySystem {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine(
                "Добро пожаловать в Sarvar Bank\n" +
                "1. регистрация" +
                "2. авторизация"
            );
        }

        
        public void Register() {
            /*bool registered = false;

            while(!registered) {
                Console.Write("Введите номер телефона:");
                string phoneNumber = Console.ReadLine();

                bool takenNumber = false; //TODO: добвить проверку занят ли номера телефона

                if(takenNumber){
                    Console.WriteLine("Пользователь с таким номером уже существует");
                
                    continue;
                }


            }*/
        }

        public void LogIn() {
        }
    }
}

/*
User test = new User(
    phoneNumber: "935056800", 
    type: UserType.Client, 
    passportData: new PassportData(){
        FirstName   = "Сарвар",
        LastName    = "Абдуллоев",
        MiddleName  = "Мамадамонович",
        BirthDate = new DateTime(year: 2005, month: 06, day: 22)
    }
);

System.Console.WriteLine(test);
*/
