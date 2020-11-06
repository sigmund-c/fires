    using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MiniCrystalAction
{
    Stop,
    Spinning,
    Guarding
}

public class MiniCrystal : MonoBehaviour
{
    public float orbitSpeed = 10f;
    public float orbitRadius = 0f; 

    private MiniCrystalAction curState = MiniCrystalAction.Spinning;
    private Vector2 parentPos;

    public float changeRadiusSpeed = 3f;
    private Transform spriteTransform;

    // Start is called before the first frame update
    void Start()
    {
        parentPos = transform.parent.position;

        spriteTransform = GetComponentInChildren<SpriteRenderer>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        //spriteTransform.rotation = Quaternion.Inverse(transform.rotation);

        switch (curState)
        {
            case MiniCrystalAction.Stop:
                break;

            case MiniCrystalAction.Spinning:
                HandleSpinState();
                break;

            case MiniCrystalAction.Guarding:
                HandleGuardState();
                break;
        }
    }

    private void HandleSpinState()
    {
        //transform.RotateAround(parentPos, Vector3.forward, orbitSpeed * Time.deltaTime);
        //spriteTransform.rotation = Quaternion.identity;
    }

    private void HandleGuardState()
    {

    }

    public void SwitchState(MiniCrystalAction state)
    {
        curState = state;
    }
    

    public void setRadius(float newRadius)
    {
        orbitRadius = newRadius;
        StartCoroutine(ChangeRadius());
    }

    IEnumerator ChangeRadius()
    {
        float distance = Vector2.Distance(transform.position, parentPos);
        while (Mathf.Abs(distance - orbitRadius) > 0.05f)
        {
            //Debug.Log(distance);
            if (distance > orbitRadius)
                transform.position = Vector2.MoveTowards(transform.position, parentPos, changeRadiusSpeed * Time.deltaTime);
            else
                transform.position = Vector2.MoveTowards(transform.position, parentPos, -changeRadiusSpeed * Time.deltaTime);

            distance = Vector2.Distance(transform.position, parentPos);
            yield return null;
        }

    }
}
