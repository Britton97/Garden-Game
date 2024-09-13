using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public enum GameState
{
    Start,
    Play,
    Build,
    Pause,
    Watering,
    Dossier,
    End
}
[CreateAssetMenu]
[InlineEditor]
public class GameAction_GameState : ScriptableObject
{
    //need an enter and exit action too
    public GameState _gameState;
    [SerializeField] private bool ignoreStateChange = false;
    public UnityAction<GameState> enterAction;
    public UnityAction<GameState> action;
    public UnityAction<GameState> exitAction;

    public void InvokeEnterAction(GameState gameState)
    {
        _gameState = gameState;
        enterAction?.Invoke(gameState);
    }

    public void InvokeUpdateAction(GameState gameState)
    {
        action?.Invoke(gameState);
    }

    public void InvokeExitAction(GameState gameState)
    {
        exitAction?.Invoke(gameState);
    }

    public void SwitchGameStates(GameState gameState)
    {
        if (ignoreStateChange) { return; }

        exitAction?.Invoke(_gameState);
        _gameState = gameState;
        enterAction?.Invoke(gameState);
    }

    public void IgnoreStateChange(bool ignore)
    {
        ignoreStateChange = ignore;
    }

    public void DebugString(string message)
    {
        Debug.Log(message);
    }
}
