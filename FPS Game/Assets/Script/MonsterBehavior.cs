using UnityEngine;
public class MonsterBehavior : MonoBehaviour
{
    private string playerTag = "Player";
    public int health = 100;
    public float speed = 5f;
    public float attackRange = 2f;
    public float minDistance = 3f; 
    public float attackCooldown = 1.5f; // Duration between attacks in seconds
    private float nextAttackTime = 0f; // Time when the next attack can be performed

    private GameObject player;
    private bool isAttacking = false;
    private bool isMoving = false;
    private bool isDead = false;
    private CharacterController characterController;
    private GameObject sword;
    private MeleeAtk meleeAtk;
    private PlayerHealthManager playerHealthManager;
    void Start()
    {
        GameObject gameManager = GameObject.FindWithTag("GameManager");
        if (gameManager != null)
        {
            playerHealthManager = gameManager.GetComponent<PlayerHealthManager>();
        }
        sword = GameObject.FindGameObjectWithTag("Sword");

        if (sword != null)
        {
            // Find the MeleeAtk script on the sword GameObject
            meleeAtk = sword.GetComponent<MeleeAtk>();

            if (meleeAtk == null)
            {
                meleeAtk = sword.GetComponentInParent<MeleeAtk>();
            }
            if (meleeAtk == null)
            {
                meleeAtk = sword.GetComponentInChildren<MeleeAtk>();
            }
        }
        player = GameObject.FindGameObjectWithTag(playerTag);
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        
        if (!isDead && !isAttacking)
        {
            if (player != null)
            {
                float distance = Vector3.Distance(transform.position, player.transform.position);

                
                if (distance > minDistance)
                {
                    transform.LookAt(player.transform);
                    characterController.Move(transform.forward * speed * Time.deltaTime);
                    isMoving = true;
                }
                else
                {
                    isMoving = false;
                }
            }
            else
            {
                isMoving = false;
            }
        }

        GetComponent<Animator>().SetBool("IsMoving", isMoving);
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance <= attackRange && distance >= minDistance && !isAttacking && !isDead && Time.time >= nextAttackTime)
            {
                isAttacking = true;
                isMoving = false;
                GetComponent<Animator>().SetTrigger("Attack");
            }
            else
            {
                isAttacking = false;
                isMoving = true;
            }
        }
    }
    public void ApplyDamage()
    {
        if (playerHealthManager != null && !isDead)
        {
            playerHealthManager.TakeDamage(10);
            nextAttackTime = Time.time + attackCooldown; // Set the time for the next attack
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            health -= 15;
            if (health > 0)
            {
                GetComponent<Animator>().SetTrigger("Damaged");
                isMoving = false;
            }

            if (health <= 0 && !isDead)
            {
                isDead = true;
                GetComponent<Animator>().SetTrigger("Death");
                Destroy(gameObject, 5f);
            }
        }
        if (other.CompareTag("Sword") &&  meleeAtk.Attack)
        {
            Debug.Log("Sword Hit Reg");
            health -= 25;
            meleeAtk.Attack = false;
            if (health > 0)
            {
                GetComponent<Animator>().SetTrigger("Damaged");
                isMoving = false;
            }

            if (health <= 0 && !isDead)
            {
                isDead = true;
                GetComponent<Animator>().SetTrigger("Death");
                Destroy(gameObject, 5f);
            }
        }
    }
}
