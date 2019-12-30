using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public abstract class Spawner : MonoBehaviour
{

    public List<Transform> ObjectsToSpawn = new List<Transform>();

    public Vector2 SpawnAreaX;
    public Vector2 SpawnAreaY;

    public Vector2 SpawnInterval;

    private bool _active;
    private float _timer;

    private void Update()
    {

        if (_active == false)
            return;

        _timer -= Time.deltaTime;

        if (_timer <= 0)
        {
            _timer = SpawnInterval.RandomValue();
            Spawn();
        }

    }

    protected Transform GetRandomObject()
    {
        return ObjectsToSpawn[new Vector2Int(0, ObjectsToSpawn.Count).RandomValue()];
    }

    protected Vector2 GetRandomPosition()
    {
        return new Vector2(SpawnAreaX.RandomValue(), SpawnAreaY.RandomValue());
    }

    protected abstract void Spawn();

    public void Activate()
    {
        _active = true;
        _timer = 0;
    }

    public void Deactivate()
    {
        _active = false;
    }

}
