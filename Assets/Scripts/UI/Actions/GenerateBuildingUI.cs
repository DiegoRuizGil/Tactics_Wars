using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GenerateBuildingUI : MonoBehaviour
{
    [SerializeField]
    private Button[] _generateBuildingButtons;

    [SerializeField]
    private InputManager _inputManager;

    public void SetButtons()
    {
        Unit unit = _inputManager.SelectedUnit;
        if (unit == null)
            return;

        BuildingGenerator buildingGenerator = unit.gameObject.GetComponent<BuildingGenerator>();
        if (buildingGenerator == null)
            return;

        List<BuildingInfoSO> buildingsToBuild = buildingGenerator.GetBuildingsToBuild();

        int foodAmount = GameManager.Instance.FoodResources[GameManager.Instance.PlayerTeam];
        int goldAmount = GameManager.Instance.GoldResources[GameManager.Instance.PlayerTeam];

        // BuildingInfoSO buildingInfo;
        for (int i = 0; i < _generateBuildingButtons.Length; i++)
        {
            if (i >= buildingsToBuild.Count)
            {
                _generateBuildingButtons[i].gameObject.SetActive(false);
                continue;
            }

            BuildingInfoSO buildingInfo = buildingsToBuild[i];

            _generateBuildingButtons[i].gameObject.SetActive(true);
            _generateBuildingButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = buildingInfo.Entity.name;

            if (foodAmount < buildingInfo.FoodAmount || goldAmount < buildingInfo.FoodAmount)
            {
                _generateBuildingButtons[i].interactable = false;
                continue;
            }

            _generateBuildingButtons[i].onClick.RemoveAllListeners();
            _generateBuildingButtons[i].onClick.AddListener(
                () => _inputManager.SetBuildState(buildingInfo)
            );
        }
    }
}
