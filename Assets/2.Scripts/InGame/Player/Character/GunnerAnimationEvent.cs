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
    public Transform rifleHolster;
    public Transform rifleGrip;
    public Transform sniperHolster;
    public Transform sniperGrip;
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

    public void EquipRifle()
    {
        rifle.transform.parent = rifleGrip;
        rifle.transform.localPosition = Vector3.zero;
        rifle.transform.localRotation = Quaternion.identity;
        rifle.transform.localScale = Vector3.one;
    }

    public void UnequipRifle()
    {
        rifle.transform.parent = rifleHolster;
        rifle.transform.localPosition = Vector3.zero;
        rifle.transform.localRotation = Quaternion.identity;
        rifle.transform.localScale = Vector3.one;
    }

    public void EquipSniper()
    {
        sniperRifle.transform.parent = sniperGrip;
        sniperRifle.transform.localPosition = Vector3.zero;
        sniperRifle.transform.localRotation = Quaternion.identity;
        sniperRifle.transform.localScale = Vector3.one;
    }

    public void UnequipSniper()
    {
        sniperRifle.transform.parent = sniperHolster;
        sniperRifle.transform.localPosition = Vector3.zero;
        sniperRifle.transform.localRotation = Quaternion.identity;
        sniperRifle.transform.localScale = Vector3.one;
    }



}
