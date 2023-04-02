using System.Collections;
using System.Collections.Generic;
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
        UnitGenerator unitGenerator = building.gameObject.GetComponent<UnitGenerator>();

        for (int i = 0; i < _generateUnitButtons.Length; i++)
        {
            if (i >= unitGenerator.UnitsInfo.Length)
            {
                Debug.Log($"Indice fuera del rango: {i}");
                _generateUnitButtons[i].gameObject.SetActive(false);
                continue;
            }

            Debug.Log($"Indice dentro del rango: {i}");
            _generateUnitButtons[i].gameObject.SetActive(true);
            _generateUnitButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = unitGenerator.UnitsInfo[i].Entity.name;

            UnitInfoSO unitInfo = unitGenerator.UnitsInfo[i];

            _generateUnitButtons[i].onClick.AddListener(
                () => _inputManager.SetGenerateUnitState(unitInfo)
            );
        }
    }

    public void Pruebas(int n)
    {
        Debug.Log($"Acción numero {n}");
    }
}
