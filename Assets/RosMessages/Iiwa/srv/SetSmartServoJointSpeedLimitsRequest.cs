//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.Iiwa
{
    [Serializable]
    public class SetSmartServoJointSpeedLimitsRequest : Message
    {
        public const string k_RosMessageName = "iiwa_msgs/SetSmartServoJointSpeedLimits";
        public override string RosMessageName => k_RosMessageName;

        //  This service allows to set relative joint velocity and acceleration of the robot at runtime.
        //  Set a parameter to -1 to ignore it
        //  Relative velocity of the individual joints, 0.0 < value <= 1
        public double joint_relative_velocity;
        //  Relative acceleration of the individual joints, 0.0 < value <= 1.0
        public double joint_relative_acceleration;
        //  Override the acceleration factor for all joints. Must be between 0.0 and 10.0. 
        public double override_joint_acceleration;

        public SetSmartServoJointSpeedLimitsRequest()
        {
            this.joint_relative_velocity = 0.0;
            this.joint_relative_acceleration = 0.0;
            this.override_joint_acceleration = 0.0;
        }

        public SetSmartServoJointSpeedLimitsRequest(double joint_relative_velocity, double joint_relative_acceleration, double override_joint_acceleration)
        {
            this.joint_relative_velocity = joint_relative_velocity;
            this.joint_relative_acceleration = joint_relative_acceleration;
            this.override_joint_acceleration = override_joint_acceleration;
        }

        public static SetSmartServoJointSpeedLimitsRequest Deserialize(MessageDeserializer deserializer) => new SetSmartServoJointSpeedLimitsRequest(deserializer);

        private SetSmartServoJointSpeedLimitsRequest(MessageDeserializer deserializer)
        {
            deserializer.Read(out this.joint_relative_velocity);
            deserializer.Read(out this.joint_relative_acceleration);
            deserializer.Read(out this.override_joint_acceleration);
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.joint_relative_velocity);
            serializer.Write(this.joint_relative_acceleration);
            serializer.Write(this.override_joint_acceleration);
        }

        public override string ToString()
        {
            return "SetSmartServoJointSpeedLimitsRequest: " +
            "\njoint_relative_velocity: " + joint_relative_velocity.ToString() +
            "\njoint_relative_acceleration: " + joint_relative_acceleration.ToString() +
            "\noverride_joint_acceleration: " + override_joint_acceleration.ToString();
        }

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
#else
        [UnityEngine.RuntimeInitializeOnLoadMethod]
#endif
        public static void Register()
        {
            MessageRegistry.Register(k_RosMessageName, Deserialize);
        }
    }
}
