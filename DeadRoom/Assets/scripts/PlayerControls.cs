using UnityEngine;
using System.Collections;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class PlayerControls : MonoBehaviourPunCallbacks, IPunObservable
{
    private bool isGrounded; //переменная, которая говорит о том, стоим ли мы на земле
    public float speed;
    public float jumpSpeed;
    public Rigidbody2D rigidbody;
    private float moveInput;
   // private SpriteRenderer sprRend;
    public Animator animator;
    public GameObject walkParticle;
    public LayerMask whatIsGround; //публичная переменная, которая берёт образец земли
    public Transform groundCheck;  //объект проверки на землю
    public float checkRadius; //радиус окружности для проверки
    private Joystick joystick;
    public float jumpTime;
    public float dashSpeed;
    public float startDashTime;
    public GameObject dashParticle;
    public TrailRenderer trailRenderer;
    public GameObject cam;
    public SpriteRenderer sprite;
    public TextMeshPro Nickname;
    public static bool ButtonJump = false;
    public static bool ButtonDash = false;
    private bool Flip;
    private float jumpTimeCounter;
    private float dashTime;
    private int direction;
    private Animator camAnim;
    public PhotonView photonView;
    [SerializeField] private GameObject PlayerController;
     public static float DashStaticTime=5f;
     public static float DashTimeCounter=0;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
            stream.SendNext(Flip);
        
        if (stream.IsReading)
            Flip = (bool)stream.ReceiveNext();
        
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        trapLifetime otherTrap = other.GetComponent<trapLifetime>();
        if (other.tag == "trap" && !PhotonNetwork.IsMasterClient)
        {
            GameManager MyGM = FindObjectOfType<GameManager>();
            MyGM.Killed();
        }
    }



    void Start()
    {
        DashTimeCounter = DashStaticTime;

       if (RoleDispenser.iKiller==0)
            Instantiate(PlayerController);

         joystick = FindObjectOfType<FixedJoystick>();
        camAnim = cam.GetComponent<Animator>();

        rigidbody = GetComponent<Rigidbody2D>();

        dashTime = startDashTime;

        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.cyan, 0.0f), new GradientColorKey(Color.white, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1, 0.0f), new GradientAlphaKey(0, 1.0f) }
        );
        trailRenderer.colorGradient = gradient;

        Nickname.SetText(photonView.Owner.NickName);
        if (photonView.IsMine)
          Nickname.color = Color.green;
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround); //принимает значения в зависимости от того что находится внутри круга
            Move();
            Animation();
            Dash();
        }

        if (!Flip) sprite.flipX = false;
        if (Flip) sprite.flipX = true;

        if (DashTimeCounter >= 0)
            DashTimeCounter -= Time.deltaTime;
    }

    private void DashColdown()
    {
        DashTimeCounter = DashStaticTime;
    }

    public void PlayerDash()
    {
        if (DashTimeCounter <= 0)
        {
            ButtonDash = true;
            DashColdown();
        }
    }

    void Dash()
    {
        if (direction == 0)
        {
            if (ButtonDash)
            {
                if (moveInput < 0)
                {
                    Instantiate(dashParticle, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), Quaternion.identity);
                    animator.SetTrigger("dash");
                    camAnim.SetTrigger("zoomin");
                    direction = 1;
                }
                else if (moveInput > 0)
                {
                    Instantiate(dashParticle, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), Quaternion.identity);
                    animator.SetTrigger("dash");
                    camAnim.SetTrigger("zoomin");
                    direction = 2;
                }
            }
        }
        else
        {
            if (dashTime <= 0)
            {
                direction = 0;
                dashTime = startDashTime;
                rigidbody.velocity = Vector2.zero;
                //trailRenderer.widthCurve = new AnimationCurve(new Keyframe(0, 0.27f), new Keyframe(0.5f, 0.7f), new Keyframe(1, 0));
                Gradient gradient = new Gradient();
                gradient.SetKeys(
                    new GradientColorKey[] { new GradientColorKey(Color.cyan, 0.0f), new GradientColorKey(Color.white, 1.0f) },
                    new GradientAlphaKey[] { new GradientAlphaKey(1, 0.0f), new GradientAlphaKey(0, 1.0f) }
                );
                trailRenderer.colorGradient = gradient;
            }
            else
            {
                Gradient gradient2 = new Gradient();
                gradient2.SetKeys(
                    new GradientColorKey[] { new GradientColorKey(Color.red, 0.0f), new GradientColorKey(Color.yellow, 1.0f) },
                    new GradientAlphaKey[] { new GradientAlphaKey(1, 0.0f), new GradientAlphaKey(0, 1.0f) }
                );
                trailRenderer.colorGradient = gradient2;
                Instantiate(dashParticle, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), Quaternion.identity);
                dashTime -= Time.deltaTime;

                if (direction == 1)
                    rigidbody.velocity = Vector2.left * dashSpeed;
                
                else if (direction == 2)
                    rigidbody.velocity = Vector2.right * dashSpeed;
                
            }
        }
        ButtonDash = false;

    }

    void Move()
    {
        //-------------------------------MOVE--------------------------------------------------------

        moveInput =joystick.Horizontal;  

        rigidbody.velocity = new Vector2(moveInput * speed, rigidbody.velocity.y);


        if (moveInput > 0)
            Flip = false;
        
        if (moveInput < 0)
            Flip = true;
        

        if (isGrounded && ButtonJump)    //&& ButtonJump
        {
            for (int i = 0; i <= 50; i++)
            {
                Instantiate(walkParticle, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.5f), Quaternion.identity);
            }

            animator.SetTrigger("jump");
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpSpeed);
            isGrounded = false;

            jumpTimeCounter = jumpTime;
           // isJumping = true;
            ButtonJump = false;
        }

    }

    public void PlayerJump()
    {
        //-------------------------------JUMP-------------------------------------------------------
        if(!isGrounded)
        PlayerControls.ButtonJump=true;
  
    }

    void Animation()
    {
        if (moveInput != 0)
        {
            animator.SetBool("walking", true);
            if (isGrounded == true)
                Instantiate(walkParticle, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.5f), Quaternion.identity);
        }
        else
         animator.SetBool("walking", false);
    }

    private IEnumerator End()
    {
        Debug.Log("Killer Leave!");
        PhotonNetwork.Instantiate("WinOrLost", new Vector3(0, 0), Quaternion.identity);
        Text WinOrLoseTG = GameObject.FindGameObjectWithTag("EndGame").GetComponent<Text>();
        WinOrLoseTG.text = "Player wins!";
        yield return new WaitForSeconds(5f);
        Timer.KillerWin = false;
        GameManager.Leave();
    }

    [PunRPC]
    private void KillerLeave()
    {
        StartCoroutine(End());
    } 
}