#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(UnitEvent))]
public class UnitGameEventEditor : BaseGameEventEditor<Unit, UnitEvent> { }
#endif