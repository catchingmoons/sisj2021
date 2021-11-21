using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupSpawner : MonoBehaviour
{
    public GameObject PopupCenter_prefab;
    public GameObject PopupBottom_prefab;
    public Canvas canvas;

    // Update is called once per frame
    void Update()
    {
        //TODO: Get rid of these before ship, they're just for debug/showoff of the feature
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SpawnPopupCenter();
        }

        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            SpawnPopupBottom();
        }   
    }

    public void SpawnPopupCenter()
    {
        GameObject popup = Instantiate(PopupCenter_prefab, transform.position, Quaternion.identity);
        popup.transform.SetParent(canvas.transform);
    }

    public void SpawnPopupBottom()
    {
        GameObject popup = Instantiate(PopupBottom_prefab, transform.position, Quaternion.identity);
        popup.transform.SetParent(canvas.transform);
    }
}
