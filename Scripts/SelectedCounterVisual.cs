using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    //all the counters listen to the player event 
    //pb je crois que chaque counter va écouter donc c'est  couteux mais ici genre une douzaine de counter donc pas de pb

    //on peut faire comme avant et mettre une ref et drag la reférence  il dit que ce serait chiant de drag and drop 
    //we're going to have like a dozen counters in our map so it would be pretty tedious to drag and drop the player reference to every single of them
    //pas capté tu peux pas juste le faire dans la prefab ?
    //genre [SerializeField] private GameInput gameInput; puis on s'abonne 

    //Singleton pattern     one instance of something    just a single player because solo game 

    [SerializeField] private BaseCounter clearCounter;
    [SerializeField] private GameObject[] visualGameObjectArray;

    private void Start() //en start car tu mets dans le player le signleton pattern en awake pour qu'il se déclenche avant 
    {
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if(e.selectedCounter == clearCounter)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    //Si je comprends bien la logique dans le player si on a un counter  on l'envoie et ici chaque counter a son counter donc on regarde si lui envoyé et le 
    //me^me que celui de l'objet si oui on montre le visual 
    private void Show()
    {
        foreach (GameObject visualGameObject in visualGameObjectArray)
        {
            visualGameObject.SetActive(true);
        }
    }
    private void Hide()
    {
        foreach (GameObject visualGameObject in visualGameObjectArray)
        { 
            visualGameObject.SetActive(false);
        }
    }
}
