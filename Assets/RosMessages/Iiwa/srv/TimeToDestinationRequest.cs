//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.Iiwa
{
    [Serializable]
    public class TimeToDestinationRequest : Message
    {
        public const string k_RosMessageName = "iiwa_msgs/TimeToDestination";
        public override string RosMessageName => k_RosMessageName;


        public TimeToDestinationRequest()
        {
        }
        public static TimeToDestinationRequest Deserialize(MessageDeserializer deserializer) => new TimeToDestinationRequest(deserializer);

        private TimeToDestinationRequest(MessageDeserializer deserializer)
        {
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
        }

        public override string ToString()
        {
            return "TimeToDestinationRequest: ";
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