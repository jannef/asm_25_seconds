using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public Vector2 PlayerStartPos = new Vector2();

    bool _exitUnlocked;
    char[][] _mapArray;
    Tile[][] _mapTiles;
    Vector3 _tileScaleVector = new Vector3();

    GameObject _closedGate;
    GameObject _openGate;

    GameObject _mapContainer;

    public int LoadedLevel = 0;

	// Use this for initialization
	void Start () {
        _instance = this;
        LoadLevel(0);
	}

    public void LoadNextLevel()
    {
        LoadLevel(LoadedLevel + 1);
    }
    public void LoadLevel(int index)
    {
        if (index < Maps.Array.Length)
        {
            // Cleanup
            if (_mapContainer != null)
            {
                GameObject newMapContainer = _mapContainer;
                DestroyImmediate(newMapContainer);
            }
            _mapArray = null;
            _mapTiles = null;

            _mapContainer = new GameObject("Map Container");
            _mapArray = ParseMapString(Maps.Array[index]);
            _mapTiles = ParseMapChars(_mapArray);
            SpawnTiles(_mapTiles);
            Invoke("PlacePlayer",0.0f);
            _closedGate = GameObject.FindWithTag("gate_closed");
            _openGate = GameObject.FindWithTag("gate_open");

            UnlockExit(false);

            LoadedLevel = index;
        } else
        {
            SceneManager.LoadScene(0);
        }
    }

    private void PlacePlayer()
    {
        if (Player != null && Player.ActivePlayer == null)
        {
            Player p = Instantiate(Player, GetTileScenePosition((int)PlayerStartPos.y, (int)PlayerStartPos.x), Quaternion.identity);
            p.PositionTileX = (int)PlayerStartPos.x;
            p.PositionTileY = (int)PlayerStartPos.y;
        } else if(Player.ActivePlayer != null)
        {
            Player.ActivePlayer.MoveTo((int)PlayerStartPos.y, (int)PlayerStartPos.x);
            Player.ActivePlayer.ResetHealth();
        } else
        {
            Debug.LogWarning("Player not defined for MapManager");
        }
    }

    /// <summary>
    /// 1/3: Takes in a string definition of a map and returns an array of characters with the same data
    /// </summary>
    /// <param name="mapStr"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 2/3: Takes an array of map symbols and produces a table of the Tile types
    /// </summary>
    /// <param name="charMap"></param>
    /// <returns></returns>
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
                if (charMap[i][j].Equals('@'))
                {
                    PlayerStartPos.x = j;
                    PlayerStartPos.y = i;
                }
                // Find corresponding tile and put it to the array
                foreach (Tile t in TileDefinitions)
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
    
    /// <summary>
    /// 3/3: Takes a table of Tile types and instantiates a game object with the tiles
    /// </summary>
    /// <param name="tileMap"></param>
    void SpawnTiles(Tile[][] tileMap)
    {
        maxX = tileMap.Length;
        maxY = tileMap[0].Length;

        Vector3 spawnPosition = new Vector3();
        for (int i = 0; i < tileMap.Length; i++)
        {
            for (int j = 0; j < tileMap[i].Length; j++)
            {
                if (tileMap[i][j] != null)
                {
                    spawnPosition = GetTileScenePosition(i, j);
                    Tile tileTransform = Instantiate(tileMap[i][j], spawnPosition, Quaternion.identity);
                    tileMap[i][j] = tileTransform;
                    tileTransform.transform.SetParent(_mapContainer.transform);

                    if (tileTransform.EnemyOnTile != null)
                    {
                        Enemy e = Instantiate(tileTransform.EnemyOnTile, tileTransform.transform.position, Quaternion.identity);
                        tileTransform.EnemyOnTile = e;
                        Transform t = e.transform;
                        t.SetParent(tileTransform.transform);
                    }
                    if (tileTransform.ItemOnTile != null)
                    {
                        Item itm = Instantiate(tileTransform.ItemOnTile, tileTransform.transform.position + Vector3.up, tileTransform.ItemOnTile.transform.rotation);
                        tileTransform.ItemOnTile = itm;
                        itm.transform.SetParent(tileTransform.transform);
                    }
                }
            }
        }
    }

    internal void Exit()
    {
        if (_exitUnlocked)
        {
            LoadNextLevel();
        } else
        {
            // TODO prompt
            Debug.LogFormat("<color='orange'>Find the key!</color>");
        }
    }

    internal void UnlockExit(bool unlock)
    {
        _exitUnlocked = unlock;
        _closedGate.GetComponent<Renderer>().enabled = !unlock;
        _openGate.GetComponent<Renderer>().enabled = unlock;
    }

    public int maxX = 0;
    public int maxY = 0;

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
        Vector3 scenePos = new Vector3(y - maxY / 2f + 0.5f, 0, maxX / 2f - x);
        return scenePos;
    }
}
