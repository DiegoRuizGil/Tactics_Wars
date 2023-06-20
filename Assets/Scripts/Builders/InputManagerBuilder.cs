using UnityEngine;

public class InputManagerBuilder : IBuilder<InputManager>
{
    private readonly InputManager _manager;

    public InputManagerBuilder()
    {
        GameObject go = new GameObject();
        _manager = go.AddComponent<InputManager>();
    }

    public InputManagerBuilder WithSelectedUnit(Unit unit)
    {
        _manager.SelectedUnit = unit;
        return this;
    }

    public InputManager Build()
    {
        return _manager;
    }

    public static implicit operator InputManager(InputManagerBuilder builder)
    {
        return builder.Build();
    }
}
