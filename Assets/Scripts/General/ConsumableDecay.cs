using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableDecay : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] LogicType logicType;
    [SerializeField] float decayTimer;
    [SerializeField] bool isActive;
    void Start()
    {
        StartCoroutine(decayCo(decayTimer));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") &&isActive)
        {
           // Debug.Log("entered");
            PlayerController player = collision.GetComponent<PlayerController>();
            isActive = false;
            switch (logicType)
            {
                case LogicType.mush:
                    if (player.playerState==PlayerState.small)
                    {
                        SoundManager.PlaySfx(9);
                        player.ChangeState(PlayerState.big);
                        player.forScaleChange();
                    }
                    Destroy(gameObject);
                    break;
                case LogicType.coins:
                    Destroy(gameObject);
                    break;
                case LogicType.flower:
                    SoundManager.PlaySfx(9);
                    if (player.playerState==PlayerState.small)
                    {
                        player.ChangeState(PlayerState.big);
                        player.forScaleChange();
                    }
                    else if(player.playerState == PlayerState.big)
                    {
                        player.ChangeState(PlayerState.red);
                    }
                    Destroy(gameObject);
                    break;
                case LogicType.star:
                    player.StartStar();
                    Destroy(gameObject);
                    break;
                default:
                    break;
            }
       
        
        }
    }
    private IEnumerator decayCo(float decayTime)
    {
        while(decayTime>0)
        {
            decayTime = decayTime - .5f;
            yield return new WaitForSeconds(.5f);
            if (decayTime==0)
            {
                Destroy(gameObject);
            }
        }
    }
}
