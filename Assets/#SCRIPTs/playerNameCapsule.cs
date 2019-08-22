using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerNameCapsule : MonoBehaviour
{
    // Why this script ?
    // -> Just a way to store the player name
    // -> After the player achieve to get out the maze, all datas are deleted (difficulty, timer & player name)
    // -> But I don't want that the user needs to reenter his name each time
    // -> So I use a script like that 

    public string playerName = "";

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
