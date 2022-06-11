using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    enemy1,
}
public enum EnemyBehavior
{
    patrol,
}
public class EnemyScript : MonoBehaviour
{
    [Header("General")]
    public EnemyType enemyType;
    public EnemyBehavior enemyBehavior;
    [SerializeField] float moveSpeed;
    [SerializeField] Rigidbody2D enemyRb;
    [SerializeField] float moveBuffer;
    [SerializeField] SpriteRenderer spriteRenderer;
    [Header("Combat")]
    [SerializeField] float health;
    [SerializeField] BoxCollider2D topHitBox;
    [SerializeField] BoxCollider2D mainHitBox;
    [Header("For Patrol")]
    private Vector2 startPos;
    [SerializeField] float maxtimePerside;
    float timePerside;
    bool isLeft = true;
    // bool isFacingRight=false;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        timePerside = maxtimePerside;
        StartCoroutine(PatrolCo());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Patrol();
        switch (enemyBehavior)
        {
            case EnemyBehavior.patrol:
                Patrol();
                break;
            default:
                break;
        }
    }

    void Patrol()
    {
        if (isLeft == true)
        {
            enemyRb.velocity = Vector2.left * moveSpeed * Time.fixedDeltaTime * moveBuffer;
            //enemyRb.AddForce(Vector2.left * moveSpeed * Time.fixedDeltaTime * moveBuffer);
        }
        else
        {
            enemyRb.velocity = Vector2.right * moveSpeed * Time.fixedDeltaTime * moveBuffer;
            //  enemyRb.AddForce(Vector2.right * moveSpeed * Time.fixedDeltaTime * moveBuffer);
        }
    }

    private IEnumerator PatrolCo()
    {
        while (health > 0 && timePerside > 0)
        {
            while (isLeft == true)
            {
                timePerside = timePerside - 0.5f;
                yield return new WaitForSeconds(0.5f);
                if (timePerside == 0)
                {
                    isLeft = false;
                    timePerside = maxtimePerside;
                    Flip();
                }
            }
            while (isLeft == false)
            {
                timePerside = timePerside - 0.5f;
                yield return new WaitForSeconds(0.5f);
                if (timePerside == 0)
                {
                    isLeft = true;
                    timePerside = maxtimePerside;
                    Flip();
                }
            }
        }
    }
    void Flip()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("playerHurtBox"))
        {
            PlayerController player = collision.transform.parent.GetComponent<PlayerController>();


            if (player.playerState == PlayerState.star)
            {
                gameObject.transform.GetChild(0).GetComponent<EnemyTopHitBox>().Death();
            }
            else
            {
                player.TakeDamage();
            }
        }
    }
}
