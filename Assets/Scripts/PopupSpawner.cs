using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupSpawner : MonoBehaviour
{
    public GameObject PopupCenter_prefab;
    public GameObject PopupBottom_prefab;
    public Canvas canvas;

    public bool first_time;
    public int message;
    private GameObject message_obj;
    private Text message_text;

    // Update is called once per frame
    void Update()
    {
        //TODO: Get rid of these before ship, they're just for debug/showoff of the feature
        /*
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SpawnPopupCenter();
        }

        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            SpawnPopupBottom();
        }  
        */
    }

    void Awake()
    {
        if (!first_time)
        {
            if (message == 1)
            {
            SpawnPopupBottom();
            message_text.text = "Welcome to the Lodge!" + '\n' + "Possess Objects using E" + '\n' + "Deliver coffee to tired patrons to brighten their day";
            first_time = true;
            }

            if (message == 2)
            {
            SpawnPopupBottom();
            message_text.text = "Press F to FILL the coffee cup" + '\n' + "Press E to Exit";
            first_time = true;
            }
        }
    }

    public void SpawnPopupCenter()
    {
        GameObject popup = Instantiate(PopupCenter_prefab, transform.position, Quaternion.identity);
        popup.transform.SetParent(canvas.transform);
        message_obj = popup.transform.GetChild(1).gameObject;
        message_text = message_obj.GetComponent<Text>();
        

    }

    public void SpawnPopupBottom()
    {
        GameObject popup = Instantiate(PopupBottom_prefab, transform.position, Quaternion.identity);
        popup.transform.SetParent(canvas.transform);
        message_obj = popup.transform.GetChild(1).gameObject;
        message_text = message_obj.GetComponent<Text>();
    }
}
