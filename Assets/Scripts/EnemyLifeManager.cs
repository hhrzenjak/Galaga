using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class EnemyLifeManager : LifeManager
{

    public static UnityEvent EnemyCreatedEvent = new UnityEvent();
    public static UnityEvent EnemyKilledEvent = new UnityEvent();

    public int Points;
    public int PickUpSpawnPossibility = 20;

    public Transform deathParticles;

    private bool _dead;

    private void Start()
    {
        EnemyCreatedEvent.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<LifeManager>())
                other.GetComponent<LifeManager>().Hit();
            Hit();
        }

        if (other.CompareTag("Screen"))
            _active = true;

    }
//
//    private void OnTriggerExit2D(Collider2D other)
//    {
//        if (other.CompareTag("Screen"))
//            Destroy(gameObject);
//    }

    public override void Hit()
    {

        if (!_active)
            return;

        _lives--;
        Shake();

        if (_dead == false && _lives <= 0)
        {

            _dead = true;
            EnemyDead();
            
        }

    }

    private void EnemyDead()
    {
        if (GameManager.GM)
            GameManager.GM.AddToScore(Points);
            
        Instantiate(deathParticles, transform.position, Quaternion.identity);

        //spawn pickup
        int value = Random.Range(0, 100);
        if (PickUpSpawnPossibility >= value && PickUpGenerator.Generator)
            Instantiate(PickUpGenerator.Generator.GeneratePickUp(), transform.position, Quaternion.identity);

        EnemyKilledEvent.Invoke();
        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    
    private void OnBecameVisible()
    {
        //pocni tek tafa pucati
        Activate();
    }
}
