using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlayerPositioner : MonoBehaviour
{
    [SerializeField] private int numberOfPlayers = 20;
    [SerializeField] private int columns = 5;
    [SerializeField] private Vector2 spacing = Vector2.zero;
    [SerializeField] private float idPositionY = 1f;

    private float currentX, currentZ;

    public static PlayerPositioner Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }


    public Vector3 GetPosition(ulong clientID)
    {
        Vector3 position = transform.position + Vector3.forward * (currentZ * spacing.y) + Vector3.right * (currentX * spacing.x);
        currentX++;
        if(currentX >= columns)
        {
            currentX = 0;
            currentZ++;
        }
        return position;
    }

    float x, z;
    Vector3 calculatedPosition;


    private void OnDrawGizmos()
    {
        x = 0;
        z = 0;
        for (int i = 0; i < numberOfPlayers; i++)
        {
            Gizmos.color = i % 2 == 0 ? Color.navajoWhite : Color.peru;
            calculatedPosition = transform.position + Vector3.forward * (z * spacing.y) + Vector3.right * (x * spacing.x);
            x++;
            if(x >= columns)
            {
                x = 0;
                z++;
            }
            Gizmos.DrawWireSphere(calculatedPosition, .3f);
#if UNITY_EDITOR
            // Dibuja el texto ligeramente por encima del objeto
            Handles.Label(calculatedPosition + Vector3.up * idPositionY, i.ToString());
#endif
        }
    }

    private void OnValidate()
    {
        if(spacing.x <= 0)
        {
            spacing.x = 1;
        }
        if (spacing.y <= 0)
        {
            spacing.y = 1;
        }
    }
}
