
using System;
using System.Collections.Generic;

namespace Mustang.Algorithm
{
    public class TreeNode
    {
        public char value;
        public int frequency;
        public TreeNode left;
        public TreeNode right;

        public TreeNode(char value, int frequency)
        {
            this.value = value;
            this.frequency = frequency;
            left = null;
            right = null;
        }
    }

    public class OptimalBinaryTree
    {
        public static void Main()
        {
            string inputString = "ABBCCCDDDDEEEEE";
            List<TreeNode> nodeList = new List<TreeNode>();
            foreach (char c in inputString)
            {
                bool nodeExists = false;
                foreach (TreeNode node in nodeList)
                {
                    if (node.value == c)
                    {
                        node.frequency++;
                        nodeExists = true;
                        break;
                    }
                }
                if (!nodeExists)
                {
                    nodeList.Add(new TreeNode(c, 1));
                }
            }

            while (nodeList.Count > 1)
            {
                nodeList.Sort((x, y) => x.frequency.CompareTo(y.frequency));
                TreeNode leftChild = nodeList[0];
                TreeNode rightChild = nodeList[1];
                TreeNode parentNode = new TreeNode('\0', leftChild.frequency + rightChild.frequency);
                parentNode.left = leftChild;
                parentNode.right = rightChild;
                nodeList.RemoveAt(1);
                nodeList.RemoveAt(0);
                nodeList.Add(parentNode);
            }

            TreeNode root = nodeList[0];
            Dictionary<char, string> codes = new Dictionary<char, string>();
            GenerateCodes(root, "", codes);

            foreach (KeyValuePair<char, string> code in codes)
            {
                Console.WriteLine(code.Key + " : " + code.Value);
            }
        }

        public static void GenerateCodes(TreeNode node, string code, Dictionary<char, string> codes)
        {
            if (node.left == null && node.right == null)
            {
                codes.Add(node.value, code);
                return;
            }
            GenerateCodes(node.left, code + "0", codes);
            GenerateCodes(node.right, code + "1", codes);
        }
    }

}
