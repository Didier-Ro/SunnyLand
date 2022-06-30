using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rigidBody2D = default;
    private Animator _animator = default;

    [Header("Configuracion Personaje")]
    [SerializeField] private float playerSpeed = default;
    [SerializeField] private float jumpForce = default;
    [Header("Checador Suelo")]
    [SerializeField] private Vector3 checkGroundPosition = default;
    [SerializeField] private float checkGroudnRadious = default;
    [SerializeField] private bool isGround = default;
    [SerializeField] private LayerMask layerMask = default;
    [Header("Agachado")]
    private bool isDown = false;

    private void Start()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if(isDown == false)
        {
            Vector2 vel = _rigidBody2D.velocity;
            vel.x = Input.GetAxisRaw("Horizontal") * playerSpeed;
            _rigidBody2D.velocity = vel;

            Physics2D.queriesHitTriggers = false;
            isGround = Physics2D.OverlapCircle(transform.position + checkGroundPosition, checkGroudnRadious, layerMask);
            Physics2D.queriesHitTriggers = true;

            _animator.SetInteger("Velocidad", Mathf.FloorToInt(Mathf.Abs(vel.x)));
            Flip(vel.x);
        }
    }

    void Update()
    {
        _animator.SetBool("Agachado", isDown);

        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            _rigidBody2D.AddForce(Vector2.up * jumpForce);
        }

        if (Input.GetKey(KeyCode.S) && isGround)
        {
            isDown = true;
        }
        else
        {
            isDown = false;
        }
    }

    void Flip(float dir)
    {
        Vector3 LocalScale = transform.localScale;
        if (dir > 0)
        {
            LocalScale.x = 1f;
        }
        else if (dir < 0)
        {
            LocalScale.x = -1;
        }

        transform.localScale = LocalScale;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + checkGroundPosition, checkGroudnRadious);
    }
}