using Microsoft.Data.Sqlite;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using traning_Tbot.Weather;

namespace traning_Tbot
{
    internal class BotWorker
    {

        public TelegramBotClient botClient;

        private string Token; //Токен бота
        string SqliteConnectionString;
        CancellationTokenSource cts;
        DBWorker dBWorker;

        public BotWorker(string Token, string pathDB, CancellationTokenSource cts)
        {
            this.Token = Token;
            this.SqliteConnectionString = "Data Source =" + pathDB;
            this.cts = cts;
            this.dBWorker = new DBWorker(SqliteConnectionString);

            botClient = new TelegramBotClient(this.Token);

            // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = Array.Empty<UpdateType>(), // receive all update types except ChatMember related updates
                ThrowPendingUpdates = true // Херня которая говорит будет ли бот принимать сообщения пока он был офлайн
            };

            botClient.StartReceiving(
                updateHandler: HandleUpdateAsynctest,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );
            // Send cancellation request to stop bot

        }

        //async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        //{


        //    switch (update.Type)
        //    {
        //        case UpdateType.CallbackQuery:
        //            {
        //                var message = update.CallbackQuery;

        //                var user = message.From;

        //                var sw = new Stopwatch();
        //                sw.Start();
        //                // Измеряемый код


        //                Console.WriteLine("ЗАписано");
        //                sw.Stop();
        //                Console.WriteLine(sw.Elapsed);
        //                break;
        //            }
        //        case UpdateType.Message:
        //            {
        //                var message = update.Message;
        //                var user = message.From;


        //                Console.WriteLine($"{user.FirstName} ({user.Id}) написал сообщение: {message.Text}");

        //                var chatId = message.Chat.Id;


        //                //Первое что сделаем это запишем в бд полученное сообщение
        //                await using (var connection = new SqliteConnection(SqliteConnectionString))
        //                {
        //                    connection.Open();

        //                    string sqlExpression = "INSERT INTO Stat_Message (Message, Date, Id_user, Nik_name) VALUES (@message, @date, @id_user, @nik_name)";
        //                    SqliteCommand command = new SqliteCommand(sqlExpression, connection);
        //                    // создаем параметр для сообщения
        //                    SqliteParameter messageParam = new SqliteParameter("@message", message.Text);
        //                    command.Parameters.Add(messageParam);
        //                    // создаем параметр для возраста
        //                    SqliteParameter dateParam = new SqliteParameter("@date", message.Date.ToString("g"));
        //                    command.Parameters.Add(dateParam);
        //                    SqliteParameter id_userParam = new SqliteParameter("@id_user", message.From.Id);
        //                    command.Parameters.Add(id_userParam);
        //                    SqliteParameter nik_nameParam = new SqliteParameter("@nik_name", message.From.Username);
        //                    command.Parameters.Add(nik_nameParam);

        //                    string sqlExpression2 = "INSERT OR IGNORE INTO Chat_Id (Id) VALUES (@Id)";
        //                    SqliteCommand command2 = new SqliteCommand(sqlExpression2, connection);
        //                    // создаем параметр для сообщения
        //                    SqliteParameter IdParam = new SqliteParameter("@Id", message.Chat.Id);
        //                    command2.Parameters.Add(IdParam);

        //                    command.ExecuteNonQueryAsync(cancellationToken);
        //                    command2.ExecuteNonQueryAsync(cancellationToken);

        //                }



        //                if (message.Text == "/start")
        //                {
        //                    // Тут все аналогично Inline клавиатуре, только меняются классы
        //                    // НО! Тут потребуется дополнительно указать один параметр, чтобы
        //                    // клавиатура выглядела нормально, а не как абы что

        //                    var replyKeyboard = new ReplyKeyboardMarkup(
        //                        new List<KeyboardButton[]>()
        //                        {
        //                                new KeyboardButton[]
        //                                {
        //                                    new KeyboardButton("Семен"),
        //                                    new KeyboardButton("Артем"),
        //                                },
        //                                new KeyboardButton[]
        //                                {
        //                                    new KeyboardButton("Стас D-класс"),
        //                                    new KeyboardButton("Погода")

