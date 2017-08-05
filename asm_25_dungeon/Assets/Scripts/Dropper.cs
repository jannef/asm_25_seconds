using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropper : MonoBehaviour
{

    [SerializeField] private GameObject PrefabToSpawn;

    public void DropIt()
    {
        if (PrefabToSpawn != null)
        {
            var go = Instantiate(PrefabToSpawn);
            go.transform.position = transform.position;
        }
    }
}
