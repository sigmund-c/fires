using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Patrol : MonoBehaviour
{
    public LayerMask ignoreLayer;
    public float moveSpeed;
    public float maxFallDist = 2f;
    public float wallDetectionDist = 0.1f;
    public float playerDetectionDist = 8f;
    public float chargeAmount = 10f;

    public Transform groundDetector;

    private Rigidbody2D rb;
    private Animator anim;
    private AudioSource audioSource;
    private bool movingLeft = true;

    private bool isCharging = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetector.position, Vector2.down, maxFallDist, ~ignoreLayer);
        if (groundInfo.collider == false) // No ground found
        {
            Rotate();
        } else // Check for obstructions/walls
        {

            RaycastHit2D wallInfo;
            RaycastHit2D playerInfo;
            if (movingLeft)
            {
                wallInfo = Physics2D.Raycast(groundDetector.position, Vector2.left, wallDetectionDist, ~ignoreLayer);
                playerInfo = Physics2D.Raycast(groundDetector.position, Vector2.left, playerDetectionDist);
            } else
            {
                wallInfo = Physics2D.Raycast(groundDetector.position, Vector2.right, wallDetectionDist, ~ignoreLayer);
                playerInfo = Physics2D.Raycast(groundDetector.position, Vector2.right, playerDetectionDist);
            }
            if (wallInfo.collider == true && wallInfo.collider.tag != "Player" && wallInfo.collider.tag != "PlayerComponent" && wallInfo.collider.tag != "CameraTrigger")
            {
                Rotate();
            }
            if (playerInfo.collider == true && playerInfo.collider.CompareTag("PlayerComponent"))
            {
                StartCoroutine(Charge());
            }
        }
    }

    IEnumerator Charge()
    {
        if (isCharging)
        {
            yield break;
        }

        isCharging = true;
        audioSource.Play();
        anim.SetTrigger("Attack");
        if (movingLeft)
        {
            rb.AddForce(Vector2.left * chargeAmount, ForceMode2D.Impulse);
        } else
        {
            rb.AddForce(Vector2.right * chargeAmount, ForceMode2D.Impulse);
        }

        yield return new WaitForSeconds(1.5f);

        isCharging = false;
    }

    void Rotate()
    {
        if (movingLeft == true) // Moving Left
        {
            transform.Rotate(0, 180, 0);
            movingLeft = false;
        }
        else // Moving Right
        {
            transform.Rotate(0, 180, 0);
            movingLeft = true;
        }
    }
}
