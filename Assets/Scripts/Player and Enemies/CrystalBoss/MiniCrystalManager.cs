using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniCrystalManager : MonoBehaviour
{
    private int crystalAmount;
    private List<MiniCrystal> miniCrystals;
    public GameObject miniCrystalPrefab;
    public LaserController laserController;
    public Transform PlayerTransform;

    public float orbitRadius;
    public float orbitSpeed;

    public float crystalMoveSpeed = 7f;

    private bool stateRunning = false;
    public bool isSpinning = false;

    private Transform guardPos;
    private Transform[] guardPoses;
    private Transform shieldPos;
    private Transform[] shieldPoses;
    

    public void StartCrystals(int crystals, float orbitRadius, float orbitSpeed)
    {
        crystalAmount = crystals;
        this.orbitRadius = orbitRadius;
        this.orbitSpeed = orbitSpeed;

        miniCrystals = new List<MiniCrystal>();
        StartCoroutine(CreateMiniCrystals());
        isSpinning = true;

        guardPos = transform.Find("GuardPosManager");
        guardPoses = new Transform[guardPos.childCount];
        for (int i = 0; i < guardPos.childCount; i++)
        {
            guardPoses[i] = guardPos.GetChild(i);
        }
        shieldPos = transform.parent.Find("ShieldPosManager");
        shieldPoses = new Transform[shieldPos.childCount];
        for (int i = 0; i < shieldPos.childCount; i++)
        {
            shieldPoses[i] = shieldPos.GetChild(i);
        }
    }

    public int GetRemainingCrystals()
    {
        UpdateRemainingCrystals();
        return miniCrystals.Count;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSpinning)
        {
            transform.Rotate(0, 0, orbitSpeed * Time.deltaTime);
        }
    }


    IEnumerator CreateMiniCrystals()
    {
        stateRunning = true;

        for (int i = 0; i < crystalAmount; i++)
        {
            miniCrystals.Add(Instantiate(miniCrystalPrefab, new Vector3(0, -0.3f, 0), Quaternion.identity, transform).GetComponent<MiniCrystal>());
            miniCrystals[i].setRadius(orbitRadius);
            miniCrystals[i].orbitSpeed = orbitSpeed * 2;
            yield return new WaitForSeconds(0.5f);
        }

        stateRunning = false;
    }

    public void FaceTowards(Vector3 pos)
    {
        transform.up = pos - transform.position;
    }

    public void LerpTowards(Vector3 pos, float speed = -1)
    {
        if (speed == -1) speed = orbitSpeed / 8;
        transform.up = Vector3.MoveTowards(transform.up, pos - transform.position, speed * Time.deltaTime);
        //transform.up = transform.up + (pos - transform.up).normalized * speed * Time.deltaTime;
        /*
        if (Vector3.Dot(pos, transform.right) < 0)
        {
            transform.Rotate(0, 0, speed * 100 * Time.deltaTime);
        } else
        {
            transform.Rotate(0, 0, -speed * 100 * Time.deltaTime);
        }*/
    }

    public void SetEqualDistance()
    {
        StartCoroutine(EqualDistance());
    }

    private IEnumerator EqualDistance()
    {
        isSpinning = true;
        
        UpdateRemainingCrystals();

        Vector3[] pos = new Vector3[miniCrystals.Count];
        float sectionAngle = Mathf.PI * 2 / miniCrystals.Count;

        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = transform.position + orbitRadius * new Vector3(Mathf.Cos(sectionAngle * i), Mathf.Sin(sectionAngle * i));
        }

        float elapsedTime = 0f;
        float waitTime = 1f;

        while (elapsedTime < waitTime)
        {
            for (int i = 0; i < miniCrystals.Count; i++)
            {
                if (miniCrystals[i] == null) continue;
                miniCrystals[i].transform.position = Vector3.MoveTowards(miniCrystals[i].transform.position, pos[i], crystalMoveSpeed * 3 * Time.deltaTime);
            }
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        ResetRotation();

    }

    public void SetGuarding(float seconds)
    {
        StartCoroutine(Guarding(seconds));
    }

    private IEnumerator Guarding(float seconds)
    {
        isSpinning = false;

        float elapsedTime = 0f;
        float waitTime = seconds;

        ShootBigLaser(seconds);

        while (elapsedTime < waitTime)
        {
            for (int i = 0; i < miniCrystals.Count; i++)
            {
                if (miniCrystals[i] == null) continue;
                miniCrystals[i].transform.position = Vector3.MoveTowards(miniCrystals[i].transform.position, guardPoses[i].position, crystalMoveSpeed * 3 * Time.deltaTime);
            }
            ResetRotation();
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        SetEqualDistance();
    }

    public void ShootBigLaser(float seconds)
    {
        laserController.ShootLaser(Vector3.up, seconds);
    }

    public void ShootSmallLasers(float seconds)
    {
        UpdateRemainingCrystals();

        for (int i = 0; i < miniCrystals.Count; i++)
        {
            // every other mini crystal
            if (i % 2 == 1 || miniCrystals[i] == null) continue;
            miniCrystals[i].ShootLaser(seconds);
            miniCrystals[i].VulnerableFor(seconds);
        }
    }
    
    private void UpdateRemainingCrystals()
    {
        miniCrystals.RemoveAll(item => item == null);
    }

    private void ResetRotation()
    {
        // reset rotation
        for (int i = 0; i < miniCrystals.Count; i++)
        {
            if (miniCrystals[i] == null) continue;
            miniCrystals[i].transform.up = miniCrystals[i].transform.position - transform.position;
        }
    }

    private void SetAllCrystalRadius(float newRadius)
    {
        foreach (MiniCrystal mini in miniCrystals)
        {
            if (mini != null)
            {
                mini.setRadius(newRadius);
            }
        }
    }
}
