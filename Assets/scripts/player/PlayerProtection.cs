using UnityEngine;

public class PlayerProtection : MonoBehaviour
{
    public float protectionDuration = 7f;
    public float protectionRadius = 3f;
    public float bubbleYOffset = 1f;
    public Color bubbleColor = new Color(0.4f, 0.85f, 1f, 0.5f);

    public bool IsProtected { get; private set; }

    private float timer;
    private GameObject bubble;
    private Material runtimeMat;

    void Awake()
    {
        CreateBubble();
    }

    void CreateBubble()
    {
        bubble = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        bubble.name = "ProtectionBubble";
        bubble.transform.position = transform.position + Vector3.up * bubbleYOffset;
        bubble.transform.localScale = Vector3.one * protectionRadius * 2f;

        Destroy(bubble.GetComponent<SphereCollider>());

        Shader shader = Shader.Find("Custom/Bubble");
        if (shader == null)
        {
            Debug.LogError("[PlayerProtection] Custom/Bubble shader not found! Check Assets/scripts/Bubble.shader");
            shader = Shader.Find("Universal Render Pipeline/Unlit");
        }

        Debug.Log($"[PlayerProtection] Using shader: {shader.name}");

        runtimeMat = new Material(shader);
        runtimeMat.SetColor("_BaseColor", bubbleColor);
        runtimeMat.color = bubbleColor;
        runtimeMat.renderQueue = 3000;

        MeshRenderer mr = bubble.GetComponent<MeshRenderer>();
        mr.material = runtimeMat;
        mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        mr.receiveShadows = false;

        bubble.SetActive(false);
    }

    public void ActivateProtection()
    {
        Debug.Log("[PlayerProtection] Protection activated");
        IsProtected = true;
        timer = protectionDuration;
        if (bubble != null)
        {
            bubble.SetActive(true);
            if (runtimeMat != null)
            {
                runtimeMat.SetColor("_BaseColor", bubbleColor);
                runtimeMat.color = bubbleColor;
            }
        }
    }

    void LateUpdate()
    {
        if (bubble != null && bubble.activeSelf)
        {
            bubble.transform.position = transform.position + Vector3.up * bubbleYOffset;
            bubble.transform.localScale = Vector3.one * protectionRadius * 2f;
        }
    }

    void Update()
    {
        if (!IsProtected) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            IsProtected = false;
            if (bubble != null) bubble.SetActive(false);
            Debug.Log("[PlayerProtection] Protection expired");
        }
    }

    void OnDestroy()
    {
        if (bubble != null) Destroy(bubble);
    }
}
