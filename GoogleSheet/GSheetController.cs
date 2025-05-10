using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using dotenv.net;

namespace SpendTrackrTeleBot.GoogleSheet
{
    public class GSheetController
    {
        public static async Task<string> RunSheet(string email)
        {

            // Load environment variables
            DotEnv.Load();

            string[] Scopes = [SheetsService.Scope.Spreadsheets, DriveService.Scope.Drive];
            string ApplicationName = "SpendTrackerBot";
            string ServiceAccountFile = "./GoogleSheetConfig/Cred/cred.json";

            GoogleCredential credential;
            using (var stream = new FileStream(ServiceAccountFile, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
            }

            // Create Sheets API service
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Create new spreadsheet
            var spreadsheet = new Spreadsheet
            {
                Properties = new SpreadsheetProperties
                {
                    Title = "Spend Tracker"
                }
            };

            var createRequest = service.Spreadsheets.Create(spreadsheet);
            var createdSheet = createRequest.Execute();
            var spreadsheetId = createdSheet.SpreadsheetId;

            if (!string.IsNullOrEmpty(createdSheet.SpreadsheetId))
            {
                Console.WriteLine("Spreadsheet created successfully!");
            }
            else
            {
                Console.WriteLine("Failed to create spreadsheet.");
            }

            Console.WriteLine($"Created spreadsheet ID: {spreadsheetId}");
            Console.WriteLine($"Spreadsheet URL: https://docs.google.com/spreadsheets/d/{spreadsheetId}");

            await AddRolesOnSheet(credential, spreadsheetId, email);
            // Insert header row and one data row
            var range = "Sheet1!A1:D2";
            var valueRange = new ValueRange
            {
                Values =
                [
                    ["Date", "Category", "Description", "Amount"],
                    ["2025-05-05", "Food", "Lunch", "12.50"]
                ]
            };

            var updateRequest = service.Spreadsheets.Values.Update(valueRange, spreadsheetId, range);
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
            _ = updateRequest.Execute();

            Console.WriteLine("Data written to sheet.");

            return $"https://docs.google.com/spreadsheets/d/{spreadsheetId}";
        }

        public static Task AddRolesOnSheet(GoogleCredential credential, string? spreadsheetId, string email)
        {
            var driveService = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "SpendTrackerBot",
            });

            var permission = new Permission
            {
                Type = "user",
                Role = "reader",
                EmailAddress = email  // the account you want to share with
            };

            var request = driveService.Permissions.Create(permission, spreadsheetId);
            request.Fields = "id";
            request.Execute();

            Console.WriteLine($"Spreadsheet shared with {email}");
            return Task.CompletedTask;
        }
    }
}