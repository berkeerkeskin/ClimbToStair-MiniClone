using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;
    
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject spawner;
    [SerializeField] private GameObject upgradeCanvas;
    
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private GameObject temporaryEnvironment;

        private void Awake()
    {
        Instance = this;
    }

    public void StartState()
    {
        GameManager.Instance.UpdateGameState(GameState.PlayState);
    }

    public void PlayState()
    {
        player.SetActive(true);
        spawner.SetActive(true);
        upgradeCanvas.SetActive(true);
        Player.Instance.isAlive = true;
    }

    public void UpgradeState()
    {
        foreach (Transform child in temporaryEnvironment.transform) {
            Destroy(child.gameObject);
        }
        
        Player.Instance.isAlive = false;
        Player.Instance.gameObject.transform.position = playerSpawnPoint.position;
        player.transform.rotation = Quaternion.Euler(new Vector3(0f, -90f, 0f));
    }
}
