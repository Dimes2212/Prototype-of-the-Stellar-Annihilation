using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Text;

[RequireComponent(typeof(TMP_Text))]
public class TypewriterEffect : MonoBehaviour
{
    [Header("Text Settings")]
    [Tooltip("Speed between characters (in seconds)")]
    [SerializeField] private float typingSpeed = 0.05f;
    [Tooltip("Should text clear on start?")]
    [SerializeField] private bool clearOnStart = true;
    [Tooltip("Optional audio clip for typing sound")]
    [SerializeField] private AudioClip typingSound;

    [Header("Button Settings")]
    [Tooltip("Button that appears after text completes")]
    [SerializeField] private Button continueButton;
    [Tooltip("Delay before button appears")]
    [SerializeField] private float buttonAppearDelay = 0.5f;
    [Tooltip("Button fade-in duration")]
    [SerializeField] private float buttonFadeDuration = 0.3f;

    private TMP_Text tmpTextComponent;
    private AudioSource audioSource;
    private string fullText;
    private Coroutine typingCoroutine;
    private bool isTyping = false;

    void Awake()
    {
        // Получаем необходимые компоненты
        tmpTextComponent = GetComponent<TMP_Text>();

        // Добавляем AudioSource если есть звук
        if (typingSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.clip = typingSound;
        }

        // Инициализация текста
        if (tmpTextComponent != null)
        {
            fullText = tmpTextComponent.text;
            if (clearOnStart)
            {
                tmpTextComponent.text = "";
            }
        }

        // Настройка кнопки
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

            // Гарантируем наличие CanvasGroup
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
        // Останавливаем текущую печать
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        // Устанавливаем новый текст если передан
        if (!string.IsNullOrEmpty(customText))
        {
            fullText = customText;
        }

        // Запускаем корутину печати
        typingCoroutine = StartCoroutine(TypeTextRoutine());
    }

    private IEnumerator TypeTextRoutine()
    {
        isTyping = true;
        tmpTextComponent.text = "";
        StringBuilder stringBuilder = new StringBuilder();

        for (int i = 0; i < fullText.Length; i++)
        {
            // Добавляем символ
            stringBuilder.Append(fullText[i]);
            tmpTextComponent.text = stringBuilder.ToString();

            // Проигрываем звук
            if (audioSource != null && typingSound != null)
            {
                audioSource.Play();
            }

            // Пропускаем спецсимволы TMP
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

        // Завершение печати
        isTyping = false;

        // Показываем кнопку если есть
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