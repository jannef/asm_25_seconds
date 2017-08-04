using UnityEngine;

[RequireComponent(typeof(PlayerAttributes))]
[RequireComponent(typeof(TimerHealth))]

public class Player : MonoBehaviour {
    public static Player ActivePlayer;
    public int PositionTileY = 0;
    public int PositionTileX = 0;
    public int MoveLimit = -1;
    private PlayerAttributes _attr;

	// Use this for initialization
	void Start () {
        ActivePlayer = this;
        _attr = GetComponent<PlayerAttributes>();
		
	}
	
    public void GainXP(int amount)
    {
        _attr.GainXP(amount);
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
        Tile targetTile;
        while (IsPassable(targetTile = MapManager.Instance.GetTileAt(targetTileY+directionY, targetTileX+directionX)) && (steps < MoveLimit || MoveLimit < 0))
        {
            targetTileY += directionY;
            targetTileX += directionX;
            steps++;
        }

        switch (CheckTileForEvent(targetTile))
        {
            case Tile.MovementEvent.Item:
                MapManager.Instance.UnlockExit(true);
                break;
            case Tile.MovementEvent.Exit:
                MapManager.Instance.Exit();
                break;
            case Tile.MovementEvent.Enemy:
                Fight(targetTile);
                break;
        }

        transform.position = MapManager.Instance.GetTileScenePosition(targetTileY, targetTileX);
        PositionTileY = targetTileY;
        PositionTileX = targetTileX;
    }

    private bool IsPassable(Tile tile)
    {
        return tile != null && CheckTileForEvent(tile) == Tile.MovementEvent.Pass;
    }
    private Tile.MovementEvent CheckTileForEvent(Tile tile)
    {
        if(tile == null)
        {
            return Tile.MovementEvent.None;
        }
        /*
        // TODO set and fetch event from the tile's object
        if (tile.Key.Equals('*'))
        {
            return Tile.MovementEvent.Pass;
        }
        else if (tile.Key.Equals('#'))
        {
            return Tile.MovementEvent.Exit;
        } else if (tile.Key.Equals('$'))
        {
            return Tile.MovementEvent.Item;
        }
        else
        {
            return Tile.MovementEvent.Blocked;
        }*/

        return tile.GetMovementEvent();
    }

    private void Fight(Tile tileEnemy)
    {
        if(tileEnemy == null)
        {
            Debug.LogWarning("Tried to fight a null enemy!");
            return;
        }
        float dmg = tileEnemy.EnemyOnTile.GetAttackPower();
        _attr.TakeDamage(dmg);
        tileEnemy.EnemyOnTile.TakeDamage(_attr.Attack);
        Debug.LogFormat("<color='red'>ENEMY ATTACKED</color>");
    }
}
