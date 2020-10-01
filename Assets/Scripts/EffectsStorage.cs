using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsStorage : MonoBehaviour
{
    public List<GameObject> effects;


    public void PlayEffect(int index)
    {
        Instantiate(effects[index], transform.position, Quaternion.identity);
    }
}
