using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDoor : MonoBehaviour
{
    public bool isOpen = false;
    public Animator anim;  // Animation -> Animator로 변경

    public BoxCube[] cubes;  // 큐브들을 배열로 참조

    private void Awake() {
        anim = GetComponent<Animator>();  // Animator 컴포넌트를 가져옴
    }

    private void Start() {
        isOpen = false;  // 게임 시작 시 문을 닫혀 있도록 설정
    }

    private void Update() {
        // 모든 큐브가 완료되었는지 확인
        bool allCubesCompleted = true;

        foreach (BoxCube cube in cubes) {
            if (!cube.isCompleted) {
                allCubesCompleted = false;
                break;  // 하나라도 완료되지 않으면 종료
            }
        }

        if (allCubesCompleted && !isOpen) {
            OpenDoor();
        } else if (!allCubesCompleted && isOpen) {
            CloseDoor();
        }
    }

    private void OpenDoor() {
        Debug.Log("GateOpen");
        anim.SetTrigger("Open");  // Animator에서 트리거를 이용해 애니메이션 실행
        isOpen = true;
    }

    private void CloseDoor() {
        Debug.Log("GateClose");
        anim.SetTrigger("Close");  // Animator에서 트리거를 이용해 애니메이션 실행
        isOpen = false;
    }
}
