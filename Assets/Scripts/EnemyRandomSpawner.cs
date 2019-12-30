using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class EnemyRandomSpawner : Spawner
{

    private void Awake()
    {
        Activate();
    }

    protected override void Spawn()
    {
        Instantiate(GetRandomObject(), GetRandomPosition(), Quaternion.identity);
    }

}
