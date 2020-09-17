using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    public Transform[] snaps;

    private Transform target;

    // Update is called once per frame
    void LateUpdate()
    {
        target = player;

        foreach(Transform snap in snaps) {
            if (Vector2.Distance(player.position, snap.position) < 5)
            {
                target = snap;
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, target.position + new Vector3(0,0, -10f), 0.1f);
    }
}
