using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingBehaviour : MonoBehaviour {

    private Vector2 startPos, destPos;
    public bool looping = false;
    public bool waitingForDelay = false;
    private bool stopped = false;
    public bool triggeredManually = false;
    public bool startOnSpawn = true;
    public GameObject destination;
    public float delay = 1f;
    public float d_time, t_time; //delaytimeelapsed etc
    public float tripTime = 3f;
    private bool aller = true; // != retour

    /* start parameters */
    private bool b_looping;
    private bool b_waitingForDelay;
    private bool b_triggeredManually;
    private bool b_startOnSpawn;

    private float startTime;

    // Use this for initialization
    void Start () {
        b_looping = looping;
        b_startOnSpawn = startOnSpawn;
        b_triggeredManually = triggeredManually;
        b_waitingForDelay = waitingForDelay;

        startPos = transform.position;
        destPos = destination.transform.position;

        d_time = delay;
        startTime = Time.time;
    }

    public void triggerManually()
    {
        triggeredManually = true;
        startTime = Time.time;
    }
	
	// Update is called once per frame
	void Update () {
        if (startOnSpawn || triggeredManually)
        {
            if (!waitingForDelay && !stopped)
            {
                t_time = Time.time - startTime;
                if (aller)
                {
                    transform.position = Vector2.Lerp(startPos, destPos, t_time / tripTime);
                    if (Vector2.Distance(transform.position, destPos) < .05f)
                    {
                        transform.position = destPos;
                        waitingForDelay = true;
                    }
                }
                else
                {
                    transform.position = Vector2.Lerp(destPos, startPos, t_time / tripTime);
                    if (Vector2.Distance(transform.position, startPos) < .05f)
                    {
                        transform.position = startPos;
                        waitingForDelay = true;
                    }
                }
            }
            else
            {
                d_time -= Time.deltaTime;
                if (d_time < 0)
                {
                    waitingForDelay = false;
                    d_time = delay;
                    if (looping) aller = !aller;
                    t_time = 0;
                    startTime = Time.time;
                    if (!looping) stopped = true;
                }
            }
        }
    }

    public void disableMovement()
    {
        looping = false;
        startOnSpawn = false;
    }

    public void restore()
    {
        stopped = false;
        looping = b_looping;
        startOnSpawn = b_startOnSpawn;
        triggeredManually = b_triggeredManually;
        waitingForDelay = b_waitingForDelay;
        transform.position = startPos;
        GameObject.Find("poule").transform.parent = null;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other)
        {
            if (other.gameObject.tag == "Player")
            {
                other.transform.parent = gameObject.transform;
            }
            else if (other.gameObject.tag == "graines")
            {
                if(other.transform.parent != null)
                    other.transform.parent.transform.parent = gameObject.transform;
            }
        }
    }

    void OnTriggerLeave2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if(other.transform.parent == this.transform) other.transform.parent = null;
        }
        else if(other.gameObject.tag == "graines")
        {
            other.transform.parent.transform.parent = null;
        }
    }
}
