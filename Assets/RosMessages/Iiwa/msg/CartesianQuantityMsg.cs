//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.Iiwa
{
    [Serializable]
    public class CartesianQuantityMsg : Message
    {
        public const string k_RosMessageName = "iiwa_msgs/CartesianQuantity";
        public override string RosMessageName => k_RosMessageName;

        public float x;
        public float y;
        public float z;
        public float a;
        public float b;
        public float c;

        public CartesianQuantityMsg()
        {
            this.x = 0.0f;
            this.y = 0.0f;
            this.z = 0.0f;
            this.a = 0.0f;
            this.b = 0.0f;
            this.c = 0.0f;
        }

        public CartesianQuantityMsg(float x, float y, float z, float a, float b, float c)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.a = a;
            this.b = b;
            this.c = c;
        }

        public static CartesianQuantityMsg Deserialize(MessageDeserializer deserializer) => new CartesianQuantityMsg(deserializer);

        private CartesianQuantityMsg(MessageDeserializer deserializer)
        {
            deserializer.Read(out this.x);
            deserializer.Read(out this.y);
            deserializer.Read(out this.z);
            deserializer.Read(out this.a);
            deserializer.Read(out this.b);
            deserializer.Read(out this.c);
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.x);
            serializer.Write(this.y);
            serializer.Write(this.z);
            serializer.Write(this.a);
            serializer.Write(this.b);
            serializer.Write(this.c);
        }

        public override string ToString()
        {
            return "CartesianQuantityMsg: " +
            "\nx: " + x.ToString() +
            "\ny: " + y.ToString() +
            "\nz: " + z.ToString() +
            "\na: " + a.ToString() +
            "\nb: " + b.ToString() +
            "\nc: " + c.ToString();
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