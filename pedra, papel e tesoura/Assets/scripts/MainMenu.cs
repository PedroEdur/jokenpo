using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public static class NetworkSessionConfig
{
    public static bool NetworkEnabled { get; private set; }
    public static bool IsHost { get; private set; }

    public static void StartLocal()
    {
        NetworkEnabled = false;
        IsHost = true;
    }

    public static void StartHost()
    {
        NetworkEnabled = true;
        IsHost = true;
    }

    public static void StartClient()
    {
        NetworkEnabled = true;
        IsHost = false;
    }
}

public class MainMenu : MonoBehaviour
{
    [SerializeField] private bool criarBotoesDeRede = true;

    private void Start()
    {
        if (criarBotoesDeRede)
        {
            CriarBotoesDeRede();
        }
    }

    public void PlayGame()
    {
        NetworkSessionConfig.StartLocal();
        SceneManager.LoadScene("GameScene");
    }

    public void HostGame()
    {
        NetworkSessionConfig.StartHost();
        SceneManager.LoadScene("GameScene");
    }

    public void JoinGame()
    {
        NetworkSessionConfig.StartClient();
        SceneManager.LoadScene("GameScene");
    }

    private void CriarBotoesDeRede()
    {
        Canvas canvas = FindObjectOfType<Canvas>();

        if (canvas == null || EncontrarFilho(canvas.transform, "HOST") != null || EncontrarFilho(canvas.transform, "ENTRAR") != null)
        {
            return;
        }

        Transform sair = EncontrarFilho(canvas.transform, "SAIR");

        if (sair is RectTransform sairRect)
        {
            sairRect.anchoredPosition = new Vector2(sairRect.anchoredPosition.x, -172f);
        }

        CriarBotao(canvas.transform, "HOST", "HOST", new Vector2(0f, -50f), HostGame);
        CriarBotao(canvas.transform, "ENTRAR", "ENTRAR", new Vector2(0f, -111f), JoinGame);
    }

    private Transform EncontrarFilho(Transform parent, string nome)
    {
        foreach (Transform child in parent.GetComponentsInChildren<Transform>(true))
        {
            if (child.name == nome)
            {
                return child;
            }
        }

        return null;
    }

    private void CriarBotao(Transform parent, string nome, string texto, Vector2 posicao, UnityEngine.Events.UnityAction acao)
    {
        GameObject botaoObjeto = new GameObject(nome, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
        botaoObjeto.transform.SetParent(parent, false);

        RectTransform rect = botaoObjeto.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = posicao;
        rect.sizeDelta = new Vector2(162f, 46f);

        Image image = botaoObjeto.GetComponent<Image>();
        image.color = Color.white;

        Button button = botaoObjeto.GetComponent<Button>();
        button.targetGraphic = image;
        button.onClick.AddListener(acao);

        GameObject textoObjeto = new GameObject("Text (TMP)", typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
        textoObjeto.transform.SetParent(botaoObjeto.transform, false);

        RectTransform textoRect = textoObjeto.GetComponent<RectTransform>();
        textoRect.anchorMin = Vector2.zero;
        textoRect.anchorMax = Vector2.one;
        textoRect.offsetMin = Vector2.zero;
        textoRect.offsetMax = Vector2.zero;

        TextMeshProUGUI label = textoObjeto.GetComponent<TextMeshProUGUI>();
        label.text = texto;
        label.alignment = TextAlignmentOptions.Center;
        label.fontSize = 24f;
        label.color = Color.black;
    }

    public void QuitGame()
    {
        Debug.Log("Saindo do jogo...");

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
