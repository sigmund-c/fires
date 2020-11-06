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
    private BossActionType curState = BossActionType.Idle;
    public GameObject Player;
    private Transform playerTransform;
    public LaserController laserController;
    public GameObject miniCrystalPrefab;
    public int crystalAmount = 6;
    private MiniCrystal[] miniCrystals;
    private bool stateRunning;

    // Start is called before the first frame update
    void Start()
    {
        stateRunning = false;
        playerTransform = Player.GetComponent<Transform>();
        miniCrystals = new MiniCrystal[crystalAmount];
        StartCoroutine(CreateMiniCrystals());
    }

    IEnumerator CreateMiniCrystals()
    {
        for (int i = 0; i < crystalAmount; i++)
        {
            miniCrystals[i] = Instantiate(miniCrystalPrefab, new Vector3(0, 0.3f, 0), Quaternion.identity, transform).GetComponent<MiniCrystal>();
            miniCrystals[i].setRadius(5f);
            miniCrystals[i].orbitSpeed = 70f;
            yield return new WaitForSeconds(1f);
        }
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
                StartCoroutine(HandleBigLaserState());
                break;

            case BossActionType.SpinLaser:
                StartCoroutine(HandleSpinLaserState());
                break;
        }
    }

    private void HandleIdleState()
    {
        StartCoroutine(waitAndDecide());
    }

    IEnumerator waitAndDecide()
    {
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
    }

    private IEnumerator HandleBigLaserState()
    {
        stateRunning = true;
        //Start charging sequence
        Vector3 currentPlayerPos = playerTransform.position;
        //Fire at player
        laserController.ShootLaser(currentPlayerPos);

        yield return new WaitForSeconds(2f);
        curState = BossActionType.Idle;
        stateRunning = false;
    }

    private IEnumerator HandleSpinLaserState()
    {
        yield return new WaitForSeconds(2f);
    }
}
