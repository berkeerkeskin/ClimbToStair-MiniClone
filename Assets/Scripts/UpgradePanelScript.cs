using DG.Tweening;
using TMPro;
using UnityEngine;
 
public class UpgradePanelScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playText;
    [SerializeField] private TextMeshProUGUI staminaCost;
    [SerializeField] private TextMeshProUGUI moneyUpgradeCost;
    
    [SerializeField] private UnityEngine.UI.Button staminaButton;
    [SerializeField] private UnityEngine.UI.Button moneyButton;

    private float _money;
    private float _staminaCost;
    private float _moneyCost;
    private void Awake()
    {
        Player.UpdateMoney += UpdateMoney;
    }

    private void OnDestroy()
    {
        Player.UpdateMoney -= UpdateMoney;
    }

    void Start()
    {
        ShakeText();
        staminaButton.onClick.AddListener(StaminaOnClick);
        moneyButton.onClick.AddListener(MoneyOnClick);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.Instance.UpdateGameState(GameState.PlayState);
        }
    }

    private void UpdateMoney(float money)
    {
        if (GameManager.Instance.State != GameState.UpgradeState)
        {
            _money = money;
        
            _staminaCost = Random.Range(_money * 0.35f, _money);
            _staminaCost = Mathf.Round(_staminaCost * 100f) / 100f;
        
            _moneyCost = Random.Range(_money * 0.35f, _money);
            _moneyCost = Mathf.Round(_moneyCost * 100f) / 100f;
        
            staminaCost.text = "$ " + _staminaCost;
            moneyUpgradeCost.text = "$ " + _moneyCost;
        }
    }
    private void ShakeText()
    {
        playText.transform.DORotate(Vector3.forward * 15f, 1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        playText.transform.DOScale(Vector3.one * 2f, 1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }

    private void StaminaOnClick()
    {
        if (_staminaCost < _money)
        {
            Player.Instance.UpgradeProperties(Upgrades.Stamina, _staminaCost);
            _money -= _staminaCost;
            staminaButton.gameObject.SetActive(false);
        }
    }

    private void MoneyOnClick()
    {
        if (_moneyCost < _money)
        {
            Player.Instance.UpgradeProperties(Upgrades.MoneyMagnitude, _moneyCost);
            _money -= _moneyCost;
            moneyButton.gameObject.SetActive(false);
        }
    }
}

    public enum Upgrades
    {
        Stamina,
        MoneyMagnitude
    }