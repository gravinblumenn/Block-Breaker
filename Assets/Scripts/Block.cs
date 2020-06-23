using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] AudioClip breakSound;
    [SerializeField] GameObject blockSparklesVFX;
    [SerializeField] int maxHits;
    [SerializeField] Sprite[] hitSprites;
     
     //cached reference
     Level level;

     //state variable
     [SerializeField] int timesHit; //serialized for debugging purposes
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (tag == "Breakable")
        {
            HandleHit();
        }
        
    }
    private void ShowNextHitSprite()
    {
        int spriteIndex = timesHit - 1;
        GetComponent<SpriteRenderer>().sprite = hitSprites[spriteIndex];
    }

    private void HandleHit()
    {
         timesHit++;
         if (timesHit >= maxHits)
        {
             DestroyBlock();
        }
        else 
        {
            ShowNextHitSprite();
        }
    }

    private void DestroyBlock()
    {
        PlayBlockSFX();
        Destroy(gameObject, 0f);
        level.BlockDestroyed();

        TriggerSparklesVFX();
    }

    private void PlayBlockSFX()
    {
        //adds score per block destroyed
        FindObjectOfType<GameStatus>().AddToScore();

        AudioSource.PlayClipAtPoint(breakSound, Camera.main.transform.position, 1f);
    }

    private void TriggerSparklesVFX()
    {
        GameObject sparkles = Instantiate(blockSparklesVFX, transform.position, transform.rotation);
        Destroy(sparkles, 2f);
    }

    private void Start()
    {
        CountBreakableBlocks();
    }

    private void CountBreakableBlocks()
    {
        level = FindObjectOfType<Level>();
        if (tag == "Breakable")
        {
            level.CountBreakableBlocks();
        }
    }
}
