using System.IO;
using UnityEngine;

public class SaveToLoadSOBuilder : IBuilder<SaveToLoadSO>
{
    private readonly SaveToLoadSO _so;

    public SaveToLoadSOBuilder()
    {
        _so = ScriptableObject.CreateInstance<SaveToLoadSO>();
    }

    public SaveToLoadSOBuilder IsNewGame(bool isNewGame)
    {
        _so.IsNewGame = isNewGame;
        return this;
    }

    public SaveToLoadSOBuilder WithFileInfo(FileInfo file)
    {
        _so.SaveToLoad = file;
        return this;
    }

    public SaveToLoadSO Build()
    {
        return _so;
    }

    public static implicit operator SaveToLoadSO(SaveToLoadSOBuilder builder)
    {
        return builder.Build();
    }
}
