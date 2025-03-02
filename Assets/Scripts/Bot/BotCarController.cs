using UnityEngine;
using TMPro;
public class BotCarController : MonoBehaviour
{
    public float moveSpeed = 10f;  
    public int number;
    public TMP_Text numberText;
    private void Start()
    {
        number = GenerateRandomPowerOfTwo();
        numberText.text = number.ToString();
    }

    private void Update()
    {
        MoveCar();
    }

    private void MoveCar()
    {
        // Arabanýn ileri doðru hareket etmesi için
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        if (GameObject.FindGameObjectWithTag("Player").transform.position.z > gameObject.transform.position.z + 20)
        {
            Destroy(gameObject);
        }
    }
    
    private int GenerateRandomPowerOfTwo()
    {
        int minExpo = (int)(Mathf.Log(GameObject.FindGameObjectWithTag("Player").GetComponent<GameController>().number) / Mathf.Log(2));
        int randomExponent = Random.Range(minExpo, (minExpo + 4)); 
        int randomPowerOfTwo = (int)Mathf.Pow(2, randomExponent); 
        return randomPowerOfTwo; 
    }
}
