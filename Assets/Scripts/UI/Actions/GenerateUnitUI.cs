using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GenerateUnitUI : MonoBehaviour
{
    [SerializeField]
    private Button[] _generateUnitButtons;

    [SerializeField]
    private InputManager _inputManager;

    public void SetButtons(Building building)
    {
        // COMPROBAR QUE EL EDIFICIO TENGA EL COMPONENTE UnitGenerator
        UnitGenerator unitGenerator = building.gameObject.GetComponent<UnitGenerator>();

        int foodAmount = GameManager.Instance.FoodResources[GameManager.Instance.PlayerTeam];
        int goldAmount = GameManager.Instance.GoldResources[GameManager.Instance.PlayerTeam];

        UnitInfoSO unitInfo = null;
        for (int i = 0; i < _generateUnitButtons.Length; i++)
        {
            if (i >= unitGenerator.UnitsInfo.Length)
            {
                _generateUnitButtons[i].gameObject.SetActive(false);
                continue;
            }

            unitInfo = unitGenerator.UnitsInfo[i];

            _generateUnitButtons[i].gameObject.SetActive(true);
            _generateUnitButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = unitInfo.Entity.name;

            if (foodAmount < unitInfo.FoodAmount || goldAmount < unitInfo.FoodAmount)
            {
                _generateUnitButtons[i].interactable = false;
                continue;
            }

            _generateUnitButtons[i].onClick.AddListener(
                () => _inputManager.SetGenerateUnitState(unitInfo)
            );
        }
    }
}
