using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TopHUDManager : MonoBehaviour
{
    [Header("Team Icons UI")]
    [SerializeField] private Image _blueTeamIcon;
    [SerializeField] private Image _redTeamIcon;

    [Header("Resources Text")]
    [SerializeField] private TextMeshProUGUI _foodResourcesText;
    [SerializeField] private TextMeshProUGUI _goldResourcesText;

    [Header("Buttons UI")]
    [SerializeField] private Button _endTurnButton;

    public void UpdateHUD(TeamEnum team)
    {
        UpdateTeamIcon(team);
        UpdateResourcesAmount(team);
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

    private void UpdateResourcesAmount(TeamEnum team)
    {
        if (team != GameManager.Instance.PlayerTeam)
            return;

        _foodResourcesText.text = GameManager.Instance.FoodResources[team].ToString();
        _goldResourcesText.text = GameManager.Instance.GoldResources[team].ToString();
    }

    private void SetInteracionEndTurn(TeamEnum team)
    {
        _endTurnButton.interactable = team == GameManager.Instance.PlayerTeam;
    }
}
