//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.Iiwa
{
    [Serializable]
    public class CartesianControlModeLimitsMsg : Message
    {
        public const string k_RosMessageName = "iiwa_msgs/CartesianControlModeLimits";
        public override string RosMessageName => k_RosMessageName;

        //  Sets the maximum cartesian deviation accepted. If the deviation is exceeded a stop condition occurs. 
        //  [x, y, z] in [mm]. Precondition: value > 0.0.
        //  [a, b, c] in [rad]. Precondition: value > 0.0.
        public CartesianQuantityMsg max_path_deviation;
        //  The maximum control force parameter.
        //  [x, y, z] in [N]. Precondition: value > 0.0.
        //  [a, b, c] in [Nm]. Precondition: value > 0.0.
        public CartesianQuantityMsg max_control_force;
        //  Indicates whether a stop condition is fired if the maximum control force is exceeded. 
        public bool max_control_force_stop;
        //  Maximum Cartesian velocity parameter 
        //  [x, y, z] in [mm/s]. Precondition: value > 0.0.
        //  [a, b, c] in [rad/s]. Precondition: value > 0.0.
        public CartesianQuantityMsg max_cartesian_velocity;

        public CartesianControlModeLimitsMsg()
        {
            this.max_path_deviation = new CartesianQuantityMsg();
            this.max_control_force = new CartesianQuantityMsg();
            this.max_control_force_stop = false;
            this.max_cartesian_velocity = new CartesianQuantityMsg();
        }

        public CartesianControlModeLimitsMsg(CartesianQuantityMsg max_path_deviation, CartesianQuantityMsg max_control_force, bool max_control_force_stop, CartesianQuantityMsg max_cartesian_velocity)
        {
            this.max_path_deviation = max_path_deviation;
            this.max_control_force = max_control_force;
            this.max_control_force_stop = max_control_force_stop;
            this.max_cartesian_velocity = max_cartesian_velocity;
        }

        public static CartesianControlModeLimitsMsg Deserialize(MessageDeserializer deserializer) => new CartesianControlModeLimitsMsg(deserializer);

        private CartesianControlModeLimitsMsg(MessageDeserializer deserializer)
        {
            this.max_path_deviation = CartesianQuantityMsg.Deserialize(deserializer);
            this.max_control_force = CartesianQuantityMsg.Deserialize(deserializer);
            deserializer.Read(out this.max_control_force_stop);
            this.max_cartesian_velocity = CartesianQuantityMsg.Deserialize(deserializer);
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.max_path_deviation);
            serializer.Write(this.max_control_force);
            serializer.Write(this.max_control_force_stop);
            serializer.Write(this.max_cartesian_velocity);
        }

        public override string ToString()
        {
            return "CartesianControlModeLimitsMsg: " +
            "\nmax_path_deviation: " + max_path_deviation.ToString() +
            "\nmax_control_force: " + max_control_force.ToString() +
            "\nmax_control_force_stop: " + max_control_force_stop.ToString() +
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
