using AGXUnity;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.Assertions;
using UnityEngine;

public class Controller : MonoBehaviour {

    // Creation of Key lables
    public KeyCode KeyForward = KeyCode.W;
    public KeyCode KeyReverse = KeyCode.S;
    public KeyCode KeyRight = KeyCode.D;
    public KeyCode KeyLeft = KeyCode.A;
    public KeyCode KeyStandLeft = KeyCode.Q;
    public KeyCode KeyStandRight = KeyCode.E;
    public KeyCode KeyUp = KeyCode.UpArrow;
    public KeyCode KeyDown = KeyCode.DownArrow;
    public KeyCode KeyRotateLeft = KeyCode.Keypad4;
    public KeyCode KeyRotateRight = KeyCode.Keypad6;
    public KeyCode KeyLiftUp = KeyCode.Keypad7;
    public KeyCode KeyLiftDown = KeyCode.Keypad9;
    public KeyCode KeyLiftUp2 = KeyCode.Keypad8;
    public KeyCode KeyLiftDown2 = KeyCode.Keypad2;
    public KeyCode KeyMoveTool = KeyCode.KeypadMinus;
    public KeyCode KeyMoveTool2 = KeyCode.KeypadPlus;
    public KeyCode KeyOnTool = KeyCode.Keypad1;

    private float BackAxis;
    private float FrontAxis;

    //Public definition of speed values 
    public float SpeedWheels = 150.0f;
    public float SpeedTurn = 2.0f;
    public float SpeedRotationCrane = 5.0f;
    public float SpeedHolder = 2f;
    public float SpeedHidraulicCrane1 = 10.0f;
    public float SpeedHidraulicCrane2 = 10.0f;
    public float SpeedMoveTool = 1.0f;
    public float SpeedRotationDisk = 5.0f;

    // Call the joints objects 
    public Constraint HingeWheelLeft1 = null;
    public Constraint HingeWheelLeft2 = null;
    public Constraint HingeWheelLeft3 = null;
    public Constraint HingeWheelLeft4 = null;

    public Constraint HingeWheelRight1 = null;
    public Constraint HingeWheelRight2 = null;
    public Constraint HingeWheelRight3 = null;
    public Constraint HingeWheelRight4 = null;

    public Constraint HydraulicTurnLeft = null;
    public Constraint HydraulicTurnRight = null;

    public Constraint HydraulicStandLeft = null;
    public Constraint HydraulicStandRight = null;

    public Constraint RotateCrane = null;
    public Constraint LiftCrane = null;
    public Constraint LiftCrane2 = null;
    public Constraint ToolMove = null;
    public Constraint RotationTool = null;

