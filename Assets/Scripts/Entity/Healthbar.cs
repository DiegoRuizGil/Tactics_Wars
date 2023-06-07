using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Healthbar : MonoBehaviour
{
    [Header("Unity Events")]
    [SerializeField]
    private UnityEvent _onDeathEvent;

    [SerializeField]
    private Image _healthbar;
    [SerializeField]
    private float _reduceSpeed = 2f;
    private float _target = 1f;
    private bool _updateProgress;

    public void UpdateHealthbar(float amount, bool updateProgress)
    {
        _target = amount;
        _updateProgress = updateProgress;

        if (!updateProgress)
        {
            _healthbar.fillAmount = _target;
        }
    }

    private void Update()
    {
        if (_updateProgress)
        {
            _healthbar.fillAmount = Mathf.MoveTowards(_healthbar.fillAmount, _target, _reduceSpeed * Time.deltaTime);
        }

        if (_healthbar.fillAmount == 0f)
        {
            gameObject.SetActive(false);
            _onDeathEvent?.Invoke();
        } 
    }
}
