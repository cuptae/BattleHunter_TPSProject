using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoParticleReturn : MonoBehaviour
{
    void OnParticleSystemStopped()
    {
        PoolManager.Instance.ReturnObject(gameObject.name, gameObject);
    }

    void Start()
    {
        var ps = GetComponent<ParticleSystem>();
        var main = ps.main;
        main.stopAction = ParticleSystemStopAction.Callback;
    }
}
