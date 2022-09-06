using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMapUpdate : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        MapManager.managerInstance.updateCurrentMap(transform.parent.parent.gameObject);
        //Debug.Log("Load new stuff");
    }
}
