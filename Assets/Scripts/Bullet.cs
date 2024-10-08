using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 pointB; // End point
    [SerializeField] private float height = 2f; // Height of the arc
    [SerializeField] private float duration = 2f; // Duration of the movement
     [SerializeField] private Canvas hitUI;
    public void CallShoot()
    {
        StartCoroutine(MoveInArc(transform.position, pointB, height, duration));
    }

    private IEnumerator MoveInArc(Vector3 start, Vector3 end, float height, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            // Calculate the arc position
            Vector3 currentPos = Vector3.Lerp(start, end, t);
            // Calculate the Y position for the arc
            currentPos.y += height * Mathf.Sin(t * Mathf.PI); // Creates the arc

            transform.position = currentPos;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final position is exactly at point B
        transform.position = end;
        Destroy(gameObject,0.5f);
    }
   public void DisplayUi()
   {
        Canvas hitCanvas = Instantiate( hitUI,transform.position, Quaternion.identity);
        hitCanvas.GetComponent<Canvas>().worldCamera = Camera.main;
        TextMeshProUGUI textMeshProUGUI = hitCanvas.GetComponentInChildren<TextMeshProUGUI>();
        textMeshProUGUI.transform.LookAt(Camera.main.transform.position);
        textMeshProUGUI.text = "MISS";
        textMeshProUGUI.color = Color.white;
    }

}
