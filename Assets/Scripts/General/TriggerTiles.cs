using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LogicType
{
    mush,
    coins,
    flower,
    star,
}

public class TriggerTiles : MonoBehaviour
{
    [SerializeField] LogicType logicType;
    [SerializeField] List<Sprite> sprites = new List<Sprite>();
    [SerializeField] int coinsToGive;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] List<GameObject> effectPrefabs = new List<GameObject>();
    //[SerializeField] BoxCollider2D boxCollider2D;
    [SerializeField] bool isActive=true;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("TopCollision")&&isActive==true)
        {
            PlayerController player = collision.GetComponentInParent<PlayerController>();
            //isActive = false;
            switch (logicType)
            {
                case LogicType.mush:
                    spawnPrefab();

                    // player.ChangeState(PlayerState.big);
                    break;
                case LogicType.coins:
                    spawnPrefab();
                    if (coinsToGive > 0)
                    {
                        player.AddCoin(1);
                        coinsToGive--;
                    }
                    if (coinsToGive == 0)
                    {
                        spriteRenderer.sprite = sprites[1];
                        isActive = false;
                    }
                    break;
                case LogicType.flower:
                    // player.ChangeState(PlayerState.star);
                    spawnPrefab();
                    break;
                case LogicType.star:
                    spawnPrefab();

                    //  player.ChangeState(PlayerState.red);
                    break;
                default:
                    break;
            }
        }
    }

    public void spawnPrefab()
    {
        if (isActive)
        {

            switch (logicType)
            {
                case LogicType.mush:
                    GameObject obj = Instantiate(effectPrefabs[0], transform);
                    isActive = false;
                    spriteRenderer.sprite = sprites[1];
                    SoundManager.PlaySfx(2);
                    break;
                case LogicType.coins:
                    GameObject obj2 = Instantiate(effectPrefabs[1], transform);
                    SoundManager.PlaySfx(4);
                    break;
                case LogicType.flower:
                    GameObject obj3 = Instantiate(effectPrefabs[2], transform);
                    isActive = false;
                    spriteRenderer.sprite = sprites[1];
                    SoundManager.PlaySfx(2);
                    break;
                case LogicType.star:
                    GameObject obj4 = Instantiate(effectPrefabs[3], transform);
                    isActive = false;
                    spriteRenderer.sprite = sprites[1];
                    SoundManager.PlaySfx(2);
                    break;
                default:
                    break;
            }
        }
    }

}
