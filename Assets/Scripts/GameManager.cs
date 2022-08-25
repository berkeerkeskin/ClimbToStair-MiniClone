using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState State;

    public static event Action<GameState> OnGameStateChanged;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateGameState(GameState.StartState);
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.StartState:
                HandleStartState();
                break;
            case GameState.PlayState:
                HandlePlayState();
                break;
            case GameState.UpgradeState:
                HandleUpgradeState();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnGameStateChanged?.Invoke(newState);
    }

    private void HandleStartState()
    {
        
    }

    private void HandlePlayState()
    {
        UnitManager.Instance.PlayState();
    }

    private void HandleUpgradeState()
    {
        UnitManager.Instance.UpgradeState();
    }
}
    public enum GameState
    {
        StartState,
        PlayState,
        UpgradeState
    }