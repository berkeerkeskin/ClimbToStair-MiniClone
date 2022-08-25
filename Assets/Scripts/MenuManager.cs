using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject startScreenPanel;
    [SerializeField] private GameObject buttonsAndTexts;
    [SerializeField] private GameObject staminaButton;
    [SerializeField] private GameObject moneyButton;

    private void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    private void GameManagerOnGameStateChanged(GameState state)
    {
        startScreenPanel.SetActive(state == GameState.StartState);
        
        buttonsAndTexts.SetActive(state == GameState.UpgradeState);
        staminaButton.SetActive(state == GameState.UpgradeState);
        moneyButton.SetActive(state == GameState.UpgradeState);
    }
}
