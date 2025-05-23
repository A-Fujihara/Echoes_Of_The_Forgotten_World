using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float downwardSpeed = 0.5f;
    public Transform leftBoundary;
    public Transform rightBoundary;
    
    [Header("Descent Settings")]
    public float maxDescendY = 0f; // How low it can go before resetting
    public float startY = 7f; // Starting Y position
    public float stopDescendingY = 2f; // Y position where it stops descending
    
    [Header("Pattern Settings")]
    public float zigzagAmount = 0.5f; // How much it zigzags
    public float patternSpeed = 2f; // Speed of the pattern
    
    private int direction = 1; // 1 for right, -1 for left
    private bool isDescending = true;
    private float patternOffset = 0f;
    
    void Start()
    {
        // Start at the top position
        transform.position = new Vector3(transform.position.x, startY, transform.position.z);
    }
    
    void Update()
    {
        // Calculate zigzag pattern
        patternOffset += Time.deltaTime * patternSpeed;
        float zigzag = Mathf.Sin(patternOffset) * zigzagAmount;
        
        // Move the enemy left/right with zigzag
        Vector3 movement = new Vector3(direction * moveSpeed * Time.deltaTime, 0, 0);
        movement.y += zigzag * Time.deltaTime;
        transform.Translate(movement);
        
        // Move down if still descending
        if (isDescending)
        {
            transform.Translate(Vector2.down * downwardSpeed * Time.deltaTime);
            
            // Stop descending at certain height
            if (transform.position.y <= stopDescendingY)
            {
                isDescending = false;
            }
        }
        
        // Check if it's gone too low (off screen) and reset
        if (transform.position.y < maxDescendY)
        {
            // Natural reset - gradually move back to start
            ResetPosition();
        }
        
        // Check boundaries and reverse direction
        if (transform.position.x >= rightBoundary.position.x && direction == 1)
        {
            direction = -1;
        }
        else if (transform.position.x <= leftBoundary.position.x && direction == -1)
        {
            direction = 1;
        }
    }
    
    void ResetPosition()
    {
        // Smoothly transition back to the top
        float resetSpeed = 5f;
        transform.position = Vector3.MoveTowards(transform.position, 
            new Vector3(transform.position.x, startY, transform.position.z), 
            resetSpeed * Time.deltaTime);
        
        // Once back at top, start descending again
        if (transform.position.y >= startY - 0.1f)
        {
            isDescending = true;
        }
    }
}