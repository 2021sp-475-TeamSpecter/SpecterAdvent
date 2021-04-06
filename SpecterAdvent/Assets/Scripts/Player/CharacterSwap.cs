using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSwap : MonoBehaviour
{

    // Start is called before the first frame update
   /* public GameObject avatar1, avatar2;

    int switchAvatar = 1;
*/
    GameObject X, Zero;
    int CharacterSelect;
    void Start()
    {
    	//avatar1.gameObject.SetActive (true);
    	//avatar2.gameObject.SetActive (false);   
    	CharacterSelect = 1;

        X = GameObject.Find("X");
        Zero = GameObject.Find("Zero");

        X.SetActive(true);
        Zero.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetButtonDown("Fire1"))
        {
            if(CharacterSelect == 1)
            {
                CharacterSelect = 2;             
                Zero.transform.position = X.transform.position;
                X.SetActive(false);
            	Zero.SetActive(true);
                // make sure zero is not attacking 
                Zero.GetComponent<PlayerController2D>().StopAttacking();
                Zero.GetComponent<PlayerController2D>().hitBox.SetActive(false);
            }
            else if (CharacterSelect == 2)
            {
                CharacterSelect = 1;
                X.transform.position = Zero.transform.position;              
                // make sure zero is not attacking 
                Zero.GetComponent<PlayerController2D>().StopAttacking();
                Zero.GetComponent<PlayerController2D>().hitBox.SetActive(false);

                Zero.SetActive(false);
                X.SetActive(true);       
            }
        }
        
        
    }
}
