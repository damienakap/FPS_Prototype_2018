using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo
{
    private int playerNumber;
    private int joyconNumber;
    private int teamNumber;

    public PlayerInfo( int pn, int jn, int tn)
    {
        playerNumber = pn;
        joyconNumber = jn;
        teamNumber = tn;
    }

    public int getJoyconNumber() { return joyconNumber; }
    public int getPlayerNumber() { return playerNumber; }

}
