using System;
using Fooxboy.NucleusBot.Interfaces;
using Fooxboy.NucleusBot.Models;
using Microsoft.Extensions.Logging;

namespace CCDPlanetHelper.Commands
{
    public class CalcCommand:INucleusCommand
    {
        public string Command => "calc";
        public string[] Aliases => new[] {"калькулятор", "посчитай"};
        public void Execute(Message msg, IMessageSenderService sender, IBot bot)
        {

            var words = msg.Text.Split(" ");

            var expression = string.Empty;

            for (int i = 1; i < words.Length; i++)
            {
                expression += words[i] + " ";
            }

            if (expression == String.Empty)
            {
                sender.Text("⛔ Вы не указали выражение. Например: \n Посчитай 5 + 5", msg.ChatId);
                return;
            }

            try
            {
                System.Data.DataTable table = new System.Data.DataTable();
                table.Columns.Add("expression", string.Empty.GetType(), expression);
                System.Data.DataRow row = table.NewRow();
                table.Rows.Add(row);
                var resultEx = double.Parse((string) row["expression"]);

                sender.Text($"✨ {expression} = {resultEx}", msg.ChatId);

            }
            catch (Exception e)
            {
                sender.Text("⛔ Мы не смогли посчитать такое выражение, может Вы где-то ошиблись? Например: \n Посчитай 5+5", msg.ChatId);
            }
            
        }

        public void Init(IBot bot, ILoggerService logger)
        {
        }
    }
}