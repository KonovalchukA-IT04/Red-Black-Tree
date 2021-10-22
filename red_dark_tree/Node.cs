using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace red_dark_tree
{
    public class Node
    {
        public Node NextLeft { get; set; }
        public Node NextRight { get; set; }
        public Node Parent { get; set; }

        public bool Is_Red { get; set; }

        public int Key { get; set; }
        public int Count { get; set; }
        public string Data { get; set; }

        public Node(int key)
        {
            Key = key;
            Count = 1;
            Is_Red = true;
            NextLeft = null;
            NextRight = null;
        }        
        public Node(int key, Node parent) : this(key)
        {
            Parent = parent;
        }
        [JsonConstructor]
        public Node(int key, int count, bool is_red, Node nextLeft, Node nextRight, Node parent)
        {
            Key = key;
            Count = count;
            Is_Red = is_red;
            NextLeft = nextLeft;
            NextRight = nextRight;
            Parent = parent;
        }
    }
}
