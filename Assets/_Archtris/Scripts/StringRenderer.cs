using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
public class StringRenderer : MonoBehaviour
{
    [Header("Settings")]
    public Gradient pullColor = null;

    [Header("References")]
    public PullMeasurer pullMeaserer = null;

    [Header("Render Positions")]
    public Transform start = null;
    public Transform middle = null;
    public Transform end = null;

    private LineRenderer lineRenderer = null;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Application.isEditor && !Application.isPlaying)
            UpdatePositions();
    }

    private void OnEnable()
    {
        Application.onBeforeRender += UpdatePositions;
        pullMeaserer.Pulled.AddListener(UpdateColor);
    }

    private void OnDisable()
    {
        Application.onBeforeRender -= UpdatePositions;  
        pullMeaserer.Pulled.RemoveListener(UpdateColor);
    }

    private void UpdatePositions()
    {
        Vector3[] positions = new Vector3[] { start.position, middle.position, end.position };
        lineRenderer.SetPositions(positions);
    }

    private void UpdateColor(Vector3 pullPosition, float pullAmount)
    {
        Color color = pullColor.Evaluate(pullAmount);
        lineRenderer.material.color = color;
    }
}
