//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
using RosMessageTypes.Std;

namespace RosMessageTypes.Iiwa
{
    [Serializable]
    public class JointTorqueMsg : Message
    {
        public const string k_RosMessageName = "iiwa_msgs/JointTorque";
        public override string RosMessageName => k_RosMessageName;

        public HeaderMsg header;
        public JointQuantityMsg torque;

        public JointTorqueMsg()
        {
            this.header = new HeaderMsg();
            this.torque = new JointQuantityMsg();
        }

        public JointTorqueMsg(HeaderMsg header, JointQuantityMsg torque)
        {
            this.header = header;
            this.torque = torque;
        }

        public static JointTorqueMsg Deserialize(MessageDeserializer deserializer) => new JointTorqueMsg(deserializer);

        private JointTorqueMsg(MessageDeserializer deserializer)
        {
            this.header = HeaderMsg.Deserialize(deserializer);
            this.torque = JointQuantityMsg.Deserialize(deserializer);
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.header);
            serializer.Write(this.torque);
        }

        public override string ToString()
        {
            return "JointTorqueMsg: " +
            "\nheader: " + header.ToString() +
            "\ntorque: " + torque.ToString();
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
