
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FightUI : MonoBehaviour 
{
    FightClockUI mClock;
    FightMiniMapUI mMiniMap;
    FightScoreUI mScore;
    FightTeamUI mTeam;

    void Awake()
    {
        mClock = transform.Find("clock").GetComponent<FightClockUI>();
        mMiniMap = transform.Find("minimap").GetComponent<FightMiniMapUI>();
        mScore = transform.Find("score").GetComponent<FightScoreUI>();
        mTeam = transform.Find("team").GetComponent<FightTeamUI>();
    }

    public FightClockUI ClockUI { get { return mClock; } }
    public FightMiniMapUI MiniMapUI { get { return mMiniMap; } }
    public FightScoreUI ScoreUI { get { return mScore; } }
    public FightTeamUI TeamUI { get { return mTeam; } }

    void Update()
    {
        float elapsedTime = Time.deltaTime;
        mMiniMap.Tick(elapsedTime);
        mClock.Tick(elapsedTime);
    }
}
