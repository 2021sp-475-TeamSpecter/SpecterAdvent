using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastControl : MonoBehaviour
{
	Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
    	rb2d = GetComponent<Rigidbody2D>();
    	Invoke("KillCommand", .4f);
        
    }

    // Update is called once per frame
    void Update()
    {
    	rb2d.AddForce(new Vector2(-3,0));
        
    }

    private void KillCommand()
    {
    	Destroy(gameObject);
    }
}
