using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(TextMesh))]
public class LevelNumberDisplay : MonoBehaviour {
    TextMesh _tm;
	// Use this for initialization
	void Awake() {
        _tm = GetComponent<TextMesh>();
	}
    private void Start()
    {
        _tm.text = (MapManager.Instance.LoadedLevel+1).ToString();
    }    
}
