using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Text;

[RequireComponent(typeof(TMP_Text))]
public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] private float typingSpeed = 0.05f;
    [SerializeField] private bool clearOnStart = true;
    [SerializeField] private AudioClip typingSound;
    [SerializeField] private Button continueButton;
    [SerializeField] private float buttonAppearDelay = 0.5f;
    [SerializeField] private float buttonFadeDuration = 0.3f;

    private TMP_Text tmpTextComponent;
    private AudioSource audioSource;
    private string fullText;
    private Coroutine typingCoroutine;
    private bool isTyping = false;

    void Awake()
    {
        tmpTextComponent = GetComponent<TMP_Text>();

        if (typingSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.clip = typingSound;
        }

        if (tmpTextComponent != null)
        {
            fullText = tmpTextComponent.text;
            if (clearOnStart)
            {
                tmpTextComponent.text = "";
            }
        }

        InitializeButton();
    }

    void Start()
    {
        if (clearOnStart && !string.IsNullOrEmpty(fullText))
        {
            StartTyping();
        }
    }

    void InitializeButton()
    {
        if (continueButton != null)
        {
            continueButton.gameObject.SetActive(false);

            CanvasGroup cg = continueButton.GetComponent<CanvasGroup>();
            if (cg == null)
            {
                cg = continueButton.gameObject.AddComponent<CanvasGroup>();
            }
            cg.alpha = 0;
            continueButton.interactable = false;
        }
    }

    public void StartTyping(string customText = null)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        if (!string.IsNullOrEmpty(customText))
        {
            fullText = customText;
        }

        typingCoroutine = StartCoroutine(TypeTextRoutine());
    }

    private IEnumerator TypeTextRoutine()
    {
        isTyping = true;
        tmpTextComponent.text = "";
        StringBuilder stringBuilder = new StringBuilder();

        for (int i = 0; i < fullText.Length; i++)
        {
            stringBuilder.Append(fullText[i]);
            tmpTextComponent.text = stringBuilder.ToString();

            if (audioSource != null && typingSound != null)
            {
                audioSource.Play();
            }

            if (fullText[i] == '<')
            {
                while (i < fullText.Length && fullText[i] != '>')
                {
                    stringBuilder.Append(fullText[i]);
                    i++;
                }
                if (i < fullText.Length)
                {
                    stringBuilder.Append(fullText[i]);
                }
                tmpTextComponent.text = stringBuilder.ToString();
                continue;
            }

            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;

        if (continueButton != null)
        {
            yield return new WaitForSeconds(buttonAppearDelay);
            yield return ShowButtonRoutine();
        }
    }

    private IEnumerator ShowButtonRoutine()
    {
        continueButton.gameObject.SetActive(true);
        CanvasGroup cg = continueButton.GetComponent<CanvasGroup>();

        float timer = 0f;
        while (timer < buttonFadeDuration)
        {
            cg.alpha = Mathf.Lerp(0f, 1f, timer / buttonFadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        cg.alpha = 1f;
        continueButton.interactable = true;
    }

    public void SkipTyping()
    {
        if (isTyping && typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            tmpTextComponent.text = fullText;
            isTyping = false;

            if (continueButton != null)
            {
                continueButton.gameObject.SetActive(true);
                CanvasGroup cg = continueButton.GetComponent<CanvasGroup>();
                cg.alpha = 1f;
                continueButton.interactable = true;
            }
        }
    }

    public void SetText(string newText, bool startTyping = true)
    {
        fullText = newText;
        if (startTyping)
        {
            StartTyping();
        }
        else
        {
            tmpTextComponent.text = fullText;
        }
    }

    public bool IsTyping()
    {
        return isTyping;
    }
}