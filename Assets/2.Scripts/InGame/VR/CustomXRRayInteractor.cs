using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CostomXRRayInteractor : XRRayInteractor
{
    private VRGun currentGun;

    protected override void OnSelectEntered(XRBaseInteractable interactable)
    {
        base.OnSelectEntered(interactable);

        if (interactable.CompareTag("intercom"))
        {
            Intercom intercom = interactable.GetComponent<Intercom>();
            if (intercom != null)
            {
                intercom.OpenDoor();
            }
        }

        if (interactable.CompareTag("gun"))
        {
            Rigidbody gun = interactable.GetComponent<Rigidbody>();
            Collider gunCollider = interactable.GetComponent<Collider>();
            gunCollider.isTrigger = true;
            gun.isKinematic = true;

            currentGun = interactable.GetComponent<VRGun>();  // ✅ 총 캐싱
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        if (args.interactable.CompareTag("gun"))
        {
            Rigidbody gun = args.interactable.GetComponent<Rigidbody>();
            Collider gunCollider = args.interactable.GetComponent<Collider>();
            gunCollider.isTrigger = false;
            gun.isKinematic = false;

            currentGun = null;
        }
    }

    // 트리거 버튼 활성화 처리
    public void OnTriggerPressed()
    {
        if (currentGun != null && currentGun.canFire)
        {
            currentGun.Fire();
        }
    }

    // 트리거 버튼 비활성화 처리
    public void OnTriggerReleased()
    {
        // 트리거 버튼이 떼어졌을 때 처리할 로직
    }
}