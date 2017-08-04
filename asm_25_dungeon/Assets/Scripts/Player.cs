using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerAttributes))]
[RequireComponent(typeof(TimerHealth))]

public class Player : MonoBehaviour
{
    public UnityEvent OnMovementCompleted;
    public UnityEvent OnMovementStarted;

    [SerializeField, Range(0.1f, 50f)] protected float AnimationSpeed = 10.0f;
    [SerializeField] protected AnimationCurve MovementCurve;
    [SerializeField] protected float InputBufferingDuration = 0.25f;

    private MovementCommand pooledCommand;
    

    public bool MovementInProgress
    {
        get { return _movementInProgress; }

        private set
        {
            if (OnMovementStarted != null && _movementInProgress && !value)
            {
                // falling edge for the lock
                OnMovementCompleted.Invoke();
            }
            else if (OnMovementStarted != null && !_movementInProgress && value)
            {
                // rising edge of the lock
                OnMovementStarted.Invoke();
            }
            _movementInProgress = value;
        }
    }

    struct MovementCommand
    {
        public int y;
        public int x;
        public float lifetime;
    }

    private bool _movementInProgress = false;

    public static Player ActivePlayer;
    public int PositionTileY = 0;
    public int PositionTileX = 0;
    public int MoveLimit = -1;
    private PlayerAttributes _attr;

    void Awake()
    {
        _attr = GetComponent<PlayerAttributes>();
    }

	// Use this for initialization
	void Start () {
        ActivePlayer = this;
	    pooledCommand = new MovementCommand
	    {
	        x = 0,
	        y = 0,
	        lifetime = -1.0f
	    };
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
	    else if (Input.GetKeyDown(KeyCode.S))
	    {
	        Move(0, -1);
	    }
	    else if (Input.GetKeyDown(KeyCode.A))
	    {
	        Move(-1, 0);
	    }
	    else if (Input.GetKeyDown(KeyCode.D))
	    {
	        Move(1, 0);
	    }

        if (!_movementInProgress)
	    {
	        if (pooledCommand.lifetime > 0f)
	        {
	            Move(pooledCommand.x, pooledCommand.y);
	            pooledCommand.lifetime = -1f;
	        }
	    }

        pooledCommand.lifetime -= Time.deltaTime;
	}

    public void Move(float directionX, float directionY)
    {
        directionX = Mathf.Sign(directionX);
        directionY = Mathf.Sign(directionY);
        Move((int)directionX, (int)directionY);
    }
    public void Move(int directionX, int directionY)
    {
        if (!MovementInProgress)
        {
            int targetTileY = PositionTileY;
            int targetTileX = PositionTileX;
            if (directionX == 0 && directionY == 0)
            {
                return;
            }
            if (directionX != 0 && directionY != 0)
            {
                directionX = 0;
            }

            // Direction fix
            directionY *= -1;
            int steps = 0;
            Tile targetTile;
            while (IsPassable(targetTile =
                       MapManager.Instance.GetTileAt(targetTileY + directionY, targetTileX + directionX)) &&
                   (steps < MoveLimit || MoveLimit < 0))
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

            StartMovementAnimation(targetTileY, targetTileX, steps);
            PositionTileY = targetTileY;
            PositionTileX = targetTileX;
        }
        else
        {
            pooledCommand.x = directionX;
            pooledCommand.y = directionY;
            pooledCommand.lifetime = InputBufferingDuration;
        }
    }

    private void StartMovementAnimation(int targetY, int targetX, int distSqres)
    {
        var target = MapManager.Instance.GetTileScenePosition(targetY, targetX);
        StartCoroutine(MovementAnimation(target, distSqres));
    }

    private IEnumerator MovementAnimation(Vector3 targetPosition, int distanceInSquares)
    {
        _movementInProgress = true;
        
        if (distanceInSquares > 0 && AnimationSpeed > 0f)
        {
            var originalPosition = transform.position;
            var stateTime = 0.0f;
            var duration = ((float)(distanceInSquares)) / AnimationSpeed;

            while (stateTime < duration)
            {
                transform.position = Vector3.Lerp(originalPosition, targetPosition, MovementCurve.Evaluate(stateTime / duration));
                stateTime += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPosition;
        }
        _movementInProgress = false;
    }


    private bool IsPassable(Tile tile)
    {
        Tile.MovementEvent ev = CheckTileForEvent(tile);
        return tile != null && (ev == Tile.MovementEvent.Pass || ev == Tile.MovementEvent.Item);
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
