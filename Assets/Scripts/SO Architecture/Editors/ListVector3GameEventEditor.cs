#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ListVector3Event))]
public class ListVector3GameEventEditor : BaseGameEventEditor<List<Vector3>, ListVector3Event> { }
#endif