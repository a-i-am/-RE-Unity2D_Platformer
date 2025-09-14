using System.Collections;
using UnityEngine;

public class TestFollowCamera : MonoBehaviour
{
    [SerializeField] private GameObject Player;

    [SerializeField] private float minX;
    [SerializeField] private float minY;
    [SerializeField] private float maxX;
    [SerializeField] private float maxY;
    // Start is called before the first frame update
    void Start()
    {
        minX = -50f;
        maxX = 50f;
        minY = -50f;
        maxY = 50f;
        transform.position = Player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Player != null)
        {
            var position = Player.transform.position;
            float posX = Mathf.Clamp(position.x, minX, maxX);
            float posY = Mathf.Clamp(position.y, minY, maxY);
            transform.position = new Vector3(posX, posY, -10);
        }
    }
}