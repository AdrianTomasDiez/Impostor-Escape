using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class PlayerControls : MonoBehaviour
{
    public Rigidbody2D rb;
    public int velocity;
    public int startVelocity;
    public int jumpForce;
    public Transform groundCheck;
    public float groudCheckRaduius;
    public LayerMask whatIsGround;
    private bool onGround;
    public Vector2 spawnPosition;
    public int deathCount;
    public TextMeshProUGUI numberOfDeats;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI pauseScoreText;
    public TextMeshProUGUI highScoreText;
    public AudioSource audioSource;
    private AudioClip[] audioClips;
    public Animator animator;
    private bool canClick;
    public bool isDead;
    private KillerWalk killer;
    private GameManager gameManager;
    public float scoreFloat;
    public int scoreInt;
    public int highScore;

    // Start is called before the first frame update
    void Start()
    {
        highScore = PlayerPrefs.GetInt("highScore", 0);
        Application.targetFrameRate = 60;
        gameManager = FindObjectOfType<GameManager>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        audioClips = Resources.LoadAll<AudioClip>("Audio");
        animator = GetComponent<Animator>();
        startVelocity = velocity;
        canClick = true;
        killer = FindObjectOfType<KillerWalk>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = new Vector2(velocity, rb.velocity.y);
        onGround = Physics2D.OverlapCircle(groundCheck.position, groudCheckRaduius, whatIsGround);
        animator.SetBool("onGround", onGround);
        isDead = animator.GetBool("isDead");
        if (Input.GetMouseButton(0) && onGround && canClick && !EventSystem.current.IsPointerOverGameObject())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            audioSource.clip = audioClips[1];
            audioSource.Play();
        }
        if (isDead)
        {
            // Guardar el valor anterior de highScore antes de reiniciar la puntuación actual
            int previousHighScore = highScore;
            scoreFloat = 0;
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            // Establecer highScore en el valor anterior
            highScore = previousHighScore;
        }
        else if (!isDead)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            scoreFloat += Time.deltaTime * 2.3f;
            scoreInt = Mathf.RoundToInt(scoreFloat);
            scoreText.text = scoreInt.ToString();
            pauseScoreText.text = scoreInt.ToString();
            highScoreText.text = highScore.ToString(); // Actualizar highScoreText con la puntuación más alta actual
            if (highScore < scoreInt)
            {
                highScore = scoreInt;
            }
        }
    }

    private void Death()
    {
        deathCount += 1;
        numberOfDeats.text = deathCount.ToString();
        audioSource.clip = audioClips[0];
        audioSource.Play();
        velocity = 0;
        killer.velocity = 0;
        animator.SetBool("isDead", true);
        canClick = false;
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        PlayerPrefs.SetInt("highScore", highScore);
        Invoke("Die", 1.5f);
    }
    public void Respawn()
    {
        gameManager.ResetLevel();
        gameManager.Pause();
        transform.position = new Vector2(spawnPosition.x, spawnPosition.y);
        killer.transform.position = new Vector2(17.5f, 6.72f);
        velocity = startVelocity;
        killer.velocity = killer.startVelocity;
        animator.SetBool("isDead", false);
        canClick = true;
        scoreFloat = 0;
    }
    private void Die()
    {
        gameManager.Pause();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Damage" && isDead == false)
        {
            Death();
        }
    }



}
