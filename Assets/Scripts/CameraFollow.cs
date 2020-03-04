using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public GameObject targetObj;
    private float distToTarget;

    // Start is called before the first frame update
    void Start()
    {

        // get distance between camera and player starting positions
        distToTarget = transform.position.x - targetObj.transform.position.x;

    }

    // Update is called once per frame
    void Update()
    {

        // get x co-ordinate of target object (player) and move camera to that position
        float targetObjX = targetObj.transform.position.x;
        Vector3 newCamPos = transform.position;
        newCamPos.x = targetObjX + distToTarget;
        transform.position = newCamPos;

    }

}
