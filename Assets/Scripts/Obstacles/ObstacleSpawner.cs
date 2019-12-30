using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ObstacleSpawner : Spawner
{

    public Vector2 FallDirection;

    protected override void Spawn()
    {

        Transform obstacle = Instantiate(GetRandomObject(), GetRandomPosition(), Quaternion.identity);

        if (obstacle.GetComponent<Obstacle>())
            obstacle.GetComponent<Obstacle>().Launch(FallDirection);

    }

}
