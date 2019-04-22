using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private GameObject enermy;
    private List<GameObject> ENERMYS = new List<GameObject>();
    public static float SPEED = 0.2f;
    public static int COUNT = 20;
    private bool isServer = false;
    public bool SERVER
    {
        get { return isServer; }
        set { isServer = value; }
    }
	void Start () {
        enermy = Resources.Load("EnermyPrefab/zombie") as GameObject;
        if(SERVER)
        {
            StartCoroutine(CreateEnermy());
        }
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

    IEnumerator CreateEnermy()
    {
        while (ENERMYS.Count < COUNT)
        {
            yield return new WaitForSeconds(1 / SPEED);
            GameObject startPosition = GameObject.Find("StartPosition");
            GameObject obj = GameObject.Instantiate(enermy, startPosition.transform.position + new Vector3(10, 0, 10), startPosition.transform.rotation);
            GameFacade.Instance.SpawnObject(obj);
        }
    }
}
