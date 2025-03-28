using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunnerAnimationEvent : MonoBehaviour
{
    PlayerCtrl player;
    public GameObject shotGun;
    public GameObject sniperRifle;
    public GameObject rifle;
    public Transform shotgunHolster;
    public Transform shotgunGrip;
    void Awake()
    {
        player = GetComponentInParent<PlayerCtrl>();
    }

    public void EquipShotgun()
    {
        shotGun.transform.parent = shotgunGrip;
        shotGun.transform.localPosition = Vector3.zero;
        shotGun.transform.localRotation = Quaternion.identity;
        shotGun.transform.localScale = Vector3.one;
    }

    public void UnequipShotgun()
    {
        shotGun.transform.parent = shotgunHolster;
        shotGun.transform.localPosition = Vector3.zero;
        shotGun.transform.localRotation = Quaternion.identity;
        shotGun.transform.localScale = Vector3.one;
    }
}
