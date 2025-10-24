using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    float durationUntilNextBalloon;
    Sprite circleTexture;

    void Start()
    {
        NetworkClientProcessing.SetGameLogic(this);
    }

    void Update()
    {
        durationUntilNextBalloon -= Time.deltaTime;

        if (durationUntilNextBalloon < 0)
        {
            durationUntilNextBalloon = 1f;

            float screenPositionXPercent = Random.Range(0.0f, 1.0f);
            float screenPositionYPercent = Random.Range(0.0f, 1.0f);
            Vector2 screenPosition = new Vector2(screenPositionXPercent * (float)Screen.width, screenPositionYPercent * (float)Screen.height);
            SpawnNewBalloon(screenPosition);
        }
    }

    public void SpawnNewBalloon(Vector2 screenPosition)
    {
        if (circleTexture == null)
            circleTexture = Resources.Load<Sprite>("Circle");

        GameObject balloon = new GameObject("Balloon");

        balloon.AddComponent<SpriteRenderer>();
        balloon.GetComponent<SpriteRenderer>().sprite = circleTexture;
        balloon.AddComponent<CircleClick>();
        balloon.AddComponent<CircleCollider2D>();

        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, 0));
        pos.z = 0;
        balloon.transform.position = pos;
        //go.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, -Camera.main.transform.position.z));
    }
    Dictionary<string, GameObject> balloonLookup = new Dictionary<string, GameObject>();


    // lab
    public void NetworkSpawnBalloon(string id, float xPercent, float yPercent)
    {
        if (circleTexture == null)
            circleTexture = Resources.Load<Sprite>("Circle");

        GameObject balloon = new GameObject(id);
        balloon.AddComponent<SpriteRenderer>().sprite = circleTexture;
        balloon.AddComponent<CircleCollider2D>();
        var click = balloon.AddComponent<CircleClick>();
        click.name = id;

        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(xPercent * Screen.width, yPercent * Screen.height, 0));
        pos.z = 0;
        balloon.transform.position = pos;

        balloonLookup[id] = balloon;
    }

    public void NetworkRemoveBalloon(string id)
    {
        if (balloonLookup.ContainsKey(id))
        {
            Destroy(balloonLookup[id]);
            balloonLookup.Remove(id);
        }
    }
}
