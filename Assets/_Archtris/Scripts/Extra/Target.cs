using UnityEngine;

public class Target : MonoBehaviour, IArrowHittable
{
    public float forceAmount = 1.0f;
    public Material otherMaterial = null;
    public int id;

    public void Hit(Arrow arrow)
    {
        ApplyMaterial();
        ApplyForce(arrow.transform.forward);
        EventManager.current.OnHitTarget(id);
    }

    private void ApplyMaterial()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = otherMaterial;
    }

    private void ApplyForce(Vector3 direction)
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.AddForce(direction * forceAmount);
    }
}
