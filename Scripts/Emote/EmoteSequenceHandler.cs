using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EmoteEvents
{
    public static Action StartedSequence;
    public static Action<Dictionary<StatType, float>> CompletedSequence;
}

//Example: To start the sequence in another scene: SceneManager.LoadSceneAsync(SceneNames.EMOTE_SEQUENCE, LoadSceneMode.Additive); 
public class EmoteSequenceHandler : MonoBehaviour
{
    [Header("Entirety")]
    [SerializeField] private CanvasGroup m_CanvasGroup;

    [Header("Sequence")]
    [SerializeField, Range(1, 10)] private int MinSelection = 3; // Make sure min is lower than max :3
    [SerializeField, Range(2, 10)] private int MaxSelection = 6;
    [SerializeField, Range(0f, 3f)] private float m_ShowEmoteInterval = 0.85f;
    [SerializeField] private float m_HideDuration = 2.5f;

    [Header("Emote")]
    [SerializeField] private List<EmoteData> m_Emotes;
    [SerializeField] private GameObject m_EmoteButtonPrefab;
    [SerializeField] private Button m_RemoveButtonPrefab;
    [SerializeField] private Transform m_EmoteListContainer;
    [SerializeField] private Transform m_EmoteBarContainer;
    [SerializeField] private Button m_ConfirmButton;

    [Header("Results")]
    [SerializeField] private Transform m_EmoteResultContainer;
    [SerializeField] private Image m_BigEmoteImagePrefab;

    [Header("Text")]
    [SerializeField] private Transform m_TextContainer;
    [SerializeField] private TMP_Text m_DiplomacyText;
    [SerializeField] private TMP_Text m_PeopleText;
    [SerializeField] private TMP_Text m_ResourcesText;
    [SerializeField] private TMP_Text m_FoodText;

    [Header("Audio")]
    [SerializeField] private AudioSource m_AudioSource;
    [SerializeField] private AudioClip m_PopUpSound;
    [SerializeField] private UIHoverSound m_UIHoverSound;

    [Header("Runtime")]
    private List<EmoteData> m_SelectedEmotes = new List<EmoteData>();
    private Dictionary<StatType, float> m_CurrentStats = new Dictionary<StatType, float>();

    private void Awake()
    {
        InitializeStats();
    }

    private void Start()
    {
        BuildEmoteButtons();

        HideResults();

        m_ConfirmButton.onClick.AddListener(OnConfirm);

        m_AudioSource.clip = m_PopUpSound;
        m_AudioSource.Play();

        StartCoroutine(AnimatePopup(m_CanvasGroup.gameObject));
    }

    private void OnEnable()
    {
        EmoteEvents.CompletedSequence += UpdateText;
    }

    private void OnDisable()
    {
        EmoteEvents.CompletedSequence -= UpdateText;
    }

    private void Update()
    {
        CheckForEmoteHotKeyPress();
    }

    private void InitializeStats()
    {
        System.Array statValues = System.Enum.GetValues(typeof(StatType));
        foreach (object statType in statValues)
        {
            m_CurrentStats[(StatType)statType] = 0;
        }
    }

    public void ResetSequence()
    {
        for (int i = 0; i < m_EmoteBarContainer.childCount; i++)
        {
            Transform child = m_EmoteBarContainer.GetChild(i);
            if (child.name.Contains("Background"))
            {
                continue;
            }

            Destroy(child.gameObject);
        }

        for (int i = 0; i < m_EmoteResultContainer.childCount; i++)
        {
            Transform child = m_EmoteResultContainer.GetChild(i);
            if (child.name.Contains("Background"))
            {
                continue;
            }

            Destroy(child.gameObject);
        }

        m_SelectedEmotes.Clear();

        HideResults();

        m_EmoteListContainer.gameObject.SetActive(true);
        m_EmoteBarContainer.gameObject.SetActive(true);
        m_ConfirmButton.gameObject.SetActive(false);
    }

