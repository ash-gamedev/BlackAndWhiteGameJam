using System.Collections;
using UnityEngine;

public class FloatUpAndDissapear : MonoBehaviour
{
    [SerializeField] float timeBeforeDissapear = 4f;

    void Start()
    {
        StartCoroutine(WaitAndDissappear());
    }

    IEnumerator WaitAndDissappear()
    {
        yield return new WaitForSeconds(timeBeforeDissapear);
        Destroy(gameObject);
    }
}