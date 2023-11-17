using Telegram.Bot;

public class TelegramBot
{
    private TelegramBotClient botClient;
    private readonly string API_TOKEN;

    public TelegramBot()
    {
        API_TOKEN = "YOUR_API_TOKEN_HERE";
        botClient = new TelegramBotClient(API_TOKEN);
    }

    public async Task StartAsync()
    {
        botClient.StartReceiving();
        botClient.OnMessage += Bot_OnMessage;
    }

    private async void Bot_OnMessage(object sender, MessageEventArgs e)
    {
        // здесь будет основная логика работы с сообщениями
    }





}


