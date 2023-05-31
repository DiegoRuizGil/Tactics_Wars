using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSaveToLoadSO", menuName = "Save To Load")]
public class SaveToLoadSO : ScriptableObject
{
    [SerializeField] private FileInfo _saveToLoad;
    [SerializeField] private bool _isNewGame;

    public FileInfo SaveToLoad { get { return _saveToLoad; } set { _saveToLoad = value; } }
    public bool IsNewGame { get { return _isNewGame; } set { _isNewGame = value; } }
}