        //                                },
        //                                new KeyboardButton[]
        //                                {
        //                                    new KeyboardButton("Саня"),
        //                                    new KeyboardButton("СТАТА"),
        //                                    new KeyboardButton("test")
        //                                }
        //                        })
        //                    {
        //                        // автоматическое изменение размера клавиатуры, если не стоит true,
        //                        // тогда клавиатура растягивается чуть ли не до луны,
        //                        // проверить можете сами
        //                        ResizeKeyboard = true,
        //                    };
        //                    string date = DateTime.Now.AddDays(1.0).ToString("D");
        //                    await botClient.SendTextMessageAsync(
        //                       message.Chat.Id,
        //                       $"Кто повезет парней завтра ({date})?",
        //                       replyMarkup: replyKeyboard);

        //                    return;
        //                }


        //                if (message.Text == "Семен" || message.Text == "Стас D-класс" || message.Text == "Саня" || message.Text == "Артем")
        //                {
        //                    string name = message.Text;
        //                    string date = DateTime.Now.AddDays(1.0).ToString("yyyy.MM.dd");



        //                    using (var connection = new SqliteConnection(SqliteConnectionString))
        //                    {

        //                        string sqlExpression = $"SELECT COUNT(*) FROM statistic WHERE date = \"{date}\";";

        //                        SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
        //                        connection.Open();

        //                        int count = 0;
        //                        SqliteCommand command = new SqliteCommand(sqlExpression, connection);
        //                        using (SqliteDataReader reader = command.ExecuteReader())
        //                        {
        //                            if (reader.HasRows) // если есть данные
        //                            {
        //                                while (reader.Read())   // построчно считываем данные
        //                                {
        //                                    count = int.Parse(reader.GetValue(0).ToString());

        //                                    Console.WriteLine($"{count}");
        //                                }
        //                            }
        //                        }

        //                        if (count == 1)
        //                        {

        //                            Message sendErrorMessage = await botClient.SendTextMessageAsync(
        //                            chatId: chatId,
        //                            text: $"Кто-то забронил, но мы его ебанули ради тебя дорогой",
        //                            disableNotification: true,
        //                            cancellationToken: cancellationToken);

        //                            command = new SqliteCommand();
        //                            command.Connection = connection;
        //                            command.CommandText = $"DELETE FROM statistic WHERE date = \"{date}\"";
        //                            int number2 = command.ExecuteNonQuery();
        //                        }


        //                        command = new SqliteCommand();
        //                        command.Connection = connection;
        //                        command.CommandText = $"INSERT INTO statistic (name, date) VALUES (\"{name}\", \"{date}\")";
        //                        var str = $"INSERT INTO Statistic (name, date) VALUES (\"{name}\", \"{date}\")";
        //                        //await Console.Out.WriteLineAsync(str);
        //                        int number = command.ExecuteNonQuery();

        //                        date = DateTime.Now.AddDays(1.0).ToString("dd MMMM");

        //                        Message sendMessage = await botClient.SendTextMessageAsync(
        //                        chatId: chatId,
        //                        text: $"{date} повезет нормальных пацанов *{name}*",
        //                        /*parseMode: ParseMode.MarkdownV2,*/
        //                        disableNotification: true,
        //                        // replyToMessageId: update.Message.MessageId,

        //                        cancellationToken: cancellationToken);
        //                        return;
        //                    }
        //                }
        //                if (message.Text == "СТАТА")
        //                {
        //                    GetStat(message);
        //                }
        //                if (message.Text == "Погода")
        //                {
        //                    GetWeather(message);
        //                }
        //                if (message.Text == "test")
        //                {
        //                    //GetTestKeyboard(message);
        //                }
        //                if (message.Text == "/test2")
        //                {
        //                    GetSettings(message, update);
        //                }
        //                break;
        //            }
        //    }
        //}


