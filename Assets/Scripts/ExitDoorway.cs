using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(Rigidbody2D),typeof(BoxCollider2D))]
public class ExitDoorway : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Reset()
    {
        GetComponent<Rigidbody2D>().isKinematic = true;
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        box.size = Vector2.one * 0.1f;
        box.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
