using Telegram.Bot;
using traning_Tbot;


config config = new config();



Console.WriteLine(config.Configuration["Token"], config.Configuration["DataBase"]);


using CancellationTokenSource cts = new();

var botWorker = new BotWorker(config.Configuration["Token"], config.Configuration["DataBase"], cts);

var me = await botWorker.botClient.GetMeAsync();

Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();
cts.Cancel();




