using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MasterController : MonoBehaviour
{
    public static MasterController Instance;

    public System.Action<GameObject> onReturn;

    //Todo common UI?
    public string main = "LodgeMap";
    public string coffeePour = "CoffeePour";

    [HideInInspector]
    public GameObject passingObj;

    private string activeScene;
    private GameObject[] hidden;

    private Scene mainScene;

    public void Awake()
    {
        if (Instance != null) return;

        var loaded = SceneManager.GetSceneByName(main);
        if (loaded == null || loaded.GetRootGameObjects().Length == 0)
        {
            SceneManager.LoadScene(main, LoadSceneMode.Additive);
        }
        mainScene = SceneManager.GetSceneByName(main);

        if (FindObjectOfType<MusicController>() == null)
        {
            Debug.Log("Warning: Music not started.");
        }

        Instance = this;
    }

    public void Start()
    {
        SceneManager.SetActiveScene(mainScene);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            return;
        }
    }

    public void StartScene(string name, System.Action<GameObject> returnFunc = null)
    {
        hidden = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (var obj in hidden)
        {
            obj.SetActive(false);
        }

        activeScene = name;
        SceneManager.LoadScene(name, LoadSceneMode.Additive);

        onReturn = returnFunc;
    }

    public void EndScene(GameObject toPass)
    {
        if (toPass != null)
        {
            passingObj = toPass;
            toPass.transform.parent = null; //MUST be null!
            SceneManager.MoveGameObjectToScene(toPass, SceneManager.GetSceneByName(main));
        }

        if (onReturn != null)
        {
            onReturn.Invoke(passingObj);
            onReturn = null;
        }

        SceneManager.UnloadScene(activeScene); //DANGEROUS?

        foreach (var obj in hidden)
        {
            obj.SetActive(true);
        }
        SceneManager.SetActiveScene(mainScene);
    }
}
