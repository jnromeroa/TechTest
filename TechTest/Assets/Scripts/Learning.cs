using System;
using System.Collections;
using UnityEngine;
using Mirror;
using UnityEditor.Hardware;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Learning : NetworkBehaviour
{
    [SerializeField] private Slider healthBar;
    public const float MaxHealth = 100;

    [SyncVar(hook = nameof(OnHealthChanged))]
    public float Health = 100;

    public void OnHealthChanged(float oldValue, float newValue)
    {
        healthBar.value = Mathf.Clamp01(newValue / MaxHealth);
    }
    
    [ContextMenu("Take 10 points of damage")]
    public void Take10pointsOfDamage()
    {
        Health -= 10;
        OnHealthChanged(Health, Health);
    }

[SerializeField] InputActionReference inputAction;
    bool isKeyPressed = false;



    public IEnumerator DialogoCoroutine()
    {
       inputAction.action.Enable();
       inputAction.action.performed += ctx => isKeyPressed = true;
        
       CmdGritarMilhouse();
       yield return new WaitUntil(() => isKeyPressed);
       isKeyPressed = false;
       CmdGritarMilhouse2();
       yield return new WaitUntil(() => isKeyPressed);
       isKeyPressed = false;
       CmdGritarMilhouse3();
    }


    [Command(requiresAuthority = false)]
    private void CmdGritarMilhouse(NetworkConnectionToClient sender = null)
    {
        Debug.Log("Milhouseeee!!");
        TargetGritarHomero(sender);
    }

    [TargetRpc]
    private void TargetGritarHomero(NetworkConnectionToClient sender)
    {
        Debug.Log("Queeeeee!!");
    }

    [Command(requiresAuthority = false)]
    private void CmdGritarMilhouse2(NetworkConnectionToClient sender = null)
    {
        Debug.Log("Dile a Bart que venga aqui!!");
        TargetGritarHomero2(sender);
    }
    
    [TargetRpc]
    private void TargetGritarHomero2(NetworkConnectionToClient sender)
    {
        Debug.Log("Creo que esta con Nelson!");
    }
    
    [Command(requiresAuthority = false)]
    private void CmdGritarMilhouse3(NetworkConnectionToClient sender = null)
    {
        Debug.Log("Quien es Nelson!!");
    }
}
