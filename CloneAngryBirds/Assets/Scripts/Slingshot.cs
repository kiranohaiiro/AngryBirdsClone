using UnityEngine;
using UnityEngine.SceneManagement;

public class Slingshot : MonoBehaviour
{
    public LineRenderer[] lineRenderers;  // Array of LineRenderer components for drawing the slingshot lines
    public Transform[] stripPosition;  // Array of Transform positions for the slingshot strips
    public Transform center;  // Transform position of the slingshot center
    public Transform idlePosition;  // Transform position of the slingshot idle position

    public AudioSource slingshotFx;  // Fx for slingshot
    public AudioClip shotBird;  // Fx for shot bird 
    public AudioClip onMouseDownFx;  
    public AudioClip onMouseUpFx;  


    public Vector3 currentPosition;  // Current position of the slingshot 

    public float maxLength;  // Maximum length of the slingshot

    bool isMouseDown;  // Flag to track if the mouse button is being held down

    public GameObject birdPrefab;  // Prefab of the bird game object

    public float birdPositionOffset;  // Offset for the bird's position relative to the slingshot

    // Shot counter
    public int shotCounter;

    // Enemy counter
    int enemyCounter;

    Rigidbody2D bird; 
    Collider2D birdCollider;  

    public float force;  // Force applied to the bird when shot

    void Start()
    {
        // Set initial positions of the line renderers
        lineRenderers[0].positionCount = 2;
        lineRenderers[1].positionCount = 2;
        lineRenderers[0].SetPosition(0, stripPosition[0].position);
        lineRenderers[1].SetPosition(0, stripPosition[1].position);

        CreateBird();  // calling the method which create the bird at the start
    }

    // Method which create the bird 
    void CreateBird()
    {
        bird = Instantiate(birdPrefab).GetComponent<Rigidbody2D>();  // Instantiate the bird prefab and get the Rigidbody2D component
        birdCollider = bird.GetComponent<Collider2D>();  // Get the Collider2D component of the bird

        birdCollider.enabled = false;  // Disable the bird's collider
        bird.isKinematic = true;  // Make the bird (non-physical)

        shotCounter++;  // Increment the shot counter

        ResetStrips();  
    }

    void Update()
    {
        if (isMouseDown)
        {
            // Get the mouse position and convert it to world coordinates
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10;
            currentPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            // Clamp the current position within the maximum length from the slingshot center
            currentPosition = center.position + Vector3.ClampMagnitude(currentPosition - center.position, maxLength);

            SetStrips(currentPosition);  // Set the positions of the slingshot strips

            if (birdCollider)
            {
                birdCollider.enabled = true;  // Enable the bird's collider
            }
        }
        else
        {
            ResetStrips();  
        }

        enemyCounter = GameObject.FindGameObjectsWithTag("Enemies").Length;  // Count the number of game objects with the "Enemies" tag

        if (enemyCounter == 0)
        {
            Invoke("LoadCompleteScene", 3);  // Load the complete scene after a delay of 3 seconds
        }
    }

    private void OnMouseDown()
    {
        slingshotFx.PlayOneShot(onMouseDownFx, 2);  
        isMouseDown = true;  // Flag on MouseDown
    }

    private void OnMouseUp()
    {
        slingshotFx.PlayOneShot(onMouseUpFx, 2);  
        slingshotFx.PlayOneShot(shotBird, 2);  
        isMouseDown = false;  
        Shoot();  
    }

    void Shoot()
    {
        bird.isKinematic = false;  
        Vector3 birdForce = (currentPosition - center.position) * force * -1;  
        bird.velocity = birdForce;  

        bird = null;  
        birdCollider = null;  

        if (shotCounter < 3)
        {
            Invoke("CreateBird", 2);  // Create a new bird after a delay of 2 seconds
        }
        if (shotCounter == 3 && enemyCounter != 0)
        {
            Invoke("LoadLoseScene", 8);  // Load the lose scene after a delay of 8 seconds if shotCounter is 3 and there are remaining enemies
        }
    }

    // Reset the slingshot strips
    void ResetStrips()
    {
        currentPosition = idlePosition.position;  // Set the current position to the idle position
        SetStrips(currentPosition);  // Set the positions of the slingshot strips
    }

    // Set the positions of the slingshot strips and adjust the bird's position and rotation
    void SetStrips(Vector3 position)
    {
        lineRenderers[0].SetPosition(1, position);
        lineRenderers[1].SetPosition(1, position);

        if (bird)
        {
            Vector3 dir = position - center.position;
            bird.transform.position = position + dir.normalized * birdPositionOffset;
            bird.transform.right = -dir.normalized;
        }
    }

    // Load the complete scene
    void LoadCompleteScene()
    {
        SceneManager.LoadScene(2);
    }

    // Load the lose scene
    void LoadLoseScene()
    {
        SceneManager.LoadScene(3);
    }
}
