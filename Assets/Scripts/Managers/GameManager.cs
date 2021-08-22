using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameState;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameStateManager gameStateManager;

    private void Awake()
    {
        // Make sure we only have one instance of this script
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        // Set up the instance
        instance = this;
        DontDestroyOnLoad(gameObject);

        // Set up the game state manager
        gameStateManager = new GameStateManager();
    }

    public void TogglePauseGame()
    {
        if (gameStateManager.currentState is PauseGameState)
        {
            gameStateManager.GoToPrevState();
        }
        else
        {
            gameStateManager.ChangeState("Pause");
        }
    }
}