    private Keycode CraneRotation= OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick)[0]; 

    private bool DiskRotation;
    private float CraneSpeed;
    private float JoytsickAxis; 

    void Start () {

        // Inicial defition of no Disk rotation joint 
        DiskRotation = false;

        //Enabling the target speed motor of joints
        HingeWheelLeft1.GetController<TargetSpeedController>().Enable = true;
        HingeWheelLeft2.GetController<TargetSpeedController>().Enable = true;
        HingeWheelLeft3.GetController<TargetSpeedController>().Enable = true;
        HingeWheelLeft4.GetController<TargetSpeedController>().Enable = true;
        HingeWheelRight1.GetController<TargetSpeedController>().Enable = true;
        HingeWheelRight2.GetController<TargetSpeedController>().Enable = true;
        HingeWheelRight3.GetController<TargetSpeedController>().Enable = true;
        HingeWheelRight4.GetController<TargetSpeedController>().Enable = true;
        LiftCrane2.GetController<TargetSpeedController>().Enable = true;
        ToolMove.GetController<TargetSpeedController>().Enable = true;
        RotateCrane.GetController<TargetSpeedController>().Enable = true;
        LiftCrane.GetController<TargetSpeedController>().Enable = true;

        // Inicial definition of motor target speed 
        HingeWheelLeft1.GetController<TargetSpeedController>().Speed = 0.0f;
        HingeWheelLeft2.GetController<TargetSpeedController>().Speed = 0.0f;
        HingeWheelLeft3.GetController<TargetSpeedController>().Speed = 0.0f;
        HingeWheelLeft4.GetController<TargetSpeedController>().Speed = 0.0f;
        HingeWheelRight1.GetController<TargetSpeedController>().Speed = 0.0f;
        HingeWheelRight2.GetController<TargetSpeedController>().Speed = 0.0f;
        HingeWheelRight3.GetController<TargetSpeedController>().Speed = 0.0f;
        HingeWheelRight4.GetController<TargetSpeedController>().Speed = 0.0f;
        HydraulicTurnLeft.GetController<TargetSpeedController>().Speed = 0.0f;
        HydraulicTurnRight.GetController<TargetSpeedController>().Speed = 0.0f;
        HydraulicStandLeft.GetController<TargetSpeedController>().Speed = 0.0f;
        HydraulicStandRight.GetController<TargetSpeedController>().Speed = 0.0f;

        // Lock the motor joints at zero speed
        HydraulicTurnLeft.GetController<TargetSpeedController>().LockAtZeroSpeed = true;
        HydraulicTurnRight.GetController<TargetSpeedController>().LockAtZeroSpeed = true;
        HydraulicStandLeft.GetController<TargetSpeedController>().LockAtZeroSpeed = true;
        HydraulicStandRight.GetController<TargetSpeedController>().LockAtZeroSpeed = true;
        RotationTool.GetController<TargetSpeedController>().LockAtZeroSpeed = true;
    }

    void Update () {

        JoytsickAxis = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick)[0]; 

        // keep updating the speed at zero
        HydraulicTurnLeft.GetController<TargetSpeedController>().Speed = 0.0f;
        HydraulicTurnRight.GetController<TargetSpeedController>().Speed = 0.0f;
        HydraulicStandLeft.GetController<TargetSpeedController>().Speed = 0.0f;
        HydraulicStandRight.GetController<TargetSpeedController>().Speed = 0.0f;
        RotateCrane.GetController<TargetSpeedController>().Speed = 0.0f;
        LiftCrane.GetController<TargetSpeedController>().Speed= 0.0f;
        LiftCrane2.GetController<TargetSpeedController>().Speed = 0.0f;
        ToolMove.GetController<TargetSpeedController>().Speed = 0.0f;
  
        //Get the axis value from the joysticks
        BackAxis = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger);
        FrontAxis = -OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger);

        // If keys are detected, the speed on the specific motor joint is updated to the value defined
        if (Input.GetKey(KeyReverse) || Input.GetKey(KeyForward))
        {
            HingeWheelLeft1.GetController<TargetSpeedController>().Speed = HingeWheelLeft2.GetCurrentSpeed();
            HingeWheelLeft2.GetController<TargetSpeedController>().Speed =  -Input.GetAxis("Vertical") * SpeedWheels;
            HingeWheelLeft3.GetController<TargetSpeedController>().Speed = HingeWheelLeft4.GetCurrentSpeed();
            HingeWheelLeft4.GetController<TargetSpeedController>().Speed =  -Input.GetAxis("Vertical") * SpeedWheels;

            HingeWheelRight1.GetController<TargetSpeedController>().Speed = HingeWheelRight2.GetCurrentSpeed();
            HingeWheelRight2.GetController<TargetSpeedController>().Speed =  -Input.GetAxis("Vertical") * SpeedWheels;
            HingeWheelRight3.GetController<TargetSpeedController>().Speed = HingeWheelRight4.GetCurrentSpeed();
            HingeWheelRight4.GetController<TargetSpeedController>().Speed =  -Input.GetAxis("Vertical") * SpeedWheels;
        }

        if (BackAxis != 0) {
            HingeWheelLeft1.GetController<TargetSpeedController>().Speed = BackAxis * SpeedWheels;
            HingeWheelLeft2.GetController<TargetSpeedController>().Speed = HingeWheelLeft1.GetCurrentSpeed();
            HingeWheelLeft3.GetController<TargetSpeedController>().Speed = BackAxis * SpeedWheels;
            HingeWheelLeft4.GetController<TargetSpeedController>().Speed = HingeWheelLeft3.GetCurrentSpeed();

            HingeWheelRight1.GetController<TargetSpeedController>().Speed = BackAxis * SpeedWheels;
            HingeWheelRight2.GetController<TargetSpeedController>().Speed = HingeWheelRight1.GetCurrentSpeed();
            HingeWheelRight3.GetController<TargetSpeedController>().Speed = BackAxis * SpeedWheels;
            HingeWheelRight4.GetController<TargetSpeedController>().Speed = HingeWheelRight3.GetCurrentSpeed();
        } else if (FrontAxis != 0) {
            HingeWheelLeft1.GetController<TargetSpeedController>().Speed = FrontAxis * SpeedWheels;
            HingeWheelLeft2.GetController<TargetSpeedController>().Speed = HingeWheelLeft1.GetCurrentSpeed();
            HingeWheelLeft3.GetController<TargetSpeedController>().Speed = FrontAxis * SpeedWheels;
            HingeWheelLeft4.GetController<TargetSpeedController>().Speed = HingeWheelLeft3.GetCurrentSpeed();

            HingeWheelRight1.GetController<TargetSpeedController>().Speed = FrontAxis * SpeedWheels;
            HingeWheelRight2.GetController<TargetSpeedController>().Speed = HingeWheelRight1.GetCurrentSpeed();
            HingeWheelRight3.GetController<TargetSpeedController>().Speed = FrontAxis * SpeedWheels;
            HingeWheelRight4.GetController<TargetSpeedController>().Speed = HingeWheelRight3.GetCurrentSpeed();
        }

        if (Input.GetKey(KeyLeft) || Input.GetKey(KeyRight)) {
            HydraulicTurnLeft.GetController<TargetSpeedController>().Speed = SpeedTurn *Input.GetAxis("Horizontal");
            HydraulicTurnRight.GetController<TargetSpeedController>().Speed = -HydraulicTurnLeft.GetController<TargetSpeedController>().Speed;
        }

        if  (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) != 0) {
            HydraulicTurnLeft.GetController<TargetSpeedController>().Speed = -SpeedTurn * OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger);
            HydraulicTurnRight.GetController<TargetSpeedController>().Speed = SpeedTurn * OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger);
        }

        if (OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) != 0 ) {
            HydraulicTurnLeft.GetController<TargetSpeedController>().Speed = SpeedTurn * OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger);
            HydraulicTurnRight.GetController<TargetSpeedController>().Speed = -SpeedTurn * OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger);
        }

        if(Input.GetKey(KeyStandLeft) && Input.GetKey(KeyDown) || OVRInput.Get(OVRInput.Button.Three)) {
            HydraulicStandLeft.GetController<TargetSpeedController>().Speed = SpeedHolder ;
        }

        if (Input.GetKey(KeyStandLeft) && Input.GetKey(KeyUp) || OVRInput.Get(OVRInput.Button.Four)) {
            HydraulicStandLeft.GetController<TargetSpeedController>().Speed = -SpeedHolder ;
        }

        if (Input.GetKey(KeyStandRight) && Input.GetKey(KeyDown) || OVRInput.Get(OVRInput.Button.One)) {
            HydraulicStandRight.GetController<TargetSpeedController>().Speed = SpeedHolder ;
        }

        if (Input.GetKey(KeyStandRight) && Input.GetKey(KeyUp) || OVRInput.Get(OVRInput.Button.Two)) {
            HydraulicStandRight.GetController<TargetSpeedController>().Speed = -SpeedHolder ;
        }

        RotateCrane.GetController<TargetSpeedController>().Speed = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick)[0] * SpeedRotationCrane;

        if (Input.GetKey(KeyRotateLeft))
        { 
            RotateCrane.GetController<TargetSpeedController>().Speed = -SpeedRotationCrane ;
        }

        if (Input.GetKey(KeyRotateRight))
        { 
            RotateCrane.GetController<TargetSpeedController>().Speed = SpeedRotationCrane ;
        }

        LiftCrane.GetController<TargetSpeedController>().Speed = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick)[1] * SpeedHidraulicCrane1;

        if (Input.GetKey(KeyLiftUp))
        { 
            LiftCrane.GetController<TargetSpeedController>().Speed = SpeedHidraulicCrane1 ;
        }

        if (Input.GetKey(KeyLiftDown))
        {
            LiftCrane.GetController<TargetSpeedController>().Speed = -SpeedHidraulicCrane1 ;
        }

        LiftCrane2.GetController<TargetSpeedController>().Speed = - OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick)[1] * SpeedHidraulicCrane2;

        if (Input.GetKey(KeyLiftUp2))
        { 
            LiftCrane2.GetController<TargetSpeedController>().Speed = -SpeedHidraulicCrane2 ;
        }

        if (Input.GetKey(KeyLiftDown2))
        { 
            LiftCrane2.GetController<TargetSpeedController>().Speed = SpeedHidraulicCrane2;
        }

        ToolMove.GetController<TargetSpeedController>().Speed = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick)[0] * SpeedMoveTool;

        if (Input.GetKey(KeyMoveTool))
        { 
            ToolMove.GetController<TargetSpeedController>().Speed = SpeedMoveTool;
        }

        if(Input.GetKey(KeyMoveTool2))
        { 
            ToolMove.GetController<TargetSpeedController>().Speed = -SpeedMoveTool ;
        }

        RotationTool.GetController<TargetSpeedController>().Speed = 0.0f;
        RotationTool.GetController<TargetSpeedController>().Enable = DiskRotation;

        if (Input.GetKeyDown(KeyOnTool) || OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick)) {
            DiskRotation = !DiskRotation;
            RotationTool.GetController<TargetSpeedController>().Speed = SpeedRotationDisk;
        }
    }
}
