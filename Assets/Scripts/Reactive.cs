using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reactive : MonoBehaviour
{
    public GameObject react_prefab;

    void Update()
    {
        //TODO: Get rid of these before ship, they're just for debug/showoff of the feature
        if(Input.GetKeyDown(KeyCode.Z))
        {
            react_pos();
        }

        if(Input.GetKeyDown(KeyCode.X))
        {
            react_neg();
        }        
    }

    public void react_pos()
    {
        GameObject reaction = Instantiate(react_prefab, transform.position + new Vector3(0f,1.5f,0f), Quaternion.identity);
        reaction.GetComponent<ReactionController>().positive = true;
    }

    public void react_neg()
    {
        GameObject reaction = Instantiate(react_prefab, transform.position + new Vector3(0f,1.5f,0f), Quaternion.identity);
        reaction.GetComponent<ReactionController>().positive = false;
    }
}
