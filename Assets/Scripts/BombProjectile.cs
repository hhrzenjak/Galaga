using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BombProjectile : LaserBeam
{
    public Transform ExplosionParticleEffect;
    public float explodeTimer = 1f;

    private void Start()
    {
        float randomTimer = Random.Range(1.3f, 2f);
        explodeTimer = randomTimer;
    }

    public void FixedUpdate()
    {
        explodeTimer -= Time.deltaTime;
        if (explodeTimer <= 0)
        {
            Detonate();
        }
    }

    private void Detonate()
    {
        Instantiate(ExplosionParticleEffect, transform.position, transform.rotation);
        //check  if player inside explosion radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1.8f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.tag.Equals("Player"))
            {
                collider.gameObject.GetComponent<PlayerLifeManager>().Hit();
            }
        }

        Destroy(gameObject);
    }
}