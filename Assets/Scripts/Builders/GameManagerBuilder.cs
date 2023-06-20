using UnityEngine;

public class GameManagerBuilder : IBuilder<GameManager>
{
    private readonly GameManager _gameManager;

    public GameManagerBuilder()
    {
        GameObject go = new GameObject();
        _gameManager = go.AddComponent<GameManager>();
    }

    public GameManagerBuilder WithUnitParent(TeamEnum team, Transform parent)
    {
        _gameManager.SetUnitParent(team, parent);
        return this;
    }

    public GameManagerBuilder WithBuildingParent(TeamEnum team, Transform parent)
    {
        _gameManager.SetBuildingParent(team, parent);
        return this;
    }

    public GameManagerBuilder WithFoodAmount(TeamEnum team, int amount)
    {
        _gameManager.UpdateResource(team, ResourceType.FOOD, amount);
        return this;
    }

    public GameManagerBuilder WithGoldAmount(TeamEnum team, int amount)
    {
        _gameManager.UpdateResource(team, ResourceType.GOLD, amount);
        return this;
    }

    public GameManager Build()
    {
        return _gameManager;
    }

    public static implicit operator GameManager(GameManagerBuilder builder)
    {
        return builder.Build();
    }
}
