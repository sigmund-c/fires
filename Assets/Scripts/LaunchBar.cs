using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchBar : MonoBehaviour
{
    private Transform bar;
    // Start is called before the first frame update
    void Start()
    {
        bar = transform.Find("Bar");
        bar.localScale = new Vector3(.4f, 1f);
    }

    public void SetSize(float sizeNormalized)
    {
        if (bar != null)
        {
            bar.localScale = new Vector3(sizeNormalized, 1f);
        }
    }
    
}
