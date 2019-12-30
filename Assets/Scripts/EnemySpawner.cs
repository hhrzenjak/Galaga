using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = System.Random;

public class EnemySpawner : MonoBehaviour
{
    public enum SpawnState
    {
        SPAWNING,
        WAITING,
        COUNTING,
        OVER
    };
    
    [Serializable]
    public class Wave
    {
        public string Name;
        public Transform Enemy;
        public int Count;
        public float delay;
//        public float horizontalSpeed;
//        public float verticalSpeed;
//        public float amplitude;

    }

    public Wave[] Waves;
    public Transform[] SpawnPoints;
    private int _nextWave = 0;
    
    public float timeBetweenWaves = 8f;
    public float waveCountdown;

    private float searchCountdown = 1f;
    public bool mainSpawner = false;

    public SpawnState state = SpawnState.COUNTING;
    
    public static UnityEvent SpawnBossEvent = new UnityEvent();

    private void Awake()
    {
        PlayerLifeManager.GameOverEvent.AddListener(StopSpawning);
    }

    private void Start()
    {
        waveCountdown = timeBetweenWaves;
    }

    private void Update()
    {
        if (state == SpawnState.WAITING)
        {
            WaveCompleted();   
        }
        
        if (waveCountdown <= 0 && state!=SpawnState.OVER)
        {
            

            if (state != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnWave(Waves[_nextWave]));
            }

                
        }
        else if(state!=SpawnState.OVER)
        {
            waveCountdown -= Time.deltaTime;
        }
    }

    IEnumerator SpawnWave(Wave wave)
    {
        state = SpawnState.SPAWNING;

        for (int i = 0; i < wave.Count; i++)
        {
            SpawnEnemy(wave.Enemy, i%SpawnPoints.Length);
            //wait between enemies of one wave
            yield return new WaitForSeconds(wave.delay);
        }

        state = SpawnState.WAITING;
    }

    void SpawnEnemy(Transform enemy, int spawnPointIndex)
    {
        float valueY = UnityEngine.Random.Range(-1.3f, 1.3f);
        float valueX = UnityEngine.Random.Range(-1f, 1f);
        Transform spawnLocation = SpawnPoints[spawnPointIndex] ;
        Instantiate(enemy, spawnLocation.position + new Vector3(valueX, valueY, 0), Quaternion.identity);
    }

    void WaveCompleted()
    {
        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        if (_nextWave + 1 > Waves.Length - 1)
        {
            state = SpawnState.OVER;
            if(mainSpawner) 
                StartCoroutine(Delay());
        }
        else
        {
            _nextWave++;
        }
        
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(5f);
        SpawnBossEvent.Invoke();
    }
    
    //if we want to wait until all enemies are destroyed
    private bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
                return false;
        }
        return true;
    }

    private void StopSpawning()
    {
        state = SpawnState.OVER;
    }
}
