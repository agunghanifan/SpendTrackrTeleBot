# SpendTrackr Telegram Bot

A Telegram bot that helps users track their daily expenses and manage their finances.

## Features

- Track expenses with simple commands
- Categorize spending
- View daily/monthly summaries
- Set budget limits
- Export expense reports

## Usage

1. Start the bot: Search for your bot name on Telegram
2. Commands:
    - `/start` - Begin using the bot 
    - `/add amount category note` - Add new expense
    - `/summary` - View spending summary
    - `/budget` - Set budget limits
    - `/report` - Generate expense report

## Development

### Prerequisites
- .NET 8.0 or higher
- Telegram Bot Token
- Database (PostgreSQL)

### Setup
1. Create bot at Telegram @BotFather first and get the bot token
1. Clone repository
2. Add bot token in `.env` file
3. Put your Credential google service account in GoogleSheetConfig/Cred/cred.json
4. Run migrations
5. Build and run project

### Environment Variables
Create a `.env` file in the root of the project with the following content:
TELEGRAM_BOT_TOKEN=<Your telegram bot token>

## Contributing

Pull requests welcome. For major changes, please open an issue first.

## License

This project is licensed under the [MIT License](LICENSE).