using System;

class Credit {
    public string UserPhoneNumber {get; init;}
    private User _userTakingCredit = null;
    public User UserTakingCredit(SQLQueryHandler queryHandler) {
        if(_userTakingCredit == null)
            return _userTakingCredit = queryHandler.GetUserByNumber(UserPhoneNumber);
        return _userTakingCredit;
    } 
    public int CreditAmount {get; init;}
    public int UserIncome {get; init;}
    public int CreditAmountFromTotalIncome => (int)(((double)CreditAmount / (double)UserIncome) * 100);
    public int CreditHistory {get; init;}
    public int DelayedCreditHistory {get; init;}
    public decimal MonthlyFee => ((decimal)CreditAmount / CreditDuration) * 1.03m; 
    public int CreditDuration {get; init;} // должно быть в месяцах
    public CreditPurpose CreditPurpose {get; init;}
    public DateTime ProcessingDate {get; init; } //дата оформления
    public DateTime CreditDeadline => ProcessingDate + new TimeSpan(days: 31 * CreditDuration, hours: 0, minutes: 0, seconds: 0); 
    //ремарка:
    //так как нельзя складывать 2 объекта типа DateTime,
    //то я решил добавить TimeSpan к DateTime (это делать можно).
    //небольшая загвоздка в том, что TimeSpan не принимает как параметр месяцы
    //из-за чего к примеру если кредит взяли в начале февраля и его длительность будет 
    //2 месяца, то там не будет 28(Февраль) + 31(Март) = 59 дней а будет 62 дня 
    //т.к. я считаю количество дней простым умножением количества месяцов на 31
    
    public int CalculateScore() {
        int score = 0;

        int CAFTI = CreditAmountFromTotalIncome;
        
        if(CAFTI < 80)
            score += 4;
        else if (CAFTI < 150)
            score += 3;
        else if (CAFTI < 250)
            score += 2;
        else
            score += 1;

        int CH = CreditHistory;

        if(CH > 3) 
            score += 2;
        else if (CH > 1) 
            score += 1;
        else
            score += -1;
        

        int DCH = DelayedCreditHistory;

        if(DCH > 7)
            score += -3;
        else if(DCH > 4)
            score += -2;
        else if(DCH == 4)
            score += -1;
        
        score += (int) CreditPurpose;
        score += CreditHistory > 12 ? 1 : 1; //мог написать += 1, но решил что так понятнее будет :D 
        return score;
    }

    public override string ToString() {
        User user = _userTakingCredit;
        string creditPurposeStr = CreditPurpose switch {
            CreditPurpose.Appliances    => "бытовая техника",
            CreditPurpose.Renovation    => "ремонт",
            CreditPurpose.Phone         => "телефон",
            CreditPurpose.Other         => "другое",
            _ => throw new Exception("неправильная причина кредита")
        };;
        string result = 
            //$"Пользователь берущий кредит: {user.PassportData.MiddleName} {user.PassportData.FirstName} {user.PassportData.LastName} \n" +
            $"Размер кредита: {CreditAmount} сомони\n" +
            $"Процент кредита: 3%\n" +
            $"Цель кредита: {creditPurposeStr}\n" +
            $"Дата взятия кредита: {ProcessingDate}\n" +
            $"Срок кредита: {CreditDuration} месяцев\n" +
            $"Ежемесячная оплата: {Math.Round(MonthlyFee, 2)} сомони";
        return result;
            
    }
}

public enum CreditPurpose {
    Other       = -1,
    Phone       = 0,
    Renovation  = 1, //ремонт
    Appliances  = 2, //бытовая техника
}