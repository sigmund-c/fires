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

    public MiniCrystalManager miniCrystals;

    public bool stateRunning = false;

    private float crystalMoveSpeed = 7f;
    public GameObject Player;
    private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        miniCrystals = GetComponentInChildren<MiniCrystalManager>();

        Player = GameObject.Find("Player");
        playerTransform = Player.GetComponent<Transform>();

        StartCoroutine(CreateMiniCrystals());
    }

    IEnumerator CreateMiniCrystals()
    {
        stateRunning = true;

        miniCrystals.StartCrystals(crystalAmount, orbitRadius, orbitSpeed);
        yield return new WaitForSeconds(0.5f * crystalAmount);

        stateRunning = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (stateRunning)
        {
            return;
        }

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

        miniCrystals.SetEqualDistance();

        yield return new WaitForSeconds(2f);

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

        
        float elapsedTime = 0f;
        float waitTime = 7f;

        miniCrystals.SetGuarding(waitTime);
        miniCrystals.ShootBigLaser(waitTime);

        miniCrystals.FaceTowards(playerTransform.position);
        while (elapsedTime < waitTime)
        {
            miniCrystals.LerpTowards(playerTransform.position);
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        
        miniCrystals.isSpinning = true;

        curState = BossActionType.Idle;
        stateRunning = false;
    }
    

    private void HandleSpinLaserState()
    {
        if (stateRunning)
        {
            return;
        }

        Debug.LogWarning("spinn");
        StartCoroutine(SpinLaser());
    }

    // spin laser in one direction for 1 sec, then in the other for 1 sec, then spin regularly for 1 sec.
    private IEnumerator SpinLaser()
    {
        stateRunning = true;

        /*
        SetAllCrystalRadius(0);
        yield return new WaitForSeconds(.5f);

        SetAllCrystalRadius(orbitRadius);
        yield return new WaitForSeconds(.5f);
        */

        float elapsedTime = 0;
        float waitTime = 5f;
        float attackSpeed = (0.2f + ((crystalAmount - miniCrystals.GetRemainingCrystals())/(float)crystalAmount * 0.8f)) * 1.5f * orbitSpeed; // faster as less crystals survive

        Debug.LogWarning(attackSpeed);

        while (elapsedTime < waitTime)
        {
            if (elapsedTime / waitTime < 0.5f) // accelerate
            {
                miniCrystals.orbitSpeed = Mathf.Lerp(orbitSpeed, attackSpeed, elapsedTime / waitTime * 2); ;
            }
            else if (elapsedTime / waitTime > 0.5f) // deccelerate
            {
                miniCrystals.orbitSpeed = Mathf.Lerp(attackSpeed, 0, elapsedTime / waitTime * 2 - 1); ;
            }
                
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }


        // spin in opposite direction
        elapsedTime = 0f;
        while (elapsedTime < waitTime)
        {
            if (elapsedTime / waitTime < 0.5f) // accelerate
            {
                miniCrystals.orbitSpeed = Mathf.Lerp(0, -attackSpeed, elapsedTime / waitTime * 2); ;
            }
            else if (elapsedTime / waitTime > 0.5f) // deccelerate
            {
                miniCrystals.orbitSpeed = Mathf.Lerp(-attackSpeed, orbitSpeed, elapsedTime / waitTime * 2 - 1); ;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(1);

        curState = BossActionType.Idle;

        stateRunning = false;
    }

}
