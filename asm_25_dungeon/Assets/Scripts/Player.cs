using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public enum MovementEvent { None, Blocked, Pass, Enemy, Item, Exit }
    public int PositionTileY = 0;
    public int PositionTileX = 0;
    public int MoveLimit = -1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Move(0, 1);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Move(0, -1);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Move(-1, 0);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Move(1, 0);
        }
	}

    public void Move(float directionX, float directionY)
    {
        directionX = Mathf.Sign(directionX);
        directionY = Mathf.Sign(directionY);
        Move((int)directionX, (int)directionY);
    }
    public void Move(int directionX, int directionY)
    {
        int targetTileY = PositionTileY;
        int targetTileX = PositionTileX;
        if (directionX == 0 && directionY == 0)
        {
            return;
        }
        if(directionX != 0 && directionY != 0)
        {
            directionX = 0;
        }
        // Direction fix
        directionY *= -1;
        int steps = 0;
        TileBind targetTile;
        while (IsPassable(targetTile = MapManager.Instance.GetTileAt(targetTileY+directionY, targetTileX+directionX)) && (steps < MoveLimit || MoveLimit < 0))
        {
            targetTileY += directionY;
            targetTileX += directionX;
            steps++;
        }

        switch (CheckTileForEvent(targetTile))
        {
            case MovementEvent.Item:
                MapManager.Instance.UnlockExit(true);
                break;
            case MovementEvent.Exit:
                MapManager.Instance.Exit();
                break;
            case MovementEvent.Enemy:
                Fight(targetTile);
                break;
        }

        transform.position = MapManager.Instance.GetTileScenePosition(targetTileY, targetTileX);
        PositionTileY = targetTileY;
        PositionTileX = targetTileX;
    }

    private bool IsPassable(TileBind tile)
    {
        return CheckTileForEvent(tile) == MovementEvent.Pass;
    }
    private MovementEvent CheckTileForEvent(TileBind tile)
    {
        // TODO set and fetch event from the tile's object
        if (tile.Key.Equals('*'))
        {
            return MovementEvent.Pass;
        }
        else if (tile.Key.Equals('#'))
        {
            return MovementEvent.Exit;
        } else if (tile.Key.Equals('$'))
        {
            return MovementEvent.Item;
        }
        else
        {
            return MovementEvent.Blocked;
        }
    }

    private void Fight(TileBind enemyTile)
    {
        // TODO Encounter
        Debug.LogFormat("[color='red']ENEMY[/color]");
    }
}
