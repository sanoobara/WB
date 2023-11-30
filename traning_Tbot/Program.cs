using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics.Metrics;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;


/*var builder = new ConfigurationBuilder(). 
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{Environments.Development}.json", optional: true, reloadOnChange: true);
*/
var botClient = new TelegramBotClient("6368222490:AAFoJCSEjozm8oZDa0Iu6C-zVSH2WR3szfE");

using CancellationTokenSource cts = new();

// StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
ReceiverOptions receiverOptions = new()
{
    AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
};

botClient.StartReceiving(
    updateHandler: HandleUpdateAsync2,
    pollingErrorHandler: HandlePollingErrorAsync,
    receiverOptions: receiverOptions,
    cancellationToken: cts.Token
);

var me = await botClient.GetMeAsync();

Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();

// Send cancellation request to stop bot
cts.Cancel();


async Task HandleUpdateAsync2(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    // Only process Message updates: https://core.telegram.org/bots/api#message
    if (update.Message is not { } message)
        return;
    // Only process text messages
    if (message.Text is not { } messageText)
        return;

    //if (message.Sticker is not { } sticker)
    //    return;

    var chatId = message.Chat.Id;

    if (message.Text == "/start")
    {
        // Тут все аналогично Inline клавиатуре, только меняются классы
        // НО! Тут потребуется дополнительно указать один параметр, чтобы
        // клавиатура выглядела нормально, а не как абы что

        var replyKeyboard = new ReplyKeyboardMarkup(
            new List<KeyboardButton[]>()
            {
                                        new KeyboardButton[]
                                        {
                                            new KeyboardButton("Семен"),
                                            new KeyboardButton("Артем"),
                                        },
                                        new KeyboardButton[]
                                        {
                                            new KeyboardButton("Стас D-класс")
                                        },
                                        new KeyboardButton[]
                                        {
                                            new KeyboardButton("Саня"),
                                            new KeyboardButton("СТАТА")
                                        }
            })
        {
            // автоматическое изменение размера клавиатуры, если не стоит true,
            // тогда клавиатура растягивается чуть ли не до луны,
            // проверить можете сами
            ResizeKeyboard = true,
        };
        string date = DateTime.Now.AddDays(1.0).ToString("D");
        await botClient.SendTextMessageAsync(
           message.Chat.Id,
           $"Кто повезет парней завтра ({date})?",
           replyMarkup: replyKeyboard); 

        return;
    }


    if (message.Text == "Семен" || message.Text == "Стас D-класс" || message.Text == "Саня" || message.Text == "Артем")
    {
        string name = message.Text;
        string date = DateTime.Now.AddDays(1.0).ToString("yyyy.MM.dd");



        using (var connection = new SqliteConnection("Data Source=DB.db"))
        {

            string sqlExpression = $"SELECT COUNT(*) FROM statistic WHERE date = \"{date}\";";

            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
            connection.Open();

            int count = 0;
            SqliteCommand command = new SqliteCommand(sqlExpression, connection);
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows) // если есть данные
                {
                    while (reader.Read())   // построчно считываем данные
                    {
                        count = int.Parse(reader.GetValue(0).ToString());

                        Console.WriteLine($"{count}");
                    }
                }
            }

            if (count == 1)
            {

                Message sendErrorMessage = await botClient.SendTextMessageAsync(
        chatId: chatId,
        text: $"Кто-то забронил, но мы его ебанули ради тебя дорогой",

        disableNotification: true,


        cancellationToken: cancellationToken);

                command = new SqliteCommand();
                command.Connection = connection;
                command.CommandText = $"DELETE FROM statistic WHERE date = \"{date}\"";

                //await Console.Out.WriteLineAsync(str);
                int number2 = command.ExecuteNonQuery();





            }


            command = new SqliteCommand();
            command.Connection = connection;
            command.CommandText = $"INSERT INTO statistic (name, date) VALUES (\"{name}\", \"{date}\")";
            var str = $"INSERT INTO Statistic (name, date) VALUES (\"{name}\", \"{date}\")";
            //await Console.Out.WriteLineAsync(str);
            int number = command.ExecuteNonQuery();

            date = DateTime.Now.AddDays(1.0).ToString("dd MMMM");
            // await Console.Out.WriteLineAsync(date);
            Message sendMessage = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: $"{date} повезет нормальных пацанов *{name}*",
            /*parseMode: ParseMode.MarkdownV2,*/
            disableNotification: true,
            replyToMessageId: update.Message.MessageId,

            cancellationToken: cancellationToken);




            return;
        }
    }
    if (message.Text == "СТАТА")
    {
        string mess = $"Cтатистика по трушным пацанам с 27.11:\n";


        string sqlExpression = "SELECT name, COUNT(*) as counter FROM statistic GROUP BY name";
        using (var connection = new SqliteConnection("Data Source=DB.db"))
        {
            connection.Open();

            SqliteCommand command = new SqliteCommand(sqlExpression, connection);
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows) // если есть данные
                {
                    Dictionary<string, string> valuePairs = new();
                    while (reader.Read())   // построчно считываем данные
                    {
                        valuePairs.Add(reader.GetValue(0).ToString(), reader.GetValue(1).ToString());
                        

                    }
                    foreach (var item in valuePairs)
                    {
                        mess += item.Key + "=" + item.Value+"\n";
                        
                    }
                }
            }
        }

        


        await botClient.SendTextMessageAsync(
           message.Chat.Id,
           mess
           );


    }
}

  









Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException
            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        _ => exception.ToString()
    };

    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}



