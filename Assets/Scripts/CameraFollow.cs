using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    public Transform[] snaps;
    public float[] snapDistances;

    public float cameraInitialSpeed;
    public float cameraMaxSpeed = 1f;
    public float cameraAccel;

    private bool hasOverrideTarget = false;
    private Vector3 overrideTarget;
    protected Vector3 target;
    protected Camera cam;

    private float shakeMagnitude;
    private Vector3 initialPosition;
    protected float shakeDuration = 0f;

    protected void Start()
    {
        cam = GetComponent<Camera>();

        player = GameObject.Find("Player").transform;
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;

            shakeDuration -= Time.deltaTime;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (shakeDuration <= 0)
        {
            DecideTarget();
            transform.position = Vector3.MoveTowards(transform.position, target + new Vector3(0, 0, -10f), cameraMaxSpeed);

        }
    }

    protected void DecideTarget()
    {
        if (hasOverrideTarget)
        {
            target = overrideTarget;
            return;
        }

        for (int i = 0; i < snaps.Length; i++)
        {
            float snapDistance = 5;
            if (i < snapDistances.Length)
            {
                snapDistance = snapDistances[i];
            }
            if (Vector2.Distance(player.position, snaps[i].position) < snapDistance)
            {
                target = snaps[i].position;
                //cam.orthographicSize = snapDistance;
                return;
            }
        }

        target = player.position;
        return;
    }

    public void FocusOn(Vector2 position, float duration)
    {
        overrideTarget = position;
        hasOverrideTarget = true;
        StartCoroutine(NullOverrideTarget(duration));
    }

    public void FocusOn(int cameraSnapIndex, float duration)
    {
        overrideTarget = snaps[cameraSnapIndex].position;
        hasOverrideTarget = true;
        StartCoroutine(NullOverrideTarget(duration));
    }

    IEnumerator NullOverrideTarget(float duration)
    {
        yield return new WaitForSeconds(duration);
        hasOverrideTarget = false;
    }

    public void TriggerShake(float magnitude, float duration)
    {
        initialPosition = transform.position;
        shakeMagnitude = magnitude;
        shakeDuration = duration;
    }
}
