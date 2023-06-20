using UnityEngine;
using TMPro;

public class LoadGameButtonController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _saveDateText;
    [SerializeField]
    private TextMeshProUGUI _foodAmountText;
    [SerializeField]
    private TextMeshProUGUI _goldAmountText;
    [SerializeField]
    private TextMeshProUGUI _entitiesAmountText;
    [SerializeField]
    private TextMeshProUGUI _currentTurnText;

    public string SaveDateText { get { return _saveDateText.text; } set { _saveDateText.text = value; } }
    public string FoodAmountText { get { return _foodAmountText.text; } set { _foodAmountText.text = value; } }
    public string GoldAmountText { get { return _goldAmountText.text; } set { _goldAmountText.text = value; } }
    public string EntitiesAmountText { get { return _entitiesAmountText.text; } set { _entitiesAmountText.text = value; } }
    public string currentTurnText { get { return _currentTurnText.text; } set { _currentTurnText.text = value; } }
}
