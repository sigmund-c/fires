using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossActionType
{
    Idle,
    BigLaser,
    SpinLaser
}


public class AI_CrystalBoss : MonoBehaviour
{
    public BossActionType curState = BossActionType.Idle;

    public GameObject miniCrystalPrefab;
    public int crystalAmount = 6;
    public float orbitRadius = 5f;
    public float orbitSpeed = 70f;
    
    private List<MiniCrystal> miniCrystals;

    public bool stateRunning = false;

    private Transform[] guardPos;
    private Transform[] shieldPos;

    private float crystalMoveSpeed = 7f;

    // Start is called before the first frame update
    void Start()
    {
        miniCrystals = new List<MiniCrystal>();
        StartCoroutine(CreateMiniCrystals());
        guardPos = transform.Find("GuardPosManager").GetComponentsInChildren<Transform>();
        shieldPos = transform.Find("ShieldPosManager").GetComponentsInChildren<Transform>();
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
        SetAllCrystalSpeed(orbitSpeed);

        stateRunning = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (curState)
        {
            case BossActionType.Idle:
                HandleIdleState();
                break;

            case BossActionType.BigLaser:
                HandleBigLaserState();
                break;

            case BossActionType.SpinLaser:
                HandleSpinLaserState();
                break;
        }
    }

    private void HandleIdleState()
    {
        if (stateRunning)
        {
            return;
        }
        Debug.LogWarning("idleing");
        StartCoroutine(waitAndDecide());
    }

    IEnumerator waitAndDecide()
    {
        stateRunning = true;

        StartCoroutine(SpinWithEqualDistance());

        yield return new WaitForSeconds(2f);

        foreach(MiniCrystal mini in miniCrystals)
        {
            if (mini != null)
                mini.SwitchState(MiniCrystalAction.Spinning);
        }

        int choice = Random.Range(0, 2);
        switch (choice)
        {
            case 0:
                curState = BossActionType.BigLaser;
                break;

            case 1:
                curState = BossActionType.SpinLaser;
                break;
        }

        stateRunning = false;
    }

    private void HandleBigLaserState()
    {

        if (stateRunning)
        {
            return;
        }
        Debug.LogWarning("big lasering");
        StartCoroutine(BigLaser());

    }

    private IEnumerator BigLaser()
    {
        stateRunning = true;
        
        SetAllCrystalSpeed(0);

        float elapsedTime = 0f;
        float waitTime = 7f;
        
        while (elapsedTime < waitTime)
        {
            for (int i = 0; i < miniCrystals.Count; i++)
            {
                miniCrystals[i].transform.position = Vector3.MoveTowards(miniCrystals[i].transform.position, guardPos[i].position, crystalMoveSpeed * Time.deltaTime);
            }
            elapsedTime += Time.deltaTime;

            yield return null;
        }


        StartCoroutine(SpinWithEqualDistance());

        SetAllCrystalSpeed(orbitSpeed);

        curState = BossActionType.Idle;
        stateRunning = false;
    }

    private IEnumerator SpinWithEqualDistance()
    {
        UpdateRemainingCrystals();

        SetAllCrystalSpeed(0);

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
                miniCrystals[i].transform.position = Vector3.MoveTowards(miniCrystals[i].transform.position, pos[i], crystalMoveSpeed * 3 * Time.deltaTime);
            }
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        SetAllCrystalSpeed(orbitSpeed);
    }

    private void HandleSpinLaserState()
    {
        if (stateRunning)
        {
            return;
        }

        Debug.LogWarning("spinn");
        UpdateRemainingCrystals();
        StartCoroutine(SpinLaser());
    }

    // spin laser in one direction for 1 sec, then in the other for 1 sec, then spin regularly for 1 sec.
    private IEnumerator SpinLaser()
    {
        stateRunning = true;

        SetAllCrystalRadius(0);
        yield return new WaitForSeconds(.5f);

        SetAllCrystalRadius(orbitRadius);
        yield return new WaitForSeconds(.5f);

        float elapsedTime = 0;
        float waitTime = 5f;
        float attackSpeed = (0.2f + ((crystalAmount - miniCrystals.Count)/(float)crystalAmount * 0.8f)) * 1.5f * orbitSpeed; // faster as less crystals survive

        Debug.LogWarning(attackSpeed);

        while (elapsedTime < waitTime)
        {
            foreach (MiniCrystal mini in miniCrystals)
            {
                if (mini == null)
                {
                    continue;
                }
                // mini.SwitchState(MiniCrystalAction.Spinning);
                if (elapsedTime / waitTime < 0.5f) // accelerate
                {
                    mini.orbitSpeed = Mathf.Lerp(orbitSpeed, attackSpeed, elapsedTime / waitTime * 2);
                    Debug.Log(elapsedTime / waitTime * 2);
                } else if (elapsedTime / waitTime > 0.5f) // deccelerate
                {
                    mini.orbitSpeed = Mathf.Lerp(attackSpeed, 0, elapsedTime / waitTime * 2 - 1);
                    Debug.Log(elapsedTime / waitTime * 2 - 1);
                }
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }


        // spin in opposite direction
        elapsedTime = 0f;
        while (elapsedTime < waitTime)
        {
            foreach (MiniCrystal mini in miniCrystals)
            {
                if (mini == null)
                {
                    continue;
                }
                if (elapsedTime / waitTime < 0.5f) // accelerate
                {
                    mini.orbitSpeed = Mathf.Lerp(0, -attackSpeed, elapsedTime / waitTime * 2);
                }
                else if (elapsedTime / waitTime > 0.5f) // deccelerate
                {
                    mini.orbitSpeed = Mathf.Lerp(-attackSpeed, orbitSpeed, elapsedTime / waitTime * 2 - 1f);
                }
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(1);

        curState = BossActionType.Idle;

        stateRunning = false;
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

    private void SetAllCrystalSpeed(float newSpeed)
    {
        foreach (MiniCrystal mini in miniCrystals)
        {
            if (mini != null)
            {
                mini.setRadius(newSpeed);
            }
        }
    }

    private void UpdateRemainingCrystals()
    {
        miniCrystals.RemoveAll(item => item == null);
    }
}
