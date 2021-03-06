using System.Collections.Generic;
using UnityEngine;

public  class RoomConfigs : MonoBehaviour
{
    public static RoomConfigs instance;

    private void Start()
    {
        instance = this;
        heightTime = gameplayMaxTime + gameplayTimeBase;
    }

    //CreateRoomC
    public int maxRoomPlayers;

    //characterSelection
    public int characterSelectionMaxTime;

    //Gameplay
    public int gameplayMaxTime;
    public int heightTime;
    public int gameplayTimeBase;

    //SceneIndex
    public int menuSceneIndex;
    public int CharSelecSceneIndex;
    public int gameplaySceneIndex;

    //Players
    public int timeToRespawn;
    [Range(0, 2f)] public float mouseScrollingX;
    [Range(0, 2f)] public float mouseScrollingY;

    //team
    public Color blueTeamColor = new Color(0, 129, 255, 255);
    public Color redTeamColor = new Color(233, 36, 41, 255);
    public Color noneColor;

    public Character[] charactersOrdered;


   
}
