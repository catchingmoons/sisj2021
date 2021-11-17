using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionController : MonoBehaviour
{
    public Animator foreground;
    public int rand;
    public bool positive;

    // Start is called before the first frame update
    void Start()
    {
        if(!positive)
        {
            rand = Random.Range(0,3);
        } else {
            rand = Random.Range(3,6);
        }
        foreground.SetInteger("selector", rand);
    }

    public void kys()
    {
        Destroy(gameObject);
    }
}
