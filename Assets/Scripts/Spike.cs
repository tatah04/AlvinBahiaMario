using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float speed;
    [SerializeField] float decayTimer;
    [SerializeField] bool isNegative;
     public bool trigger=false;

  
    private void Update()
    {
        //   rb.velocity = new Vector2(Mathf.Abs(rb.velocity.x), rb.velocity.y);
        if (trigger==true)
        {
            DetectBounce();
        }
    }

    public void Fire(bool isRight)
    {
        trigger = true;
        if (isRight)
        {
            rb.AddForce(Vector2.right * speed, ForceMode2D.Impulse);
            isNegative = false;
        }
        else
        {
            rb.AddForce(Vector2.left * speed, ForceMode2D.Impulse);
            isNegative = true;
        }
        StartCoroutine(decayCo());
    }
    private void DetectBounce()
    {
        if (isNegative)
        {
            if (rb.velocity.x>0)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (rb.velocity.x < 0)
            {
                Destroy(gameObject);
            }
        }
    }
   
    IEnumerator decayCo()
    {
        while (decayTimer>0)
        {
            decayTimer = decayTimer - .5f;
            yield return new WaitForSeconds(.5f);
            if (decayTimer==0)
            {
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemyHurtBox"))
        {
            collision.GetComponent<EnemyTopHitBox>().Death();
            Destroy(gameObject);
        }
    }
    
}
