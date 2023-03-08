//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.Iiwa
{
    [Serializable]
    public class MoveToJointPositionResult : Message
    {
        public const string k_RosMessageName = "iiwa_msgs/MoveToJointPosition";
        public override string RosMessageName => k_RosMessageName;

        //  Result
        public bool success;
        public string error;

        public MoveToJointPositionResult()
        {
            this.success = false;
            this.error = "";
        }

        public MoveToJointPositionResult(bool success, string error)
        {
            this.success = success;
            this.error = error;
        }

        public static MoveToJointPositionResult Deserialize(MessageDeserializer deserializer) => new MoveToJointPositionResult(deserializer);

        private MoveToJointPositionResult(MessageDeserializer deserializer)
        {
            deserializer.Read(out this.success);
            deserializer.Read(out this.error);
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.success);
            serializer.Write(this.error);
        }

        public override string ToString()
        {
            return "MoveToJointPositionResult: " +
            "\nsuccess: " + success.ToString() +
            "\nerror: " + error.ToString();
        }

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
#else
        [UnityEngine.RuntimeInitializeOnLoadMethod]
#endif
        public static void Register()
        {
            MessageRegistry.Register(k_RosMessageName, Deserialize, MessageSubtopic.Result);
        }
    }
}
