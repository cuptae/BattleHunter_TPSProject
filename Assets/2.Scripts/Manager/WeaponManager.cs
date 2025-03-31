using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance;

    [Header("Bullet")]
    [SerializeField]
    private Transform bulletPoint;
    [SerializeField]
    private GameObject bulletObj;
    [SerializeField]
    private float maxShootDelay = 0.2f;
        [SerializeField]
    private float currentShootDelay = 0.2f;
    [SerializeField]
    private Text bulletText;
    private int maxBullet = 30;
    private int currentBullet = 0;

    [Header("Weapon FX")]
    [SerializeField]
     private GameObject weaponFlashFX;
    [SerializeField]
     private Transform bulletCasePoint;
    [SerializeField]
    private GameObject bulletCaseFX;
    [SerializeField]
    private GameObject weaponClipPoint;
    [SerializeField]
    private GameObject weaponClipFX;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        currentShootDelay = 0;
        currentBullet = 0;
        InitBullet();
    }

    // Update is called once per frame
    void Update()
    {
        bulletText.text = currentBullet + " / " + maxBullet;
    }
    public void Shooting(Vector3 targetPosition)
    {
        currentShootDelay += Time.deltaTime;
        if (currentShootDelay < maxShootDelay || currentBullet <= 0)
        return ;

        currentBullet -= 1;
        currentShootDelay = 0;
   
        Instantiate(weaponFlashFX,bulletPoint);
        Instantiate(bulletCaseFX,bulletCasePoint);

        Vector3 aim = (targetPosition - bulletPoint.position).normalized;
        Instantiate(bulletObj, bulletPoint.position, quaternion.LookRotation(aim,Vector3.up));
    }

    public void ReroadClip()
    {
        Instantiate (weaponClipFX,weaponClipPoint);
        InitBullet();
    }

    private void InitBullet()
    {
        currentBullet = maxBullet;
    }

    private void Instantiate(GameObject weaponClipFX, GameObject weaponClipPoint)
    {
        throw new NotImplementedException();
    }
}
