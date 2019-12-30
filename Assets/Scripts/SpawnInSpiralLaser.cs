using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnInSpiralLaser : LaserBeam
{
    public int numObjects = 20;

    public Transform projectile1;
    private int current;
    
    private void Start()
    {
        float randomTimer = Random.Range(1.2f, 2f);
        float delay = randomTimer;
        
        for (int i = 0; i < numObjects; i++)
        {
            float angle = -i * Mathf.PI*2f / numObjects;
            StartCoroutine(InstantiateBallDelay(angle, delay, i));
            delay += 0.2f;
        }
    }

    private void Update()
    {
        if(current >= numObjects - 1)
            Destroy(gameObject);
    }

    IEnumerator InstantiateBallDelay(float angle, float delay, int number)
    {
        yield return new WaitForSeconds(delay);
        float radius = 0.1f;
        Vector3 newPos = new Vector3(Mathf.Cos(angle)*radius, Mathf.Sin(angle)*radius, 0);
        Transform laserBeam = Instantiate(projectile1, transform.position + newPos, Quaternion.identity);
        Vector2 direction = new Vector2(Mathf.Cos(angle) * 0.5f, Mathf.Sin(angle) * 0.5f);
        if (laserBeam.GetComponent<LaserBeam>())
            laserBeam.GetComponent<LaserBeam>().Launch(direction);
        current = number;
    }
}
