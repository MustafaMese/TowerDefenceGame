using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    
    [SerializeField] private TextMeshProUGUI scoreText;

    private int _score = 0;

    public void Initialize()
    {
        Instance = this;
        
        if(GameManager.Instance.AutoSaveHandler.IsLoadSelected)
            LoadScore(GameManager.Instance.AutoSaveHandler.GetGameState().GetScore());
        
        if(_score == 0)
            scoreText.SetText(0.ToString());
        else
            scoreText.SetCharArray(_score.ConvertToCharArray());
        
        GameManager.Instance.CommandManager.AddCommandListener<AutoSaveCommand>(AutoSaveCommand);
    }

    private void AutoSaveCommand(AutoSaveCommand command)
    {
        GameManager.Instance.AutoSaveHandler.GetGameState().SetScore(_score);
    }

    public void IncreaseScore()
    {
        _score++;
        
        scoreText.SetCharArray(_score.ConvertToCharArray());
    }

    public void LoadScore(int score)
    {
        _score = score;
    }
}
