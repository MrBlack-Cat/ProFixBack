using System.Text.RegularExpressions;

namespace Infrastructure.Services;

public class QuestionRouter
{
    public enum QuestionTopic
    {
        Bookings,         
        Posts,            
        Certificates,     
        Guarantees,       
        Likes,             
        Messages,          
        Profile,           
        Services,          
        Notifications,     
        General,           
        Global,
        Frontend,
        Unknown            
    }

    public QuestionTopic DetectTopic(string userMessage)
    {
        var message = userMessage.ToLower();

        if (Regex.IsMatch(message, @"бронирован|бронирование|бронирования|букинг|забронировать|бронь|бронь"))
            return QuestionTopic.Bookings;

        if (Regex.IsMatch(message, @"пост|посты|публикац|публикация|публикации|контент"))
            return QuestionTopic.Posts;

        if (Regex.IsMatch(message, @"сертификат|сертификаты|документ подтверждения|подтверждение"))
            return QuestionTopic.Certificates;

        if (Regex.IsMatch(message, @"гаранти|гарантии|гарантия|гарантированный|гарантийный талон"))
            return QuestionTopic.Guarantees;

        if (Regex.IsMatch(message, @"лайк|лайков|лайки|нравится|понравилось|оценка"))
            return QuestionTopic.Likes;

        if (Regex.IsMatch(message, @"сообщение|сообщения|чат|переписка|написать"))
            return QuestionTopic.Messages;

        if (Regex.IsMatch(message, @"профиль|аккаунт|данные пользователя|данные профиля"))
            return QuestionTopic.Profile;

        if (Regex.IsMatch(message, @"услуга|услуги|предоставляемые услуги|список услуг"))
            return QuestionTopic.Services;

        if (Regex.IsMatch(message, @"уведомлен|уведомления|оповещение|оповещения"))
            return QuestionTopic.Notifications;

        if (Regex.IsMatch(message, @"какой сегодня день|сегодняшняя дата|сейчас|сегодня|вчера|завтра|день недели"))
            return QuestionTopic.General;

        if (Regex.IsMatch(message, @"всё|все|общий|вся информация|что у меня есть|все данные"))
            return QuestionTopic.Global;

        if (Regex.IsMatch(message, @"(как\s)?(войти|выйти|открыть|создать|добавить|редактировать|настро|профил|личный\sкабинет|вкладка|форма|страница|пост|сертификат|гарант)"))
            return QuestionTopic.Frontend;

        return QuestionTopic.Unknown;
    }
}
