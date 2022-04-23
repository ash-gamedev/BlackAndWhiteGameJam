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
    private PauseMenu pauseMenu;
    bool triggerEventHappened = false;

    // Use this for initialization
    void Start()
    {
        pauseMenu = FindObjectOfType<PauseMenu>();
        StartCoroutine(StartTutorialLevel());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickNextTutorialStep()
    {
        // check the current step
        switch (currentTutorialStep.EnumTutorialStep)
        {
            case EnumTutorialStep.PlaceTile:
                Pause();
                break;
            case EnumTutorialStep.RemoveTile:
                StartCoroutine(SpawnCustomers());
                Resume();
                break;
            case EnumTutorialStep.CustomerOrder:
                Pause();
                break;
            case EnumTutorialStep.MultipleCustomerOrder:
                Pause();
                break;
            case EnumTutorialStep.ClickToPlaceNewOrder:
                Resume();
                StartCoroutine(FinishTutorial());
                break;
        }

        GetNextTutorialStep();

        if (PauseMenu.GameIsPaused)
        {
            UpdateTutorialStepUI();
        }
    }

    public void GetNextTutorialStep()
    {
        // get next tutorial step
        if (currentTutorialStepIndex + 1 < tutorialSteps.Count)
        {
            // get next index
            currentTutorialStepIndex++;
        }
        else
        {
            Resume();
        }
    }

    IEnumerator StartTutorialLevel()
    {
        yield return new WaitForSeconds(0.5f);
        Pause();
        UpdateTutorialStepUI();
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

    void SpawnCustomer(Seat seat, EnumOrder? enumOrder)
    {
        seat.SetCustomer(customer, enumOrder);
    }

    IEnumerator SpawnCustomers()
    {
        SpawnCustomer(seat2, EnumOrder.Croissant);
        yield return new WaitForSeconds(5);
        SpawnCustomer(seat3, EnumOrder.Coffee);
    }

    void Pause()
    {
        if (PauseMenu.GameIsPaused == false)
            pauseMenu.Pause();
    }

    void Resume()
    {
        pauseMenu.Resume();
    }

    IEnumerator FinishTutorial()
    {
        // wait until all customers have left
        yield return new WaitUntil(() => FindObjectsOfType<Customer>().Length == 0);

        LevelManager.OnLevelComplete();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Tutorial Treigger");
        if(triggerEventHappened == false)
        {
            triggerEventHappened = true;
            OnClickNextTutorialStep();
        }
    }
}