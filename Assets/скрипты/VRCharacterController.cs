using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
  
    public Transform headTarget; // XR Origin Camera
    public Transform leftHandTarget; // Левый контроллер
    public Transform rightHandTarget; // Правый контроллер

    public Transform characterHead; // Голова модели
    public Transform characterLeftHand; // Левая рука модели
    public Transform characterRightHand; // Правая рука модели

    void Update()
    {
        if (headTarget && characterHead)
            characterHead.position = headTarget.position;

        if (leftHandTarget && characterLeftHand)
            characterLeftHand.position = leftHandTarget.position;

        if (rightHandTarget && characterRightHand)
            characterRightHand.position = rightHandTarget.position;
    }


}
