using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelGenerator : MonoBehaviour {
    public Text Console;
    public int Width = 6;
    public int Height = 9;
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Space))
        {
            Console.text = MapToString(Generate());

        }
	}
    char[,] Generate()
    {
        char[,] generatedMap = new char[Height, Width];
        // Initialize the map
        for(int y = 0;y < Height ; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                char c = '0';
                generatedMap[y, x] = c;
            }
        }

        for(int x = 0; x < Width; x++)
        {
            
        }

        return generatedMap;
    }
    string MapToString(char[,] map)
    {
        string str = "";
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                str += map[y, x];
            }

            str += '\n';
        }
        return str;
    }
}
