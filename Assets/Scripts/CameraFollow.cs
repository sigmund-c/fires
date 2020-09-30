using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    public Transform[] snaps;
    public float[] snapDistances;

    private Transform target;
    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (player != null)
        {
            target = player;

            for (int i = 0; i < snaps.Length; i++)
            {
                float snapDistance = 5;
                if (i < snapDistances.Length)
                {
                    snapDistance = snapDistances[i];
                }
                if (Vector2.Distance(player.position, snaps[i].position) < snapDistance)
                {
                    target = snaps[i];
                    // cam.orthographicSize = snapDistance;
                }
            }

            transform.position = Vector3.MoveTowards(transform.position, target.position + new Vector3(0, 0, -10f), 0.1f);
        }
    }
}
