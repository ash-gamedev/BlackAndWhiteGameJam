using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [Header("Tutorial Steps")]
    [SerializeField] List<TutorialStep> tutorialSteps;
    [SerializeField] TextMeshProUGUI tutorialHeader;
    [SerializeField] TextMeshProUGUI tutorialDescription;
    [SerializeField] Animator animator;

    [Header("CustomerSpawns")]
    [SerializeField] GameObject customer = null;
    [SerializeField] Seat seat1;
    [SerializeField] Seat seat2;
    [SerializeField] Seat seat3;

    private int currentTutorialStepIndex = 0;
    private TutorialStep currentTutorialStep
    {
        get
        {
            return tutorialSteps[currentTutorialStepIndex];
        }
    }

    // Use this for initialization
    void Start()
    {
        UpdateTutorialStepUI();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GetNextTutorialStep()
    {
        // get next tutorial step
        if (currentTutorialStepIndex + 1 < tutorialSteps.Count)
        {
            // get next index
            currentTutorialStepIndex++;

            UpdateTutorialStepUI();
        }
        else
        {
            //TODO
            return;
        }

        // custom actions based on the tutorial step
        switch (currentTutorialStep.EnumTutorialStep)
        {
            case EnumTutorialStep.CustomerOrder:
                SpawnCustomer(seat1);
                break;
            case EnumTutorialStep.MultipleCustomerOrder:
                StartCoroutine(SpawnCustomers());
                break;
        }
    }

    void UpdateTutorialStepUI()
    {
        // update text
        tutorialHeader.text = currentTutorialStep.StepHeader;
        tutorialDescription.text = currentTutorialStep.StepDescription;

        // animation
        ChangeAnimationState(currentTutorialStep.EnumTutorialStep);
    }

    void ChangeAnimationState(EnumTutorialStep newState)
    {
        Debug.Log(newState.ToString());

        //play the animation
        animator.Play(newState.ToString());
    }

    void SpawnCustomer(Seat seat)
    {
        seat.SetCustomer(customer);
    }

    IEnumerator SpawnCustomers()
    {
        SpawnCustomer(seat2);
        yield return new WaitForSeconds(4);
        SpawnCustomer(seat3);
    }
}