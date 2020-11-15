﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_BossSnake : MonoBehaviour
{
    public bool hasStarted = false;

    public GameObject projectilePrefab;
    public GameObject spawnIndicator;
    public GameObject deathEffect;
    public GameObject nextLevelCrystal;
    public List<GameObject> enemyPrefabs;
    public float spawnDelay = 1.5f;

    public GameObject healthUI;
    
    private Animator headAnim;
    private Animator tailAnim;

    private float idleTime = 2f;
    private float nextAttack = 0;
    
    public int shootAmount = 0;
    public int tailAmount = 0;
    public int spawnAmount = 0;

    private Transform headTransform;
    public Transform target;
    private float timeBetweenShots = 0.5f;
    private float nextShot;

    private float timeBetweenTails = 0.5f;
    private float nextTail;
    private List<int> tails;
    private List<Animator> tailAnimators = new List<Animator>();

    private float nextSpawn;
    private GameObject activeSpawnIndicator;
    private List<Transform> spawnTransforms = new List<Transform>();

    private EffectsStorage effectsStorage;
    BossDamageable headDamageable;

    // Start is called before the first frame update
    void Start()
    {
        headTransform = transform.Find("Snake").Find("SnakeHead");
        headDamageable = headTransform.GetComponent<BossDamageable>();

        Transform tailAttacks = transform.Find("tailAttacks");
        foreach (Transform child in tailAttacks)
        {
            tailAnimators.Add(child.GetComponentInChildren<Animator>());
        }

        Transform spawnLocations = transform.Find("spawnLocations");
        foreach (Transform child in spawnLocations)
        {
            spawnTransforms.Add(child);
        }

        headAnim = headTransform.GetComponent<Animator>();
        tailAnim = transform.Find("Snake").Find("SnakeTailSprite").GetComponent<Animator>();

        effectsStorage = GetComponent<EffectsStorage>();
    }

    public void StartAI()
    {
        if (!hasStarted)
        {
            hasStarted = true;
            nextAttack = idleTime;
            healthUI.SetActive(true);

            spawnAmount = 3; //spawns 3 enemies at start
            nextSpawn = spawnDelay;
        }
    }

    public void StopAI()
    {
        if (hasStarted)
        {
            hasStarted = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hasStarted)
        {
            DoAI();
        }    
    }

    private void DoAI()
    {
        if (nextAttack <= 0)
        {
            GenerateNewAttack();
            nextAttack = idleTime;
        } else
        {
            if (shootAmount > 0)
            {
                AIShoot();
            }
            if (tailAmount > 0)
            {
                AITail();
            }
            if (spawnAmount > 0)
            {
                AISpawn();
            }
            
            if (shootAmount <= 0 && tailAmount <= 0 && spawnAmount <= 0)
            {
                nextAttack -= Time.deltaTime;

                if (activeSpawnIndicator != null) // destroy spawn indicator created from parallel runs
                {
                    Destroy(activeSpawnIndicator);
                }
            }
        }
        
    }

    private void GenerateNewAttack()
    {
        //Debug.LogWarning("generating new attack");
        if ((double)headDamageable.currHealth / headDamageable.maxHealth > 0f) // change to have different "Phases"
        {
            int choice = Random.Range(0, 3);
            if (choice == 0)
            {
                //Debug.LogWarning("shooting");
                shootAmount = Random.Range(2, 5);
                nextShot = timeBetweenShots;
                tailAmount = 0;
                spawnAmount = 0;
            } else if (choice == 1)
            {
                //Debug.LogWarning("tailing");
                shootAmount = 0;
                tailAmount = Random.Range(1, 4);
                nextTail = timeBetweenTails;
                spawnAmount = 0;
            } else
            {
                //Debug.LogWarning("spawning");
                shootAmount = 0;
                tailAmount = 0;
                spawnAmount = Random.Range(2, 4);
                nextSpawn = spawnDelay;
            }
        } // TODO: other health amounts
    }

    private void AIShoot()
    {
        if (shootAmount < 0)
        {
            return;
        }

        if (target != null)
        {
            // 2D equivalent of tranform.LookAt()
            //headTransform.up = Quaternion.Euler(0, 0, -90) * (target.position - headTransform.position);

            if (nextShot > 0)
            {
                nextShot -= Time.deltaTime;
            }
            else
            {
                headAnim.SetTrigger("Attack");
                ShootProjectile();
                nextShot = timeBetweenShots;
            }
        }
    }
    
    private void ShootProjectile()
    {
        effectsStorage.PlayEffect(0); // Shoot SFX
        Vector2 aimDirection = target.position - headTransform.position;
        GameObject projectileInst = Instantiate(projectilePrefab, (Vector2)headTransform.position + aimDirection.normalized * 1.5f, Quaternion.FromToRotation(Vector3.up, aimDirection));
        shootAmount--;
    }


    private void AITail()
    {
        if (tailAmount < 0)
        {
            return;
        }

        if (nextTail > 0)
        {
            nextTail -= Time.deltaTime;
        }
        else
        {
            tailAnim.SetTrigger("Attack");
            TailAttack();
            nextTail = timeBetweenTails;
        }
    }

    private void TailAttack()
    {
        int tailNum = Random.Range(0, tailAnimators.Count);

        tailAnimators[tailNum].SetTrigger("Attack");
        tailAmount--;
    }

    private void AISpawn()
    {
        if (spawnAmount < 0)
        {
            return;
        }

        if (nextSpawn > 0)
        {
            if (activeSpawnIndicator == null)
            {
                int chosenLocation = Random.Range(0, spawnTransforms.Count);
                activeSpawnIndicator = Instantiate(spawnIndicator, spawnTransforms[chosenLocation].position, Quaternion.identity, transform);
            }
            nextSpawn -= Time.deltaTime;
        } else
        {
            Spawn();
            nextSpawn = spawnDelay;
        }
    }

    private void Spawn()
    {
        int chosenEnemy = Random.Range(0, enemyPrefabs.Count);
        Instantiate(enemyPrefabs[chosenEnemy], activeSpawnIndicator.transform.position, Quaternion.identity);
        Destroy(activeSpawnIndicator);
        spawnAmount--;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "iceSpike")
        {
            DieBySpike();
        }
    }

    private void DieBySpike()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        healthUI.SetActive(false);
        headDamageable.Die();

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (enemy.transform.root != transform.root && enemy.GetComponent<Damageable>() != null)
            {
                enemy.GetComponent<Damageable>().Die();
            }
        }

        GameObject.FindGameObjectWithTag("Persistent").GetComponent<PersistentAudio>().ChangeMusic(0);
        Instantiate(nextLevelCrystal, transform.position, Quaternion.identity).GetComponent<NextLevelCore>().sceneName = "Level 2-1";
        Destroy(gameObject);
    }
}
