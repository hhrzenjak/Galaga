using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Random = UnityEngine.Random;

public class ObstacleLifeManager : LifeManager
{

    public int Points;
    public int PickUpSpawnPossibility = 20;

    private bool _dead;

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
            if (other.GetComponent<LifeManager>())
                other.GetComponent<LifeManager>().Hit();

        if (other.CompareTag("Screen"))
            _active = true;

    }

    public override void Hit()
    {

        if (_active == false)
            return;

        _lives--;
        Shake();

        if (_dead == false && _lives <= 0)
        {

            _dead = true;

            if (GameManager.GM)
                GameManager.GM.AddToScore(Points);
            
            int value = Random.Range(0, 100);

            
            if (PickUpSpawnPossibility >= value && PickUpGenerator.Generator)
                Instantiate(PickUpGenerator.Generator.GeneratePickUp(), transform.position, Quaternion.identity);

            Destroy(gameObject);

        }

    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
