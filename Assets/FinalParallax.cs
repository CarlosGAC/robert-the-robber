using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalParallax : MonoBehaviour
{
    private List<GameObject> objectsToParallax;
    private GameObject firstParallaxObject;
    private GameObject lastParallaxObject;

    private float objectsToParallaxSizeX;

    public Transform leftBound;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        objectsToParallax = new List<GameObject>();
        
        SetParallaxObjectsToList();
        UpdateParallaxIndexes();

        objectsToParallaxSizeX = firstParallaxObject.GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void SetParallaxObjectsToList()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            objectsToParallax.Add(transform.GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        CheckFirstParallaxObjectPosition();
    }

    public void CheckFirstParallaxObjectPosition()
    {
        float firstParallaxPosX = firstParallaxObject.transform.position.x;
        float leftBoundPosX = leftBound.position.x;

        if ((firstParallaxPosX - objectsToParallaxSizeX) < leftBoundPosX)
        {
            UpdateObjectToParallaxPosition();
            UpdateParallaxList();
            UpdateParallaxIndexes();
        }
    }

    public void UpdateObjectToParallaxPosition()
    {
        Vector3 newParallaxObjectPosition = new Vector3(lastParallaxObject.transform.position.x + objectsToParallaxSizeX, 0, 0);
        firstParallaxObject.transform.SetPositionAndRotation(newParallaxObjectPosition, Quaternion.identity);

    }

    public void UpdateParallaxList()
    {
        objectsToParallax.Remove(firstParallaxObject);
        objectsToParallax.Add(firstParallaxObject);
    }

    public void UpdateParallaxIndexes()
    {
        firstParallaxObject = objectsToParallax[0];
        lastParallaxObject = objectsToParallax[objectsToParallax.Count - 1];
    }

}
