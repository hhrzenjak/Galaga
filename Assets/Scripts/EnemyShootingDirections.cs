using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Shooting Directions")]

public class EnemyShootingDirections : ScriptableObject
{

    public EnemyDifficulty Difficulty;
    public List<Vector2> ShootingDirections = new List<Vector2>();

}
