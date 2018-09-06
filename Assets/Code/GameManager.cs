using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public CylinderObj FirstObj;
    public int StartSpawn = 20;

    List<CylinderObj> myPool = new List<CylinderObj>();
    int ObjCount;
    CylinderObj Current;

    public HUD myHUD;

    public SO_Settings mySettings;

    private static GameManager instance;


    public GameObject StartCameraPos;

    GameObject EndCameraPos;
    Camera myCamera;
    GameObject NextCameraPos;


    private float gameOver = -1f;

    public static GameManager Instance
    { get { return instance; } }

    // Use this for initialization
    void Start() {

        if (instance != null && instance != this)
        {
            Destroy(instance);
        }
        instance = this;

        if (EndCameraPos == null)
            EndCameraPos = new GameObject();

        myCamera = Camera.main;

        FirstObj.SetScaleAndPerfect(1f, true);


        // Filling up our object pool
        if (myPool.Capacity < StartSpawn)
            myPool.Capacity = StartSpawn;
        for (int i = myPool.Count; i < StartSpawn; ++i)
        {
            myPool.Add(Instantiate<CylinderObj>(FirstObj));

            myPool[i].gameObject.SetActive(false);
        }


        NewGame();
    }

    // Update is called once per frame
    void Update() {


        // Debug staff
        if (Input.GetKeyDown(KeyCode.O))
        {
            SpawnObj();
            if (Current.Stop())
                myHUD.UpdateScore(ObjCount);
            Current.SetScaleAndPerfect(1f, Input.GetKeyDown(KeyCode.LeftControl));
        }

        if (Input.GetKeyDown(KeyCode.P))
            PerfectMove();


        if ((myCamera.transform.position - NextCameraPos.transform.position).magnitude > 0.1f)
        {
            myCamera.transform.position = Vector3.Lerp(myCamera.transform.position, NextCameraPos.transform.position, 2f * Time.deltaTime);
        }
        if ((myCamera.transform.rotation.eulerAngles - NextCameraPos.transform.rotation.eulerAngles).magnitude > 5)
            myCamera.transform.rotation = Quaternion.Lerp(myCamera.transform.rotation, NextCameraPos.transform.rotation, 3f * Time.deltaTime);

        if (Input.GetMouseButtonDown(0))
        {

            if (gameOver < 0f)
                SpawnObj();
            
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (gameOver == 0f)
            {
                NewGame();
            }

            if (Current.Stop() && gameOver < 0f)
                myHUD.UpdateScore(ObjCount);
        }



        if (Input.touches.Length > 0)
        {
            TouchPhase myPhase = Input.touches[0].phase;

            if (myPhase == TouchPhase.Began)
            {
                if (gameOver < 0f)
                    SpawnObj();
            }
            else if (myPhase == TouchPhase.Canceled || myPhase == TouchPhase.Ended)
            {
                if (gameOver == 0f)
                {
                    NewGame();
                }

                if (Current.Stop() && gameOver < 0f)
                    myHUD.UpdateScore(ObjCount);
            }
        }



        if (gameOver > 0f)
        {
            gameOver -= Time.deltaTime;
            if (gameOver < 0f)
            {
                gameOver = 0f;

                float angle = myCamera.fieldOfView * Mathf.Deg2Rad;

                Vector3 pos = new Vector3(0, (ObjCount + 1) * 2.0f * FirstObj.ObjToScale.transform.localScale.y * mySettings.CameraOnEndDistance);

                pos.z = -pos.y / Mathf.Tan(angle);

                angle = Mathf.Tan(angle / 2);

                angle = Mathf.Atan(1.1f / (angle * (0.1f / (1 - angle * angle) + 1.1f))) * Mathf.Rad2Deg / 1.5f;

                EndCameraPos.transform.position = pos;
                EndCameraPos.transform.rotation = Quaternion.Euler(angle, 0f, 0f);

                NextCameraPos = EndCameraPos;

                myHUD.GameOverMenu(true);
            }
        }

    }

    private void SpawnObj()
    {
        if (ObjCount >= myPool.Count)
        {
            myPool.Add(Instantiate<CylinderObj>(FirstObj));

            ObjCount = myPool.Count - 1;
        }

        myPool[ObjCount].Create(Current);

        Current = myPool[ObjCount];

        NextCameraPos = Current.CameraPos;

        ++ObjCount;
    }

    public void NewGame()
    {
        StopAllCoroutines();

        NextCameraPos = FirstObj.CameraPos;

        Current = FirstObj;

        gameOver = -1f;
        
        for (int i = 0; i < myPool.Count; ++i)
        {
            if (!myPool[i].gameObject.activeSelf)
                break;
            myPool[i].Expload();

        }
        ObjCount = 0;

        myHUD.GameOverMenu(false);
        myHUD.UpdateScore(ObjCount);

    }

    public void GameOver()
    {
        print("Game Over");

        Current.Expload(1f);

        gameOver = 2f;
    }

    public void PerfectMove()
    {        
        StartCoroutine(PerfectCoroutine());
    }

    public IEnumerator PerfectCoroutine()
    {


        for(int i = ObjCount - 2; i >= 0; --i)
        {
            yield return new WaitForSeconds(mySettings.WaveDelay);
            myPool[i].PerfectMove();
            
        }

        yield return new WaitForSeconds(mySettings.WaveDelay);

        FirstObj.PerfectMove();

        yield break;
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    // OnRestart:
    // ObjCount = 0;
    // Current = null
    // All objects destroy
}
