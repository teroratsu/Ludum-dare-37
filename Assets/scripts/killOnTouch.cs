using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killOnTouch : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter2D(Collision2D hit)
    {
        if (hit.collider.gameObject.tag == "graines")
        {
                Destroy(hit.collider.gameObject);
        }
        else
        if(hit.collider.gameObject.tag == "Player" && !GameObject.Find("Scene Manager").GetComponent<SceneManager>().isloading())
        {
            GameObject.Find("GameController").GetComponent<GameManager>().unfortunatePls();
            GameObject.Find("GameController").GetComponent<GameManager>().thatIsUnfortunate();
        }
    }
}
