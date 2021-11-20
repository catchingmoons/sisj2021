using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CoffeePourInputController : MonoBehaviour
{
    public CoffeeController coffeePrefab;

    public Slider fillSlider;

    public AudioSource sound;

    public float fillrate = 0.3f;

    public float fullness = 0;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            return;
        }

        if (Input.GetKeyDown(KeyCode.E) && MasterController.Instance != null)
        {
            var coffee = Instantiate(coffeePrefab);
            coffee.fullness = fullness;
            MasterController.Instance.EndScene(coffee.gameObject);
        }

        if (Input.GetKey(KeyCode.F)) //purposely not in fixed update - is sensistive to frame rate
        {
            if (!sound.isPlaying)
            {
                sound.Play();
            }

            fullness += fillrate * Time.deltaTime;

            fillSlider.value = fullness;
        }
        else if (sound != null && sound.isActiveAndEnabled && sound.isPlaying)
        {
            sound.Pause();
        }
    }
}
