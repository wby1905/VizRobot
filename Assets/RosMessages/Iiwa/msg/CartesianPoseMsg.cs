//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.Iiwa
{
    [Serializable]
    public class CartesianPoseMsg : Message
    {
        public const string k_RosMessageName = "iiwa_msgs/CartesianPose";
        public override string RosMessageName => k_RosMessageName;

        //  Target Pose including redundancy information.
        public Geometry.PoseStampedMsg poseStamped;
        //  If you have issues with the robot not executing the motion copy this value from the Cartesian Position Tab of the
        //  robot SmartPad. Set both parameters to -1 to disable them.
        public RedundancyInformationMsg redundancy;

        public CartesianPoseMsg()
        {
            this.poseStamped = new Geometry.PoseStampedMsg();
            this.redundancy = new RedundancyInformationMsg();
        }

        public CartesianPoseMsg(Geometry.PoseStampedMsg poseStamped, RedundancyInformationMsg redundancy)
        {
            this.poseStamped = poseStamped;
            this.redundancy = redundancy;
        }

        public static CartesianPoseMsg Deserialize(MessageDeserializer deserializer) => new CartesianPoseMsg(deserializer);

        private CartesianPoseMsg(MessageDeserializer deserializer)
        {
            this.poseStamped = Geometry.PoseStampedMsg.Deserialize(deserializer);
            this.redundancy = RedundancyInformationMsg.Deserialize(deserializer);
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.poseStamped);
            serializer.Write(this.redundancy);
        }

        public override string ToString()
        {
            return "CartesianPoseMsg: " +
            "\nposeStamped: " + poseStamped.ToString() +
            "\nredundancy: " + redundancy.ToString();
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