using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    static LoadingScreen Instance;
    [SerializeField] GameObject loadingScreenPanel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            return;
        }
        Destroy(this.gameObject);
    }

    public static void Open()
    {
        Instance.loadingScreenPanel.SetActive(true);
    }

    public static void Close()
    {
        Instance.loadingScreenPanel.SetActive(false);
    }
}
