using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private Possession possessor;
    [SerializeField]
    private MusicController music;

    private Vector2 direction;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            return;
        }

        if (Input.GetKeyDown(KeyCode.C)) 
        {
            possessor.Possess();
        }

        direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        //DEBUGGING - probably don't want keys to control music
        if (music.isActiveAndEnabled)
        {
            if (Input.GetKeyDown(KeyCode.L)) music.StopMusic();
            if (Input.GetKeyDown(KeyCode.P)) music.PlayMusic();
            if (Input.GetKeyDown(KeyCode.N)) music.NextTrack();
        }
    }

    public void FixedUpdate()
    {
        player.Move(direction, Time.fixedDeltaTime);
    }
}
