using System.Collections;
using UnityEngine;
using TMPro;

public class TypewriterUI : MonoBehaviour
{
    private TMP_Text _tmpProText;
    private string writer;
    private AudioSource audioSource;

    private bool animating = false;
    private bool skip = false;

    [SerializeField] float timeBtwChars = 0.1f;
    [SerializeField] string leadingChar = "";
    [SerializeField] bool leadingCharBeforeDelay = false;

    [SerializeField] GameObject skullScreen;
    [SerializeField] GameObject continueButton;
    [SerializeField] GameObject returnButton;
    [SerializeField] DataPersistanceManager dpm;
    [SerializeField] private DeathHandler dh;

    void Start()
    {
        _tmpProText = GetComponent<TMP_Text>()!;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && animating)
        {
            skip = true;
        }
    }

    public void StartTypewrite()
    {
        if (_tmpProText != null)
        {
            writer = _tmpProText.text;
            _tmpProText.text = "";
        }

        StartCoroutine(TypeWriterTMP());
    }

    IEnumerator TypeWriterTMP()
    {
        _tmpProText.text = leadingCharBeforeDelay ? leadingChar : "";
        animating = true;
        bool successfullySkipped = false;
        yield return new WaitForSeconds(DeathLength.DEATH_LENGTH + 2f);
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
                successfullySkipped = true;
                _tmpProText.text = writer;
                break;
            }

            audioSource.Play();
            yield return new WaitForSeconds(timeBtwChars);
        }

        if (dpm.GetGameState().NUMBER_OF_LIVES == 1)
        {
            yield return new WaitForSeconds(2f);
            dh.StartGameOverSequence();
            yield return null;
        }
        
        continueButton.GetComponent<ContinueButtonController>().Animate();
        returnButton.GetComponent<ContinueButtonController>().Animate();
        if (leadingChar != "" && !successfullySkipped)
        {
            _tmpProText.text = _tmpProText.text.Substring(0, _tmpProText.text.Length - leadingChar.Length);
        }
        skip = false;
        animating = false;
    }

    private uint GetMaxIndex()
    {
        Transform[] transforms = skullScreen.GetComponentsInChildren<Transform>();
        return (uint)(transforms.Length - 1);
    }
}