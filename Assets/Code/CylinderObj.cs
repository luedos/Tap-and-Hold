using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CylinderObj : MonoBehaviour {

    [Header("DIY")]
    public GameObject ObjToScale;

    public GameObject CameraPos;

    public ParticleObj myParticle;

    public MeshRenderer myMR;

    

    private float destScale = 0.1f;

    private float currentX = -1f;
    private float SineMax;
    private float SineMin;
    
    private bool ExpandingUp = true;

    private bool Expanding = false;
    private CylinderObj myPrevious;

    private bool wasPerfect = false;

    private float DestroyTimer;

    private float MaxDestroyTime;



    public void Create(CylinderObj inPrevious)
    {
        if (myMR != null)
            myMR.material.color = Color.white;

        DestroyTimer = 0f;

        myPrevious = inPrevious;

        transform.position = myPrevious == null ? 
            new Vector3(0f,0.5f,0f) : 
            new Vector3(0, myPrevious.transform.position.y + myPrevious.ObjToScale.transform.localScale.y * 2, 0);

        destScale = 0f;

        ExpandingUp = true;

        wasPerfect = false;

        ObjToScale.transform.localScale = new Vector3(destScale, ObjToScale.transform.localScale.y, destScale);

        Expanding = true;

        currentX = -1f;

        gameObject.SetActive(true);
    }

    public void SetScaleAndPerfect(float inDestScale, bool inWasPerfect)
    {
        destScale = inDestScale;
        wasPerfect = inWasPerfect;
        ObjToScale.transform.localScale = new Vector3(destScale, ObjToScale.transform.localScale.y, destScale);
    }


    public void Update()
    {
        if (GameManager.Instance == null)
            return;

        if (Expanding)
        {
            float PastBlockScale = (myPrevious == null ? 1.1f : myPrevious.destScale * 1.1f);

            if (ExpandingUp)
            {
                destScale += GameManager.Instance.mySettings.ScaleSpeed * Time.deltaTime;
                if (destScale >= PastBlockScale)
                    ExpandingUp = false;
            }
            else
            {
                destScale -= GameManager.Instance.mySettings.ScaleSpeed * Time.deltaTime;
                if (destScale <= 0.1f)
                    ExpandingUp = true;
            }

            ObjToScale.transform.localScale = new Vector3(destScale, ObjToScale.transform.localScale.y, destScale);

        }
        else if (currentX >= 0)
        {
            currentX += Time.deltaTime * GameManager.Instance.mySettings.WaveSpeed;

            float NowScale = Mathf.Sin(currentX) * SineMax + SineMin;

            if (currentX > 0.5f && NowScale < destScale)
            {
                NowScale = destScale;

                currentX = -1f;
            }

            ObjToScale.transform.localScale = new Vector3(NowScale, ObjToScale.transform.localScale.y, NowScale);
        }

        if (DestroyTimer > 0f)
        {
            if (myMR != null && MaxDestroyTime > 0f)
                myMR.material.color = Color.Lerp(Color.white, Color.red, 1 - DestroyTimer / MaxDestroyTime);

            DestroyTimer -= Time.deltaTime;
            if (DestroyTimer < 0f)
            {
                DestroyTimer = 0f;
                Destroy();
            }
        }
    }



    public bool Stop()
    {

        if (!Expanding)
            return true;

        if (destScale < 0.1f)
        {
            destScale = 0.1f;
            ObjToScale.transform.localScale = new Vector3(destScale, ObjToScale.transform.localScale.y, destScale);
        }


        Expanding = false;

        float PastBlockScale = (myPrevious == null ? 1.0f : myPrevious.destScale);


        if (destScale / PastBlockScale >= 1f)
        {
            GameManager.Instance.GameOver();
            return false;
        }
        else if (destScale > PastBlockScale - GameManager.Instance.mySettings.PerfectDelta)
        {
            wasPerfect = true;

            currentX = 0f;

            SineMax = 0.4f;
            SineMin = ObjToScale.transform.localScale.x;
            destScale = destScale < 0.8f ? destScale + 0.2f : 1f;


            GameManager.Instance.PerfectMove();
        }

        return true;

    }

    public void PerfectMove()
    {
        SineMax = 0.3f;

        if(wasPerfect)
        {
            SineMin = destScale;

            currentX = Mathf.Asin((ObjToScale.transform.localScale.x - destScale) / SineMax);
        }
        else
        {
            if(GameManager.Instance.mySettings.SmollerOnPerfect)
            {
                destScale *= 0.8f;
                if (destScale < 0.1f)
                    destScale = 0.1f;
            }

            if(ObjToScale.transform.localScale.x < destScale)
            {
                currentX = 0;
                SineMin = ObjToScale.transform.localScale.x;

            }
            else
            {
                SineMin = destScale;
                currentX = Mathf.Asin((ObjToScale.transform.localScale.x - destScale) / SineMax);
            }
        }
    }

    public void Expload(float onDur = 0f)
    {
        DestroyTimer = onDur;
        MaxDestroyTime = onDur;
        if (onDur <= 0f)
            Destroy();
    }

    private void Destroy()
    {
        DestroyTimer = 0f;
        if (myParticle != null)
        {
            myParticle.transform.position = transform.position;
            myParticle.Spawn();
        }

        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        if (myParticle != null)
            Destroy(myParticle);
    }

}
