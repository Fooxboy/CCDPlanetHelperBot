using System.Collections.Generic;
using System.IO;
using System.Linq;
using CCDPlanetHelper.Database;
using CCDPlanetHelper.Models;
using Fooxboy.NucleusBot;
using Fooxboy.NucleusBot.Interfaces;
using Fooxboy.NucleusBot.Models;
using Newtonsoft.Json;

namespace CCDPlanetHelper.Commands
{
    public class AddTargetCommand:INucleusCommand
    {
        public string Command => "addtarget";
        public string[] Aliases => new string[0];
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
            
            var carTarget = long.Parse(msg.Payload.Arguments[0]);

            using (var db = new BotData())
            {
                var targetInfo = db.Targets.SingleOrDefault(t => t.UserId == msg.MessageVK.FromId);
                var kb = new KeyboardBuilder(bot);
                kb.AddButton("🔙 Назад к авто", "carinfo", new List<string>() {carTarget.ToString()});
                kb.AddLine();
                kb.AddButton("🔙 В меню", "menu");
                kb.SetOneTime();
                if (targetInfo is null)
                {
                    targetInfo = new Target();
                    targetInfo.UserId = msg.ChatId;
                    targetInfo.Count = 0;
                    targetInfo.CarId = carTarget;
                    db.Targets.Add(targetInfo);
                    sender.Text("✔ Цель добавлена", msg.ChatId, kb.Build());
                }
                else
                {
                    targetInfo.CarId = carTarget;
                    sender.Text("✔ Цель обновлена", msg.ChatId, kb.Build());
                }

                db.SaveChanges();
            }
        }

        public void Init(IBot bot, ILoggerService logger)
        {
        }
    }
}