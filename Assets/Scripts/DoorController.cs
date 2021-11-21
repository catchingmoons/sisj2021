using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public LayerMask layers;
    public float speed;
    public float duration;
    public float targetSwingYDegrees;

    ISet<GameObject> inRange = new HashSet<GameObject>();
    private Quaternion closed;
    private Quaternion open;

    private float swingCompletion;

    void Awake()
    {
        closed = transform.rotation;
        open = Quaternion.Euler(closed.eulerAngles.x, targetSwingYDegrees, closed.eulerAngles.z);
        swingCompletion = 0;
    }

    void FixedUpdate()
    {
        if (inRange.Count == 0 && closed != transform.rotation)
        {
            swingCompletion -= speed * Time.fixedDeltaTime;
            swingCompletion = Mathf.Max(swingCompletion, 0);
        }
        else if (inRange.Count > 0 && open != transform.rotation)
        {
            swingCompletion += speed * Time.fixedDeltaTime;
            swingCompletion = Mathf.Min(swingCompletion, duration);
        }
        transform.rotation = Quaternion.Lerp(closed, open, (swingCompletion / duration));
    }

    void OnTriggerEnter(Collider other)
    {
        if (!inRange.Contains(other.gameObject) && (layers.value & (1 << other.gameObject.layer)) != 0)
        {
            inRange.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        inRange.Remove(other.gameObject);
    }
}
