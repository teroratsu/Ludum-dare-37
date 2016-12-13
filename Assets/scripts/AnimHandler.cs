using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimHandler : MonoBehaviour {

    public bool active = false;
    private bool notDone = false;

    public enum AnimDirection { up, left, right, down};
    public AnimDirection dir = AnimDirection.down;

    private float startTime = 0f;
    private float elapsedTime;
    private float animDuration = 1.5f;

    private Vector3 startPos;
    private Vector3 dirVec;

    public float distance = 12f;

	// Use this for initialization
	void Start () {
        startPos = transform.position;

        switch (dir)
        {
            case AnimDirection.down: dirVec = Vector3.down * distance; break;
            case AnimDirection.left: dirVec = Vector3.left * distance; ; break;
            case AnimDirection.right: dirVec = Vector3.right * distance; ; break;
            case AnimDirection.up: dirVec = Vector3.up * distance; ; break;
        }
    }
	
	// Update is called once per frame
	void Update () {
		if(notDone)
        {
            animate(Time.deltaTime);
        }
	}

    public void activeElt(bool isActive)
    {
        active = isActive;
        notDone = true;
        startTime = Time.time;
        elapsedTime = startTime;
    }


    void animate(float dt)
    {
        elapsedTime += dt;
        if(elapsedTime - startTime > animDuration)
        {
            notDone = false;
            elapsedTime = startTime + animDuration;
        }
        if(!active)
        {
            transform.position = Vector3.Lerp(startPos, startPos + dirVec, (elapsedTime - startTime) / animDuration);
        }
        else
        {
            transform.position = Vector3.Lerp(startPos + dirVec, startPos, (elapsedTime - startTime) / animDuration);
        }
    }

}
