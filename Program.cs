using System;

namespace ConsistentRingHashing
{
    class Program
    {
        static void Main(string[] args)
        {
            RingHashing ringHasing = new RingHashing();
            ringHasing.addNode("Node1");
            ringHasing.addNode("Node2");
            ringHasing.addNode("Node3");
            ringHasing.addNode("Node4");
        }
    }
}
