using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMe : MonoBehaviour
{
    public Transform target;
    public float distanceFromCamera = 2.0f;
    public float speed = 1.0f;
    public Collider dogCollider;
    public Collider toolCollider;
    public Animator animator;

    private bool isMovingToTarget = false;
    private Vector3 targetPosition;
    private float arrivalTime = 5.0f;
    private float elapsedTime = 0.0f;

    private bool Walking = false;
    private bool Eating = false;
    private bool Sneezing = false;
    private bool Idle = true;

    private void Start()
    {
        if (dogCollider == null)
        {
            dogCollider = GetComponent<Collider>();
        }
    }

    private void Update()
    {
        if (target == null)
            return;

        if (isMovingToTarget)
        {
            MoveToTarget();
            return;
        }

        Vector3 targetPosition = target.position;
        Vector3 cameraPosition = Camera.main.transform.position;

        Vector3 desiredPosition = targetPosition + (cameraPosition - targetPosition).normalized * distanceFromCamera;
        Vector3 newPosition = Vector3.Lerp(transform.position, desiredPosition, speed * Time.deltaTime);
        transform.position = newPosition;

        if (dogCollider.bounds.Contains(toolCollider.bounds.min) && dogCollider.bounds.Contains(toolCollider.bounds.max))
        {
            StartMovingToTarget(toolCollider.transform.position);
        }
    }

    private void MoveToTarget()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= arrivalTime)
        {
            elapsedTime = 0.0f;
            isMovingToTarget = false;
            targetPosition = Vector3.zero;
            // 상호작용 로직을 수행하세요.
            // 예: 도구와 상호작용, 애니메이션 재생 등
            if (Sneezing)
            {
                // Sneezing 상태에서의 로직 수행
                // 예: 애니메이션 재생, 효과음 재생 등
                animator.SetBool("Walking", Walking);
                animator.SetBool("Eating", Eating);
                animator.SetBool("Sneezing", Sneezing);
                animator.SetBool("Idle", Idle);
                Sneezing = false;
            }
        }
        else
        {
            Vector3 newPosition = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
            transform.position = newPosition;
        }
    }

    private void StartMovingToTarget(Vector3 position)
    {
        isMovingToTarget = true;
        targetPosition = position;
        elapsedTime = 0.0f;
        Idle = false;
        Walking = true;
        Eating = false;
        Sneezing = false;
        animator.SetBool("Walking", Walking);
        animator.SetBool("Eating", Eating);
        animator.SetBool("Sneezing", Sneezing);
        animator.SetBool("Idle", Idle);
    }

    public void TriggerSneeze()
    {
        // Sneezing 상태를 발동시키는 로직 수행
        // 예: 외부에서 호출될 때 Sneezing 변수를 true로 설정
        Sneezing = true;
    }
}
