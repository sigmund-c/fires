using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    [SerializeField] private float distanceRay = 100;
    public LineRenderer m_lineRenderer;
    Transform m_transform;

    private void Awake()
    {
        m_transform = GetComponent<Transform>();
    }

    private void Update()
    {
        ShootLaser();
    }

    void ShootLaser()
    {
        if(Physics2D.Raycast(m_transform.position, transform.right))
        {
            Debug.Log("LASER HIT");
            RaycastHit2D _hit = Physics2D.Raycast(m_transform.position, transform.right);
            Draw2DRay(m_transform.position, _hit.point);
        } else
        {
            Draw2DRay(m_transform.position, m_transform.transform.right * distanceRay);
        }
    }

    void Draw2DRay(Vector2 startPos, Vector2 endPos)
    {
        m_lineRenderer.SetPosition(0, startPos);
        m_lineRenderer.SetPosition(1, startPos);
    }
}
