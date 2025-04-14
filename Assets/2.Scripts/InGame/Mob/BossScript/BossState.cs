using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossState : StateMachineBehaviour
{
    protected Boss boss;
    protected List<GameObject> playersInRange = new List<GameObject>();
    protected float detectionRadius = 40f;

    protected float GetScaledDetectionRadius(Transform bossTransform)
    {
        Vector3 scale = bossTransform.localScale;
        float scaleFactor = scale.x * scale.z;
        return detectionRadius * Mathf.Sqrt(scaleFactor);
    }

    protected GameObject FindRandomTargetInRange(Transform bossTransform)
    {
        playersInRange.Clear();

        float scaledRadius = GetScaledDetectionRadius(bossTransform);
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(bossTransform.position, player.transform.position);
            if (distance <= scaledRadius)
            {
                playersInRange.Add(player);
            }
        }

        if (playersInRange.Count > 0)
        {
            int index = Random.Range(0, playersInRange.Count);
            GameObject chosen = playersInRange[index];
            boss.currentTarget = chosen;
            return chosen;
        }

        boss.currentTarget = null;
        return null;
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<Boss>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }
}
