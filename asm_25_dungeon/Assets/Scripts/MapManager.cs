using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {
    static MapManager _instance;
    public static MapManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new MapManager();
            }
            return _instance;
        }
    }


    [Multiline(8)]
    public string MapInput = "";
    public Tile[] TileDefinitions;
    public Player Player;

    bool _exitUnlocked;
    char[][] _mapArray;
    Tile[][] _mapTiles;
    Vector3 _tileScaleVector = new Vector3();

	// Use this for initialization
	void Start () {
        _instance = this;
        _mapArray = ParseMapString(MapInput);
        _mapTiles = ParseMapChars(_mapArray);
        SpawnTiles(_mapTiles);
        PlacePlayer();        
	}

    private void PlacePlayer()
    {
        if (Player != null)
        {
            Instantiate(Player.transform, GetTileScenePosition(Player.PositionTileY, Player.PositionTileX), Quaternion.identity);
        } else
        {
            Debug.LogWarning("Player not defined for MapManager");
        }
    }

    private Tile[][] ParseMapChars(char[][] charMap)
    {
        Tile[][] tileMap = new Tile[charMap.Length][];
        for(int i = 0; i < charMap.Length; i++)
        {
            // Initialize tile map row
            tileMap[i] = new Tile[charMap[i].Length];
            // Populate tile map row
            for(int j = 0; j < charMap[i].Length; j++)
            {
                // Find corresponding tile and put it to the array
                foreach(Tile t in TileDefinitions)
                {
                    if (t.Key.Equals(charMap[i][j]))
                    {
                        tileMap[i][j] = t;
                        break;
                    }
                }
                if(tileMap[i][j] == null)
                {
                    Debug.LogWarning("There was no tile found for " + charMap[i][j] + "!");
                }
            }
        }
        return tileMap;
    }

    internal void Exit()
    {
        if (_exitUnlocked)
        {
            // TODO next level
            Debug.Log("Level complete!");
        } else
        {
            // TODO prompt
            Debug.LogFormat("<color='orange'>Find the key!</color>");
        }
    }

    internal void UnlockExit(bool unlock)
    {
        _exitUnlocked = unlock;
    }

    char[][] ParseMapString(string mapStr)
    {
        char[][] parsedMap;
        string[] rows = mapStr.Split('\n');
        parsedMap = new char[rows.Length][];
        for (int i = 0; i < parsedMap.Length; i++)
        {
            parsedMap[i] = rows[i].Trim('\r').ToCharArray();
        }

        return parsedMap;
    }

    float GetScaleFactor(Tile[][] tileMap)
    {

        return Screen.width/tileMap.Length;
    }

    void SpawnTiles(Tile[][] tileMap)
    {
        Vector3 spawnPosition = new Vector3();
        float scale = GetScaleFactor(tileMap);
        for (int i = 0; i < tileMap.Length; i++)
        {
            for(int j = 0; j < tileMap[i].Length; j++)
            {
                if (tileMap[i][j] != null)
                {                    
                    spawnPosition = GetTileScenePosition(i,j);
                    //spawnPosition.z = -spawnPosition.y;
                    
                    //spawnPosition.x = i;
                    //spawnPosition.z = j;
                    //spawnPosition.y = 0;

                    Tile tileTransform = Instantiate(tileMap[i][j], spawnPosition, Quaternion.identity);
                    tileMap[i][j] = tileTransform;

                    if (tileTransform.EnemyOnTile != null)
                    {
                        Enemy e = Instantiate(tileTransform.EnemyOnTile, tileTransform.transform.position, Quaternion.identity);
                        tileTransform.EnemyOnTile = e;
                        Transform t = e.transform;
                        t.SetParent(tileTransform.transform);


                    }
                    // TODO Scale boxes
                    //tileTransform.localScale = Camera.main.ScreenToWorldPoint(_tileScaleVector);
                }

            }
        }
    }

    public Tile GetTileAt(int x, int y)
    {
        Tile tile;
        try
        {
            tile = _mapTiles[x][y];
        }
        catch
        {
            tile = null;
        }
        return tile;
    }
    public Vector3 GetTileScenePosition(int x, int y)
    {
        float sceneScale = Screen.width / _mapTiles[x].Length;
        Vector3 scenePos = Camera.main.ScreenToWorldPoint(new Vector3(sceneScale * (y + 0.5f), Screen.height-sceneScale*(x+0.5f), 0));

        //scenePos.z = y;
        scenePos.y = 0;

        return scenePos;
    }
}
