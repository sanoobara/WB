﻿//using Telegram.Bot;

//var botClient = new TelegramBotClient("6368222490:AAFoJCSEjozm8oZDa0Iu6C-zVSH2WR3szfE");


//var me = await botClient.GetMeAsync();
//Console.WriteLine($"Hello, I'm user {me.Id} amd my name is {me.FirstName}.");

using System;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

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

    Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

    // Echo received message text
    Message sentMessage = await botClient.SendTextMessageAsync(
        chatId: chatId,
        text: "You said:\n" + messageText,
        cancellationToken: cancellationToken);

    Message stikerMessage = await botClient.SendStickerAsync(
    chatId: chatId,
    sticker: InputFile.FromUri("https://github.com/TelegramBots/book/raw/master/src/docs/sticker-dali.webp"),
    cancellationToken: cancellationToken);

    Message Testmessage = await botClient.SendTextMessageAsync(
    chatId: chatId,
    text: "Trying *all the parameters* of `sendMessage` method",
    parseMode: ParseMode.MarkdownV2,
    disableNotification: true,
    replyToMessageId: update.Message.MessageId,
    replyMarkup: new InlineKeyboardMarkup(
        InlineKeyboardButton.WithUrl(
            text: "Check sendMessage method",
            url: "https://core.telegram.org/bots/api#sendmessage")),
    cancellationToken: cancellationToken);

    Console.WriteLine(
    $"{Testmessage.From.FirstName} sent message {Testmessage.MessageId} " +
    $"to chat {Testmessage.Chat.Id} at {Testmessage.Date}. " +
    $"It is a reply to message {Testmessage.ReplyToMessage.MessageId} " +
    $"and has {Testmessage.Entities.Length} message entities.");


    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
{
    new KeyboardButton[] { "Help me", "Call me ☎️" },
})
    {
        ResizeKeyboard = true
    };



    Message sentMessage2 = await botClient.SendTextMessageAsync(
    chatId: chatId,
    text: "Choose a response",
    //replyMarkup: replyKeyboardMarkup,
    cancellationToken: cancellationToken);




    if (message.Text == "/reply")
    {
        // Тут все аналогично Inline клавиатуре, только меняются классы
        // НО! Тут потребуется дополнительно указать один параметр, чтобы
        // клавиатура выглядела нормально, а не как абы что

        var replyKeyboard = new ReplyKeyboardMarkup(
            new List<KeyboardButton[]>()
            {
                                        new KeyboardButton[]
                                        {
                                            new KeyboardButton("Привет!"),
                                            new KeyboardButton("Пока!"),
                                        },
                                        new KeyboardButton[]
                                        {
                                            new KeyboardButton("Позвони мне!")
                                        },
                                        new KeyboardButton[]
                                        {
                                            new KeyboardButton("Напиши моему соседу!")
                                        }
            })
        {
            // автоматическое изменение размера клавиатуры, если не стоит true,
            // тогда клавиатура растягивается чуть ли не до луны,
            // проверить можете сами
            ResizeKeyboard = false,
        };

        await botClient.SendTextMessageAsync(
           message.Chat.Id ,
            "Это reply клавиатура!",
            replyMarkup: replyKeyboard); // опять передаем клавиатуру в параметр replyMarkup

        return;
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


