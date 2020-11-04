using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LaserController : MonoBehaviour
{
    public float RayDistance = 100f;
    public UnityEngine.Rendering.VolumeProfile volumeProfile;
    public float bloomIntensity = 1.5f;
    private Bloom bloom;
    private ParticleSystem particleSystem;
    LineRenderer m_lineRenderer;
    Transform m_transform;

    private void Awake()
    {
        volumeProfile.TryGet(out bloom);
        m_transform = GetComponent<Transform>();
        m_lineRenderer = GetComponent<LineRenderer>();
        particleSystem = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        ShootLaser();
    }

    void Draw2DRay(Vector2 startPos, Vector2 endPos)
    {
        //Draw Line
        m_lineRenderer.SetPosition(0, startPos);
        m_lineRenderer.SetPosition(1, endPos);

        //Particle Direction (Velocity)
        ParticleSystem.VelocityOverLifetimeModule vel = particleSystem.velocityOverLifetime;
        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(startPos.x, startPos.y);
        curve.AddKey(endPos.x, endPos.y);
        ParticleSystem.MinMaxCurve minMaxCurve = new ParticleSystem.MinMaxCurve(1.0f, curve);
        vel.x = minMaxCurve;
        vel.y = minMaxCurve;
        vel.z = minMaxCurve;
    }

    public void ShootLaser()
    {
        //bloom.intensity.value = bloomIntensity;
        RaycastHit2D hit = Physics2D.Raycast(m_transform.position, transform.right);
        if(hit)
        {
            Draw2DRay(m_transform.position, hit.point);
        } else
        {
            Draw2DRay(m_transform.position, m_transform.transform.right * RayDistance);
        }
    }
}
