using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinFloat : MonoBehaviour
{
    [SerializeField] float floatTime;
    void Start()
    {
        //floatTime = 0.2f;
        StartCoroutine(floatAnim());
    }
    
    IEnumerator floatAnim()
    {
        while(true)
        {
            transform.Rotate(0,2.5f,0);
            yield return new WaitForSeconds(floatTime);
        }    
    }
}
