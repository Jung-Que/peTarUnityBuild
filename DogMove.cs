using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class DogMove : MonoBehaviour
{
    public enum DogState
    {
        Idle,
        Walk,
        Eat
    }

    Transform target;
    public float speed = 3.0f;
    private Transform targetTomove = null;
    private Animator anim;

    private DogState currentState = DogState.Idle;

    void Start()
    {
        target = Camera.main.transform;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (modeChg.mode == 0)
        {
            transform.LookAt(target);
            return;
        }

        if (targetTomove != null)
        {
            MoveToPosition(targetTomove.position);
        }
        if (targetTomove == transform)
        {
            targetTomove = null;
        }

        UpdateAnimationState();
    }

    void MoveToPosition(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        transform.position += transform.forward * speed * Time.deltaTime;
        anim.SetBool("Walking", true);

    }

    void UpdateAnimationState()
    {
        switch (currentState)
        {
            case DogState.Idle:
                anim.SetBool("Walking", false);
                anim.SetBool("Eating", false);
                anim.SetBool("Sneezing", false);
                break;
            case DogState.Walk:
                anim.SetBool("Walking", true);
                anim.SetBool("Eating", false);
                anim.SetBool("Sneezing", false);
                break;
            case DogState.Eat:
                anim.SetBool("Walking", false);
                anim.SetBool("Eating", true);
                anim.SetBool("Sneezing", false);
                break;
        }
    }

    public void FindTargetGoal(GameObject flag)
    {
        targetTomove = flag.transform;

        // 각 상태에 따라 애니메이션 상태 변경
        if (currentState == DogState.Idle)
        {
            currentState = DogState.Walk;
        }
        else if (currentState == DogState.Walk)
        {
            currentState = DogState.Eat;
        }
    }

    public void TriggerSneeze()
    {
         anim.SetBool("Sneezing", true);
    }

}
