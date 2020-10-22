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

    public Sprite idleHead;
    public Sprite attackHead;
    public Sprite idleTail;
    public Sprite attackTail;
    private SpriteRenderer headSprite;
    private SpriteRenderer tailSprite;

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

        headSprite = headTransform.GetComponent<SpriteRenderer>();
        tailSprite = transform.Find("Snake").Find("SnakeTailSprite").GetComponent<SpriteRenderer>();
        idleTail = tailSprite.sprite;

        effectsStorage = GetComponent<EffectsStorage>();
    }

    public void StartAI()
    {
        if (!hasStarted)
        {
            hasStarted = true;
            nextAttack = idleTime;
            Debug.Log("AI started");
            healthUI.SetActive(true);
        }
    }

    public void StopAI()
    {
        if (hasStarted)
        {
            hasStarted = false;
            Debug.Log("AI stopped");
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
            Debug.Log("generating new attack");
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

        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (shootAmount > 0)
        {
            headSprite.sprite = attackHead;
        } else
        {
            headSprite.sprite = idleHead;
        }

        if (tailAmount > 0)
        {
            tailSprite.sprite = attackTail;
        } else
        {
            tailSprite.sprite = idleTail;
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
                spawnAmount = Random.Range(1, 3);
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
            headTransform.up = Quaternion.Euler(0, 0, -90) * (target.position - headTransform.position);

            if (nextShot > 0)
            {
                nextShot -= Time.deltaTime;
            }
            else
            {
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
        Instantiate(nextLevelCrystal, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
