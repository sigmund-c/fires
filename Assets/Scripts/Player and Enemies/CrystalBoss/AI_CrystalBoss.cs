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

    public GameObject miniCrystalPrefab;
    public int crystalAmount = 6;
    public float orbitRadius = 5f;
    public float orbitSpeed = 70f;
    private MiniCrystal[] miniCrystals;

    // Start is called before the first frame update
    void Start()
    {
        miniCrystals = new MiniCrystal[crystalAmount];
        StartCoroutine(CreateMiniCrystals());
    }

    IEnumerator CreateMiniCrystals()
    {
        for (int i = 0; i < crystalAmount; i++)
        {
            miniCrystals[i] = Instantiate(miniCrystalPrefab, new Vector3(0, -0.3f, 0), Quaternion.identity, transform).GetComponent<MiniCrystal>();
            miniCrystals[i].setRadius(orbitRadius);
            miniCrystals[i].orbitSpeed = orbitSpeed;
            yield return new WaitForSeconds(1f);
        }
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

        int choice = Random.Range(0, 1);
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

    private void HandleBigLaserState()
    {

    }

    private void HandleSpinLaserState()
    {

    }
}
