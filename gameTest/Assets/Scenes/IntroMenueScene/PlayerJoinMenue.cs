using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerJoinMenue : MonoBehaviour
{

    public Text player1JoinText;
    public Text player2JoinText;
    public Text player3JoinText;
    public Text player4JoinText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.playerJoined(1))
            player1JoinText.text = "     Joined";
        else
            player1JoinText.text = "Press [X]\n  to Join";

        if (GameManager.playerJoined(2))
            player2JoinText.text = "     Joined";
        else
            player2JoinText.text = "Press [X]\n  to Join";

        if (GameManager.playerJoined(3))
            player3JoinText.text = "     Joined";
        else
            player3JoinText.text = "Press [X]\n  to Join";

        if (GameManager.playerJoined(4))
            player4JoinText.text = "     Joined";
        else
            player4JoinText.text = "Press [X]\n  to Join";


    }
}
