using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TypewriterUI : MonoBehaviour
{
    private Text _text;
    private TMP_Text _tmpProText;
    private string writer;
    private AudioSource audioSource;

    private bool animating = false;
    private bool skip = false;

    [SerializeField] float delayBeforeStart = 0f;
    [SerializeField] float timeBtwChars = 0.1f;
    [SerializeField] string leadingChar = "";
    [SerializeField] bool leadingCharBeforeDelay = false;

    [SerializeField] GameObject skullScreen;
    [SerializeField] GameObject continueButton;

    // Use this for initialization
    void Start()
    {
        _text = GetComponent<Text>()!;
        _tmpProText = GetComponent<TMP_Text>()!;
        audioSource = GetComponent<AudioSource>();

        if (_text != null)
        {
            writer = _text.text;
            _text.text = "";

            StartCoroutine("TypeWriterText");
        }

        if (_tmpProText != null)
        {
            writer = _tmpProText.text;
            _tmpProText.text = "";

            StartCoroutine("TypeWriterTMP");
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && animating)
        {
            skip = true;
        }
    }

    IEnumerator TypeWriterText()
    {
        _text.text = leadingCharBeforeDelay ? leadingChar : "";

        yield return new WaitForSeconds(delayBeforeStart);

        foreach (char c in writer)
        {
            if (_text.text.Length > 0)
            {
                _text.text = _text.text.Substring(0, _text.text.Length - leadingChar.Length);
            }
            _text.text += c;
            _text.text += leadingChar;
            yield return new WaitForSeconds(timeBtwChars);
        }

        if (leadingChar != "")
        {
            _text.text = _text.text.Substring(0, _text.text.Length - leadingChar.Length);
        }
    }

    IEnumerator TypeWriterTMP()
    {
        _tmpProText.text = leadingCharBeforeDelay ? leadingChar : "";
        animating = true;
        yield return new WaitForSeconds(delayBeforeStart);
        skullScreen.GetComponent<DestroySkull>().Destroy(GetMaxIndex());

        foreach (char c in writer)
        {
            if (_tmpProText.text.Length > 0)
            {
                _tmpProText.text = _tmpProText.text.Substring(0, _tmpProText.text.Length - leadingChar.Length);
            }
            _tmpProText.text += c;
            _tmpProText.text += leadingChar;

            if(skip)
            {
                skip = false;
                _tmpProText.text = writer;
                break;
            }

            audioSource.Play();
            yield return new WaitForSeconds(timeBtwChars);
        }
        continueButton.GetComponent<ContinueButtonController>().Animate();
        animating = false;
        if (leadingChar != "")
        {
            _tmpProText.text = _tmpProText.text.Substring(0, _tmpProText.text.Length - leadingChar.Length);
        }
    }

    private uint GetMaxIndex()
    {
        Transform[] transforms = skullScreen.GetComponentsInChildren<Transform>();
        return (uint)(transforms.Length - 1);
    }
}