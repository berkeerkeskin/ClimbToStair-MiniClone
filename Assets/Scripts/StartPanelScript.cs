using DG.Tweening;
using TMPro;
using UnityEngine;

public class StartPanelScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI startText;

    private void Start()
    {
        ShakeText();
    }

    void Update()
    {
        ChangeState();
    }

    private void ChangeState()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UnitManager.Instance.StartState();
        }
    }
    
    private void ShakeText()
    {
        startText.transform.DORotate(Vector3.forward * 15f, 1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        startText.transform.DOScale(Vector3.one * 2f, 1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }
}
