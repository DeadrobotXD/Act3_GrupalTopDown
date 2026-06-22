using UnityEngine;

public class CharacterController: MonoBehaviour, Ivisible
{
    [SerializeField] Ivisible.Side side = Ivisible.Side.Neutral;
    [SerializeField] float movementSpeed = 3f;

    Rigidbody2D rb2D;
    Animator animator;
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        rb2D.linearVelocity = rawMove * movementSpeed;
    }

    Vector2 rawMove = Vector2.zero;
    public void SetRawMove(Vector2 rawMove)
    {
        this.rawMove = rawMove;
        animator.SetFloat("HorizontalVelocity", rawMove.x);
        animator.SetFloat("VerticalVelocity", rawMove.y);
    }

    Ivisible.Side Ivisible.GetSide()
    {
        return side;
    }

    Transform Ivisible.GetTransform()
    {
        return transform;
    }
}
