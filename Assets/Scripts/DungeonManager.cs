using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DungeonManager : MonoBehaviour
{
    public GameObject[] randomItems;
    public GameObject floorPrefab,wallPrefab,tilePrefab,exitPrefab;

    public float minX, maxX,minY,maxY;
    [Range(50,5000)]public int totalFloorCount;
    [Range(0,100)]public int itemSpawnPercent;
    List<Vector3> floorList = new List<Vector3>();
    LayerMask floorMask,wallMask;
    void Start(){
        floorMask = LayerMask.GetMask("Floor");
        wallMask = LayerMask.GetMask("Wall");
        RandomWalker();
    }

    void Update()
    {
        if(Application.isEditor && Input.GetKeyDown(KeyCode.Backspace))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void RandomWalker()
    {
        Vector3 currentPos = Vector3.zero;
        floorList.Add(currentPos);
        //set floor tile at current pos
        while(floorList.Count < totalFloorCount)
        {
            switch (Random.Range(1,5))
            {
                case 1:
                    currentPos += Vector3.up;
                    break;
                case 2: 
                    currentPos += Vector3.right;
                    break;
                case 3:
                    currentPos += Vector3.down;
                    break;
                case 4:
                    currentPos += Vector3.left;
                    break;
            }
            bool inFloorList = false;
            for(int i = 0; i < floorList.Count; i++)
            {
                if(Vector3.Equals(currentPos, floorList[i]))
                {
                    inFloorList = true;
                    break;
                }
            }
            if(!inFloorList)
            {
                floorList.Add(currentPos);
            }
        }
        for(int i = 0; i < floorList.Count; i++)
        {
            GameObject goTile = Instantiate(tilePrefab,floorList[i],Quaternion.identity) as GameObject;
            goTile.name = tilePrefab.name;
            goTile.transform.SetParent(transform);
        }
        StartCoroutine(DelayProgress());
    }

    IEnumerator DelayProgress()
    {
        while(FindObjectsOfType<TileSpawner>().Length > 0)
        {
            yield return null;
        }
        ExitDoorway();
        Vector2 hitSize = Vector2.one * 0.8f;
        for(int x = (int)minX - 2; x <= (int)maxX + 2; x++)
        {
            for(int y = (int)minY - 2; y <= (int)maxY + 2; y++)
            {
                Collider2D hitFloor = Physics2D.OverlapBox(new Vector2(x,y),hitSize, 0, floorMask);
                if(hitFloor)
                {
                    if(!Vector2.Equals(hitFloor.transform.position,floorList[floorList.Count - 1]))
                    {
                        Collider2D hitTop = Physics2D.OverlapBox(new Vector2(x,y + 1),hitSize, 0, wallMask);
                        Collider2D hitRight = Physics2D.OverlapBox(new Vector2(x + 1,y),hitSize, 0, wallMask);
                        Collider2D hitBottom = Physics2D.OverlapBox(new Vector2(x,y - 1),hitSize, 0, wallMask);
                        Collider2D hitLeft = Physics2D.OverlapBox(new Vector2(x - 1,y),hitSize, 0, wallMask);
                        RandomItems(hitFloor,hitTop,hitRight,hitBottom,hitLeft);
                    }
                }
            }
        }
    }

    void RandomItems(Collider2D hitFloor,Collider2D hitTop,Collider2D hitRight,Collider2D hitBottom,Collider2D hitLeft)
    {
        if((hitTop || hitRight || hitBottom || hitLeft) && !(hitTop || hitBottom) && !(hitLeft && hitRight))
        {
            int roll = Random.Range(0,101);
            if(roll <= itemSpawnPercent)
            {
                int itemIndex = Random.Range(0,randomItems.Length);
                GameObject goItem = Instantiate(randomItems[itemIndex],hitFloor.transform.position,Quaternion.identity) as GameObject;
                goItem.name = randomItems[itemIndex].name;
                goItem.transform.SetParent(hitFloor.transform);
            }
        }
    }

    void ExitDoorway()
    {
        Vector3 doorPos = floorList[floorList.Count - 1];
        GameObject goDoor = Instantiate(exitPrefab,doorPos,Quaternion.identity) as GameObject;
        goDoor.name = exitPrefab.name;
        goDoor.transform.SetParent(transform);
    }
}
