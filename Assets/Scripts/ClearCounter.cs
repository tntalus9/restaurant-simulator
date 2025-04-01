using UnityEngine;

public class ClearCounter : MonoBehaviour
{
    [SerializeField] private GameObject tomatoPrefab;
    [SerializeField] private GameObject counterPrefabMesh;
    
    public void Interact()
    {
        Renderer counterRenderer = counterPrefabMesh.GetComponent<Renderer>();
        if (counterRenderer == null)
        {
            Debug.Log("Clear Counter prefab does not have a Renderer!");
            return;
        }
        Vector3 counterCenterPosition = counterRenderer.bounds.center;
        Vector3 counterHeightExtent = Vector3.up * counterRenderer.bounds.extents.y; // height along y-axis
        Vector3 counterTopPostion = counterCenterPosition + counterHeightExtent;

        Instantiate(tomatoPrefab, counterTopPostion, tomatoPrefab.transform.rotation);
    }
}
