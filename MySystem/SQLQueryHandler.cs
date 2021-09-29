using System.Data.SqlClient;
using System.Collections.Generic;
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

    public bool AddCredit(in Credit credit) {
        string query = 
            "insert into Credits(Amount, Duration, Purpose, UserPhoneNumber, UserIncome, UserCreditHistory, UserDelayedCreditHistory, ProcessingDate)" +
            "values (@Amount, @Duration, @Purpose, @UserPhoneNumber, @UserIncome, @UserCreditHistory, @UserDelayedCreditHistory, @ProcessingDate)";
        SqlCommand command = sqlConnection.CreateCommand();
        command.CommandText = query;
        command.Parameters.AddWithValue("@Amount", credit.CreditAmount);
        command.Parameters.AddWithValue("@Duration", credit.CreditDuration);
        
        string creditPurposeStr = credit.CreditPurpose switch {
            CreditPurpose.Appliances    => "A",
            CreditPurpose.Renovation    => "R",
            CreditPurpose.Phone         => "P",
            CreditPurpose.Other         => "O",
            _ => throw new Exception("неправильная причина кредита")
        };

        command.Parameters.AddWithValue("@Purpose",                 creditPurposeStr);
        command.Parameters.AddWithValue("@UserPhoneNumber",         credit.UserPhoneNumber);
        command.Parameters.AddWithValue("@UserIncome",              credit.UserIncome);
        command.Parameters.AddWithValue("@UserCreditHistory",       credit.CreditHistory);
        command.Parameters.AddWithValue("@UserDelayedCreditHistory",credit.DelayedCreditHistory);
        command.Parameters.AddWithValue("@ProcessingDate",          credit.ProcessingDate);
        
        int result = 0;

        try {
            result = command.ExecuteNonQuery();
        } catch (System.Exception e) {
            System.Console.WriteLine(e.Message);
            throw;
        }

        return result != 0;
    }

    public List<Credit> GetUserCreditHistory(in User user) {
        List<Credit> userCredits = new List<Credit>();

        string query = $"select * from Credits where UserPhoneNumber = \'{user.PhoneNumber}\'";

        SqlCommand command = sqlConnection.CreateCommand();
        command.CommandText = query;

        SqlDataReader reader = command.ExecuteReader();

        while(reader.Read()) {
            CreditPurpose creditPurpose = reader["Purpose"].ToString() switch {
                "A" => CreditPurpose.Appliances ,
                "R" => CreditPurpose.Renovation ,
                "P" => CreditPurpose.Phone      ,
                "O" => CreditPurpose.Other      ,
                _ => throw new Exception("неправильная причина кредита")
            };
            Credit creditToAdd = new Credit() {
                UserPhoneNumber = reader["UserPhoneNumber"].ToString(),
                CreditAmount = int.Parse(reader["Amount"].ToString()),
                UserIncome = int.Parse(reader["UserIncome"].ToString()),
                CreditHistory = int.Parse(reader["UserCreditHistory"].ToString()),
                DelayedCreditHistory = int.Parse(reader["UserDelayedCreditHistory"].ToString()),
                CreditDuration = int.Parse(reader["Duration"].ToString()),
                CreditPurpose = creditPurpose,
                ProcessingDate = DateTime.Parse(reader["ProcessingDate"].ToString())
            };
            userCredits.Add(creditToAdd);
            //System.Console.WriteLine(creditToAdd);
        }

        return userCredits;
    }
}
/*Введите номер телефона: +992933608303
Введите Имя: Давлатбегим. 
Введите Фамилию: Шовалиева.
Введите Отчество: Джамшедовна.
Введите дату рождения: 2005.09.10
Введите свои пол(М / Ж): ж.
Введите своё семейное положение: Не замужем.

Введите гражданство:
Таджикистанец.  
*/