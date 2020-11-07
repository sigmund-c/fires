using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LaserController : MonoBehaviour
{
    public float RayDistance = 80f;
    public float laserTriggerDuration = 3.0f;

    public UnityEngine.Rendering.VolumeProfile volumeProfile;
    public float bloomIntensity;
    private float bloomIntensityDefault;
    private Bloom bloom;

    LineRenderer m_lineRenderer;
    Transform m_transform;
    ParticleSystem particleSystem;
    private Vector3 laserTargetPos;
    private bool laserTriggered;
    private float laserTriggerTimer;

    public LayerMask ignoreLayer;

    private void Awake()
    {
        laserTriggered = false;
        volumeProfile.TryGet(out bloom);
        bloomIntensityDefault = bloom.intensity.value;
        m_transform = GetComponent<Transform>();
        m_lineRenderer = GetComponent<LineRenderer>();
        particleSystem = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (laserTriggered)
        {
            if(!particleSystem.isPlaying)
            {
                particleSystem.Play();
                bloom.intensity.value = bloomIntensity;
            }
            DrawRayAndParticles(m_transform.position, laserTargetPos);

            RaycastHit2D hit = Physics2D.Raycast(m_transform.position, (laserTargetPos-m_transform.position).normalized, RayDistance, ~ignoreLayer);
            if(hit && hit.collider.gameObject.tag == "Player")
            {
                PlayerDamageable player = hit.collider.gameObject.GetComponent<PlayerDamageable>();
                player.TakeDamage(1);
            }

            laserTriggerTimer -= Time.deltaTime;
            if(laserTriggerTimer <= 0)
            {
                laserTriggered = false;
            }
        } else {
            if(particleSystem.isPlaying)
            {
                particleSystem.Stop();
                bloom.intensity.value = bloomIntensityDefault;
                m_lineRenderer.positionCount = 0;
            }
        }
    }

    void DrawRayAndParticles(Vector3 startPos, Vector3 endPos)
    {
        //Draw Line
        m_lineRenderer.positionCount = 2;
        m_lineRenderer.SetPosition(0, startPos);
        m_lineRenderer.SetPosition(1, endPos);

        //Particle System direction (rotation)
        Vector3 relativePos = endPos - startPos;
        ParticleSystem.ShapeModule shp = particleSystem.shape;
        Vector3 rot = Quaternion.LookRotation(relativePos, Vector3.up).eulerAngles;
        shp.rotation = new Vector3(rot.x, rot.y, 0f);
    }

    public void ShootLaser(Vector3 target)
    {
        Vector3 direction = target - m_transform.position;
        laserTargetPos = direction.normalized * RayDistance;

        //DELAY FOR CHARGING
        StartCoroutine(Delay(2f));

        laserTriggered = true;
        laserTriggerTimer = laserTriggerDuration;
    }

    private IEnumerator Delay(float time)
    {
         yield return new WaitForSeconds(time);
    }
}
