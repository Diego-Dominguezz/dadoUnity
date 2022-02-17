using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Dado : MonoBehaviour
{
    protected Rigidbody dadots;
    public GameObject button;
    public float speed;
    public float angularSpeed;
    protected float rotationx, rotationy, rotationz;
    public bool awake, hasNotWon;
    public int number;
    public Vector3 cubeCenterOfMass;
    public List<Vector3> directions;
    public List<int> sideValues;
    static int gamble;
    public Image winImg,lose;

    // Start is called before the first frame update
    void Start()
    {

        winImg.enabled = false;
        lose.enabled = false;

        hasNotWon = GameValues.hasNotWon;
        dadots = GetComponent<Rigidbody>();
        transform.rotation = Random.rotation;
        button.gameObject.SetActive(false);

        if(hasNotWon == true)
        {
            number = GameValues.gamble;
        }

        rotationx = Random.Range(30f, 90f);
        rotationy = Random.Range(16f, 60f);
        rotationz = Random.Range(12f, 30f);

        setTorque(5);
        dadots.velocity = transform.forward * Time.deltaTime * 300;


        if (directions.Count == 0)
        {
            // Object space directions
            directions.Add(Vector3.up);
            sideValues.Add(3); // up
            directions.Add(Vector3.down);
            sideValues.Add(4); // down

            directions.Add(Vector3.left);
            sideValues.Add(2); // left
            directions.Add(Vector3.right);
            sideValues.Add(5); // right

            directions.Add(Vector3.forward);
            sideValues.Add(6); // fw
            directions.Add(Vector3.back);
            sideValues.Add(1); // back
        }

        // Assert equal side of lists
        if (directions.Count != sideValues.Count)
        {
            Debug.LogError("Not consistent list sizes");
        }

        InvokeRepeating("win",1f,1f);

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        dadots.WakeUp();
        awake = !dadots.IsSleeping();
        if (hasNotWon && (number == 1))
        {
            // mueve centro de masa a favor del numero seleccionado
            cubeCenterOfMass = new Vector3(0f, 0f, 0.3f);
        }
        else if (hasNotWon && (number == 2))
        {
            cubeCenterOfMass = new Vector3(0.3f, 0f, 0f);
        }
        else if (hasNotWon && (number == 3))
        {
            cubeCenterOfMass = new Vector3(0f, -0.3f, 0f);
        }
        else if (hasNotWon && (number == 4))
        {
            cubeCenterOfMass = new Vector3(0f, 0.3f, 0f);
        }
        else if (hasNotWon && (number == 5))
        {
            cubeCenterOfMass = new Vector3(-0.3f, 0f, 0f);
        }
        else if (hasNotWon && (number == 6))
        {
            cubeCenterOfMass = new Vector3(0f, 0f, -0.3f);
        }

        dadots.centerOfMass = cubeCenterOfMass;

    }

    void setTorque(int repeticiones)
    {
        for (int i = 1; i <= repeticiones; i++)
        {
            speed = dadots.velocity.magnitude;
            angularSpeed = dadots.angularVelocity.magnitude * Mathf.Rad2Deg;

            dadots.angularVelocity = new Vector3(Mathf.PI * rotationx, Mathf.PI * rotationy, rotationz);

        }


    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + transform.rotation * cubeCenterOfMass, 0.3f);
    }

    void win()
    {
        if (dadots.velocity.magnitude < 0.05)
        {
            if (GetNumber(Vector3.up, 30f) == GameValues.gamble)
            {
                Debug.Log(dadots.name + " dado ganador");
                winImg.enabled = true;

            }
            else
            {
                Debug.Log(dadots.name + " dado perdedor");
                lose.enabled = true;
            }
            button.gameObject.SetActive(true);
        } 

    }
    public int GetNumber(Vector3 referenceVectorUp, float epsilonDeg = 5f)
    {
        // here I would assert lookup is not empty, epsilon is positive and larger than smallest possible float etc
        // Transform reference up to object space
        Vector3 referenceObjectSpace = transform.InverseTransformDirection(referenceVectorUp);

        // Find smallest difference to object space direction
        float min = float.MaxValue;
        int mostSimilarDirectionIndex = -1;
        for (int i = 0; i < directions.Count; ++i)
        {
            float a = Vector3.Angle(referenceObjectSpace, directions[i]);
            if (a <= epsilonDeg && a < min)
            {
                min = a;
                mostSimilarDirectionIndex = i;
            }
        }

        // -1 as error code for not within bounds
        return (mostSimilarDirectionIndex >= 0) ? sideValues[mostSimilarDirectionIndex] : -1;
    }
}
