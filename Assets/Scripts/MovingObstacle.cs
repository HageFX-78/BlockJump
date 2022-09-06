using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]float moveSpeed;
    int hmMultiplier;
    void Start()
    {
        //moveSpeed = 0.01f;
        StartCoroutine(moveSideToSide());
        if (PlayerPrefs.GetInt("hmActive")==1)
        {
            moveSpeed /=2;
        }

    }

    IEnumerator moveSideToSide()
    {
        float moveDir = 0.1f;
        while(true)
        {
            if(transform.position.x>3|| transform.position.x < -3)
            {
                moveDir = -moveDir;
            }

            transform.position += new Vector3(moveDir, 0,0);
            yield return new WaitForSeconds(moveSpeed);
        }
    }
}
