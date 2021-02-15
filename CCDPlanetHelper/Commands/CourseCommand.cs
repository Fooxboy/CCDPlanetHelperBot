using System.IO;
using CCDPlanetHelper.Models;
using Fooxboy.NucleusBot.Interfaces;
using Fooxboy.NucleusBot.Models;
using Newtonsoft.Json;

namespace CCDPlanetHelper.Commands
{
    public class CourseCommand:INucleusCommand
    {
        public string Command => "cource";
        public string[] Aliases => new string[] { "Курс"};
        public void Execute(Message msg, IMessageSenderService sender, IBot bot)
        {
            var courseText = File.ReadAllText("CourceConfig.json");
            var course = JsonConvert.DeserializeObject<CourseModel>(courseText);

            var text = $"💲 Курс валют: \n 💵 Доллар = {course.Dollar} \n 💶 Евро = {course.Euro}";
            sender.Text(text, msg.ChatId);
        }

        public void Init(IBot bot, ILoggerService logger)
        {
            
        }
    }
}