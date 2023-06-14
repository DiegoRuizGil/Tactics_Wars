#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(VoidEvent))]
public class VoidGameEventEditor : BaseGameEventEditor<Void, VoidEvent> { }
#endif