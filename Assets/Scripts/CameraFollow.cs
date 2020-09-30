using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    public Transform[] snaps;

    public float moveSpeed = 0.1f;
    public float snapDistance = 5f;

    private Transform target;

    // Update is called once per frame
    void LateUpdate()
    {
        if (player != null)
        {
            target = player;

            foreach (Transform snap in snaps)
            {
                if (Vector2.Distance(player.position, snap.position) < snapDistance)
                {
                    target = snap;
                }
            }

            transform.position = Vector3.MoveTowards(transform.position, target.position + new Vector3(0, 0, -10f), moveSpeed);
        }
    }
}
