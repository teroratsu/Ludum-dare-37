using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FlagTrigger : MonoBehaviour {

    private Animator anim, animPoule;

    public UnityEvent methods;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        animPoule = GameObject.Find("poule").GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && !GameObject.Find("GameController").GetComponent<GameManager>().inMenu)
        {
            anim.SetBool("victory", true);
            animPoule.SetBool("victory", true);
            methods.Invoke();
        }
    }
    //todo ontriggerstay rather than exit

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            anim.SetBool("victory", false);
            animPoule.SetBool("victory", false);
        }
    }
}
