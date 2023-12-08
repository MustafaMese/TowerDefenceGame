using UnityEngine;

public class FloatingTextController : MonoBehaviour
{
    public static FloatingTextController Instance { get; private set; }
    
    [SerializeField] private FloatingText floatingTextPrefab;
    [SerializeField] private float jumpDistance = 0.1f;
    
    private ObjectPool<FloatingText> _floatingTextPool;
    
    private Vector3 _jumpVector;

    public void Initialize()
    {
        Instance = this;
        _jumpVector = Vector3.up * jumpDistance;
        
        _floatingTextPool = new ObjectPool<FloatingText>(floatingTextPrefab,50);
    }

    public void GetText(char[] text, Vector3 position)
    {
        var floatingText = _floatingTextPool.Pop();
        floatingText.gameObject.SetActive(true);
        
        floatingText.Activate(text, 0.5f, position, position + _jumpVector, OnEnd);
    }

    private void OnEnd(FloatingText obj)
    {
        obj.gameObject.SetActive(false);
        _floatingTextPool.Push(obj);
    }
}
