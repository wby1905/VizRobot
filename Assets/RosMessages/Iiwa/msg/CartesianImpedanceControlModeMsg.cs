//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.Iiwa
{
    [Serializable]
    public class CartesianImpedanceControlModeMsg : Message
    {
        public const string k_RosMessageName = "iiwa_msgs/CartesianImpedanceControlMode";
        public override string RosMessageName => k_RosMessageName;

        //  Stiffness values [x, y, z, a, b, c] for the cartesian impedance, x, y, z in [N/m], a, b, c in [Nm/rad]. 
        //  Precondition: 0.0 <= x, y, z <= 5000.0 and 0.0 <= a, b, c <= 300.0. 
        public CartesianQuantityMsg cartesian_stiffness;
        //  Dimensionless damping values for the cartesian impedance control, for all degrees of freedom : [x, y, z, a, b, c].
        //  Precondition: 0.1 <= x, y, z, a, b, c <= 1.0. 
        public CartesianQuantityMsg cartesian_damping;
        //  The stiffness value for null space [Nm/rad]. 
        //  Precondition: 0.0 <= value. 
        public double nullspace_stiffness;
        //  The damping parameter for null space [Nm*s/rad]. 
        //  Precondition: value >= 0.3 and value <= 1.0. - A good damping value is 0.7. 
        public double nullspace_damping;

        public CartesianImpedanceControlModeMsg()
        {
            this.cartesian_stiffness = new CartesianQuantityMsg();
            this.cartesian_damping = new CartesianQuantityMsg();
            this.nullspace_stiffness = 0.0;
            this.nullspace_damping = 0.0;
        }

        public CartesianImpedanceControlModeMsg(CartesianQuantityMsg cartesian_stiffness, CartesianQuantityMsg cartesian_damping, double nullspace_stiffness, double nullspace_damping)
        {
            this.cartesian_stiffness = cartesian_stiffness;
            this.cartesian_damping = cartesian_damping;
            this.nullspace_stiffness = nullspace_stiffness;
            this.nullspace_damping = nullspace_damping;
        }

        public static CartesianImpedanceControlModeMsg Deserialize(MessageDeserializer deserializer) => new CartesianImpedanceControlModeMsg(deserializer);

        private CartesianImpedanceControlModeMsg(MessageDeserializer deserializer)
        {
            this.cartesian_stiffness = CartesianQuantityMsg.Deserialize(deserializer);
            this.cartesian_damping = CartesianQuantityMsg.Deserialize(deserializer);
            deserializer.Read(out this.nullspace_stiffness);
            deserializer.Read(out this.nullspace_damping);
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.cartesian_stiffness);
            serializer.Write(this.cartesian_damping);
            serializer.Write(this.nullspace_stiffness);
            serializer.Write(this.nullspace_damping);
        }

        public override string ToString()
        {
            return "CartesianImpedanceControlModeMsg: " +
            "\ncartesian_stiffness: " + cartesian_stiffness.ToString() +
            "\ncartesian_damping: " + cartesian_damping.ToString() +
            "\nnullspace_stiffness: " + nullspace_stiffness.ToString() +
            "\nnullspace_damping: " + nullspace_damping.ToString();
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