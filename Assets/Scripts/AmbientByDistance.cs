using UnityEngine;

public class AmbientByDistance : MonoBehaviour
{
    public Transform player;
    public Transform entrance;
    public Collider interiorZone;      // trigger/collider que cobre o interior
    public float maxDistance = 12f;
    public float transitionSpeed = 4f; // suaviza a mudança ao entrar/sair
    public Color outdoorAmbient = new Color(0.25f, 0.28f, 0.35f);
    public Light sun;
    public float sunOutdoorIntensity = 0.8f; // com ambiente cheio
    public float sunIndoorIntensity = 1f;    // com ambiente a zero

    float currentT = 0f;

    void Update()
    {
        bool inside = interiorZone.bounds.Contains(player.position);

        float targetT = 0f; // 0 = claridade total (exterior)
        if (inside)
        {
            float d = Vector3.Distance(player.position, entrance.position);
            targetT = Mathf.Clamp01(d / maxDistance);
        }

        currentT = Mathf.MoveTowards(currentT, targetT, transitionSpeed * Time.deltaTime);
        RenderSettings.ambientLight = Color.Lerp(outdoorAmbient, Color.black, currentT);

        
        sun.intensity = Mathf.Lerp(sunOutdoorIntensity, sunIndoorIntensity, currentT);
    }
}