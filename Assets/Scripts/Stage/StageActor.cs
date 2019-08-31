using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StageController))]
[RequireComponent(typeof(DropController))]
public class StageActor : MonoBehaviour
{
    public static StageActor instance;
    public StageController stageController;
    public DropController dropController;

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
        stageController = GetComponent<StageController>();
        dropController = GetComponent<DropController>();

        if (!stageController || !dropController)
        {
            Debug.LogError("[StageActor] Missing components!");
        }
    }
}
