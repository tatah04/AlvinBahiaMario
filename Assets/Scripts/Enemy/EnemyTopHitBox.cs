using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTopHitBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BotCollision"))
        {
            if (collision.GetComponentInParent<PlayerController>().isInvincible==false)
            {
                collision.GetComponentInParent<PlayerController>().timeAndScoreTracker.enemiesKilled++;
                Death();
            }
        }
    }

    public void Death()
    {
        Destroy(gameObject.transform.parent.gameObject);
        SoundManager.PlaySfx(8);
    }
}
