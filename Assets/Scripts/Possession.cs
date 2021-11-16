using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Possession : MonoBehaviour
{
    bool possessing;

    public static Possessable currentFocus;
    public List<Possessable> focusableObjects;

    [SerializeField]
    Transform holdAtPosition;
    [SerializeField]
    GameObject possessorModel;

    //private Color color =  new Color(0.5322179f, 0.9811321f, 0.9420714f, 1.0f);
    private int focusNdx;

    public void Update()
    {
        focusableObjects.RemoveAll(obj => !obj.inRangeToBePossessed);
        if (!focusableObjects.Contains(currentFocus))
        {
            focusNdx = 0;
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            focusNdx++;
        }
        if (focusNdx >= focusableObjects.Count)
        {
            focusNdx = 0;
        }
        currentFocus = focusableObjects.Count > 0 ? focusableObjects[focusNdx] : null;

        if (Input.GetKeyDown(KeyCode.E) && currentFocus != null)
        {
            if (currentFocus.scene.Length > 0)
            {
                //If this isn't set, we're not running Master, and can't load scenes
                if (MasterController.Instance == null) return;
                
                MasterController.Instance.StartScene(currentFocus.scene, PossessReturnedObject); //Lose control after this frame. Possess will add focus
            }
            else
            {
                TogglePossess();
            }
        }

        possessorModel.SetActive(!possessing);
    }

    public void TogglePossess()
    {
        if (possessing)
        {
            unpossess();
        }
        else
        {
            setPossessed(currentFocus);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Possessable>(out var possessable) && possessable.Unlocked && possessable.inRangeToBePossessed)
        {
            focusableObjects.Add(possessable);
        }
    }

    private void setPossessed(Possessable pos) //forces focus if not already present
    {
        if (currentFocus != pos)
        {
            currentFocus.transform.SetParent(null);
            currentFocus = pos;
        }

        currentFocus.transform.SetParent(transform);
        currentFocus.transform.position = holdAtPosition.transform.position;

        possessing = true;
    }

    private void unpossess() //Note: does not remove focus
    {
        currentFocus.transform.SetParent(null);
        
        possessing = false;
    }

    private void PossessReturnedObject(GameObject obj)
    {
        setPossessed(obj.GetComponent<Possessable>());
    }
}