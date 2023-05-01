using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TopHUDManager : MonoBehaviour
{
    [Header("Team Icons UI")]
    [SerializeField] private Image _blueTeamIcon;
    [SerializeField] private Image _redTeamIcon;

    [Header("Resources Text")]
    [SerializeField] private TextMeshProUGUI _foodResourcesTextBlue;
    [SerializeField] private TextMeshProUGUI _goldResourcesTextBlue;
    [SerializeField] private TextMeshProUGUI _foodResourcesTextRed;
    [SerializeField] private TextMeshProUGUI _goldResourcesTextRed;

    [Header("Buttons UI")]
    [SerializeField] private Button _endTurnButton;

    public void UpdateHUD(TeamEnum team)
    {
        UpdateTeamIcon(team);
        UpdateResourcesAmount();
        SetInteracionEndTurn(team);
    }

    private void UpdateTeamIcon(TeamEnum team)
    {
        Color blueIconColor = _blueTeamIcon.color;
        Color redIconColor = _redTeamIcon.color;

        blueIconColor.a = team == TeamEnum.BLUE ? 1f : 0.25f;
        redIconColor.a = team == TeamEnum.RED ? 1f : 0.25f;

        _blueTeamIcon.color = blueIconColor;
        _redTeamIcon.color = redIconColor;
    }

    public void UpdateResourcesAmount()
    {
        //if (team != GameManager.Instance.PlayerTeam)
        //    return;

        _foodResourcesTextBlue.text = GameManager.Instance.FoodResources[TeamEnum.BLUE].ToString();
        _goldResourcesTextBlue.text = GameManager.Instance.GoldResources[TeamEnum.BLUE].ToString();

        _foodResourcesTextRed.text = GameManager.Instance.FoodResources[TeamEnum.RED].ToString();
        _goldResourcesTextRed.text = GameManager.Instance.GoldResources[TeamEnum.RED].ToString();
    }

    private void SetInteracionEndTurn(TeamEnum team)
    {
        _endTurnButton.interactable = team == GameManager.Instance.PlayerTeam;
    }
}
