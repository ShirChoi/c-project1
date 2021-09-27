using System;

public class User {
    public string PhoneNumber {get; init;}
    public UserType Type {get; init;}
    public Gender Gender {get; init;}
    public PassportData PassportData {get; init;}
    public MaritalStatus MaritalStatus {get; init;}
    public Citizenship Citizenship {get; init;}

    public User() {}

    public int CalculateScore() {
        int score = 0;

        score += MaritalStatus switch {
            MaritalStatus.Unmarried => 1,
            MaritalStatus.Married   => 2,
            MaritalStatus.Divorsed  => 1,
            MaritalStatus.Widow     => 2,
            _ => throw new Exception("Undefined martial status")
        };
        
        score += (int)Citizenship;
        score += (int)Gender;

        int age = PassportData.Age;

        if(age < 26) {
            score += 0;
        } else if (age < 36) {
            score += 1;
        } else if (age < 63) {
            score += 2;
        } else {
            score += 1;
        }

        return score;
    }

    public override string ToString() {
        string martialStatusString = MaritalStatus switch {
            MaritalStatus.Unmarried => (Gender == Gender.Male ? "Не женат" : "Не замужем"),
            MaritalStatus.Married   => (Gender == Gender.Male ? "Женат" : "Замужем"),
            MaritalStatus.Divorsed  => "в Разводе",
            MaritalStatus.Widow     => (Gender == Gender.Male ? "Вдовец" : "Вдова"),
            _ => throw new Exception("Undefined martial status")
        };
        string result =
            $"Имя: {PassportData.FirstName}, \n" +
            $"Фамилия: {PassportData.LastName}, \n" +
            $"Отчество: {PassportData.MiddleName}, \n" +
            $"Дата рождения: {PassportData.BirthDate}, \n" +
            $"Пол: {(Gender == Gender.Male ? "Мужчина" : "Женщина")}, \n" +
            $"Семейное положение: {martialStatusString}, \n" +
            $"Гражданство: {(Citizenship == Citizenship.Tajik ? "Таджик" : "Иностранец")}, \n" +
            $"Номер телефона: {PhoneNumber}";
        return result;
    }
}

public class PassportData {
    public string FirstName {get; set;}
    public string MiddleName {get; set;}
    public string LastName {get; set;}
    public DateTime BirthDate {get; set;}
    public int Age => (int)((DateTime.Now - BirthDate).TotalDays / 365);

    public PassportData() {}
}

public enum UserType {
    Client  = 0,
    Admin   = 1
}

public enum Gender {
    Male    = 1,
    Female  = 2,
}

public enum MaritalStatus { //семейное положение
    Unmarried   = 1,
    Married     = 2,
    Divorsed    = 3,
    Widow       = 4,
}

public enum Citizenship {
    Foreigner   = 0,
    Tajik       = 1,
}
