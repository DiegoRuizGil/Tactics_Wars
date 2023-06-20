#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(IntEvent))]
public class IntGameEventEditor : BaseGameEventEditor<int, IntEvent> { }
#endif