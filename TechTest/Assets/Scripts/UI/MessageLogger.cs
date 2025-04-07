using UnityEngine;
using TMPro;
public class MessageLogger : MonoBehaviour
{
    public static MessageLogger Instance;
    [SerializeField] GameObject msgLoggerPanel;
    [SerializeField] TMP_Text headerTxt;
    [SerializeField] TMP_Text contentTxt;

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

    public void Clear()
    {
        headerTxt.text = string.Empty;
        contentTxt.text = string.Empty;
    }

    public static void LogMsg(string header, string content)
    {
        Instance.headerTxt.text = header;
        Instance.contentTxt.text = content;
        Instance.msgLoggerPanel.SetActive(true);
    }

}
