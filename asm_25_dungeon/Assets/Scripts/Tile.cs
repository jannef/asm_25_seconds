using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
    public enum MovementEvent { None, Blocked, Pass, Enemy, Item, Exit }
    public char Key;
    public Enemy EnemyOnTile;
    [SerializeField]
    private MovementEvent _moveEvent = MovementEvent.Pass;
    public MovementEvent GetMovementEvent()
    {
        if(EnemyOnTile != null)
        {
            return MovementEvent.Enemy;
        }
        return _moveEvent;
    }
    private void Start()
    {
    }
}
