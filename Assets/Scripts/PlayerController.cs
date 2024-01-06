using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using UnityEngine.Events;
using UnityEngine.UI;
using MoreMountains.Feedbacks;


public class PlayerController : MonoBehaviour
{
    public float turnRate = 10f, rocketForceDefault = 0.5f, maxRocketVelocity = 2f, x, currentRocketForce = 0f;

    public float smokeParticleCooldown = 0.1f, defaultSmokeParticleCooldown = 0.1f;
    Rigidbody2D rb;

    CircleCollider2D col;

    bool rocketsOn, notDead, onTheHill;
    public Transform forwardTransform;

    public GameObject deadBodyPrefab, bloodPrefab, smallBloodPrefab, smokeParticlePrefab;

    private GameObject permanentSmokeParticle;

    public GameObject levels; //So I can set the deadbody to stay on the right level

    void Awake()
    {
        
    }
    
    void Start()
    {
        onTheHill = false;
        notDead = true;
        rocketsOn = true;
        col = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        StartCoroutine(StartSequence());

        levels = GameObject.FindGameObjectWithTag("LEVELS");
    }

    // Update is called once per frame
    void Update()
    {
        if(rocketsOn)
        {
            CreateSmokeParticles();
        }

        if(onTheHill && notDead)
        {
            GameManager.Instance.timeSpentOnTheHill += Time.deltaTime;
        }

        smokeParticleCooldown -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        //Always move forward
        if(rocketsOn)
        {
            Rocket();
            ChangeDirection();
            
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

    void CreateSmokeParticles()
    {
        Vector3 positionBehindRocket = (transform.position - forwardTransform.position)*1.5f;

        if(smokeParticleCooldown < 0)
        {
            smokeParticleCooldown = defaultSmokeParticleCooldown;
            Instantiate(smokeParticlePrefab, transform.position + positionBehindRocket, Quaternion.identity, levels.transform);
        }
    }

    // called when the cube hits the floor
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "KILL" && notDead)
        {
            StartCoroutine(DeathSequence());
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "HILL")
        {
            onTheHill = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.tag == "APPLE" && notDead)
        {
            SoundEffectPlayer.Instance.playSnakeClip();
            Destroy(col.gameObject);
            GameManager.Instance.snakeScore += 1;
        }

        if(col.gameObject.tag == "HILL")
        {
            onTheHill = false;
        }
    }

    private IEnumerator StartSequence()
    {
        currentRocketForce = 0f;
        yield return new WaitForSeconds(0.5f);
        currentRocketForce = rocketForceDefault;
        RocketAudioController.Instance.playRocketClip();

    }

    private IEnumerator DeathSequence()
    {
        //Screen shake camera
        Camera.main.GetComponent<MMPositionShaker>().StartShaking();
        RocketAudioController.Instance.stopClip();
        CreateBloodSplatter();

        //Play death sound
        SoundEffectPlayer.Instance.playDeathSound();
        GameManager.Instance.StopTimer();
        notDead = false;
        rocketsOn = false;
        currentRocketForce = 0f;

        //add some randomness to torque and explosion
        float randomForce = 10f;
        float randomRadius = 10f;
        AddExplosionForcePlayer(rb, randomForce, randomRadius);
        rb.AddTorque(10f, ForceMode2D.Impulse);
        forwardTransform.position = new Vector3(-1000,-1000,-1000);
        yield return new WaitForSeconds(1f);
        //Leave corpse but destroy game object
        rb.velocity = new Vector2(0,0);
        rb.angularVelocity = 0f;
        PlaceCorpse ();  
        Destroy(this.gameObject);

        UiManager.Instance.PanelFadeIn();

    }

    void CreateBloodSplatter()
    {
        Vector3 directionToCentre = (new Vector3(0,0,0) - transform.position).normalized;

        Vector2 newPostionTowardsCentre = transform.position + (directionToCentre/6);
        
        //GameObject blood = Instantiate(bloodPrefab, transform.position, Quaternion.identity, levels.transform);

        int particles = Random.Range(12, 17);
        for(int i = 0; i<particles; i++)
        {
            Vector2 randomOffset = new Vector2(Random.Range(-0.15f, 0.15f), Random.Range(-0.15f, 0.15f));
            Vector2 finalPos = new Vector2(newPostionTowardsCentre.x + randomOffset.x, newPostionTowardsCentre.y + randomOffset.y);
            GameObject smallBlood = Instantiate(smallBloodPrefab, finalPos, transform.rotation, levels.transform);
        }
        

    }

    void DisableCollider()
    {
        col.enabled = false;
    }


    void AddExplosionForcePlayer(Rigidbody2D rb, float explosionForce, float explosionRadius, float upwardsModifier = 0.0f, ForceMode2D mode = ForceMode2D.Impulse)
    {
        var explosionDir = new Vector3(0,0,0) - transform.position;
        var explosionDistance = explosionDir.magnitude;

        // Normalize without computing magnitude again
        if (upwardsModifier == 0)
        {
            explosionDir /= explosionDistance;
        }
        else
        {
            // If you pass a non-zero value for the upwardsModifier parameter,
            // the direction will be modified by subtracting that value from the Y component of the centre point.
            explosionDir.y += upwardsModifier;
            explosionDir.Normalize();
        }

        // Calculate the force based on explosion distance and apply it
        float force = Mathf.Lerp(0, explosionForce, 1 - explosionDistance / explosionRadius);
        rb.AddForce(force * explosionDir, mode);
    }


    void PlaceCorpse () {
        
        GameObject playerChild = gameObject.transform.GetChild(0).gameObject;
    
        Instantiate (deadBodyPrefab, playerChild.transform.position, playerChild.transform.rotation, levels.transform);
    }


}
 