using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

class Program
{



    // Это клиент для работы с Telegram Bot API, который позволяет отправлять сообщения, управлять ботом, подписываться на обновления и многое другое.
    private static ITelegramBotClient _botClient;

    // Это объект с настройками работы бота. Здесь мы будем указывать, какие типы Update мы будем получать, Timeout бота и так далее.
    private static ReceiverOptions _receiverOptions;

    static async Task Main()
    {

        _botClient = new TelegramBotClient("6858154783:AAHjLzqfJx6Oun5HA-5p1OckkOqW_iPTHvU"); // Присваиваем нашей переменной значение, в параметре передаем Token, полученный от BotFather
        _receiverOptions = new ReceiverOptions // Также присваем значение настройкам бота
        {
            AllowedUpdates = new[] // Тут указываем типы получаемых Update`ов, о них подробнее расказано тут https://core.telegram.org/bots/api#update
            {
                UpdateType.Message, // Сообщения (текст, фото/видео, голосовые/видео сообщения и т.д.)
            },
            // Параметр, отвечающий за обработку сообщений, пришедших за то время, когда ваш бот был оффлайн
            // True - не обрабатывать, False (стоит по умолчанию) - обрабаывать
            ThrowPendingUpdates = true,
        };

        using var cts = new CancellationTokenSource();

        // UpdateHander - обработчик приходящих Update`ов
        // ErrorHandler - обработчик ошибок, связанных с Bot API
        _botClient.StartReceiving(UpdateHandler, ErrorHandler, _receiverOptions, cts.Token); // Запускаем бота

        var me = await _botClient.GetMeAsync(); // Создаем переменную, в которую помещаем информацию о нашем боте.
        Console.WriteLine($"{me.FirstName} запущен!");

        await Task.Delay(-1); // Устанавливаем бесконечную задержку, чтобы наш бот работал постоянно
    }

    //private static async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    //{
    //    // Обязательно ставим блок try-catch, чтобы наш бот не "падал" в случае каких-либо ошибок
    //    try
    //    {
    //        // Сразу же ставим конструкцию switch, чтобы обрабатывать приходящие Update
    //        switch (update.Type)
    //        {
    //            case UpdateType.Message:
    //                {
    //                    Console.WriteLine("Пришло сообщение!");
    //                    return;
    //                }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine(ex.ToString());
    //    }
    //}
    private static async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {

        var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJhY2Nlc3NJRCI6Ijg3MWE4OTVhLTM3YmEtNDhjZC1iZDRiLTMyODUyMWZkZTUxYiJ9.50kj13J30fZZ2eQrpOVnckL9OLuAIdcAWJW30xt5U0A";
        var url = "https://suppliers-api.wildberries.ru/api/v3/orders/new";



        // Обязательно ставим блок try-catch, чтобы наш бот не "падал" в случае каких-либо ошибок
        try
        {

            // Сразу же ставим конструкцию switch, чтобы обрабатывать приходящие Update
            switch (update.Type)
            {
                case UpdateType.Message:
                    {
                        // эта переменная будет содержать в себе все связанное с сообщениями
                        var message = update.Message;

                        // From - это от кого пришло сообщение (или любой другой Update)
                        var user = message.From;

                        // Выводим на экран то, что пишут нашему боту, а также небольшую информацию об отправителе
                        Console.WriteLine($"{user.FirstName} ({user.Id}) написал сообщение: {message.Text}");

                        // Chat - содержит всю информацию о чате
                        var chat = message.Chat;
                        await botClient.SendTextMessageAsync(
                            chat.Id,
                            message.Text, // отправляем то, что написал пользователь
                            replyToMessageId: message.MessageId // по желанию можем поставить этот параметр, отвечающий за "ответ" на сообщение
                            );
                        while (true)
                        {
                            using (var client = new HttpClient())
                            {

                                client.DefaultRequestHeaders.Add("Authorization", token);
                                var response = await client.GetStringAsync(url);
                                Console.WriteLine(response);


                                // Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();

                                Root r = JsonConvert.DeserializeObject<Root>(response);
                                if (r.orders.Count != 0)
                                {
                                    await botClient.SendTextMessageAsync(chat.Id, r.orders[0].skus[0]);

                                    Console.WriteLine(r.orders[0].skus[0]);

                                }
                                else
                                {
                                    await botClient.SendTextMessageAsync(
                                chat.Id, "EMPTY");
                                }

                            }

                            Thread.Sleep(100000);

                        }
                        return;
                    }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
    private static Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
    {
        // Тут создадим переменную, в которую поместим код ошибки и её сообщение 
        var ErrorMessage = error switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => error.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }


    public class Order
    {
        public object address { get; set; }
        public string deliveryType { get; set; }
        public object user { get; set; }
        public string orderUid { get; set; }
        public string article { get; set; }
        public string rid { get; set; }
        public DateTime createdAt { get; set; }
        public List<string> offices { get; set; }
        public List<string> skus { get; set; }
        public int id { get; set; }
        public int warehouseId { get; set; }
        public int nmId { get; set; }
        public int chrtId { get; set; }
        public int price { get; set; }
        public int convertedPrice { get; set; }
        public int currencyCode { get; set; }
        public int convertedCurrencyCode { get; set; }
        public int cargoType { get; set; }
    }

    public class Root
    {
        public List<Order> orders { get; set; }
    }

}