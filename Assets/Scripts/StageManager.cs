using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class StageManager : MonoBehaviour
{

    #region STAGE MANAGER PROPERTY

    private static StageManager _SM;

    public static StageManager SM
    {
        get
        {
            if (_SM == null)
                _SM = FindObjectOfType<StageManager>();
            return _SM;
        }
    }

    #endregion

    public UnityEvent StageClearedEvent = new UnityEvent();

    [Header("Asteroid Spawning Data")]

    public ObstacleSpawner AsteroidSpawner;
    public float AsteroidSpawnerPeriod;

    [Header("Boss Data")]

    public Transform EnemyBossPrefab;

    private bool _enemyBossCreated;

    private int _enemiesCreated;
    private int _enemiesKilled;

    public float _timer;

    private void Awake()
    {

        #region STAGE MANAGER PROPERTY SET-UP

        if (_SM == null)
            _SM = this;

        if (_SM.Equals(this) == false)
            Destroy(gameObject);

        #endregion

        if (AsteroidSpawner)
            AsteroidSpawner.Deactivate();

        EnemyBossLifeManager.BossKilledEvent.AddListener(BossKilledHandler);
//        EnemyLifeManager.EnemyCreatedEvent.AddListener(EnemyCreatedHandler);
//        EnemyLifeManager.EnemyKilledEvent.AddListener(EnemyKilledHandler);
        EnemySpawner.SpawnBossEvent.AddListener(SpawnAsteroids);

        //last enemy create bossa??
    }

    private void Update()
    {

        if (_timer > 0)
        {

            _timer -= Time.deltaTime;

            if (_timer <= 0)
                CreateBoss();

            else if (_timer <= 2.5f)
                AsteroidSpawner.Deactivate();

        }

    }

    private void BossKilledHandler()
    {
        StageClearedEvent.Invoke();
    }

    private void EnemyCreatedHandler()
    {
        _enemiesCreated++;
    }

    private void EnemyKilledHandler()
    {

        _enemiesKilled++;

        if (_enemiesKilled == _enemiesCreated)
        {
            _timer = AsteroidSpawnerPeriod + 2.5f;
            AsteroidSpawner.Activate();
        }

    }

    private void CreateBoss()
    {
        _enemiesCreated++;
        //set boss in editor
        Instantiate(EnemyBossPrefab);
    }

    private void SpawnAsteroids()
    {
        _timer = AsteroidSpawnerPeriod + 2.5f;
        AsteroidSpawner.Activate();
    }

}
