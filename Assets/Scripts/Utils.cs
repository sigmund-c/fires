using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum ParticleType { Fire, Ashes}
public class Utils : MonoBehaviour
{
    public static GameObject fireParticles = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/FireParticles.prefab", typeof(GameObject));
    public static GameObject ashParticles = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/AshParticles.prefab", typeof(GameObject));
    public static int burningLayer = LayerMask.NameToLayer("Burning");

    public static GameObject SpawnScaledParticleSystem(ParticleType type, Transform parent, float time = -1f)
    {
        GameObject prefab = type == ParticleType.Fire ? fireParticles : ashParticles;
        
        GameObject particles = Instantiate(prefab, parent);

        ParticleSystem ps = particles.GetComponent<ParticleSystem>();

        if (type == ParticleType.Fire && time != -1f)
        {
            ps.Stop();
            ParticleSystem.MainModule main = ps.main;
            main.duration = time;
            ps.Play();
        }

        ParticleSystem.EmissionModule emission = ps.emission;
        emission.rateOverTimeMultiplier = emission.rateOverTimeMultiplier * parent.localScale.x * parent.localScale.y;

        return particles;
    }
}
