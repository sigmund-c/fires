using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum ParticleType { Fire, Ashes}
public class Utils : MonoBehaviour
{
    public static GameObject fireParticles = Resources.Load<GameObject>("Prefabs/FireParticles");
    public static GameObject ashParticles = Resources.Load<GameObject>("Prefabs/AshParticles");
    public static GameObject infoText = Resources.Load<GameObject>("Prefabs/Text_DamageText");
    public static GameObject sparkle = Resources.Load<GameObject>("Prefabs/Sparkle");
    public static GameObject persistentManager = Resources.Load<GameObject>("Prefabs/PersistentManager");
    public static int burningLayer = LayerMask.NameToLayer("Burning");
    public static int charLayer = LayerMask.NameToLayer("Character");
    public static int charSwimmingLayer = LayerMask.NameToLayer("Character Swimming");
    // public static Camera cam = Camera.main; // better to cache it once in a single place -- doesn't work across scenes

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

    public static Vector3 MouseWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 10));
    }

    public static InfoText SpawnInfoText(Vector2 pos, string text, Vector2 offset = default(Vector2), InfoTextType type = InfoTextType.Plain)
    {
        InfoText inst = Instantiate(infoText, pos, Quaternion.identity).GetComponent<InfoText>();
        inst.text = text;
        inst.addedOffset = offset;
        inst.type = type;
        return inst;
    }

    public static InfoText SpawnInfoText(Transform parent, string text, Vector2 offset = default(Vector2), InfoTextType type = InfoTextType.Plain)
    {
        InfoText inst = Instantiate(infoText, parent.position, Quaternion.identity).GetComponent<InfoText>();
        inst.text = text;
        inst.addedOffset = offset;
        inst.type = type;
        inst.SetParent(parent); //cannot simply pass in parent to Instantiate, as it would follow the rotation as well
        return inst;
    }

    public static GameObject SpawnSparkle(Transform parent, Vector2 offset = default(Vector2))
    {
        GameObject inst = Instantiate(sparkle, parent);
        return inst;
    }
}
