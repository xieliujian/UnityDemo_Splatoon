
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager
{
    List<Player> mPlayerList;
    Player mMe;

    public void Init()
    {
        mPlayerList = new List<Player>();
    }

    public void Tick(float deltaTime)
    {

    }
}
