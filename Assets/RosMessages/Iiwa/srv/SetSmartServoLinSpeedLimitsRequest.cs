//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.Iiwa
{
    [Serializable]
    public class SetSmartServoLinSpeedLimitsRequest : Message
    {
        public const string k_RosMessageName = "iiwa_msgs/SetSmartServoLinSpeedLimits";
        public override string RosMessageName => k_RosMessageName;

        //  Translational and rotational speed in m/s and rad/s
        public Geometry.TwistMsg max_cartesian_velocity;

        public SetSmartServoLinSpeedLimitsRequest()
        {
            this.max_cartesian_velocity = new Geometry.TwistMsg();
        }

        public SetSmartServoLinSpeedLimitsRequest(Geometry.TwistMsg max_cartesian_velocity)
        {
            this.max_cartesian_velocity = max_cartesian_velocity;
        }

        public static SetSmartServoLinSpeedLimitsRequest Deserialize(MessageDeserializer deserializer) => new SetSmartServoLinSpeedLimitsRequest(deserializer);

        private SetSmartServoLinSpeedLimitsRequest(MessageDeserializer deserializer)
        {
            this.max_cartesian_velocity = Geometry.TwistMsg.Deserialize(deserializer);
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.max_cartesian_velocity);
        }

        public override string ToString()
        {
            return "SetSmartServoLinSpeedLimitsRequest: " +
            "\nmax_cartesian_velocity: " + max_cartesian_velocity.ToString();
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
