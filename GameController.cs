using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{
    private CubePos nowCube = new CubePos(0, 1, 0);
    public float cubelocationchangerate = 0.4f;
    public Color[] bgColoros;
    private Color toBackgroundColor;
    private Transform mainCam;
    public Transform cubeToPlace;
    public Text scoreTxt, scoreTxt2;
    public Text[] texts;
    public GameObject allcubes, vpx;
    public GameObject[] cubesToCreatev;
    public GameObject[] CanvasStartedPackets;
    private Rigidbody allcubesRB;
    private bool IsLose, firstcube = false;
    private Coroutine _ShowCubePlace;
    private float moveCamToYpos, camMoveSpeed = 2f;
    private int prevCoutmaxHonor;
    private List<GameObject> posiblesCubesToCreate = new List<GameObject>();

    private List<Vector3> busysCubePosision = new List<Vector3>
    {
        new Vector3(0, 0, 0),
        new Vector3(0, 1, 0),
        new Vector3(1, 0, 1),
        new Vector3(-1, 0, -1),
        new Vector3(0, 0, 1),
        new Vector3(0, 0, -1),
        new Vector3(1, 0, 0),
        new Vector3(-1, 0, 0),
        new Vector3(-1, 0, 1),
        new Vector3(1, 0, -1),
    };

    private void Start()
    {
        if (PlayerPrefs.GetInt("score") <= 5)
        {
            posiblesCubesToCreate.Add(cubesToCreatev[0]);
        }
        else if (PlayerPrefs.GetInt("score") <= 10)
        {
            AddPossibleCubes(2);
            DeletTxt(1);
        }
        else if (PlayerPrefs.GetInt("score") <= 20)
        {
            AddPossibleCubes(3);
            DeletTxt(2);
        }
        else if (PlayerPrefs.GetInt("score") <= 30)
        {
            AddPossibleCubes(4);
            DeletTxt(3);
        }
        else if (PlayerPrefs.GetInt("score") <= 40)
        {
            AddPossibleCubes(5);
            DeletTxt(4);
        }

        else if (PlayerPrefs.GetInt("score") <= 50)
        {
            AddPossibleCubes(6);
            DeletTxt(5);
        }
        else if (PlayerPrefs.GetInt("score") <= 75)
        {
            AddPossibleCubes(7);
            DeletTxt(6);
        }
        else if (PlayerPrefs.GetInt("score") <= 100)
        {
            AddPossibleCubes(8);
            DeletTxt(7);
        }
        else if (PlayerPrefs.GetInt("score") <= 125)
        {
            AddPossibleCubes(9);
            DeletTxt(8);
        }
        else
        {
            AddPossibleCubes(10);
            DeletTxt(10);
        }
        scoreTxt.text = "<size=40><color=#FFE82F>best :</color></size><color=#FFE82F>" + PlayerPrefs.GetInt("score") + "</color>";
        toBackgroundColor = Camera.main.backgroundColor;
        mainCam = Camera.main.transform;
        moveCamToYpos = 5.9f + nowCube.y - 1f;
        allcubesRB = allcubes.GetComponent<Rigidbody>();
        _ShowCubePlace = StartCoroutine(ShowCubePlace());
    }
    private void Update()
    {
        if ((Input.GetMouseButtonDown(0) || Input.touchCount == 1) && cubeToPlace != null
             && allcubes != null && !EventSystem.current.IsPointerOverGameObject())
        {
#if !UNITY_EDITOR
       if (Input.GetTouch(0).phase != TouchPhase.Began)
	    return;
#endif
            if (!firstcube)
            {
                firstcube = true;
                foreach (var obj in CanvasStartedPackets)
                {
                    Destroy(obj);
                }
            }

            GameObject createCube;
            if (posiblesCubesToCreate.Count == 1)
                createCube = posiblesCubesToCreate[0];
            else
                createCube = posiblesCubesToCreate[UnityEngine.Random.Range(0, posiblesCubesToCreate.Count)];

            GameObject newCube = Instantiate(createCube, cubeToPlace.position,
                                             Quaternion.identity);
            newCube.transform.SetParent(allcubes.transform);
            nowCube.SetVector(cubeToPlace.position);
            busysCubePosision.Add(nowCube.GetVector());
            scoreTxt2.gameObject.SetActive(true);
            scoreTxt.gameObject.SetActive(false);

            if (PlayerPrefs.GetString("music") != "No")
            {
                GetComponent<AudioSource>().Play();
            }

            GameObject newVpx = Instantiate(vpx, newCube.transform.position,
                                           Quaternion.identity) as GameObject;
            Destroy(newVpx, 1.5f);
            allcubesRB.isKinematic = true;
            allcubesRB.isKinematic = false;
            ShowCubePlace();
            MoveCameraChangeBg();
        }

        if (!IsLose && allcubesRB.velocity.magnitude > 0.1f)
        {
            Destroy(cubeToPlace.gameObject);
            StopCoroutine(_ShowCubePlace);
            IsLose = true;
        }
        mainCam.localPosition = Vector3.MoveTowards(mainCam.localPosition,
            new Vector3(mainCam.localPosition.x, moveCamToYpos, mainCam.localPosition.z),
                         camMoveSpeed * Time.deltaTime);
        if (Camera.main.backgroundColor != toBackgroundColor)
            Camera.main.backgroundColor = Color.Lerp(Camera.main.backgroundColor, toBackgroundColor,
                                                     Time.deltaTime / 1.5f);
        ShowCubePlace();
    }

    IEnumerator ShowCubePlace()
    {
        while (true)
        {
            SpawnPosision();
            yield return new WaitForSeconds(cubelocationchangerate);
        }
    }

    private void SpawnPosision()
    {
        List<Vector3> positions = new List<Vector3>();
        if (IsPositionEmpty(new Vector3(nowCube.x + 1, nowCube.y, nowCube.z))
            && nowCube.x + 1 != cubeToPlace.position.x)
            positions.Add(new Vector3(nowCube.x + 1, nowCube.y, nowCube.z));
        if (IsPositionEmpty(new Vector3(nowCube.x - 1, nowCube.y, nowCube.z))
            && nowCube.x - 1 != cubeToPlace.position.x)
            positions.Add(new Vector3(nowCube.x - 1, nowCube.y, nowCube.z));
        if (IsPositionEmpty(new Vector3(nowCube.x, nowCube.y + 1, nowCube.z))
            && nowCube.y + 1 != cubeToPlace.position.y)
            positions.Add(new Vector3(nowCube.x, nowCube.y + 1, nowCube.z));
        if (IsPositionEmpty(new Vector3(nowCube.x, nowCube.y - 1, nowCube.z))
            && nowCube.y - 1 != cubeToPlace.position.y)
            positions.Add(new Vector3(nowCube.x, nowCube.y - 1, nowCube.z));
        if (IsPositionEmpty(new Vector3(nowCube.x, nowCube.y, nowCube.z + 1))
            && nowCube.z + 1 != cubeToPlace.position.z)
            positions.Add(new Vector3(nowCube.x, nowCube.y, nowCube.z + 1));
        if (IsPositionEmpty(new Vector3(nowCube.x, nowCube.y, nowCube.z - 1))
            && nowCube.z - 1 != cubeToPlace.position.z)
            positions.Add(new Vector3(nowCube.x, nowCube.y, nowCube.z - 1));

        if (positions.Count > 1)
        { cubeToPlace.position = positions[UnityEngine.Random.Range(0, positions.Count)]; }
        else if (positions.Count == 0)
        { IsLose = false; }
        else
        { cubeToPlace.position = positions[0]; }
    }

    private bool IsPositionEmpty(Vector3 targetPos)
    {
        if (targetPos.y == 0)
            return false;

        foreach (var pos in busysCubePosision)
        {
            if (pos.x == targetPos.x && pos.y == targetPos.y && pos.z == targetPos.z)
                return false;
        }
        return true;
    }

    private void MoveCameraChangeBg()
    {
        int maxX = 0, maxY = 0, maxZ = 0, maxHonor;
        foreach (var pos in busysCubePosision)
        {
            if (Mathf.Abs(Convert.ToInt32(pos.x)) > maxX)
                maxX = Convert.ToInt32(pos.x);
            if (Convert.ToInt32(pos.y) > maxY)
                maxY = Convert.ToInt32(pos.y);
            if (Mathf.Abs(Convert.ToInt32(pos.z)) > maxZ)
                maxZ = Convert.ToInt32(pos.z);
        }

        maxY--;

        if (PlayerPrefs.GetInt("score") < maxY)
        { PlayerPrefs.SetInt("score", maxY); }
        scoreTxt.text = "<size=40><color=#FFE82F>best :</color></size><color=#FFE82F>" + PlayerPrefs.GetInt("score") + "</color>";
        scoreTxt2.text = "<size=180>" + maxY + "</size>";
        moveCamToYpos = 5.9f + nowCube.y - 1f;

        if (maxX > maxZ) { maxHonor = maxX; }
        else { maxHonor = maxZ; }

        if (maxHonor % 3 == 0 && prevCoutmaxHonor != maxHonor)
        {
            mainCam.localPosition -= new Vector3(0, 0, 2.5f);
            prevCoutmaxHonor = maxHonor;
        }

        if (maxY >= 1000)
            toBackgroundColor = bgColoros[8];
        else if (maxY >= 150)
            toBackgroundColor = bgColoros[7];
        else if (maxY >= 100)
            toBackgroundColor = bgColoros[6];
        else if (maxY >= 50)
            toBackgroundColor = bgColoros[5];
        else if (maxY >= 40)
            toBackgroundColor = bgColoros[4];
        else if (maxY >= 30)
            toBackgroundColor = bgColoros[3];
        else if (maxY >= 20)
            toBackgroundColor = bgColoros[2];
        else if (maxY >= 10)
            toBackgroundColor = bgColoros[1];
        else if (maxY >= 5)
            toBackgroundColor = bgColoros[0];
    }

    private void AddPossibleCubes(int needValue)
    {
        for (int i = 0; i < needValue; i++)
        {
            posiblesCubesToCreate.Add(cubesToCreatev[i]);

        }
    }
    private void DeletTxt(int needTxtValue) {
        for (int i = 0; i < needTxtValue; i++)
        {
            Destroy(texts[i]);
        }
    }
    /// <summary>
    /// Ётот структура дл€ того, что-бы узнать кординаты какого нибудь обьекта.
    /// ѕотом по архитектуре этого структуры, мы можем создать любой обьект.
    /// </summary>
    struct CubePos
    {
        public int x, y, z;

        public CubePos(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3 GetVector()
        {
            return new Vector3(x, y, z);
        }

        public void SetVector(Vector3 Posision)
        {
            x = Convert.ToInt32(Posision.x);
            y = Convert.ToInt32(Posision.y);
            z = Convert.ToInt32(Posision.z);
        }
    }
}