    private void BuildEmoteButtons()
    {
        for (int i = 0; i < m_Emotes.Count; i++)
        {
            EmoteData emoteData = m_Emotes[i];
            GameObject emoteGameObject = Instantiate(m_EmoteButtonPrefab, m_EmoteListContainer);
            Button button = emoteGameObject.GetComponent<Button>();
            Image icon = emoteGameObject.GetComponent<Image>();
            icon.sprite = emoteData.Icon;

            emoteGameObject.gameObject.AddComponent<UIHoverSound>().Initialize(m_UIHoverSound.AudioMixerGroup, m_UIHoverSound.HighlightSound, m_UIHoverSound.DeselectSound);

            button.onClick.AddListener(delegate { AddEmote(emoteData); });
        }
    }

    private void AddEmote(EmoteData emoteData)
    {
        //if (m_SelectedEmotes.Contains(emoteData))
        //{
        //    return;
        //}

        if (m_SelectedEmotes.Count >= MaxSelection)
        {
            return;
        }

        m_SelectedEmotes.Add(emoteData);
         
        // In the end this should have just been a prefab xD

        GameObject copy = new GameObject(emoteData.EmoteName, typeof(Image));
        Image image = copy.GetComponent<Image>();
        image.sprite = emoteData.Icon;
        copy.transform.SetParent(m_EmoteBarContainer, false);

        Button closeButton = Instantiate(m_RemoveButtonPrefab);
        if (closeButton != null)
        {
            closeButton.transform.SetParent(copy.transform, false);

            RectTransform rectTransform = closeButton.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(1f, 1f);
            rectTransform.anchorMax = new Vector2(1f, 1f);
            rectTransform.pivot = new Vector2(1f, 1f);
            rectTransform.anchoredPosition = new Vector2(1f, 1f); // Slight offset. Eww hardcoded

            closeButton.gameObject.AddComponent<UIHoverSound>().Initialize(m_UIHoverSound.AudioMixerGroup, m_UIHoverSound.HighlightSound, m_UIHoverSound.DeselectSound);
            closeButton.onClick.AddListener(() => RemoveEmote(emoteData, copy));
        }

        StartCoroutine(AnimatePopup(copy));

        if (m_SelectedEmotes.Count >= MinSelection)
        {
            m_ConfirmButton.gameObject.SetActive(true);
        }
    }

    private void RemoveEmote(EmoteData emoteData, GameObject iconObject)
    {
        if (m_SelectedEmotes.Contains(emoteData))
        {
            m_SelectedEmotes.Remove(emoteData);

            StartCoroutine(AnimateShrinkAndDestroy(iconObject));
        }

        if (m_SelectedEmotes.Count < MinSelection)
        {
            m_ConfirmButton.gameObject.SetActive(false);
        }
    }

    private void CheckForEmoteHotKeyPress()
    {
        for (int i = 0; i < m_Emotes.Count; i++)
        {
            EmoteData emoteData = m_Emotes[i];
            if (Input.GetKeyDown(emoteData.Hotkey))
            {
                AddEmote(emoteData);

                break;
            }
        }
    }

    private void OnConfirm()
    {
        m_EmoteListContainer.gameObject.SetActive(false);
        m_EmoteBarContainer.gameObject.SetActive(false);
        m_ConfirmButton.gameObject.SetActive(false);
        StartCoroutine(PlaySequence());
    }

    private IEnumerator PlaySequence()
    {
        List<EmoteStat> allStatChanges = new List<EmoteStat>();

        ShowResults();

        for (int i = 0; i < m_SelectedEmotes.Count; i++)
        {
            EmoteData emoteData = m_SelectedEmotes[i];

            Image image = Instantiate(m_BigEmoteImagePrefab, m_EmoteResultContainer);
            if (image != null)
            {
                image.sprite = emoteData.Icon;
            }

            m_AudioSource.clip = emoteData.Sound;
            m_AudioSource.Play();

            for (int j = 0; j < emoteData.StatChanges.Count; j++)
            {
                allStatChanges.Add(emoteData.StatChanges[j]);
            }

            StartCoroutine(AnimatePopup(image.gameObject));

            yield return new WaitForSeconds(m_ShowEmoteInterval);
            m_AudioSource.Stop();
        }

        ApplyStats(allStatChanges);

        StartCoroutine(AnimateFadeOut(m_CanvasGroup, m_HideDuration));

        yield return new WaitForSeconds(m_HideDuration + 0.6f);

        SceneManager.UnloadSceneAsync(SceneNames.EMOTE_SEQUENCE);
    }

