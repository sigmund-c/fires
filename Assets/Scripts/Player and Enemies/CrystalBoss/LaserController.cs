using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Experimental.Rendering.LWRP;

public class LaserController : MonoBehaviour
{
    public float RayDistance = 80f;
    public float laserTriggerDuration = 3.0f;
    public float laserChargingDuration = 1.5f;

    public UnityEngine.Rendering.VolumeProfile volumeProfile;
    public float bloomIntensity = 5.0f;
    private float bloomIntensityDefault;
    private Bloom bloom;
    private Collider2D col;

    LineRenderer m_lineRenderer;
    Transform m_transform;
    new ParticleSystem particleSystem;
    new UnityEngine.Experimental.Rendering.Universal.Light2D light;
    private Vector3 laserTargetPos;
    private bool laserTriggered;
    private float laserTriggerTimer;
    private float laserFireTime;

    public LayerMask ignoreLayer;
    public AudioClip chargingLaser;
    public AudioClip shootingLaser;
    private AudioSource audioSource;
    private bool laserAudioed = false;

    private void Awake()
    {
        laserTriggered = false;
        volumeProfile.TryGet(out bloom);
        bloomIntensityDefault = bloom.intensity.value;
        m_transform = GetComponent<Transform>();
        m_lineRenderer = GetComponent<LineRenderer>();
        particleSystem = GetComponent<ParticleSystem>();
        light = GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>();
        col = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (laserTriggered)
        {
            if (Time.time > laserFireTime)
            {
                if (!laserAudioed)
                {
                    laserAudioed = true;
                    audioSource.clip = shootingLaser;
                    audioSource.Play();
                }
                col.enabled = true;
                if (!particleSystem.isPlaying)
                {
                    particleSystem.Play();
                    bloom.intensity.value = bloomIntensity;
                    light.enabled = true;
                }
                DrawRayAndParticles(Vector3.zero, laserTargetPos);

                // RaycastHit2D hit = Physics2D.Raycast(m_transform.position, (laserTargetPos-m_transform.position).normalized, RayDistance, ~ignoreLayer);
                // if(hit && hit.collider.gameObject.tag == "Player")
                // {
                //     PlayerDamageable player = hit.collider.gameObject.GetComponent<PlayerDamageable>();
                //     player.TakeDamage(1);
                // }

                laserTriggerTimer -= Time.deltaTime;
                if(laserTriggerTimer <= 0)
                {
                    laserTriggered = false;
                    col.enabled = false;
                    laserAudioed = false;
                    audioSource.Stop();
                }
            }
        } else {
            if(particleSystem.isPlaying)
            {
                particleSystem.Stop();
                particleSystem.Clear();
                bloom.intensity.value = bloomIntensityDefault;
                m_lineRenderer.positionCount = 0;
                light.enabled = false;
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
        Vector3 direction = target;
        laserTargetPos = direction.normalized * RayDistance;

        //DELAY FOR CHARGING
        // StartCoroutine(Delay(2f));
        laserFireTime = Time.time + laserChargingDuration;

        laserTriggered = true;
        laserTriggerTimer = laserTriggerDuration;
        audioSource.clip = chargingLaser;
        audioSource.Play();
    }

    public void ShootLaser(Vector3 target, float dur)
    {
        Vector3 direction = target;
        laserTargetPos = direction.normalized * RayDistance;

        //DELAY FOR CHARGING
        // StartCoroutine(Delay(2f));
        laserFireTime = Time.time + laserChargingDuration;

        laserTriggered = true;
        laserTriggerTimer = dur - 2f;
        audioSource.clip = chargingLaser;
        audioSource.Play();
    }

    private IEnumerator Delay(float time)
    {
         yield return new WaitForSeconds(time);
    }
}
