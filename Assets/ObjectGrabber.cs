using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class ObjectGrabber : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private bool isGrabbed = false;

    void Start()
    {
        // Recupera il componente XRGrabInteractable dal GameObject
        grabInteractable = GetComponent<XRGrabInteractable>();

        // Aggiunge un listener per l'evento di selezione
        grabInteractable.onSelectEntered.AddListener(OnGrab);

        // Aggiunge un listener per l'evento di deselezione
        grabInteractable.onSelectExited.AddListener(OnRelease);
    }

    void OnGrab(XRBaseInteractor interactor)
    {
        // L'oggetto è stato afferrato dall'utente
        isGrabbed = true;

        // Aggiungi qui le azioni da eseguire quando l'oggetto viene afferrato
        Debug.Log("L'oggetto è stato afferrato");
    }

    void OnRelease(XRBaseInteractor interactor)
    {
        // L'oggetto è stato rilasciato dall'utente
        isGrabbed = false;

        // Aggiungi qui le azioni da eseguire quando l'oggetto viene rilasciato
        Debug.Log("L'oggetto è stato rilasciato");
    }
}
