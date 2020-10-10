using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationSceneUI : MonoBehaviour
{
    [SerializeField] 
    private GameObject animationContent;
    [SerializeField]
    private GameObject characterPanel;
    [SerializeField]
    private GameObject animationButtonPrefab;
    [SerializeField]
    private GameObject characterObjectFolder;
    private Animator _characterAnimator;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            Instantiate(animationButtonPrefab, animationContent.transform).SetActive(false);
        }
        CreateCharacterButton();
        for (int i = 0; i < characterObjectFolder.transform.childCount; i++)
        {
            characterObjectFolder.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void CreateCharacterButton()
    {
        var characterContent = characterPanel.transform.Find("Content");
        for (int i = 0; i < characterObjectFolder.transform.childCount; i++)
        {
             var character = characterObjectFolder.transform.GetChild(i);
             var newButtonObject = Instantiate(animationButtonPrefab, characterContent);
             newButtonObject.transform.GetChild(0).GetComponent<Text>().text = character.name;
             var clickedEvent = new Button.ButtonClickedEvent();
             clickedEvent.AddListener(() =>
             {
                 Debug.Log("버튼클릭");
                 ChangeAnimator(character.name); 
                 UpdateAnimationButton();
                 FocusCamera(character);
             });
             newButtonObject.GetComponent<Button>().onClick = clickedEvent;
        }
    }

    private void ChangeAnimator(string characterName)
    {
        if (_characterAnimator != null)
        {
            _characterAnimator.gameObject.SetActive(false);    
        }
        
        var newCharacter = characterObjectFolder.transform.Find(characterName);
        _characterAnimator = newCharacter.GetComponent<Animator>();
        newCharacter.gameObject.SetActive(true);
    }

    private void UpdateAnimationButton()
    {
        var animatorParameters = _characterAnimator.parameters;
        for (int i = 0; i < animatorParameters.Length; i++)
        {
            int index = i;
            var animationButtonObject = GetAnimationButtonObject(i);
            var clickedEvent = new Button.ButtonClickedEvent();
            clickedEvent.AddListener(() => _characterAnimator.SetTrigger(animatorParameters[index].name));
            animationButtonObject.GetComponent<Button>().onClick = clickedEvent;
            animationButtonObject.transform.GetChild(0).GetComponent<Text>().text = animatorParameters[index].name;
            animationContent.transform.GetChild(i).gameObject.SetActive(true);
        }

        for (int i = animatorParameters.Length; i < animationContent.transform.childCount; i++)
        {
            animationContent.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private GameObject GetAnimationButtonObject(int index)
    {
        if (animationContent.transform.childCount <= index)
        {
            for (int i = 0; i < index - animationContent.transform.childCount; i++)
            {
                Instantiate(animationButtonPrefab, animationContent.transform);   
            }
        }

        return animationContent.transform.GetChild(index).gameObject;
    }

    private void FocusCamera(Transform characterObject)
    {
        var cameraTransform = Camera.main.transform;
        cameraTransform.position = new Vector3(
            characterObject.position.x,
            characterObject.position.y, 
            cameraTransform.position.z);
        Debug.Log(cameraTransform.position);
    }

    public void ShowCharacterContent()
    {
        characterPanel.SetActive(true);
    }

    public void HideCharacterContent()
    {
        characterPanel.SetActive(false);
    }
}
