using System.Data.SqlClient;
using System;

class SQLQueryHandler {
    private string connectionString;
    private SqlConnection sqlConnection;

    public SQLQueryHandler(in string connectionString) {
        this.connectionString = connectionString;
        sqlConnection = new SqlConnection(connectionString);
    
        try {
            sqlConnection.Open();
        } catch (System.Exception e) {
            throw new System.Exception($"Не удалось открыть соединение.\n ошибка:{e.Message}");
        }
        
        if(sqlConnection.State == System.Data.ConnectionState.Closed)
            throw new System.Exception("не получилось открыть соединение");
    }

    ~SQLQueryHandler() {
        sqlConnection.Close();
    }

    public User GetUserByNumber(in string phoneNumber) {
        //SQL stuff
        User resultUser = null;
        string query = $"select * from Users where PhoneNumber = \'{phoneNumber}\'";
        SqlCommand command = sqlConnection.CreateCommand();
        command.CommandText = query;
        
        SqlDataReader reader = command.ExecuteReader();

        while(reader.Read()) {
            MaritalStatus userMartialStatus = reader["MaritalStatus"].ToString() switch {
                "U" => MaritalStatus.Unmarried,
                "M" => MaritalStatus.Married,
                "D" => MaritalStatus.Divorsed,
                "W" => MaritalStatus.Widow,
                _ => throw new System.Exception("Undefined martial status character")
            };

            resultUser = new User(){
                PhoneNumber     = reader["PhoneNumber"].ToString(),
                Type            = reader["UserType"].ToString() == "A" ? UserType.Admin : UserType.Client,
                Gender          = reader["Gender"].ToString() == "M" ? Gender.Male : Gender.Female,
                MaritalStatus   = userMartialStatus,
                Citizenship     = reader["Citizenship"].ToString() == "T" ? Citizenship.Tajik : Citizenship.Foreigner,
                
                PassportData    = new PassportData() {
                    FirstName   = reader["FirstName"].ToString(),
                    LastName    = reader["LastName"].ToString(),
                    MiddleName  = reader["MiddleName"].ToString(),
                    BirthDate   = DateTime.Parse(reader["BirthDate"].ToString())
                }
            };
        }
        reader.Close();


        return resultUser;
    }
    

    public bool AddUser(in User user) {
        //SQL stuff
        string query = 
            "insert into Users(PhoneNumber, UserType, FirstName, MiddleName, LastName, BirthDate, Gender, MaritalStatus, CitizenShip) " +
            "values (@phoneNumber, 'C', @firstName, @middleName, @lastName, @birthDate, @gender, @maritalStatus, @citizenShip)";
        
        SqlCommand command = sqlConnection.CreateCommand();
        command.CommandText = query;
        command.Parameters.AddWithValue("@phoneNumber", user.PhoneNumber);
        command.Parameters.AddWithValue("@firstName",   user.PassportData.FirstName);
        command.Parameters.AddWithValue("@middleName",  user.PassportData.MiddleName);
        command.Parameters.AddWithValue("@lastName",    user.PassportData.LastName);
        command.Parameters.AddWithValue("@birthDate",   user.PassportData.BirthDate);
        command.Parameters.AddWithValue("@gender",      user.Gender == Gender.Male ? "M" : "F");
    
        string maritalStatusString = user.MaritalStatus switch {
            MaritalStatus.Unmarried => "U",
            MaritalStatus.Married   => "M",
            MaritalStatus.Divorsed  => "D",
            MaritalStatus.Widow     => "W",
            _ => throw new System.Exception("Undefined martial status")
        };

        command.Parameters.AddWithValue("@maritalStatus", maritalStatusString);
        command.Parameters.AddWithValue("@citizenShip", user.Citizenship == Citizenship.Tajik ? "T" : "F");
        int result = 0;
        try {
            result = command.ExecuteNonQuery();
        } catch (System.Exception e) {
            System.Console.WriteLine(e.Message);
            //throw e;
        }
        
        return result != 0;
    } 
}
/*
Введите номер телефона:+992907701324
Введите Имя: Мехрона
Введите Фамилию: Карамшоева
Введите Отчество: Савлатшоевна
Введите дату рождения:06.04.2005
Введите свои пол(М / Ж):Ж
Введите своё семейное положение:Не замужем
1. Не женат / Не замужем
2. Женат / Замужем
3. Разведён / Разведена
4. Вдовец / Вдова

Введите гражданство:Таджикистанец
1. Таджикистанец
2. Иностранец
*/