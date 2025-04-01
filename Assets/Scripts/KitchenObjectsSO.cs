using UnityEngine;

[CreateAssetMenu()]
public class KitchenObjectsSO : ScriptableObject
{
    public GameObject prefab;  // use public if u don't write to SO. If not, use serialized or get.
    public Sprite sprite;
    public string ObjectName;
}
