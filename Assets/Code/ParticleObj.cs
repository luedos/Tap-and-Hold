using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleObj : MonoBehaviour {

    ParticleSystem myPS;

    public float DefouldDuration = 3f;

    float Timer;
    bool SetInctive;

    private void Update()
    {
        if(Timer > 0f)
        {
            Timer -= Time.deltaTime;
            if(Timer < 0f)
            {
                Destroy(SetInctive);
            }
        }
    }

    /// <summary>
    /// Make particle visible and play from begining (also make Parent == null)
    /// </summary>
    /// <param name="duration"> if less or equal then 0 using defoult duration </param>
    /// <param name="MakeInactiveOnDestroy"> if true object.setactive(false) after time, else Destroy </param>
    public void Spawn(float duration = 0f, bool MakeInactiveOnDestroy = true)
    {
        gameObject.SetActive(true);

        if(myPS == null)
            myPS = GetComponent<ParticleSystem>();

        Timer = duration > 0f ? duration : DefouldDuration;

        SetInctive = MakeInactiveOnDestroy;

        transform.parent = null;

        myPS.time = 0f;
        myPS.Play();
        
    }

    public void Destroy(bool MakeInactive = true)
    {
        Timer = 0f;

        if (SetInctive)
            gameObject.SetActive(false);
        else
            Destroy(this.gameObject);
    }

}
