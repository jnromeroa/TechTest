using UnityEngine;

public class TittleAnimation : MonoBehaviour
{
    private Animator anim;
    
    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public void PlayAnimation()
    {
        anim.SetBool("Exit", true);
    }
}
