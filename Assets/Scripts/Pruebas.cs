using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;

public class pruebas : MonoBehaviour
{

    [ContextMenu("Pruebas")]
    public void Pruebas()
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();

        dict["targetPosition"] = new Vector3(0,0,0);

        Vector3? pos = dict["targetPosition"] as Vector3?;

        if (pos == null)
        {
            Debug.Log("Valor: null");
        }
        else
        {
            Debug.Log($"Valor: {pos.Value}");
        }

        
    }
}
