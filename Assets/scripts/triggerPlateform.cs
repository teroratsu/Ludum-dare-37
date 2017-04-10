using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerPlateform : MonoBehaviour {

    public GameObject plateforme;
    public GameObject handle;
    public GameObject arrow;
    movingBehaviour mov;
    Animator handleAnim, arrowAnim;
    float startTime;
    float dt;
    public float triggerDelay = 2.0f;

    static AudioSource mecanisme;

    public bool triggered = false;

	// Use this for initialization
	void Start () {
        mov = plateforme.GetComponent<movingBehaviour>();
        if(handle) handleAnim = handle.GetComponent<Animator>();
        if(arrow)
        {
            arrowAnim = arrow.GetComponent<Animator>();
        }
        mecanisme = GameObject.Find("mécanisme").GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void restore()
    {
        triggered = false;
        if(handleAnim) handleAnim.SetBool("triggered", false);
        if (arrowAnim)
        {
            arrowAnim.SetBool("triggered", false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            startTime = Time.time;
            dt = startTime;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            dt += Time.deltaTime;
            if (dt - startTime > triggerDelay && !triggered)
            {
                mecanisme.Play();
                mov.triggerManually();
                triggered = true;
                if (handleAnim)  handleAnim.SetBool("triggered", true);
                if (arrowAnim)
                    arrowAnim.SetBool("triggered", true);
            }
        }
    }
}
