using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WonTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Rigidbody2D rb;
    [SerializeField] TimeAndScoreTracker score;
    [SerializeField] bool isActive;
    [SerializeField] bool isTop;
    [SerializeField] float speed;
    [SerializeField] float moveBuffer;
    [SerializeField] float timePerside;
    [SerializeField] float timePerSideMax;

    // Update is called once per frame
    private void Start()
    {
        timePerside = timePerSideMax;
        StartCoroutine(PatrolCo());
    }
    void FixedUpdate()
    {
        if (isActive)
        {
            movePlatform();
        }
    }

    private void movePlatform()
    {
        if (isTop == true)
        {
            rb.velocity = Vector2.down * speed * Time.fixedDeltaTime * moveBuffer;
            //enemyRb.AddForce(Vector2.left * moveSpeed * Time.fixedDeltaTime * moveBuffer);
        }
        else
        {
            rb.velocity = Vector2.up * speed * Time.fixedDeltaTime * moveBuffer;
            //  enemyRb.AddForce(Vector2.right * moveSpeed * Time.fixedDeltaTime * moveBuffer);
        }
    }
    private IEnumerator PatrolCo()
    {
        while (timePerside > 0)
        {
            while (isTop == true)
            {
                timePerside = timePerside - 0.5f;
                yield return new WaitForSeconds(0.5f);
                if (timePerside == 0)
                {
                    isTop = false;
                    timePerside = timePerSideMax;
                   // Flip();
                }
            }
            while (isTop == false)
            {
                timePerside = timePerside - 0.5f;
                yield return new WaitForSeconds(0.5f);
                if (timePerside == 0)
                {
                    isTop = true;
                    timePerside = timePerSideMax;
                 //   Flip();
                }
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")|| collision.CompareTag("BotCollision"))
        {
            isActive = false;
           // SoundManager.PlayBgm(16);
            Time.timeScale = 0;
            score.wonLevel();
            if (collision.CompareTag("Player"))
            {
                Stats.lastState = collision.GetComponent<PlayerController>().playerState;
            }
            else if(collision.CompareTag("BotCollision"))
            {
                Stats.lastState = collision.GetComponentInParent<PlayerController>().playerState;
            }
        }
    }
}
