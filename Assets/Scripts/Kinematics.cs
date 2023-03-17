using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UIElements;

public class Kinematics
{
    public const float SamplingDistance = 0.1f;
    public const float LearningRate = 100f;
    public const float DistanceThreshold = 0.01f;

    private static float Loss(Vector3[] initLocalPosition, Quaternion[] initLocalRotation, Vector3 target, Angle[] angles)
    {
        return DistanceFromTarget(initLocalPosition, initLocalRotation, target, angles);
    }

    private static float TorsionPenalty(Angle[] angles)
    {
        float penalty = 0;
        for (int i = 0; i < angles.Length; i++)
        {
            penalty += Mathf.Abs(angles[i].value);
        }
        return penalty / angles.Length;
    }

    private static float DistanceFromTarget(Vector3[] initLocalPosition, Quaternion[] initLocalRotation, Vector3 target, Angle[] angles)
    {
        Vector3 point = ForwardKinematics(initLocalPosition, initLocalRotation, angles);
        var dis = Vector3.Distance(point, target);
        return dis;
    }


    private static float PartialGradient(Vector3[] initLocalPosition, Quaternion[] initLocalRotation, Vector3 target, Angle[] angles, int i)
    {
        // Saves the angle,
        // it will be restored later
        float angle = angles[i].value;

        // Gradient : [F(x+SamplingDistance) - F(x)] / h
        float f_x = Loss(initLocalPosition, initLocalRotation, target, angles);
        angles[i].value += SamplingDistance;
        float f_x_plus_d = Loss(initLocalPosition, initLocalRotation, target, angles);

        float gradient = (f_x_plus_d - f_x) / SamplingDistance;

        // Restores
        angles[i].value = angle;

        return gradient;
    }

    public static void InverseKinematics(List<ArticulationBody> joints, Vector3[] initLocalPosition, Quaternion[] initLocalRotation, Vector3 target, Angle[] angles)
    {

        if (DistanceFromTarget(initLocalPosition, initLocalRotation, target, angles) < DistanceThreshold)
        {
            return;
        }

        for (int i = joints.Count - 1; i > 0; i--)
        {
            // Gradient descent
            // Update : Solution -= LearningRate * Gradient
            float gradient = PartialGradient(initLocalPosition, initLocalRotation, target, angles, i);
            angles[i].value -= LearningRate * gradient;

            angles[i].value = Mathf.Clamp(angles[i].value, joints[i].xDrive.lowerLimit, joints[i].xDrive.upperLimit);

            if (DistanceFromTarget(initLocalPosition, initLocalRotation, target, angles) < DistanceThreshold)
            {
                return;
            }

        }

    }



    public static Vector3 ForwardKinematics(Vector3[] initLocalPosition, Quaternion[] initLocalRotation, Angle[] point)
    {
        Vector3 curPosition = initLocalPosition[0];
        Quaternion curRotation = initLocalRotation[0];

        for (int i = 1; i < initLocalPosition.Length; i++)
        {
            curRotation = curRotation * initLocalRotation[i - 1];
            curRotation = curRotation * Quaternion.AngleAxis(-point[i - 1].value, Vector3.up);

            curPosition = curPosition + curRotation * initLocalPosition[i];
        }

        return curPosition;

    }

    public static Vector3 ForwardKinematics(List<ArticulationBody> joints, Angle[] point)
    {
        var initLocalPositions = new Vector3[joints.Count];
        var initLocalRotations = new Quaternion[joints.Count];
        for (int i = 0; i < joints.Count; i++)
        {
            initLocalPositions[i] = joints[i].transform.localPosition;
            initLocalRotations[i] = joints[i].transform.localRotation;
        }

        return ForwardKinematics(initLocalPositions, initLocalRotations, point);

    }



}