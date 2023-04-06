using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;


    LayerMask obstacleMask; 
    Vector2 targetPos;
    Transform GFX;
    float flipX;
    bool isMoving;
    void Start()
    {
        GFX = GetComponentInChildren<SpriteRenderer>().transform;
        flipX = GFX.localScale.x;
        obstacleMask = LayerMask.GetMask("Wall","Enemy");
    }

    void Update()
    {
        // math sign to put a negative 1 positive 1 or 0 mathf sign will not produce 0 in between will keep at one 
        // that's why system.math.sign
        float horz = System.Math.Sign(Input.GetAxisRaw("Horizontal"));
        float vert = System.Math.Sign(Input.GetAxisRaw("Vertical"));
        if(Mathf.Abs(horz) > 0 || Mathf.Abs(vert) > 0)
        {
            if(Mathf.Abs(horz) > 0)
            {
                GFX.localScale = new Vector2(flipX * horz,GFX.localScale.y);
            }
            if(!isMoving)
            {
                if(Mathf.Abs(horz) > 0)
                {
                    targetPos = new Vector2(transform.position.x + horz, transform.position.y);
                }
                else if(Mathf.Abs(vert) > 0)
                {
                    targetPos = new Vector2(transform.position.x, transform.position.y + vert);
                }
                Vector2 hitSize = Vector2.one * 0.8f;
                //check for collisions
                Collider2D hit = Physics2D.OverlapBox(targetPos,hitSize,0,obstacleMask);
                if(!hit)
                {
                    StartCoroutine(SmoothMove());
                }
            }
        }
    }

    IEnumerator SmoothMove()
    {
        isMoving = true;
        while(Vector2.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position,targetPos
            ,speed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
        isMoving = false;
    }
}