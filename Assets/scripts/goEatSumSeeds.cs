using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goEatSumSeeds : MonoBehaviour {

    bool isSeekingSeedStack;
    public string seekingObj = "graines";
    PouleManager pouleManager;

    // Use this for initialization
    void Start () {
        isSeekingSeedStack = false;
        pouleManager = gameObject.GetComponent<PouleManager>();
    }
	
	// Update is called once per frame
	void Update () {
        if (isSeekingSeedStack)
        {
            if (!pouleManager.isSeekingAStack())
                isSeekingSeedStack = false;
        }
    }

    void FixedUpdate()
    {
        if (!isSeekingSeedStack)
        {
            List<Transform> stacks_t = new List<Transform>();
            GameObject[] stacks = GameObject.FindGameObjectsWithTag(seekingObj);
            foreach (GameObject stack in stacks)
            {
                stacks_t.Add(stack.transform);
            }
            Transform closest = GetClosestStack(stacks_t);
            if (closest != null)
            {
                isSeekingSeedStack = false;
                pouleManager.setTarget(closest.gameObject);
            }
            else
            {
                pouleManager.setTarget(null);
            }
        }
    }

    Transform GetClosestStack(List<Transform> stacks)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (Transform potentialTarget in stacks)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }
}
