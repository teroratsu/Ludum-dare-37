using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eatMePls : MonoBehaviour {

    // time left before the stack is finished being eaten
    public float timeLeft = 2.0f;
    public bool isBeingEaten;
    PouleManager pouleManager;

    // Use this for initialization
    void Start () {
        isBeingEaten = false;
        pouleManager = GameObject.Find("poule").GetComponent<PouleManager>();
        Physics2D.IgnoreCollision(GameObject.Find("poule").GetComponent<Collider2D>(), transform.parent.gameObject.GetComponent<Collider2D>());
    }
	
	// Update is called once per frame
	void Update () {
		if(isBeingEaten)
        {
            timeLeft -= Time.deltaTime;
            if(timeLeft < 0)
            {
                pouleManager.isEatingSomeSeeds = false;
                Destroy(transform.parent.gameObject);
            }
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!pouleManager.isAlreadyEating())
            {
                isBeingEaten = true;
                pouleManager.isEatingSomeSeeds = true;
            }
        }
    }
    void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag == "Player")
        {
            if (!isBeingEaten && !pouleManager.isAlreadyEating())
            {
                pouleManager.isEatingSomeSeeds = true;
                isBeingEaten = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (pouleManager.isAlreadyEating() && isBeingEaten)
            {
                pouleManager.isEatingSomeSeeds = false;
            }
            isBeingEaten = false;
        }
    }
}
