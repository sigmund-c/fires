using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossActionType
{
    Idle,
    BigLaser,
    SpinLaser,
    IcicleDrop,
    ScatterShot
}


public class AI_CrystalBoss : MonoBehaviour
{
    public BossActionType curState = BossActionType.Idle;

    public GameObject miniCrystalPrefab;
    public int crystalAmount = 6;
    public float orbitRadius = 5f;
    public float orbitSpeed = 70f;

    public MiniCrystalManager miniCrystals;
    private IcicleManager icicleManager;
    private bool stateRunning = false;
    private bool bossStarted = false;

    public GameObject Player;
    private Transform playerTransform;
    private Animator animator;
    private Damageable damageable;

    void Start()
    {
        miniCrystals = GetComponentInChildren<MiniCrystalManager>();
        icicleManager = GetComponentInChildren<IcicleManager>();
        animator = GetComponentInChildren<Animator>();
        damageable = GetComponent<Damageable>();

        Player = GameObject.Find("Player");
        playerTransform = Player.GetComponent<Transform>();

        damageable.immuneTo = Team.Player;
    }

    public void StartBoss()
    {
        if (!bossStarted)
        {
            bossStarted = true;
            StartCoroutine(CreateMiniCrystals());
        }
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
        StartCoroutine(Delay(0.3f));
        if (stateRunning || !bossStarted)
        {
            return;
        }
        StartCoroutine(Delay(0.3f));
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

            case BossActionType.IcicleDrop:
                HandleIcicleDropState();
                break;

            case BossActionType.ScatterShot:
                HandleScatterShotState();
                break;
        }
    }

    private void HandleIdleState()
    {
        StartCoroutine(Delay(0.3f));
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

        int choice = Random.Range(0, 4);
        switch (choice)
        {
            case 0:
                curState = BossActionType.BigLaser;
                break;

            case 1:
                curState = BossActionType.SpinLaser;
                break;

            case 2:
                curState = BossActionType.IcicleDrop;
                break;
            
            case 3:
                curState = BossActionType.ScatterShot;
                break;
        }

        stateRunning = false;
    }

    private void HandleBigLaserState()
    {
        StartCoroutine(Delay(0.3f));
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
        SetVulnerable();

        
        float elapsedTime = 0f;
        float waitTime = 8f;

        miniCrystals.SetGuarding(waitTime);
        miniCrystals.ShootBigLaser(waitTime);

        miniCrystals.FaceTowards(playerTransform.position);
        while (elapsedTime < waitTime)
        {
            miniCrystals.LerpTowards(playerTransform.position, orbitSpeed/100f);
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        
        miniCrystals.isSpinning = true;

        curState = BossActionType.Idle;
        SetInvulnerable();
        stateRunning = false;
    }
    

    private void HandleSpinLaserState()
    {
        StartCoroutine(Delay(0.3f));
        if (stateRunning)
        {
            return;
        }

        Debug.LogWarning("spinn");
        StartCoroutine(SpinLaser());
    }

    // spin laser in one direction for 5 sec, then in the other for 5 sec. + 1 sec buffer
    private IEnumerator SpinLaser()
    {
        stateRunning = true;
        SetVulnerable();

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

        miniCrystals.ShootSmallLasers(waitTime * 2);

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
                miniCrystals.orbitSpeed = Mathf.Lerp(-attackSpeed, -orbitSpeed, elapsedTime / waitTime * 2 - 1); ;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(1);

        curState = BossActionType.Idle;
        SetInvulnerable();

        stateRunning = false;
    }

    private void HandleIcicleDropState()
    {
        StartCoroutine(Delay(0.3f));
        if (stateRunning)
        {
            return;
        }
        Debug.LogWarning("icedrop");
        StartCoroutine(IcicleDrop());
    }

    private IEnumerator IcicleDrop()
    {
        stateRunning = true;
        SetVulnerable();

        yield return new WaitForSeconds(icicleManager.IceDrop());

        curState = BossActionType.Idle;
        SetInvulnerable();
        stateRunning = false;
    }

    private void HandleScatterShotState()
    {
        StartCoroutine(Delay(0.3f));
        if (stateRunning)
        {
            return;
        }
        Debug.LogWarning("scattershot");
        StartCoroutine(ScatterShot());
    }

    private IEnumerator ScatterShot()
    {
        stateRunning = true;
        SetVulnerable();
        StartCoroutine(Delay(2f));
        miniCrystals.SetScatter(3f);
        SetInvulnerable();
        yield return new WaitForSeconds(5f);
        
        miniCrystals.isSpinning = true;
        curState = BossActionType.Idle;
        stateRunning = false;
    }

    private IEnumerator Delay(float time)
    {
        yield return new WaitForSeconds(time);
    }


    public void SetVulnerable()
    {
        animator.SetTrigger("Vulnerable");
        StartCoroutine(VulnerableAfterAnim());
    }

    IEnumerator VulnerableAfterAnim()
    {
        yield return new WaitForSeconds(1);
        damageable.immuneTo = Team.None;
    }

    public void SetInvulnerable()
    {
        animator.SetTrigger("Invulnerable");
        StartCoroutine(InvulnerableAfterAnim());
    }

    IEnumerator InvulnerableAfterAnim()
    {
        yield return new WaitForSeconds(2);
        damageable.immuneTo = Team.Player;
    }
}
