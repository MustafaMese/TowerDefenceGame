using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginUI : MonoBehaviour
{
    public void NewGame()
    {
        GameManager.Instance.CommandManager.InvokeCommand(new StartNewGameCommand(true));
    }

    public void Continue()
    {
        GameManager.Instance.CommandManager.InvokeCommand(new StartNewGameCommand(false));
    }
}