using UnityEngine;
using UnityEngine.SceneManagement;

public class Creator : MonoBehaviour {

	void Start () {
        GameObject gameManger = GameObject.Instantiate(Resources.Load("UndestroyGB/GameManager")) as GameObject;
        DontDestroyOnLoad(gameManger);
        GameObject canvas = GameObject.Instantiate(Resources.Load("UIPanel/Canvas")) as GameObject;
        DontDestroyOnLoad(canvas);
        GameObject facade = GameObject.Instantiate(Resources.Load("UndestroyGB/GameFacade")) as GameObject;
        DontDestroyOnLoad(facade);
        GameObject eventSystem = GameObject.Instantiate(Resources.Load("UndestroyGB/EventSystem")) as GameObject;
        DontDestroyOnLoad(eventSystem);
        GameObject requestProxy = GameObject.Instantiate(Resources.Load("UndestroyGB/RequestProxy")) as GameObject;
        DontDestroyOnLoad(requestProxy);
        SceneManager.LoadScene(1);
    }
	
	void Update () {
		
	}
}
