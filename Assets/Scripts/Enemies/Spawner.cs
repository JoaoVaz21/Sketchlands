using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    [SerializeField] private GameObject _prefab;
    [SerializeField] private Vector2 _velocity;
    [SerializeField] private float delay;
    [SerializeField] private float time;

    // Start is called before the first frame update
    void Start()
    {
  
    }
    private void Awake()
    {

        InvokeRepeating("CreateProjectile", delay, time);
    }
    private void CreateProjectile()
    {

            var gameObject = Instantiate(_prefab,this.transform);
            gameObject.GetComponent<IVelocitySettable>().SetVelocity(_velocity);
        
    }
}
