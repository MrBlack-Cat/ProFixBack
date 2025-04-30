using System.Text.RegularExpressions;

namespace Infrastructure.Services;

public class QuestionIntentAnalyzer
{
    public enum QuestionIntent
    {
        Count,             
        FindNext,          
        FindPrevious,      
        FindToday,        
        FindTomorrow,      
        FindTop,           
        FindStatus,        
        FindByDate,        
        GeneralInfo,       
        Global,
        Unknown
    }

    public QuestionIntent DetectIntent(string userMessage)
    {
        var message = userMessage.ToLower();

        if (Regex.IsMatch(message, @"сколько|количество|общее количество|всего"))
            return QuestionIntent.Count;

        if (Regex.IsMatch(message, @"следующ(ий|ая)|ближайший|ближайшая|когда следующее|следующий бронирование|следующий пост"))
            return QuestionIntent.FindNext;

        if (Regex.IsMatch(message, @"предыдущ(ий|ая)|прошлый|прошлая|что было вчера|вчерашний"))
            return QuestionIntent.FindPrevious;

        if (Regex.IsMatch(message, @"сегодня|на сегодня|сегодняшний"))
            return QuestionIntent.FindToday;

        if (Regex.IsMatch(message, @"завтра|на завтра|завтрашний"))
            return QuestionIntent.FindTomorrow;

        if (Regex.IsMatch(message, @"топ|лучший|самый популярный|самое популярное|лидер|рейтинговый|наибольший"))
            return QuestionIntent.FindTop;

        if (Regex.IsMatch(message, @"статус|состояние|статусы|подтвержден|завершен|ожидает"))
            return QuestionIntent.FindStatus;

        if (Regex.IsMatch(message, @"\d{2}\.\d{2}\.\d{4}|на \d{1,2} [а-я]+"))
            return QuestionIntent.FindByDate;

        if (Regex.IsMatch(message, @"мой профиль|мои данные|кто я|какие у меня данные|мой аккаунт"))
            return QuestionIntent.GeneralInfo;

        if (Regex.IsMatch(message, @"всё про меня|все данные|покажи всё|все мои|все мои данные"))
            return QuestionIntent.Global;

        return QuestionIntent.Unknown;
    }
}
