using UnityEngine;

public class GridManagerBuilder : IBuilder<GridManager>
{
    private readonly GridManager _manager;

    public GridManagerBuilder()
    {
        GameObject go = new GameObject();
        _manager = go.AddComponent<GridManager>();
    }

    public GridManager Build()
    {
        return _manager;
    }

    public static implicit operator GridManager(GridManagerBuilder builder)
    {
        return builder.Build();
    }
}
