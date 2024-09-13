using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PauseGame : MonoBehaviour
{
    public GameAction_GameState gameState;
    public UnityEvent onPause;
    public UnityEvent onUnpause;

    public void PauseOrUnpauseGame()
    {
        if (gameState._gameState == GameState.Pause)
        {
            gameState.SwitchGameStates(GameState.Play);
            onUnpause.Invoke();
        }
        else if(gameState._gameState == GameState.Play)
        {
            gameState.SwitchGameStates(GameState.Pause);
            onPause.Invoke();
        }
        else if(gameState._gameState == GameState.Start)
        {
            gameState.SwitchGameStates(GameState.Pause);
            onPause.Invoke();
        }
        else if(gameState._gameState == GameState.Build)
        {
            gameState.SwitchGameStates(GameState.Pause);
            onPause.Invoke();
        }
        else if(gameState._gameState == GameState.End)
        {
            gameState.SwitchGameStates(GameState.Pause);
            onPause.Invoke();
        }
    }
}
