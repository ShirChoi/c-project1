using System;

public enum UserType {
    Client  = 0,
    Admin   = 1
}

public class PassportData {
    public string FirstName {get; set;}
    public string MiddleName {get; set;}
    public string LastName {get; set;}
    public DateTime BirthDate {get; set;}

    public PassportData() {}
}

public class User {
    public string PhoneNumber {get; private set;}
    public UserType Type {get; private set;}
    public PassportData PassportData {get; private set;}

    public User() {}

    public User(string phoneNumber, UserType type, PassportData passportData) {
        PhoneNumber = phoneNumber;
        Type = type;
        PassportData = passportData;
    }

    public override string ToString() {
        string result =
            $"Имя: {PassportData.FirstName}, " +
            $"Фамилия: {PassportData.LastName}, " +
            $"Очество: {PassportData.MiddleName}, " +
            $"Дата рождения: {PassportData.BirthDate}, " +
            $"Номер телефона: {PhoneNumber}";
        return result;
    }
}