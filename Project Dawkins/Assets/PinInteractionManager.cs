using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinInteractionManager : MonoBehaviour {

    public Transform lineObject;
    public static PinInteractionManager instance;

    public Transform cancelButton;

    public LineScript currentLine;
    enum pinInteractionStatus { None, Dragging }
    pinInteractionStatus currentStatus;
    public enum interactionMode {None, Tying, Cutting };
    public interactionMode currentInteraction;

    // Use this for initialization
    void Awake() {
        instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
            CancelPinning();
    }


    public void PinInteraction(NailScript pin)
    {
        if (!UI_Manager.editingInformation)
        {
            //if (currentInteraction == interactionMode.Tying)
            //{
            if (currentStatus == pinInteractionStatus.None)
            {
                currentStatus = pinInteractionStatus.Dragging;
                currentLine = Instantiate(lineObject, this.transform).GetComponent<LineScript>();

                currentLine.SetA(pin);

                currentLine.SetReady();

                cancelButton.gameObject.SetActive(true);
            }
            else if (currentStatus == pinInteractionStatus.Dragging)
            {
                if (pin.CompatibleWithOtherExtreme(currentLine.pointA))
                {
                    currentStatus = pinInteractionStatus.None;

                    if (currentLine != null)
                    {
                        currentLine.SetB(pin);
                    }

                    cancelButton.gameObject.SetActive(false);
                }
            }
            //}
            //else
            //{
            //    pin.DeleteAllTies();
            //}
        }

        FamilyManager.instance.formulary.CloseFormulary();
    }

    public void CancelPinning()
    {
        if (currentLine != null && (currentLine.pointA == null || currentLine.pointB == null))
            currentLine.DeleteLine();

        currentStatus = pinInteractionStatus.None;
        cancelButton.gameObject.SetActive(false);
    }

    public void ChangeInteractionType(int to)
    {
        currentInteraction = (interactionMode)to;
        Debug.Log(currentInteraction.ToString());
    }
}
