using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro.SpriteAssetUtilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;

public class EnemyMovement : MonoBehaviour
{
    public float horizontalSpeed;
    public float verticalSpeed;
    public float amplitude;

    public Vector2 tempPosition;
    public Vector2 startPosition;
    public float timePassed = 0;
    public float distanceBeforeAttack = 3f;
    public float timeToAttack = 5f;

    public EnemyState currentEnemyState = EnemyState.ENTERING;
    
    public enum EnemyState
    {
        ENTERING,
        ATTACKING,
        LEAVING,
        OVER
    };

    private void Awake()
    {
        int value = Random.Range(0, 2);

        if (value < 1)
            verticalSpeed = -verticalSpeed;
    }

    private void Start()
    {
        tempPosition = transform.position;
        startPosition = tempPosition;

    }
    
    private void Update()
    {
        if (currentEnemyState == EnemyState.ENTERING)
        {
            timePassed += Time.deltaTime;
            tempPosition.y -= horizontalSpeed * Time.deltaTime;
            tempPosition.x = Mathf.Sin(timePassed * verticalSpeed) * amplitude + startPosition.x;
            transform.position = tempPosition;
            if (Vector2.Distance(tempPosition, startPosition) > distanceBeforeAttack)
            {
                StartCoroutine(SwitchToAttacking());
            }
        }
        else if(currentEnemyState == EnemyState.ATTACKING){
            timeToAttack -= Time.deltaTime;
            if (timeToAttack <= 0)
            {
                SwitchToExiting();
            }
            //move left/right around position
            timePassed += Time.deltaTime;
            transform.position = new Vector2(tempPosition.x + Mathf.PingPong(timePassed * 0.9f, 1),  tempPosition.y);


        }
        else if (currentEnemyState == EnemyState.LEAVING)
        {
            
        }
    }

    private IEnumerator SwitchToAttacking()
    {
        currentEnemyState = EnemyState.ATTACKING;
        timeToAttack = 5f;
        timePassed = 0;
        tempPosition = transform.position;
        yield return new WaitForSeconds(2f);

    }

    private void SwitchToExiting()
    {
        currentEnemyState = EnemyState.LEAVING;
    }
    
//    private void FixedUpdate()
//    {
//        timePassed += Time.deltaTime;
//        tempPosition.y -= horizontalSpeed * Time.deltaTime;
//        tempPosition.x = Mathf.Sin(timePassed * verticalSpeed) * amplitude + startPositionX;
//        transform.position = tempPosition;
//    }
}
