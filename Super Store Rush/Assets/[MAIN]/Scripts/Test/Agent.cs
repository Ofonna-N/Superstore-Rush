using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using TMPro;


public class Agent : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent navAgent;

    [SerializeField]
    private Transform Target;

    [SerializeField]
    private InputActionMap inputActions;

    [SerializeField]
    private int num = 10;

    [SerializeField]
    private TextMeshProUGUI text;

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions["Click"].performed -= OnClick;
        inputActions.Disable();
    }

    private void Start()
    {
        inputActions["Click"].performed += OnClick;
    }

    private void OnClick(InputAction.CallbackContext ctx)
    {
        Debug.Log("Clicked!");
        text.text = NumFormat(num);
        navAgent.SetDestination(Target.position);
    }

    public static string NumFormat(int num)
    {
        if (num >= 1000 && num < 1000000)
            return $"{num / 1000}K";

        if (num >= 1000000)
            return $"{num / 1000000}M";
        return num.ToString();
    }
}
