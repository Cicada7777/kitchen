using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";

    [SerializeField] private Player player;

    //Soit  [SerializeField] et référence depuis l'éditeur 
    //l'animator est attaché au même gameobject que ce script ici je crois le playervisuak  donc on peut faire un getcomponent 

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetBool(IS_WALKING, player.IsWalking());

    }
}