    private void ApplyStats(List<EmoteStat> newStatChanges)
    {
        List<StatType> appliedStatMultiplier = new List<StatType>();
        float multiplier = 1f;

        for (int i = 0; i < newStatChanges.Count; i++)
        {
            EmoteStat statChange = newStatChanges[i];

            // Apply multiplier only once per StatType.
            if (appliedStatMultiplier.Contains(statChange.Type) == false && statChange.Multiplier != 1f)
            {
                multiplier *= statChange.Multiplier;
                appliedStatMultiplier.Add(statChange.Type);
            }
        }

        for (int i = 0; i < newStatChanges.Count; i++)
        {
            EmoteStat statChange = newStatChanges[i];

            float amount = statChange.Amount * multiplier;

            amount = Mathf.Ceil(amount);

            m_CurrentStats[statChange.Type] += amount;
        }

        EmoteEvents.CompletedSequence?.Invoke(m_CurrentStats);
    }

    private void UpdateText(Dictionary<StatType, float> currentStats)
    {
        if (m_DiplomacyText != null)
        {
            m_DiplomacyText.text = "" + currentStats[StatType.Diplomacy];
        }

        if (m_PeopleText != null)
        {
            m_PeopleText.text = "" + currentStats[StatType.People];
        }

        if (m_ResourcesText != null)
        {
            m_ResourcesText.text = "" + currentStats[StatType.Resources];
        }

        if (m_FoodText != null)
        {
            m_FoodText.text = "" + currentStats[StatType.Food];
        }
    }

    private void ShowResults()
    {
        m_EmoteResultContainer.gameObject.SetActive(true);

        m_TextContainer.gameObject.SetActive(true);

        StartCoroutine(AnimatePopup(m_TextContainer.gameObject));
    }

    private void HideResults()
    {
        m_ConfirmButton.gameObject.SetActive(false);

        m_EmoteResultContainer.gameObject.SetActive(false);

        m_TextContainer.gameObject.SetActive (false);
    }

    private IEnumerator AnimatePopup(GameObject target)
    {
        // Make these values not hard coded :3

        float duration = 0.3f;
        float time = 0f;

        while (time < duration)
        {
            float progress = time / duration;

            float scaleValue;
            if (progress < 0.5f)
            {
                scaleValue = Mathf.Lerp(0f, 1.2f, progress * 2f); // Scale up to overshoot
            }
            else
            {
                scaleValue = Mathf.Lerp(1.2f, 1f, (progress - 0.5f) * 2f);
            }

            target.transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
            time += Time.deltaTime;
            yield return null;
        }

        target.transform.localScale = Vector3.one;
    }

    private IEnumerator AnimateShrinkAndDestroy(GameObject target)
    {
        // Make these values not hard coded :3

        Vector3 start = target.transform.localScale;
        Vector3 end = Vector3.zero;
        float time = 0f;
        float duration = 0.15f;

        while (time < duration)
        {
            target.transform.localScale = Vector3.Lerp(start, end, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        target.transform.localScale = end;

        Destroy(target);
    }

    private IEnumerator AnimateShrink(GameObject target)
    {
        // Make these values not hard coded :3 I have written this many times hahaha.

        Vector3 start = target.transform.localScale;
        Vector3 end = Vector3.zero;
        float time = 0f;
        float duration = 0.15f;

        while (time < duration)
        {
            target.transform.localScale = Vector3.Lerp(start, end, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        target.transform.localScale = end;
    }

    private IEnumerator AnimateFadeOut(CanvasGroup group, float duration)
    {
        yield return new WaitForSeconds(duration - 0.5f);
        duration -= 0.5f;

        float start = group.alpha;
        float end = -0.5f;

        float time = 0f;

        while (time < duration)
        {
            group.alpha = Mathf.Lerp(start, end, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        group.alpha = end;
    }
}
