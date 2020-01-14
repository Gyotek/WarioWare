using UnityAtoms;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private BoxCollider2D playArea = default;
    [SerializeField] private float growthRate = default;
    [SerializeField] private IntVariable difficulty = default;

    void Start()
    {
        growthRate /= difficulty.Value;
    }

    void Update()
    {
        transform.localScale += transform.localScale * growthRate * Time.deltaTime;
    }

    public void ResetPositionAndScale()
    {
        var X = Random.Range(playArea.bounds.min.x, playArea.bounds.max.x);
        var Y = Random.Range(playArea.bounds.min.y, playArea.bounds.max.y);
        transform.position = new Vector2(X, Y);

        transform.localScale = Vector2.one * 0.25f;
    }
}
