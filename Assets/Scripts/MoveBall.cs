using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MoveBall : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField]
    public float startSpeed = 5f;

    // L'angolo massimo (in gradi) con cui la pallina può rimbalzare dal paddle
    [SerializeField]
    private float maxBounceAngle = 60f;

    Vector2 direction;

    // Proprietà pubbliche per i punteggi, leggibili da fuori ma modificabili solo qui dentro
    private int leftPoints;

    public int LeftPoints => leftPoints;

    private int rightPoints;
    public int RightPoints => rightPoints;


    [SerializeField]
    private TMP_Text leftScoreText;

    [SerializeField]
    private TMP_Text rightScoreText;

    private AudioSource pongSound;

    [SerializeField]
    private ParticleSystem goalParticles;



    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        pongSound = GetComponent<AudioSource>();

        leftPoints = 0;
        rightPoints = 0;

        SetRandomDirection();
    }

    void FixedUpdate()
    {
        // Applichiamo costantemente la velocità fisica combinando la direzione e la velocità di base
        rb.linearVelocity = direction * startSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (rb != null)
        {
             if (collision.gameObject.CompareTag("BounceWall"))
            {
                pongSound.Play();
                direction.y = -direction.y;
            }

             if (collision.gameObject.CompareTag("Paddle"))
            {
                pongSound.Play();
                // Prendiamo la posizione verticale (Y) della pallina e del paddle
                float ballY = transform.position.y;
                float paddleY = collision.transform.position.y;

                // Calcoliamo l'altezza totale del collider del paddle
                float paddleHeight = collision.collider.bounds.size.y;

                // Determiniamo il punto esatto di impatto: restituisce un valore tra -1 (bordo basso) e 1 (bordo alto)
                float relativeIntersectY = (ballY - paddleY) / (paddleHeight / 2);

                // Sicurezza fisica: costringiamo il valore a rimanere tra -1 e 1 anche in caso di compenetrazioni lievi
                relativeIntersectY = Mathf.Clamp(relativeIntersectY, -1f, 1f);

                // Calcoliamo l'angolo di rimbalzo in gradi moltiplicando il punto d'impatto per l'angolo massimo
                float bounceAngle = relativeIntersectY * maxBounceAngle;

                // Convertiamo l'angolo in radianti perché le funzioni matematiche di Unity (Cos/Sin) usano i radianti
                float radians = bounceAngle * Mathf.Deg2Rad;

                // Se la pallina si muoveva verso destra (> 0), dopo il rimbalzo deve andare a sinistra (-1), e viceversa
                float bounceDirectionX = (direction.x > 0) ? -1f : 1f;

                // Ricostruiamo il vettore direzione usando il Coseno per la X e il Seno per la Y, poi lo normalizziamo
                direction = new Vector2(bounceDirectionX * Mathf.Cos(radians), Mathf.Sin(radians)).normalized;
            }

           if (collision.gameObject.CompareTag("LeftScoreWall"))
            {
                goalCoroutine();
                transform.position = Vector3.zero;

                rightPoints++;                
                rightScoreText.text = RightPoints.ToString();


                SetRandomDirection();

                // Resettiamo immediatamente la velocità fisica per il nuovo round
                rb.linearVelocity = direction * startSpeed;
            }

            if (collision.gameObject.CompareTag("RightScoreWall"))
            {
                goalCoroutine();

                transform.position = Vector3.zero;

                leftPoints++;
                leftScoreText.text = LeftPoints.ToString();


                SetRandomDirection();

                // Resettiamo immediatamente la velocità fisica per il nuovo round
                rb.linearVelocity = direction * startSpeed;
            }
        }
    }

    private IEnumerator goalCoroutine()
    {
        Instantiate(goalParticles, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(2);
    }

    private void SetRandomDirection()
    {
        // Scegliamo un angolo casuale tra -30 e 30 gradi per evitare lanci troppo verticali alla partenza
        float randomAngle = UnityEngine.Random.Range(-30f, 30f);

        // Convertiamo l'angolo di partenza in radianti
        float radians = randomAngle * Mathf.Deg2Rad;

        // Creiamo il vettore di direzione iniziale (di base rivolto verso destra)
        Vector2 newDirection = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));

        // Lanciamo una "moneta virtuale": se il valore è maggiore di 0.5, invertiamo la X per mandarla a sinistra
        if (UnityEngine.Random.value > 0.5f)
        {
            newDirection.x = -newDirection.x;
        }

        // Assegniamo il vettore finale normalizzato (lunghezza totale pari a 1) alla variabile di movimento
        direction = newDirection.normalized;
    }
}