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

    public void UpdateHealthbar(float amount)
    {
        _target = amount;
    }

    private void Update()
    {
        _healthbar.fillAmount = Mathf.MoveTowards(_healthbar.fillAmount, _target, _reduceSpeed * Time.deltaTime);
        if (_healthbar.fillAmount == 0f)
        {
            gameObject.SetActive(false);
            _onDeathEvent?.Invoke();
        } 
    }
}
