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

    public string SaveDateText { set { _saveDateText.text = value; } }
    public string FoodAmountText { set { _foodAmountText.text = value; } }
    public string GoldAmountText { set { _goldAmountText.text = value; } }
    public string EntitiesAmountText { set { _entitiesAmountText.text = value; } }
    public string currentTurnText { set { _currentTurnText.text = value; } }
}
