using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform barSpawnPoint;
    [SerializeField] private Transform originalStairSpawnPoint;

    [SerializeField] private GameObject barPrefab;
    [SerializeField] private GameObject stairPrefab;
    [SerializeField] private Transform environmentParent;

    [SerializeField] private Transform mainCamera;
    [SerializeField] private Transform target;

    [SerializeField] private TextMeshProUGUI moneyText;
    public Vector3 offset;
    
    private GameObject _barObject;
    private GameObject _stairObject;

    private Transform _tempStairSpawnPoint;
    private float _firstTimeBar;
    private float _stairCount;
    
    private float _playerTime;
    private float _InstantiateStairTime = 0.4f;

    private void Awake()
    {
        Player.UpdateMoney += ChangeMoneyText;
    }

    private void OnDestroy()
    {
        Player.UpdateMoney -= ChangeMoneyText;
    }

    void Start()
    {
        _firstTimeBar = 0;
        _stairCount = 0;
        _tempStairSpawnPoint = originalStairSpawnPoint;
    }
    
    void Update()
    {
        if (Player.Instance.isAlive)
        {
            IsMouseButtonDown();
            IsMouseButtonUp();
            SpawnBarAndStair();
        }
        else
        {
            _firstTimeBar = 0;
            _stairCount = 0;
            _tempStairSpawnPoint = originalStairSpawnPoint;
        }
    }
    //&& 
    private void FixedUpdate()
    {
        ArrangeCamera();
    }

    private void ArrangeCamera()
    {
        if (_barObject != null)
        {
            target = _barObject.transform.GetChild(0);
        }
        else if(_barObject == null)
        {
            target = barSpawnPoint;
        }
        mainCamera.transform.DOMove(target.position + offset, 0.3f);
    }
    private void IsMouseButtonDown()
    {
        if (Input.GetKey(KeyCode.Mouse0) )
        {
            _playerTime += Time.deltaTime;
        }
    }

    private void IsMouseButtonUp()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            _playerTime = 0;
        }
    }

    private void SpawnBarAndStair()
    {
        if (_playerTime > _InstantiateStairTime)
        {
            //Spawn the bar
            if (_firstTimeBar == 0)
            {
                _barObject = Instantiate(barPrefab, barSpawnPoint.position, Quaternion.identity);
                _barObject.transform.SetParent(environmentParent);
                _firstTimeBar += 1;
            }
            else
            {
                _barObject.transform.DOScale(_barObject.transform.localScale + Vector3.up * 0.15f, 0.2f).SetEase(Ease.OutBounce);
            }
            //Spawn the stair
            _stairObject = Instantiate(stairPrefab, _tempStairSpawnPoint.position, Quaternion.Euler(0f, -90f + 15f * _stairCount, 0f));
            _stairObject.transform.localScale = Vector3.zero;
            _stairObject.transform.DOScale(new Vector3(1.6327F, 0.15f, 0.6319121f), 0.2f).SetEase(Ease.OutBounce);
            _stairObject.transform.SetParent(environmentParent);
            _stairCount += 1;
            _tempStairSpawnPoint = _stairObject.transform.GetChild(0);
            
            _playerTime = 0;
        }
    }

    private void ChangeMoneyText(float money)
    {
        moneyText.text = "$" + money;
    }
}
