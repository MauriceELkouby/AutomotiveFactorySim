using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndustrialArmController : MonoBehaviour
{
    public Transform armPiece1, armPiece2, armPiece3, armPiece4, armPiece5;
    public Transform rotationPoint1, rotationPoint2, rotationPoint3, rotationPoint4, rotationPoint5;
    private int rotationSpeed = 5;
    public float degreesArm1, degreesArm2, degreesArm3, degreesArm4, degreesArm5;
    public bool isArm1Moving, isArm2Moving, isArm3Moving, isArm4Moving, isArm5Moving;
    public Dictionary<string, float> inputDegrees = new Dictionary<string, float>
    {
        { "Joint1Rotation", 0f },
        { "Joint2Rotation", 0f },
        { "Joint3Rotation", 0f },
        { "Joint4Rotation", 0f },
        { "Joint5Rotation", 0f }
    };
    public float joint1Degrees, joint2Degrees, joint3Degrees, joint4Degrees, joint5Degrees;
    public void customRotation()
    {
        inputDegrees["Joint1Rotation"] = joint1Degrees;
        inputDegrees["Joint2Rotation"] = joint2Degrees;
        inputDegrees["Joint3Rotation"] = joint3Degrees;
        inputDegrees["Joint4Rotation"] = joint4Degrees;
        inputDegrees["Joint5Rotation"] = joint5Degrees;
    }
    public void UpdateRotation(string key, float increment)
    {
        if (inputDegrees.ContainsKey(key))
        {
            inputDegrees[key] += increment;
            Debug.Log(inputDegrees[key]);
        }
        else
        {
            Debug.LogError($"Key '{key}' not found.");
        }
    }
    private void Update()
    {
        //inputDegreesArm1 = PLCIOS.arm1Joint1Signal;
        if (!isArm1Moving && degreesArm1 != inputDegrees["Joint1Rotation"])
        {
            StartCoroutine(RotateAndSetDegrees(armPiece1, armPiece1, rotationPoint1, "BASE", rotationSpeed, inputDegrees["Joint1Rotation"], value => degreesArm1 = value, () => isArm1Moving = false));
            isArm1Moving = true;
        }

        if (degreesArm2 != inputDegrees["Joint2Rotation"] && !isArm2Moving)
        {
            StartCoroutine(RotateAndSetDegrees(armPiece1, armPiece2, rotationPoint2, "Z", rotationSpeed, inputDegrees["Joint2Rotation"], value => degreesArm2 = value, () => isArm2Moving = false));
            isArm2Moving = true;
        }

        if (degreesArm3 != inputDegrees["Joint3Rotation"] && !isArm3Moving)
        {
            StartCoroutine(RotateAndSetDegrees(armPiece1, armPiece3, rotationPoint3, "Z", rotationSpeed, inputDegrees["Joint3Rotation"], value => degreesArm3 = value, () => isArm3Moving = false));
            isArm3Moving = true;
        }

        if (degreesArm4 != inputDegrees["Joint4Rotation"] && !isArm4Moving)
        {
            StartCoroutine(RotateAndSetDegrees(armPiece3, armPiece4, rotationPoint4, "X", rotationSpeed, inputDegrees["Joint4Rotation"], value => degreesArm4 = value, () => isArm4Moving = false));
            isArm4Moving = true;
        }

        if (degreesArm5 != inputDegrees["Joint5Rotation"] && !isArm5Moving)
        {
            StartCoroutine(RotateAndSetDegrees(armPiece4, armPiece5, rotationPoint5, "Y", rotationSpeed, inputDegrees["Joint5Rotation"], value => degreesArm5 = value, () => isArm5Moving = false));
            isArm5Moving = true;
        }
    }

    private IEnumerator RotateAndSetDegrees(Transform connectedArmPiece, Transform armPiece, Transform rotationPoint, string axis, int rotationSpeed, float targetDegrees, System.Action<float> onComplete, System.Action onFinish)
    {
        // Calculate rotation direction
        float remainingDegrees = targetDegrees - (armPiece == armPiece1 ? degreesArm1 : armPiece == armPiece2 ? degreesArm2 : armPiece == armPiece3 ? degreesArm3 : armPiece == armPiece4 ? degreesArm4 : armPiece == armPiece5 ? degreesArm5 : 0);
        float rotationDirection = Mathf.Sign(remainingDegrees); // +1 for forward, -1 for backward

        float degreesRotated = 0;

        while (Mathf.Abs(degreesRotated) < Mathf.Abs(remainingDegrees))
        {
            targetDegrees = armPiece == armPiece1 ? inputDegrees["Joint1Rotation"] :
                        armPiece == armPiece2 ? inputDegrees["Joint2Rotation"] :
                        armPiece == armPiece3 ? inputDegrees["Joint3Rotation"] :
                        armPiece == armPiece4 ? inputDegrees["Joint4Rotation"] :
                        inputDegrees["Joint5Rotation"];
            remainingDegrees = targetDegrees - (armPiece == armPiece1 ? degreesArm1 : armPiece == armPiece2 ? degreesArm2 : armPiece == armPiece3 ? degreesArm3 : armPiece == armPiece4 ? degreesArm4 : armPiece == armPiece5 ? degreesArm5 : 0);
            float step = rotationDirection * rotationSpeed * Time.deltaTime;

            // Prevent overshooting
            if (Mathf.Abs(degreesRotated + step) > Mathf.Abs(remainingDegrees))
                step = remainingDegrees - degreesRotated;

            // Perform rotation
            if (axis == "Z")
            {
                float angle1 = connectedArmPiece.eulerAngles.y;
                float xRotation = Mathf.Sin(Mathf.Deg2Rad * angle1) * step;
                float zRotation = Mathf.Cos(Mathf.Deg2Rad * angle1) * step;

                armPiece.RotateAround(rotationPoint.position, Vector3.forward, zRotation);
                armPiece.RotateAround(rotationPoint.position, Vector3.right, xRotation);
            }
            else if (axis == "X")
            {
                armPiece.RotateAround(rotationPoint.position, connectedArmPiece.right, step);
            }
            else if (axis == "Y")
            {
                armPiece.RotateAround(rotationPoint.position, connectedArmPiece.up, step);
            }
            else if (axis == "BASE")
            {
                armPiece.RotateAround(rotationPoint.position, Vector3.up, step);
            }

            degreesRotated += step;
            yield return null; // Wait for the next frame
        }

        // Call the onComplete action to update the degrees
        onComplete(targetDegrees);

        // Call the onFinish action to set the isArmMoving flag to false
        onFinish();
    }
    
}
