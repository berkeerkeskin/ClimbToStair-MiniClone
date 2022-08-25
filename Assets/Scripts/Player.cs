using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Player : MonoBehaviour
{
    public static Player Instance;
    
    public static event Action<float> UpdateMoney;
    
    [SerializeField] private Transform barSpawnPoint;
    [SerializeField] private Transform playerSpawnPoint;
    
    [SerializeField] private SkinnedMeshRenderer playerMeshRenderer;

    [SerializeField] private float stamina;
    [SerializeField] private float staminaMultiplier;
    [SerializeField] private int maxMouseClickCount;
    
    private float _spawnTime;
    private float _mouseHoldTime;
    private float _moneyMagnitude = 0.5f;
    private float _instantiateStairTime = 0.4f;
    private float _money;
    private int _mouseClickCount;
    public bool isAlive;
    
    private Animator _animator;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        isAlive = true;
    }
    
    private void Update()
    {
        if (isAlive)
        {
            IsMouseButtonDown();
            IsMouseButtonUp();
            MovePlayer();
            Die();
            StaminaColor();
            Debug.Log("mouseHoldTime=" + _mouseHoldTime + ", mouseclickCount= " + _mouseClickCount);
        }
        else
        {
            transform.position = playerSpawnPoint.position;
            transform.rotation = Quaternion.Euler(new Vector3(0f, -90f, 0f));
        }
    }
    
    private void MovePlayer()
    {
        if (_spawnTime > _instantiateStairTime)
        {
            transform.RotateAround(barSpawnPoint.position, Vector3.up, 15f);
            Vector3 playerPosition = transform.position;
            transform.DOMove(playerPosition + Vector3.up * 0.15f, 0.2f).SetEase(Ease.Linear);
            
            _spawnTime = 0f;
            _money += _moneyMagnitude;
            _money = Mathf.Round(_money * 100f) / 100f;
            UpdateMoney?.Invoke(_money);
        }
    }
    
    private void Die()
    {
        if (_mouseHoldTime >= stamina)
        {
            isAlive = false;
            _mouseHoldTime = 0f;
            _animator.SetBool("isClimbing", false);
            playerMeshRenderer.material.color = Color.white;
            GameManager.Instance.UpdateGameState(GameState.UpgradeState);
            UpdateMoney?.Invoke(_money);
        }
    }
    
    private void IsMouseButtonDown()
    {
        if (Input.GetKey(KeyCode.Mouse0) )
        {
            _animator.SetBool("isClimbing", true);
            _spawnTime += Time.deltaTime;
            _mouseHoldTime += Time.deltaTime * (staminaMultiplier + _mouseClickCount) ;
        }
    }

    private void IsMouseButtonUp()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            _animator.SetBool("isClimbing", false);
            _spawnTime = 0;
            
            if (maxMouseClickCount > _mouseClickCount)
            {
                _mouseClickCount += 1;
            }
        }

        if (!Input.GetKey(KeyCode.Mouse0))
        {
            if (_mouseHoldTime > 0)
            {
                _mouseHoldTime -= Time.deltaTime * (staminaMultiplier + _mouseClickCount);
            }
            else if (_mouseHoldTime < 0)
            {
                _mouseHoldTime = 0;
            }
        }
    }

    private void StaminaColor()
    {
        if (_mouseHoldTime > stamina * 0.75)
        {
            playerMeshRenderer.material.color = Color.red;
        }else if (_mouseHoldTime > stamina / 2)
        {
            playerMeshRenderer.material.color =
                Color.Lerp(Color.white, Color.red, Mathf.PingPong(Time.time, 1));
        }else if (_mouseHoldTime < stamina / 2)
        {
            playerMeshRenderer.material.color = Color.white;
        }
    }

    public void UpgradeProperties(Upgrades upgrade, float cost)
    {
        switch (upgrade)
        {
            case Upgrades.Stamina:
                stamina *= 1.25f;
                _money -= cost;
                _money = Mathf.Round(_money * 100f) / 100f;
                break;
            case Upgrades.MoneyMagnitude:
                _moneyMagnitude *= 1.2f;
                _money -= cost;
                _money = Mathf.Round(_money * 100f) / 100f;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(upgrade), upgrade, null);
        }
        UpdateMoney?.Invoke(_money);
    }
}
