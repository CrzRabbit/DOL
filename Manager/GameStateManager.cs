using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour {

	void Start () {
		
	}

	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(2);
            GameObject facade = GameObject.Find("GameFacade(Clone)");
            GameFacade gameFacade = facade.GetComponent<GameFacade>();
            gameFacade.EnterLobbySync();
            gameFacade.GameOver();
        }
	}
}
