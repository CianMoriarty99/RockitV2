using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float turnRate = 10f, rocketForceDefault = 0.5f, maxRocketVelocity = 2f, x, currentRocketForce = 0f;
    Rigidbody2D rb;

    CapsuleCollider2D col;

    bool rocketsOn, notDead;
    public Transform forwardTransform;

    public GameObject deadBodyPrefab;

    Animator m_Animator;

    public GameObject levels; //So I can set the deadbody to stay on the right level

    void Awake()
    {
        m_Animator = gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>();
    }
    
    void Start()
    {
        notDead = true;
        rocketsOn = true;
        col = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        StartCoroutine(StartSequence());

        levels = GameObject.FindGameObjectWithTag("LEVELS");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        //Always move forward
        if(rocketsOn)
        {
            Rocket();
            ChangeDirection();
            m_Animator.speed = Mathf.Clamp(rb.velocity.magnitude/10, 0f, 0.4f);
        } else {
            m_Animator.speed = 0f;
        }
           

    }

    void Rocket()
    {
        Vector2 direction = transform.position - forwardTransform.transform.position;
        rb.velocity += -currentRocketForce * direction;
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxRocketVelocity);
    }

    void ChangeDirection()
    {
        float MoveHorizontal = Input.GetAxisRaw("Horizontal");
        transform.Rotate(0, 0, MoveHorizontal * turnRate);
    }

    // called when the cube hits the floor
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "KILL" && notDead)
        {
            StartCoroutine(DeathSequence());
        }
    }

    private IEnumerator StartSequence()
    {
        currentRocketForce = 0f;
        yield return new WaitForSeconds(0.5f);
        currentRocketForce = rocketForceDefault;

    }

    private IEnumerator DeathSequence()
    {
        //Explode body away and splatter blood
        //Play death animation
        //Play death sound
        //Hide cursor
        GameManager.Instance.StopTimer();
        notDead = false;
        rocketsOn = false;
        currentRocketForce = 0f;

        //add some randomness to torque and explosion

        AddExplosionForcePlayer(rb, 1f, forwardTransform.position , 1f);
        rb.AddTorque(0.3f, ForceMode2D.Impulse);
        forwardTransform.position = new Vector3(-1000,-1000,-1000);
        yield return new WaitForSeconds(1f);
        //Leave corpse but destroy game object
        rb.velocity = new Vector2(0,0);
        rb.angularVelocity = 0f;
        PlaceCorpse ();  
        Destroy(this.gameObject);

        UiManager.Instance.PanelFadeIn();

    }

    void DisableCollider()
    {
        col.enabled = false;
    }


    void AddExplosionForcePlayer(Rigidbody2D rb, float explosionForce, Vector2 explosionPosition, float explosionRadius, float upwardsModifier = 0.0F, ForceMode2D mode = ForceMode2D.Impulse)
    {
        var explosionDir = rb.position - explosionPosition ;
        var explosionDistance = explosionDir.magnitude * explosionRadius;

        // Normalize without computing magnitude again
        if (upwardsModifier == 0)
        {
            explosionDir /= explosionDistance;
        }
        else
        {
            // If you pass a non-zero value for the upwardsModifier parameter, the direction
            // will be modified by subtracting that value from the Y component of the centre point.
            explosionDir.y += upwardsModifier;
            explosionDir.Normalize();
        }

        rb.AddForce(Mathf.Lerp(0, explosionForce, 1 - explosionDistance) * explosionDir, mode);
    }


    void PlaceCorpse () {
        
        GameObject playerChild = gameObject.transform.GetChild(0).gameObject;
    
        Instantiate (deadBodyPrefab, playerChild.transform.position, playerChild.transform.rotation, levels.transform);
    }


}
 