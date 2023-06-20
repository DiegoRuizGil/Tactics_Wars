using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInitializerBuilder : IBuilder<SceneInitializer>
{
    private readonly SceneInitializer _sceneInitializer;

    public SceneInitializerBuilder()
    {
        GameObject go = new GameObject();
        _sceneInitializer = go.AddComponent<SceneInitializer>();
    }

    public SceneInitializerBuilder WithEntitiesPrefabs(EntitiesPrefabsSO entitiesPrefabs)
    {
        _sceneInitializer.EntitiesPrefabs = entitiesPrefabs;
        return this;
    }

    public SceneInitializerBuilder WithSaveToLoadSO(SaveToLoadSO saveToLoadSO)
    {
        _sceneInitializer.SaveToLoad = saveToLoadSO;
        return this;
    }

    public SceneInitializer Build()
    {
        return _sceneInitializer;
    }

    public static implicit operator SceneInitializer(SceneInitializerBuilder builder)
    {
        return builder.Build();
    }
}
