public static class A
{
    public static NodeBuilder Node => new NodeBuilder();
    public static GridManagerBuilder GridManager => new GridManagerBuilder();
    public static BuildingBuilder Building => new BuildingBuilder();
    public static BuildingInfoSOBuilder BuildingInfoSO => new BuildingInfoSOBuilder();
    public static GameManagerBuilder GameManager => new GameManagerBuilder();
}

public static class An
{
    public static UnitBuilder Unit => new UnitBuilder();
    public static InputManagerBuilder InputManager => new InputManagerBuilder();
    public static UnitInfoSOBuilder UnitInfoSO => new UnitInfoSOBuilder();
    public static IAManagerBuilder IAManager => new IAManagerBuilder();
}
