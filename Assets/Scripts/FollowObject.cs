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
            transform.position = obj.transform.position;
        }
    }
}
