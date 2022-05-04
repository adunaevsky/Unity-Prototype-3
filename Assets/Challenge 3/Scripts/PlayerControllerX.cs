using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver;

    public float floatForce;
    private float gravityModifier = 2f;
    private Rigidbody playerRb;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;
    public AudioClip bounce;

    private float topBoundary = 15.0f;
    private float bottomBoundary = 0.02f;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
       

        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();

        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * 7, ForceMode.Impulse);

    }

    // Update is called once per frame
    void Update()
    {
        // While space is pressed and player is low enough, float up
        if (Input.GetKey(KeyCode.Space) && !gameOver && transform.position.y > bottomBoundary)
        {
            playerRb.AddForce(Vector3.up * floatForce);
        }

        if(transform.position.y <= bottomBoundary)
        {
            playerRb.AddForce(Vector3.up * 400);
            playerAudio.PlayOneShot(bounce);

        }

        //if(transform.position.y > 17f)
        //{
        //    Vector3 newPos = new Vector3(transform.position.x, transform.position.y - 10f, transform.position.z);
        //    transform.Translate(newPos);
        //    playerRb.velocity = Vector3.zero;


        //}

        // Stop the Player at the topBoundary
        if (transform.position.y >= topBoundary)
        {
            // Set Y position to topBoundary to "freeze" Player
            transform.position = new Vector3(transform.position.x, topBoundary, transform.position.z);
            // Remove any force on Player RigidBody.
            playerRb.velocity = Vector3.zero;
        }

    }

    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over!");
            Destroy(other.gameObject);
        } 

        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);

        }

    }

}
