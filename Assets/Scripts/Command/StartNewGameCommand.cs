public class StartNewGameCommand : ICommand
{
    public bool IsNewGame;
    
    public StartNewGameCommand(bool isNewGame)
    {
        IsNewGame = isNewGame;
    }
}