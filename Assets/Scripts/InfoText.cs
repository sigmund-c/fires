using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum InfoTextType { Plain, DamageText, PowerUpText }
public class InfoText : MonoBehaviour
{
    // static vars instead of prefabs so we don't create unnecessary copies

    // damageText
    public static float yOffset = 0.5f;
    public static float xNoise = 1f;
    public static float yNoise = 0.5f;
    
    // fading
    public static float fadeRiseSpeed = 0.02f;
    public static float fadeRiseDeccel = -0.0001f;

    public static float fadeStartDelay = 0.5f;
    public static float fadeTime = 0.4f;

    // no fading
    public static float riseSpeed = 0.035f;
    public static float riseDeccel = -0.0006f;

    // powerup text
    public static float powerupYOffset = 0.2f;
    public static float powerupRiseSpeed = 0.06f;
    public static float powerupRiseDeccel = -0.002f;
        
    public InfoTextType type;
    public string text;
    public Vector2 addedOffset;

    private TextMeshPro textMesh;
    private float speed;
    private Transform parent; // manually set instead of setting transform.parent to avoid inheriting rotation
    private Vector3 offsetFromParent; // for use if parented


    void Start()
    {
        textMesh = gameObject.GetComponent<TextMeshPro>();
        textMesh.text = text;

        if (type == InfoTextType.DamageText)
        {
            float randX = Random.Range(-xNoise/2, xNoise/2);
            float randY = Random.Range(-yNoise/2, yNoise/2);
            transform.position += new Vector3(randX + addedOffset.x, yOffset + randY + addedOffset.y, 0f);

            StartCoroutine(DamageTextDisappear());
            speed = riseSpeed;
        }
        else if (type == InfoTextType.PowerUpText)
        {
            transform.position += new Vector3(addedOffset.x, powerupYOffset + addedOffset.y, 0f);
            StartCoroutine(PowerupTextDisappear());
            speed = powerupRiseSpeed;
        }
        else
        {   
            textMesh.fontSize = 9;
            transform.position += (Vector3)addedOffset;
            StartCoroutine(DamageTextDisappear());
        }

        if (parent != null)
            offsetFromParent = transform.position - parent.position;       
    }


    void Update()
    {
        if (parent != null)
        {
            transform.position = parent.position + offsetFromParent;
            offsetFromParent += new Vector3(0, speed, 0);
        }
        else
        {
            transform.position += new Vector3(0, speed, 0);
        }
        
        if (speed > 0)
        {
            if (type == InfoTextType.DamageText)
                speed += riseDeccel;
            else if (type == InfoTextType.PowerUpText)
                speed += powerupRiseDeccel;
        }
        else
            speed = 0;        
    }

    public void SetParent(Transform parent)
    {
        this.parent = parent;
    }

    IEnumerator DamageTextDisappear(float time = 1f)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    IEnumerator DamageTextDisappearFade()
    {
        yield return new WaitForSeconds(0.5f);

        while (textMesh.color.a > 0)
        {
            textMesh.color += new Color(0, 0, 0, -0.1f);
            yield return new WaitForSeconds(fadeTime / 10);
        }
        Destroy(gameObject);
    }

    IEnumerator PowerupTextDisappear()
    {
        yield return new WaitForSeconds(0.8f);

        for (int i = 0; i < 3; i++)
        {
            textMesh.color = new Color(0f, 0f, 0f, 0f);
            yield return new WaitForSeconds(0.16f);
            textMesh.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(0.16f);
        }
        Destroy(gameObject);
    }
}
