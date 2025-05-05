using System;

namespace SpendTrackrTeleBot
{
    public class MainControl
    {
        public string Run(string? message)
        {
            Console.WriteLine("MainControl is running...");
            // Add your logic here
            if (message is not null)
            {
                if (message == "/start")
                {
                    string respStart = "You choose to start";
                    Console.WriteLine(respStart);
                    return respStart;
                }
                else
                {
                    string respNotStart = "You choose to not start";
                    Console.WriteLine(respNotStart);
                    return respNotStart;
                }   
            }
            else
                return "Command not found";

        }
    }
}