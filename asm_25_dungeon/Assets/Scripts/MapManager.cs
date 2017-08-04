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
        string[] rows = mapStr.Split('\n');
        var parsedMap = new char[rows.Length][];
        for (int i = 0; i < parsedMap.Length; i++)
        {
            parsedMap[i] = rows[i].Trim('\r').ToCharArray();
        }

        return parsedMap;
    }

    public int maxX = 0;
    public int maxY = 0;

    void SpawnTiles(Tile[][] tileMap)
    {
        maxX = tileMap.Length;
        maxY = tileMap[0].Length;

        Vector3 spawnPosition = new Vector3();
        for (int i = 0; i < tileMap.Length; i++)
        {
            for(int j = 0; j < tileMap[i].Length; j++)
            {
                if (tileMap[i][j] != null)
                {                    
                    spawnPosition = GetTileScenePosition(i,j);
                    Tile tileTransform = Instantiate(tileMap[i][j], spawnPosition, Quaternion.identity);
                    tileMap[i][j] = tileTransform;

                    if (tileTransform.EnemyOnTile != null)
                    {
                        Enemy e = Instantiate(tileTransform.EnemyOnTile, tileTransform.transform.position, Quaternion.identity);
                        tileTransform.EnemyOnTile = e;
                        Transform t = e.transform;
                        t.SetParent(tileTransform.transform);
                    }
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
        return new Vector3(y - maxY/2f, 0, maxX/2f - x);
    }
}
