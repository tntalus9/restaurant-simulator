using UnityEngine;

[CreateAssetMenu()]
public class KitchenObjectSO : ScriptableObject
{
    public Transform prefab;  // use public if u don't write to SO. If not, use serialized or get.
    public Sprite sprite;
    public string objectName;
}