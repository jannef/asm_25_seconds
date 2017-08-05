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
            Debug.Log(string.Format("Lock set! {0} -> {1}", _movementInProgress, value));
            if (OnMovementStarted != null && _movementInProgress && !value)
            {
                Debug.Log("Completed triggered next");
                // falling edge for the lock
                OnMovementCompleted.Invoke();
            }
            else if (OnMovementStarted != null && !_movementInProgress && value)
            {
                Debug.Log("Started triggered next");
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

    private bool _movementInProgress;
    private Coroutine _movementCoroutine;

    public static Player ActivePlayer;
    public int PositionTileY;
    public int PositionTileX;
    public int MoveLimit = -1;
    private PlayerAttributes _attr;
    private TimerHealth _timer;

    void Awake()
    {
        _attr = GetComponent<PlayerAttributes>();
        _timer = GetComponent<TimerHealth>();
        
    }

    public void ResetHealth()
    {
        _timer.ResetTime();
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

        if (!MovementInProgress)
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

            //CheckTileForEvent(targetTile); // Redundant/double dip, IsPassable() parses actions

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

    public void MoveTo(int targetX, int targetY)
    {
        _movementInProgress = false;
        if (_movementCoroutine != null)
        {
            StopCoroutine(_movementCoroutine);
        }
        pooledCommand.lifetime = -1.0f;
        PositionTileX = targetY;
        PositionTileY = targetX;
        StartCoroutine(InstantMove(PositionTileX, PositionTileY));
        //StartMovementAnimation(PositionTileY, PositionTileX, 1);
    }

    private IEnumerator InstantMove(int targetX, int targetY)
    {
        yield return new WaitForFixedUpdate();
        transform.position = MapManager.Instance.GetTileScenePosition(targetY, targetX);
    }

    private void StartMovementAnimation(int targetY, int targetX, int distSqres)
    {
        var target = MapManager.Instance.GetTileScenePosition(targetY, targetX);
        _movementCoroutine = StartCoroutine(MovementAnimation(target, distSqres));
    }

    private IEnumerator MovementAnimation(Vector3 targetPosition, int distanceInSquares)
    {
        MovementInProgress = true;
        
        if (distanceInSquares > 0 && AnimationSpeed > 0f)
        {
            var originalPosition = transform.position;
            var stateTime = 0.0f;
            var duration = ((float)(distanceInSquares)) / AnimationSpeed;

            while (stateTime < duration && MovementInProgress)
            {
                transform.position = Vector3.Lerp(originalPosition, targetPosition, MovementCurve.Evaluate(stateTime / duration));
                stateTime += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPosition;
        }
        MovementInProgress = false;
    }


    private bool IsPassable(Tile tile)
    {
        Tile.MovementEvent ev = CheckTileForEvent(tile);
        return tile != null && (ev == Tile.MovementEvent.Pass || ev == Tile.MovementEvent.Item || ev == Tile.MovementEvent.Enemy || ev == Tile.MovementEvent.Exit);
    }
    private Tile.MovementEvent CheckTileForEvent(Tile tile)
    {
        if(tile == null)
        {
            return Tile.MovementEvent.None;
        }
        Tile.MovementEvent mvEv = tile.GetMovementEvent();

        switch (mvEv)
        {
            case Tile.MovementEvent.Item:
                tile.ItemOnTile.PickUp();

                break;
            case Tile.MovementEvent.Exit:
                MapManager.Instance.Exit();
                break;
            case Tile.MovementEvent.Enemy:
                if (!Fight(tile))
                {
                    return Tile.MovementEvent.Blocked;
                }
                break;
        }

        return mvEv;
    }

    private bool Fight(Tile tileEnemy)
    {
        if(tileEnemy == null)
        {
            Debug.LogWarning("Tried to fight a null enemy!");
            return true;
        }
        float dmg = tileEnemy.EnemyOnTile.GetAttackPower();
        _attr.TakeDamage(dmg);
        Debug.LogFormat("<color='red'>ENEMY ATTACKED</color>");
        return tileEnemy.EnemyOnTile.TakeDamage(_attr.GetPlayerAttack());
    }
    void OnDeath()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }
}
