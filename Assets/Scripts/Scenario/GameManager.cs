using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public KeyCode jump;
    public KeyCode buster;
    public KeyCode left;
    public KeyCode right;
    public bool isInputEnabled;
    public string chosenScene;
    public string sceneClassicName = "GutsmanStageClassic";
    public string sceneDirectorsCutName = "GutsmanStageDirectorsCut";
    public string LayerNameGround = "Ground";
    public string LayerNameEnemy = "Enemy";
    public string LayerNameMegaman = "Player";
    public string LayerNameWall = "Wall";
    //public string LayerNameLimits = "Limits"/
    public LayerMask layerGround;
    public LayerMask layerEnemy;
    public LayerMask layerMegaman;
    public LayerMask layerWall;
    //public LayerMask layerLimits;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        LayerNameGround = "Ground";
        LayerNameEnemy = "Enemy";
        jump = KeyCode.Space;
        buster = KeyCode.Z;
        isInputEnabled = true;
        chosenScene = sceneClassicName;
        layerGround = LayerMask.GetMask(LayerNameGround);
        layerEnemy = LayerMask.GetMask(LayerNameEnemy);
        layerMegaman = LayerMask.GetMask(LayerNameMegaman);
        layerWall = LayerMask.GetMask(LayerNameWall);
    }
}
