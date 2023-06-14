#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(TeamEnumEvent))]
public class TeamEnumGameEventEditor : BaseGameEventEditor<TeamEnum, TeamEnumEvent> { }
#endif