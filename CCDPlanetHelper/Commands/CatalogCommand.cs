using System.Collections.Generic;
using System.IO;
using System.Linq;
using CCDPlanetHelper.Models;
using Fooxboy.NucleusBot;
using Fooxboy.NucleusBot.Interfaces;
using Fooxboy.NucleusBot.Models;
using Newtonsoft.Json;

namespace CCDPlanetHelper.Commands
{
    public class CatalogCommand:INucleusCommand
    {
        public string Command => "catalog";
        public string[] Aliases => new[] {"каталог"};
        public void Execute(Message msg, IMessageSenderService sender, IBot bot)
        {
            //проверка и подписка на рассылку, если пользователь пользуется ботом первый раз.
            var usrs1 = JsonConvert.DeserializeObject<MailingModel>(File.ReadAllText("MailingUsers.json"));
            if (usrs1.Users.All(u => u.UserId != msg.MessageVK.FromId.Value))
            {
                usrs1.Users.Add(new ValuesMail()
                {
                    IsActive = true,
                    UserId = msg.MessageVK.FromId.Value
                });
                
                File.WriteAllText("MailingUsers.json", JsonConvert.SerializeObject(usrs1));
            }
            var kb = new KeyboardBuilder(bot);
            kb.AddButton("Европа", "showroom", new List<string>() {"1", "0"});

            kb.AddButton("Япония", "showroom", new List<string>() {"2", "0"});
            kb.AddButton("Toyota", "showroom", new List<string>() {"3", "0"});
            kb.AddLine();
            kb.AddButton("Mercedes-Benz", "showroom", new List<string>() {"4", "0"});
            kb.AddButton("BMW", "showroom", new List<string>() {"5", "0"});
            kb.AddButton("Лада", "showroom", new List<string>() {"6", "0"});
            kb.AddButton("Яхты", "showroom", new List<string>() {"7", "0"});
            kb.AddLine();
            kb.AddButton("Вертолеты", "showroom", new List<string>() {"8", "0"});
            kb.AddButton("Америка", "showroom", new List<string>() {"9", "0"});
            kb.AddButton("Коммерческий", "showroom", new List<string>() {"10", "0"});
            kb.AddButton("🔙 Назад ", "searchmenu");
            kb.SetOneTime();

            sender.Text("Выберите необходимый автосалон.", msg.ChatId, kb.Build());
        }

        public void Init(IBot bot, ILoggerService logger)
        {
            
        }
    }
}