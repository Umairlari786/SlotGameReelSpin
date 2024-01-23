using System.Collections;
using UnityEngine;

public class Reel : MonoBehaviour
{
    public ReelManager bottleManager;
    private float introOutroSpeed = 10f;
    private float mainSpinSpeed = 20f;
    private bool spinning = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !spinning)
        {
            StartCoroutine(SpinBottles());
        }
    }

    IEnumerator SpinBottles()
    {
        spinning = true;

        Vector3[] initialPositions = new Vector3[bottleManager.reel.Length];
        for (int i = 0; i < bottleManager.reel.Length; i++)
        {
            initialPositions[i] = bottleManager.reel[i].transform.position;
        }

        yield return StartCoroutine(SpinState(introOutroSpeed, 0.7f, initialPositions));

        yield return StartCoroutine(SpinState(mainSpinSpeed, 1.6f, initialPositions));

        yield return StartCoroutine(SpinState(introOutroSpeed, 0.7f, initialPositions));

        for (int i = 0; i < bottleManager.reel.Length; i++)
        {
            bottleManager.reel[i].transform.position = initialPositions[i];
        }
        spinning = false;
    }

    IEnumerator SpinState(float speed, float duration, Vector3[] initialPositions)
    {
        float totalTime = 0f;
        float totalDistanceMoved = 0f;

        while (totalTime < duration)
        {
            float shift = speed * Time.deltaTime;

            for (int i = 0; i < bottleManager.reel.Length; i++)
            {
                bottleManager.reel[i].transform.position -= new Vector3(0f, shift, 0f);
            }

            totalDistanceMoved += shift;

            if (totalDistanceMoved > bottleManager.reel[0].GetComponent<Renderer>().bounds.size.y)
            {
                for (int i = 0; i < bottleManager.reel.Length; i++)
                {
                    bottleManager.reel[i].transform.position = initialPositions[i];
                }

                totalDistanceMoved = 0f;

                ShuffleArray(bottleManager.reel);
            }

            yield return null;
            totalTime += Time.deltaTime;
        }
    }

    void ShuffleArray(GameObject[] array)
    {
        int n = array.Length;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            GameObject value = array[k];
            array[k] = array[n];
            array[n] = value;
        }
    }
}
