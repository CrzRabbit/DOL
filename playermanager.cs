using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playermanager : MonoBehaviour {

    public static Queue<GameObject> players = new Queue<GameObject>();

    public static Queue<GameObject> PLAYERS
    {
        get { return players; }
    }

    public static void putPlayers(GameObject player)
    {
        players.Enqueue(player);
    }
}
