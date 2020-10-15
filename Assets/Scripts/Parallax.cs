using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float ParallaxSpeed;

    [SerializeField] private GameObject parallaxObject = null;
    [SerializeField] private GameObject followTarget   = null;

    private Vector3 startPosition;
    private float backgroundWidth;

    private void Awake()
    {
        startPosition = this.transform.position;
        
        var right =  Instantiate(parallaxObject, this.transform);
        var left  =  Instantiate(parallaxObject, this.transform);

        backgroundWidth = right.GetComponent<SpriteRenderer>().bounds.size.x;
        
        right.transform.position = new Vector3(parallaxObject.transform.position.x + backgroundWidth,
                                               parallaxObject.transform.position.y,
                                               parallaxObject.transform.position.z);
        left.transform.position  = new Vector3(parallaxObject.transform.position.x - backgroundWidth,
                                               parallaxObject.transform.position.y,
                                               parallaxObject.transform.position.z);
    }

    private void Update()
    {
        float temp = (followTarget.transform.position.x * (1 - ParallaxSpeed));
        float dist = (followTarget.transform.position.x * ParallaxSpeed);
        
        transform.position = new Vector3(startPosition.x + dist, transform.position.y, transform.position.z);

        if (temp > startPosition.x + backgroundWidth)
            startPosition.x += backgroundWidth;
        else if (temp < startPosition.x - backgroundWidth)
            startPosition.x -= backgroundWidth;
    }
}