        async Task HandleUpdateAsynctest(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {


            switch (update.Type)
            {
                case UpdateType.Message:
                    {

                        var message = update.Message;
                        var user = message.From;
                        await dBWorker.AddUser(user.Id, user.Username, user.FirstName);

                        if (message.Text == "/start")
                        {
                            GetInlineKeyboardButton(message);

                        }

                        break;
                    }
                case UpdateType.CallbackQuery:
                    {

                        var message = update.CallbackQuery;
                        var user = message.From;

                        await dBWorker.AddUser(user.Id, user.Username, user.FirstName);
                        switch (message.Data)
                        {
                            case "101" or "202" or "303":
                                {
                                    var date = DateTime.Now.AddDays(1).ToString("d");
                                    var s = await dBWorker.AddUserInStatMain(user.Id, int.Parse(message.Data), date);
                                    await botClient.SendTextMessageAsync(message.Message.Chat.Id, s);
                                    break;
                                }
                            
                            case "404":
                                {
                                    var s = await YaWeather.GetWeatherNow();
                                    await botClient.SendTextMessageAsync(message.Message.Chat.Id, s);
                                    break;
                                }
                            case "505":
                                {
                                    var s = await YaWeather.GetWeather();
                                    await botClient.SendTextMessageAsync(message.Message.Chat.Id, s);
                                    break;
                                }
                            case "606":
                                {
                                    var s = await dBWorker.ShowStat();
                                    await botClient.SendTextMessageAsync(message.Message.Chat.Id, s);
                                    break;
                                }
                        }



                        break;
                    }


            }
        }


        public async Task PushMessage()
        {
            DateTime desiredTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 21, 0, 0);
            while (true)
            {
                if (DateTime.Now > desiredTime)
                {
                    await GetInlineKeyboardButton();
                    desiredTime = desiredTime.AddDays(1);

                }
                else
                {
                    Thread.Sleep(60000);

                }

            }


        }













        async void GetSettings(Message message, Update? update)
        {
            var mess = "update.Id: " + update.Id + "\n";
            mess += "update.ChatMember: " + update.ChatMember + "\n";
            mess += "message.From: " + message.From.Id
                + "\n";

            await botClient.SendTextMessageAsync(
              message.Chat.Id,
              mess
              );
        }

        async Task GetInlineKeyboardButton(Message message)
        {
            InlineKeyboardMarkup inlineKeyboard = new(new[]
        {
            // first row
            new []
            {
                InlineKeyboardButton.WithCallbackData(text: "Везу", callbackData: "101"),
                InlineKeyboardButton.WithCallbackData(text: "Еду", callbackData: "202"),
                InlineKeyboardButton.WithCallbackData(text: "Отдыхаю", callbackData: "303"),
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData(text: "Погода сейчас", callbackData: "404"),
                InlineKeyboardButton.WithCallbackData(text: "Погода на утро", callbackData: "505"),
                InlineKeyboardButton.WithCallbackData(text: "Cтата", callbackData: "606"),
            },

            });

            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Выбери таблетку",
                replyMarkup: inlineKeyboard,
                cancellationToken: this.cts.Token);
        }

        async Task GetInlineKeyboardButton()
        {
            InlineKeyboardMarkup inlineKeyboard = new(new[]
        {
            // first row
            new []
            {
                InlineKeyboardButton.WithCallbackData(text: "Еду", callbackData: "101"),
                InlineKeyboardButton.WithCallbackData(text: "Везу", callbackData: "202"),
                InlineKeyboardButton.WithCallbackData(text: "Отдыхаю", callbackData: "303"),
            },
            new []
            {
                //InlineKeyboardButton.WithCallbackData(text: "Погода сейчас", callbackData: "404"),
                //InlineKeyboardButton.WithCallbackData(text: "Погода на утро", callbackData: "505"),
                InlineKeyboardButton.WithCallbackData(text: "Cтата", callbackData: "606"),
            },

            });

            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: 370802502,
                text: "Выбери таблетку",
                replyMarkup: inlineKeyboard,
                cancellationToken: this.cts.Token);
        }


        async void GetWeather(Message message)
        {

            var wheather = await YaWeather.GetWeather();
            await botClient.SendTextMessageAsync(
              message.Chat.Id,
              wheather
              );
        }


        async void GetStat(Message message)
        {
            string mess = $"Cтатистика по трушным пацанам с 20.11:\n";


            string sqlExpression = "SELECT name, COUNT(*) as counter FROM statistic GROUP BY name ";
            using (var connection = new SqliteConnection(SqliteConnectionString))
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
                            mess += item.Key + "=" + item.Value + "\n";

                        }
                    }
                }
            }

            await botClient.SendTextMessageAsync(
               message.Chat.Id,
               mess
               );


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

    }





}






