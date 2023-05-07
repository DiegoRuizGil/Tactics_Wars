using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAManagerBuilder : IBuilder<IAManager>
{
    private readonly IAManager _manager;

    public IAManagerBuilder()
    {
        GameObject go = new GameObject();
        _manager = go.AddComponent<IAManager>();
    }

    public IAManager Build()
    {
        return _manager;
    }

    public static implicit operator IAManager(IAManagerBuilder builder)
    {
        return builder.Build();
    }
}
