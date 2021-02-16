using Fooxboy.NucleusBot;
using Fooxboy.NucleusBot.Interfaces;
using Fooxboy.NucleusBot.Models;
using VkNet.Enums.SafetyEnums;

namespace CCDPlanetHelper.Commands
{
    public class AdvertisementMenu:INucleusCommand
    {
        public string Command => "adsmenu";
        public string[] Aliases => new string[0];
        public void Execute(Message msg, IMessageSenderService sender, IBot bot)
        {
            if (msg.ChatId != msg.MessageVK.FromId)
            {
                sender.Text("⛔ Эту команду нельзя вызывать в беседах. Чтобы узнать список всех команд - напишите \"команды\" ", msg.ChatId);
                return;
            }
            
            var kb = new KeyboardBuilder(bot);
            kb.AddButton("➕ Добавить объявление", "AddAds", color: KeyboardButtonColor.Positive);
            kb.AddLine();
            kb.AddButton("🔍 Список объявлений", "ads");
            kb.SetOneTime();

            sender.Text("❓ Выберите действие на клавиатуре", msg.ChatId, kb.Build());
        }

        public void Init(IBot bot, ILoggerService logger)
        {
            
        }
    }
}