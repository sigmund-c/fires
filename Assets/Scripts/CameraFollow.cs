using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow instance = null;

    public GameObject player;

    public float initialSmoothTime = 0.5f;
    public float smoothTime = 0f; // must start from 0, else it won't stick to the player when spawning from checkpoint

    private bool hasOverrideTarget = false;
    public GameObject target; // regular target - either player or triggers
    private Vector3 overrideTarget; // for cutscenes

    private Vector3 currentVelocity = Vector3.zero;

    protected Camera cam;

    private float shakeMagnitude;
    private Vector3 initialPosition;
    protected float shakeDuration = 0f;

    public bool waiting = true;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    protected void Start()
    {
        cam = GetComponent<Camera>();

        player = GameObject.Find("Player"); //.transform;
        target = player;
        StartCoroutine(FirstTimeDelay()); // prevent camera from starting at player, before the first camera trigger fires
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;

            shakeDuration -= Time.deltaTime;
        }
    }


    void LateUpdate()
    {
        if (waiting)
        {
            return;
        }

        if (shakeDuration <= 0)
        {           
            Vector3 currTarget = hasOverrideTarget ? overrideTarget : target.transform.position;
            transform.position = Vector3.SmoothDamp(transform.position, currTarget + new Vector3(0, 0, -10f), ref currentVelocity, smoothTime);
        }
    }

    public void FocusOn(Vector3 overrideTarget, float duration)
    {
        this.overrideTarget = overrideTarget;
        hasOverrideTarget = true;
        smoothTime = 0.3f;
        StartCoroutine(NullOverrideTarget(duration));
    }

    IEnumerator NullOverrideTarget(float duration)
    {
        yield return new WaitForSeconds(duration);
        hasOverrideTarget = false;
        StartCoroutine("DecreaseSmoothTime");
    }

    public void TriggerShake(float magnitude, float duration)
    {
        initialPosition = transform.position;
        shakeMagnitude = magnitude;
        shakeDuration = duration;
    }

    public void UpdateTarget(GameObject target, bool entered, float cameraSize = 8f)
    {
        if (entered)
        {
            print("Target set to " + target);
            this.target = target;
            if (cameraSize != cam.orthographicSize)
            {
                StartCoroutine(ChangeSize(cameraSize, 1f, 0.5f));
            }
        }
        else
        {
            print("Left " + target);
            this.target = player;
        }

        StopCoroutine("DecreaseSmoothTime");
        smoothTime = initialSmoothTime;
        StartCoroutine("DecreaseSmoothTime");
    }

    // decrease smooth time, i.e. increase speed, until camera sticks to target
    IEnumerator DecreaseSmoothTime()
    {
        while (smoothTime > 0f)
        {
            smoothTime -= 0.025f;
            yield return new WaitForSeconds(0.07f);
        }
        smoothTime = 0f; // deal with floating point errors
    }

    IEnumerator FirstTimeDelay()
    {
        yield return new WaitForSeconds(0.1f);
        waiting = false;
    }

    IEnumerator ChangeSize(float size, float timeToChange, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        float curr = cam.orthographicSize;

        int steps = 50;
        float interval = timeToChange / 50;
        for (int i = 1; i <= steps; i++)
        {
            cam.orthographicSize = Mathf.Lerp(curr, size, interval * i);
            yield return new WaitForSeconds(timeToChange / steps);
        }
    }
}
