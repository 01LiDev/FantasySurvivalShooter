using System.Collections;
using UnityEngine;
public class MeleeAtk : MonoBehaviour
{
    [SerializeField] private GameObject gameManager;

    public Animator animator;
    public float triggerDelay = 2.5f; // Add custom delay (in seconds) for the trigger
    private new Collider collider;
    public GameObject targetObject; // Assign the target object in the inspector

    public bool Attack = false;
    void Start()
    {

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            PerformAttack();
        }
    }

    private void PerformAttack()
    {
        if (targetObject == null)
        {
            Debug.LogWarning("Target object is not assigned");
            return;
        }
        Attack = true;
        animator.SetTrigger("Attack");
        StartCoroutine(EnableColliderDuringAnimation());
    }

    private IEnumerator EnableColliderDuringAnimation()
    {
        if (collider == null)
        {
            collider = targetObject.GetComponent<Collider>();
        }
        collider.isTrigger = true;

        // Use custom delay instead of animation length
        yield return new WaitForSeconds(triggerDelay);

        collider.isTrigger = false;
        Attack = false;
    }

}
