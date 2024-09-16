using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class ActionEvent_GameState : SerializedMonoBehaviour
{
    public GameAction_GameState gameAction;
    public GameState requiredGameState;
    
    [Button("Switch State to Start")]
    public void SwitchStateToStart()
    {
        gameAction.SwitchGameStates(GameState.Start);
    }
    [Button("Switch State to Play")]
    public void SwitchStateToPlay()
    {
        gameAction.SwitchGameStates(GameState.Play);
    }
    [Button("Switch State to Pause")]
    public void SwitchStateToPause()
    {
        gameAction.SwitchGameStates(GameState.Pause);
    }
    [Button("Switch State to End")]
    public void SwitchStateToEnd()
    {
        gameAction.SwitchGameStates(GameState.End);
    }
    [Button("Switch State to Build")]
    public void SwitchStateToBuild()
    {
        gameAction.SwitchGameStates(GameState.Build);
    }
    [Button("Switch State to Watering")]
    public void SwitchStateToWatering()
    {
        gameAction.SwitchGameStates(GameState.Watering);
    }
    [Button("Switch State to Dossier")]
    public void SwitchStateToDossier()
    {
        gameAction.SwitchGameStates(GameState.Dossier);
    }
    [Button("Switch State to Dialogue")]
    public void SwitchStateToDialogue()
    {
        gameAction.SwitchGameStates(GameState.Dialogue);
    }
    public UnityEvent<GameState> enterEvent;
    public UnityEvent<GameState> actionEvent;
    public UnityEvent<GameState> exitEvent;

    private void OnEnable()
    {
        gameAction.enterAction += EnterAction;
        //if gameAction._gameState == requiredGameState
        //subscribe to the action and exit action
        if(gameAction._gameState == requiredGameState)
        {
            gameAction.action += Action;
            gameAction.exitAction += ExitAction;
            enterEvent.Invoke(requiredGameState);
        }
    }

    private void OnDisable()
    {
        gameAction.enterAction -= EnterAction;
        gameAction.action -= Action;
        gameAction.exitAction -= ExitAction;
    }

    private void EnterAction(GameState gameState)
    {
        if (gameState == requiredGameState)
        {
            gameAction.action += Action;
            gameAction.exitAction += ExitAction;
            enterEvent.Invoke(gameState);
        }
    }
    private void Action(GameState gameState)
    {
        actionEvent.Invoke(gameState);
        Debug.Log($"Action on {this.gameObject.name}");
    }
    private void ExitAction(GameState gameState)
    {
        if (gameState == requiredGameState)
        {
            gameAction.action -= Action;
            gameAction.exitAction -= ExitAction;
            exitEvent?.Invoke(gameState);
        }
    }
}
