using traning_Tbot;


config config = new config();



//Console.WriteLine(config.Configuration["Token"], config.Configuration["DataBase"]);


//using CancellationTokenSource cts = new();

//var botWorker = new BotWorker(config.Configuration["Token"], config.Configuration["DataBase"], cts);

//var me = await botWorker.botClient.GetMeAsync();

//Console.WriteLine($"Start listening for @{me.Username}");
//Console.ReadLine();
//cts.Cancel();



Console.WriteLine(Guid.NewGuid().ToString("N"));


//var s =  ExchangeRate.GetRate();

//Console.WriteLine(s.Result);



//Console.ReadLine();

//var whether = new YaWhether();

//YaWhether.GetWether();


//await Task.Delay(3000);

//int str = 5;

//DateTime scheduledTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 0, 0); // Например, отправка в 12:00


//Console.WriteLine(scheduledTime.Subtract(DateTime.Now).Milliseconds);

//TimerCallback callback = new TimerCallback(SendMessage);
//Timer timer = new Timer(callback, str, scheduledTime.Subtract(DateTime.Now).Minutes, Timeout.Infinite);

//void SetTimer()
//{
//    DateTime desiredTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 0, 0);
//    TimeSpan timeUntilDesired = desiredTime - DateTime.Now;
//    if (timeUntilDesired.TotalMilliseconds < 0)
//    {
//        desiredTime = desiredTime.AddDays(1);
//        timeUntilDesired = desiredTime - DateTime.Now;
//    }
//    Timer timer = new Timer();
//    timer.Elapsed += new ElapsedEventHandler(SendMessage);
//    // Устанавливаем интервал в 24 часа (86400000 миллисекунд)
//    timer.Interval = 86400000;
//    // Запускаем таймер
//    timer.Enabled = true;
//    timer.Start();
//}




























//static void SendMessage(object botClient)
//{
//    var client = (TelegramBotClient)botClient;
//    long chatId = YOUR_CHAT_ID; // Идентификатор чата, куда будет отправляться сообщение
//    string message = "Hello, world!"; // Текст сообщения
//    client.SendTextMessageAsync(chatId, message).Wait();
//}

// void SetTimer()
//{
//    DateTime desiredTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 0, 0);
//    TimeSpan timeUntilDesired = desiredTime - DateTime.Now;
//    if (timeUntilDesired.TotalMilliseconds < 0)
//    {
//        desiredTime = desiredTime.AddDays(1);
//        timeUntilDesired = desiredTime - DateTime.Now;
//    }
//    Timer timer = new Timer();
//    timer.Elapsed += new ElapsedEventHandler(SendMessage);
//    // Устанавливаем интервал в 24 часа (86400000 миллисекунд)
//    timer.Interval = 86400000;
//    // Запускаем таймер
//    timer.Enabled = true;
//    timer.Start();
//}
//static void SendMessage(object str)
//{
//    Console.WriteLine(str);
//}



