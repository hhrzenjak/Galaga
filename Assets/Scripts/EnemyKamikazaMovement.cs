using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKamikazaMovement : MonoBehaviour
{
    public Transform Target;

    public float Speed;
    public float LaunchSpeed = 200;
    
    public bool _launched;

    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        if (Target == null)
        {
            Target = GameObject.FindGameObjectWithTag("Player").transform;
        }

    }

    private void Update()
    {
        if (Target == null)
        {
            Target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        if(!_launched && Vector2.Distance(transform.position, Target.position) > 2.3f)
            transform.position = Vector2.MoveTowards(transform.position, Target.position, Speed * Time.deltaTime);
        else
        {
            if (!_launched)
            {
                _launched = true;
                Launch(Target.position - transform.position);
            }
        }
    }
    
    public void Launch(Vector2 direction)
    {
        transform.eulerAngles += new Vector3(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90);

        _rigidbody.velocity = Vector2.zero;
        _rigidbody.AddForce(direction.normalized * LaunchSpeed);

    }
    
}
