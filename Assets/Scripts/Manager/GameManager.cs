using System;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get;
        private set;
    }

    [SerializeField] private LevelCreator levelCreatorPrefab;
    [SerializeField] private EnemyManager enemyManagerPrefab;
    [SerializeField] private TowerManager towerManagerPrefab;
    [SerializeField] private ScoreManager scoreManagerPrefab;
    [SerializeField] private LoginUI loginUiPrefab;
    [SerializeField] private FloatingTextController floatingTextControllerPrefab;
    [SerializeField] private float savingInterval;
    
    public CommandManager CommandManager;
    public TileEnemyMatchHandler TileEnemyMatchHandler;
    public AutoSaveHandler AutoSaveHandler;
    private LoginUI _loginUI;

    private void Awake()
    {
        Instance = this;
        
        CommandManager = new CommandManager();
        TileEnemyMatchHandler = new TileEnemyMatchHandler();
        AutoSaveHandler = new AutoSaveHandler(savingInterval);

        CommandManager.AddCommandListener<StartNewGameCommand>(StartNewGameCommand);
        
        _loginUI = Instantiate(loginUiPrefab);
    }

    private void StartNewGameCommand(StartNewGameCommand command)
    {
        if(command.IsNewGame)
            CreateLevel(true);
        else
            CreateLevel(SaveManager.GetIsLastGameDefeat());
    }

    private void CreateLevel(bool isNewGame)
    {
        Destroy(_loginUI.gameObject);
        
        SaveManager.SetIsLastGameDefeat(false);
        
        AutoSaveHandler.IsLoadSelected = !isNewGame;

        Instantiate(scoreManagerPrefab).Initialize();

        Instantiate(floatingTextControllerPrefab).Initialize();
        
        var levelCreator = Instantiate(levelCreatorPrefab);
        levelCreator.Initialize();

        var enemySpawnHandler = Instantiate(enemyManagerPrefab);
        enemySpawnHandler.Initialize(levelCreator);

        var towerManager = Instantiate(towerManagerPrefab);
        towerManager.Initialize(levelCreator);

        StartCoroutine(AutoSaveHandler.StartRecording());
    }
}
