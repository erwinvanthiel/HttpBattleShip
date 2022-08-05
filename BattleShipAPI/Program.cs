using BattleShipAPI.Data;
using Microsoft.Extensions.DependencyInjection;

// Program config
var myPort = args[0];
var opponentAddressAndPort = args[1];
var opponentPort = opponentAddressAndPort.Substring(opponentAddressAndPort.Length - 4);

// Create level file
LevelFileCreator.CreateLevelFile();

// create game object
var builder = WebApplication.CreateBuilder(args);
var gameStateService = new GameStateService();

if (Int32.Parse(myPort) < Int32.Parse(opponentPort))
{
    gameStateService.getObject().MyTurn = true;
}

// add services to DI container
{
    var services = builder.Services;
    services.AddControllers();
    services.AddSingleton<IStateService>(gameStateService);
}

var app = builder.Build();

app.Urls.Add(String.Format("http://*:{0}", myPort));

// configure HTTP request pipeline
{
    app.MapControllers();
}

// Start game
bool init = true;
CancellationTokenSource source = new CancellationTokenSource();
CancellationToken token = source.Token;
var thread1 = Task.Run(() => httpListener());
var thread2 = Task.Run(() => gameLoop());

void httpListener()
{
    app.Run();
}

async void gameLoop()
{
    
    // Keep parsing and posting moves until my ships have sunk.
    while (!(gameStateService.getObject().IsGameCrashed || gameStateService.getObject().IsGameWon || gameStateService.getObject().IsGameLost))
    {
        if (init) { gameStateService.getObject().printFreshGrid(); init = false; }
        if (gameStateService.getObject().MyTurn)
        {
            // Parse move
            string input = System.Console.ReadLine();

            // set MyTurn to false so that opponent get sent correct gamestring containing "shoot coordinate" instead of "waiting for opponent".
            gameStateService.getObject().MyTurn = false;

            // send http to opponent
            using (var client = new HttpClient())
            {
                var url = String.Format("{0}/move/{1}", opponentAddressAndPort, input);
                var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grid", gameStateService.getObject().getMygrid())
                });

                try
                {
                    var response = client.PostAsync(url, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    // by calling .Content you are performing a synchronous call
                    var responseContent = response.Content;

                    // by calling .Result you are synchronously reading the result
                    string responseString = responseContent.ReadAsStringAsync().Result;

                    // Update the opponent grid and render it to your console
                    gameStateService.getObject().OpponentGrid = responseString;
                    gameStateService.getObject().printGame();
                }
                else
                {
                    gameStateService.getObject().MyTurn = true;
                }
            }
                catch (Exception e)
            {
                    System.Console.WriteLine(e);
                    System.Console.WriteLine("Wait for opponent to be online...");
                gameStateService.getObject().MyTurn = true;
            }
        }
        }
    }
    
    // My cruise ships have sunk
    // Notify Opponent that he has lost.
    if (gameStateService.getObject().IsGameWon)
    {
        var success = false;
        while (!success && gameStateService.getObject().MustNotifyOpponent)
        {
            System.Console.WriteLine("where are you?");
            using (var client = new HttpClient())
            {
                var url = String.Format("{0}/{1}", opponentAddressAndPort, "youLose");
                var response = client.GetAsync(url).Result;
                System.Console.WriteLine(response);
                if (response.IsSuccessStatusCode)
                {
                    success = true;
                }
            }
        }
        // Log win message
        System.Console.Clear();
        System.Console.WriteLine("You win!");
    }

    // My ships have sunk
    // Notify Opponent that he has won.
    else if (gameStateService.getObject().IsGameLost)
    {
        var success = false;
        while (!success && gameStateService.getObject().MustNotifyOpponent)
        {
            using (var client = new HttpClient())
            {
                var url = String.Format("{0}/{1}", opponentAddressAndPort, "youWin");
                var response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    success = true;
                }
            }
        }
        // Log lose message
        System.Console.Clear();
        System.Console.WriteLine("You lose!");
    }

    // Game has crashed
    else 
    {
        // Log lose message
        System.Console.Clear();
        System.Console.WriteLine("Game has crashed due to unreadable level file!");
    }
}

await thread2;




