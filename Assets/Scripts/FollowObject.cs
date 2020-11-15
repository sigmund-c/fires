using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public GameObject obj;

    void Update()
    {
        if (obj != null)
        {
            Vector3 objPos = obj.transform.position;
            transform.position = new Vector3(objPos.x, objPos.y, 0f);
        }
    }
}
