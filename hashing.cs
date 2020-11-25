using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System;

namespace ConsistentRingHashing
{
    class RingHashing
    {

        public int numOfNodes { get; set; }

        public int replicationFactor { get; set; } = 3;

        private List<int> nodeArray = new List<int>();

        private Dictionary<int, string> nodeHashmap = new Dictionary<int, string>();

        public int hash(string key)
        {
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            int hashValue = (int) t.TotalSeconds;

            using (SHA256 sha256Hash = SHA256.Create()) 
            {
                hashValue = 0;
                byte[] hashedBytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(key));
                for (int pos = 0; pos < hashedBytes.Length; pos++)
                {
                    hashValue += pos*hashedBytes[pos];
                }
            }
            if (nodeArray.Contains(hashValue)) return hash(key+DateTime.UtcNow);
            return hashValue;
        }

        public string hashKey(string nodeName, int nodeNum)
        {
            return nodeName+"-"+nodeNum;
        }

        public void addNode(string nodeName)
        {
            for (int nodeNum = 0; nodeNum < this.replicationFactor; nodeNum++)
            {
                string key = hashKey(nodeName, nodeNum);
                int hashedNode = hash(key);
                nodeArray.Add(hashedNode);
                nodeHashmap.Add(hashedNode, key);
                numOfNodes += 1;
            }
            nodeArray.Sort();
        }


        public string addKey(string key)
        {
            if (nodeArray.Count == 0) throw new AccessViolationException("No nodes in ring");
            int hashedKey = hash(key);
            foreach (int nodeHash in nodeArray)
            {
                if (nodeHash > hashedKey) return nodeHashmap[nodeHash];
            }
            return nodeHashmap[nodeArray[0]];
        }
    }
}