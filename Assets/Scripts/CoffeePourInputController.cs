using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CoffeePourInputController : MonoBehaviour
{
    public Slider fillSlider;

    public float fillrate = 0.3f;

    public float fullness = 0;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            //Return to scene!
        }

        if (Input.GetKey(KeyCode.F)) //purposely not in fixed update - is sensistive to frame rate
        {
            fullness += fillrate * Time.deltaTime;

            fillSlider.value = fullness;
        }
    }
}
