using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public static class Extensions
{

    public static float RandomValue(this Vector2 range)
    {
        return Random.Range(range.x, range.y);
    }

    public static int RandomValue(this Vector2Int range)
    {
        return Random.Range(range.x, range.y);
    }

}
