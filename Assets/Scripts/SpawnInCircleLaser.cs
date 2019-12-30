using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnInCircleLaser : LaserBeam
{
    public float explosionTimer = 1f;
    public int numObjects = 8;
    
    public Transform projectile1;

    private void Start()
    {
        float randomTimer = Random.Range(0.8f, 2f);
        explosionTimer = randomTimer;
    }

    // Update is called once per frame
    void Update()
    {
        explosionTimer -= Time.deltaTime;
        if (explosionTimer <= 0)
        {
            Spawn(8);
        }
    }
    
    public void Spawn(int numObjects)
    {
        for (int i = 0; i < numObjects; i++)
        {
            float angle = i * Mathf.PI*2f / numObjects;
            InstantiateBalls(angle);
            //StartCoroutine(InstantiateBalls(angle, 1f));
            //delay += 0.4f;
        }
        Destroy(gameObject);
    }
    
    void InstantiateBalls(float angle)
    {
        float radius = 0.1f;
        Vector3 newPos = new Vector3(Mathf.Cos(angle)*radius, Mathf.Sin(angle)*radius, 0);
        Transform laserBeam = Instantiate(projectile1, transform.position + newPos, Quaternion.identity);
        Vector2 direction = new Vector2(Mathf.Cos(angle) * 0.5f, Mathf.Sin(angle) * 0.5f);
        if (laserBeam.GetComponent<LaserBeam>())
            laserBeam.GetComponent<LaserBeam>().Launch(direction);
        
    }
}
