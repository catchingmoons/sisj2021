using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Possession : MonoBehaviour
{
    bool possessing = false;
    Possessable possessable_obj;
    [SerializeField]
    private Renderer ghost_rend;
    [SerializeField]
    public Transform possessableRoot;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (possessable_obj != null && possessable_obj.scene.Length > 0 && MasterController.Instance != null)
            {
                //If MasterController.Instance isn't set, we're not running in master
                MasterController.Instance.StartScene(possessable_obj.scene, GetACoffee); //Lose control next frame
            }
            else
            {
                TogglePossess();
            }
        }
    }

    public void TogglePossess()
    {
        if (possessing)
        {
            possessable_obj.transform.SetParent(possessableRoot);
            ghost_rend.material.color = new Color(0.5322179f, 0.9811321f, 0.9420714f, 1.0f);
            possessing = false;
        }
        else if (possessable_obj != null)
        {
            setPossessed();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!possessing && other.TryGetComponent<Possessable>(out var possessable))
        {
            possessable_obj = possessable.Unlocked ? possessable : null;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == possessable_obj.gameObject)
        {
            possessable_obj = null;
        }
    }

    private void setPossessed()
    {
        possessable_obj.transform.SetParent(transform);
        ghost_rend.material.color = new Color(0.5322179f, 0.9811321f, 0.9420714f, 0.0f);
        possessing = true;
    }

    private void GetACoffee(GameObject obj)
    {
        possessable_obj = obj.GetComponent<Possessable>();
        setPossessed();
    }
}