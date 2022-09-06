using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Camera))]
public class FloatingOrigin : MonoBehaviour
{
    [SerializeField] float threshold;
    [SerializeField] MapManager mp;
    private void LateUpdate()
    {
        Vector3 campos = gameObject.transform.position;
        campos.y = 0f;

        if(campos.magnitude > threshold)
        {
            for(int x =0; x<SceneManager.sceneCount;x++)//Maybe unecessary but too lazy to change
            {
                GameObject[] rootObj = SceneManager.GetSceneAt(x).GetRootGameObjects();
                for(int y=0;y<rootObj.Length;y++)
                {                    
                    rootObj[y].transform.position -= new Vector3(0, 0, (int)campos.z);//single line so minus z axis only
                    
                }
            }
            mp.resetRegenCount();
        }
    }
}
