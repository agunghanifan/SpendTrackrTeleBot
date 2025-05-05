using SpendTrackrTeleBot.GoogleSheetConfig;

namespace SpendTrackrTeleBot
{
    public class MainControl
    {
        public async Task<string> Run(string? message)
        {
            Console.WriteLine("MainControl is running...");
            // Add your logic here
            if (message is not null)
            {
                if (message == "/start")
                {
                    string respStart = "You choose to start, please write your email";
                    Console.WriteLine(respStart);
                    return respStart;
                }
                else
                {
                    if (IsValidEmail(message))
                    {
                        GSheetConfig gsheet = new();
                        string addressGSheet = await gsheet.RunSheet(message);
                        return addressGSheet;
                    }
                    else
                    {
                        string respNotStart = "Wrong command or text, please try again write /start";
                        Console.WriteLine(respNotStart);
                        return respNotStart;
                    }
                }   
            }
            else
                return "Command not found";

        }

        private static bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();
            
            if (trimmedEmail.EndsWith(".")) 
            {
                return false;
            }

            try 
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch 
            {
                return false;
            }
        }
    }
}