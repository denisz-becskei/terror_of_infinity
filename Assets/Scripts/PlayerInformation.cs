using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GenerationManager;

public class PlayerInformation : MonoBehaviour, IDataPersistance
{
    public ChunkType currentChunkType;
    private ChunkType previousChunkType = ChunkType.None;

    public bool walkVision = false;

    private RaycastHit hit;
    public string playerCurrentWorldPosition;
    [SerializeField] private StatusEffectController statusEffectController;
    [SerializeField] private OnChangeChunkType onChangeChunkType;

    [SerializeField] private DialogUIHandler dialog;

    private Coroutine updatePositionRoutine;
    private Dictionary<ChunkType, bool> wereVisited = new Dictionary<ChunkType, bool>();
    private bool isDialogSystemRunning = false;

    private void Start()
    {
        StartCoroutine(LateStart());
        updatePositionRoutine = StartCoroutine(UpdatePlayerPosition());
    }

    public void ChunkUpdateAction()
    {
        if(previousChunkType != currentChunkType)
        {
            onChangeChunkType.StartAction(previousChunkType, currentChunkType);
            previousChunkType = currentChunkType;
        }

        // Turn off Player Position Updating when in Purgatory
        if(currentChunkType == ChunkType.Purgatory)
        {
            statusEffectController.ResetAllStatusEffects();
            StopCoroutine(updatePositionRoutine);
            updatePositionRoutine = null;
        } else { 
            updatePositionRoutine = StartCoroutine(UpdatePlayerPosition());
        }

        if(isDialogSystemRunning)
        {
            DialogUpdateAction();
        }
    }

    private IEnumerator UpdatePlayerPosition()
    {
        yield return new WaitForSeconds(2f);
        PositionUpdate();
    }

    private void PositionUpdate()
    {
        Vector3 dwn = transform.TransformDirection(Vector3.down);
        Vector3 newRaycastPosition = new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z);
        if (Physics.Raycast(newRaycastPosition, dwn, out hit, 20f))
        {
            if (hit.transform.gameObject.CompareTag("Ground"))
            {
                var rp = hit.transform.parent.GetComponent<RoomPosition>();
                if (rp != null)
                {
                    playerCurrentWorldPosition = rp.GetRoomCoordinatesInWorld();
                }
            }
        }
        updatePositionRoutine = StartCoroutine(UpdatePlayerPosition());
    }

    public void LoadData(GameStates data)
    {
        playerCurrentWorldPosition = data.PLAYER_WORLD_COORDINATES;
        Debug.Log("Loading World Position");
        // TODO: Move player there
    }

    public void SaveData(ref GameStates data)
    {
        data.PLAYER_WORLD_COORDINATES = playerCurrentWorldPosition;
        Debug.Log("Saving World Position");
    }

    private void DialogUpdateAction()
    {
        if(wereVisited.Count == 0)
        {
            PopulateVisitedDictionary();
        }

        if(currentChunkType == ChunkType.Purgatory)
        {
            return;
        } else if (wereVisited[currentChunkType] == false)
        {
            bool success = dialog.Play(WorldWideScripts.chunkTypesByInt[((int)currentChunkType)], false);
            if (success)
            {
                wereVisited[currentChunkType] = true;
            }
        }
    }

    private void PopulateVisitedDictionary()
    {
        for(int i = 1; i < 18; i++)
        {
            wereVisited.Add((ChunkType)i, false);
        }
    }

    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(5f);
        isDialogSystemRunning = true;
    }
}
