using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace red_dark_tree
{
    public class Tree
    {
        public Node _root { get; set; }
        public Tree(int key)
        {
            _root = new Node(key);
        }
        public Tree(Node node)
        {
            _root = node;
        }
        public Node AddNode(Node node, int key)
        {               
            if (key == node.Key)
            {
                node.Count++;
                return node;
            }
            if (key < node.Key)
            {
                if (node.NextLeft != null)
                    return AddNode(node.NextLeft, key);

                return node.NextLeft = new Node(key, node);
            }
            else
            {
                if (node.NextRight != null)
                    return AddNode(node.NextRight, key);

                return node.NextRight = new Node(key, node);
            }
        }
        public Node Add(int key)
        {
            Node item = AddNode(_root, key);
            if (item.Parent == null || item.Parent.Parent == null)
            {
                return item;
            }
            InsertFix(item);
            return item;
        }
        private void InsertFix(Node node)
        {
            Node uncle;
            while (node.Parent.Is_Red) //case2
            {
                if (node.Parent == node.Parent.Parent.NextRight)
                {
                    uncle = node.Parent.Parent.NextLeft;
                    if (uncle != null && uncle.Is_Red) //case3
                    {
                        uncle.Is_Red = false;
                        node.Parent.Is_Red = false;
                        node.Parent.Parent.Is_Red = true;
                        node = node.Parent.Parent; //recursion
                    }
                    else //case4
                    {
                        if (node == node.Parent.NextLeft)
                        {
                            node = node.Parent;
                            RightRotate(node);
                        }
                        node.Parent.Is_Red = false; //case5
                        node.Parent.Parent.Is_Red = true;
                        LeftRotate(node.Parent.Parent);
                    }
                }
                else
                {
                    uncle = node.Parent.Parent.NextRight;

                    if (uncle != null && uncle.Is_Red) //case3
                    {
                        uncle.Is_Red = false;
                        node.Parent.Is_Red = false;
                        node.Parent.Parent.Is_Red = true;
                        node = node.Parent.Parent; //recursion
                    }
                    else //case4
                    {
                        if (node == node.Parent.NextRight)
                        {
                            node = node.Parent;
                            LeftRotate(node);
                        }
                        node.Parent.Is_Red = false;  //case5
                        node.Parent.Parent.Is_Red = true;
                        RightRotate(node.Parent.Parent);
                    }
                }
                if (node == _root)
                {
                    break;
                }
            }
            _root.Is_Red = false; //case 1
        }
        public Node LeftRotate(Node x)
        {
            Node y = x.NextRight;
            if (y == null)
                return x;
            x.NextRight = y.NextLeft;
            if (y.NextLeft != null)
                y.NextLeft.Parent = x;
            if (x.Parent == null)
            {
                _root = y;
                y.Parent = null;
            }
            else if (x == x.Parent.NextLeft)
            {
                x.Parent.NextLeft = y;
                y.Parent = x.Parent;
            }
            else
            {
                x.Parent.NextRight = y;
                y.Parent = x.Parent;
            }
            y.NextLeft = x;
            x.Parent = y;
            return x;
        }
        public Node RightRotate(Node x)
        {
            Node y = x.NextLeft;
            if (y == null)
                return x;
            x.NextLeft = y.NextRight;
            if (y.NextRight != null)
                y.NextRight.Parent = x;
            if (x.Parent == null)
            {
                _root = y;
                y.Parent = null;
            }
            else if (x == x.Parent.NextRight)
            {
                x.Parent.NextRight = y;
                y.Parent = x.Parent;
            }
            else
            {
                x.Parent.NextLeft = y;
                y.Parent = x.Parent;
            }
            y.NextRight = x;
            x.Parent = y;
            return x;
        }

        public Node Find(int key, Node current_item)
        {
            if (current_item == null)
                return current_item;
            if (current_item.Key == key)
                return current_item;
            if (key < current_item.Key)
                return Find(key, current_item.NextLeft);
            else
                return Find(key, current_item.NextRight);
        }

        public Node Find(int key, Node current_item, ref int cnt)
        {
            cnt++;
            if (current_item == null)
                return current_item;
            if (current_item.Key == key)
                return current_item;
            if (key < current_item.Key)
                return Find(key, current_item.NextLeft, ref cnt);
            else
                return Find(key, current_item.NextRight, ref cnt);
        }
        
        private void fixDelete(Node node)
        {
            if (node != null && node != _root && node.Is_Red == false)  //case 1
            {
                if (node == node.Parent.NextLeft)
                {
                    Node s = node.Parent.NextRight;
                    if (s.Is_Red == true)   //case 2
                    {
                        s.Is_Red = false; 
                        node.Parent.Is_Red = true;
                        LeftRotate(node.Parent);
                        s = node.Parent.NextRight;
                    }
                    if ((s.NextLeft == null || s.NextLeft.Is_Red == false) && (s.NextRight == null || s.NextRight.Is_Red == false) && node.Parent.Is_Red == false) //case 3
                    {
                        s.Is_Red = true;
                        node = node.Parent;
                        fixDelete(node);
                    }
                    else
                    {
                        if ((s.NextLeft == null || s.NextLeft.Is_Red == false) && (s.NextRight == null || s.NextRight.Is_Red == false) && node.Parent.Is_Red == true) //case 4
                        {
                            s.Is_Red = true;
                            node.Parent.Is_Red = false;
                        }
                        else
                        {
                            if ((s.NextRight == null || s.NextRight.Is_Red == false) && s.NextLeft.Is_Red == true) //case 5
                            {
                                s.NextLeft.Is_Red = false;
                                s.Is_Red = true;
                                RightRotate(s);
                                s = node.Parent.NextRight;
                            }
                            s.Is_Red = node.Parent.Is_Red; //case 6
                            node.Parent.Is_Red = false;
                            s.NextRight.Is_Red = false;
                            LeftRotate(node.Parent);
                            node = _root;
                        }
                    }

                }
                else //mirror
                {
                    Node s = node.Parent.NextLeft;
                    if (s.Is_Red == true)   //case 2
                    {
                        s.Is_Red = false;
                        node.Parent.Is_Red = true;
                        RightRotate(node.Parent);
                        s = node.Parent.NextLeft;
                    }
                    if ((s.NextLeft == null|| s.NextLeft.Is_Red == false) && (s.NextRight == null || s.NextRight.Is_Red == false) && node.Parent.Is_Red == false) //case 3
                    {
                        s.Is_Red = true;
                        node = node.Parent;
                        fixDelete(node);
                    }
                    else
                    {
                        if ((s.NextLeft == null || s.NextLeft.Is_Red == false) && (s.NextRight == null || s.NextRight.Is_Red == false) && node.Parent.Is_Red == true) //case 4
                        {
                            s.Is_Red = true;
                            node.Parent.Is_Red = false;
                        }
                        else
                        {
                            if ((s.NextLeft == null || s.NextLeft.Is_Red == false) && s.NextRight.Is_Red == true) //case 5
                            {
                                s.NextRight.Is_Red = false;
                                s.Is_Red = true;
                                LeftRotate(s);
                                s = node.Parent.NextLeft;
                            }
                            s.Is_Red = node.Parent.Is_Red; //case 6
                            node.Parent.Is_Red = false;
                            s.NextLeft.Is_Red = false;
                            RightRotate(node.Parent);
                            node = _root;
                        }
                    }                    
                }
            }
            else
            if (node != null)
                node.Is_Red = false; //case 1
        }
        
        /*
        private void fixDelete(Node node)
        {
            while (node != null && node != _root && node.Is_Red == false)
            {
                if (node == node.Parent.NextLeft)
                {
                    Node s = node.Parent.NextRight;
                    if (s.Is_Red == true)   //case 2
                    {
                        s.Is_Red = false;
                        node.Parent.Is_Red = true;
                        LeftRotate(node.Parent);
                        s = node.Parent.NextRight;
                    }
                    if (s.NextLeft.Is_Red == false && s.NextRight.Is_Red == false && node.Parent.Is_Red == false) //case 3-4
                    {
                        s.Is_Red = true;
                        node = node.Parent;
                    }
                    else
                    {
                        if (s.NextRight.Is_Red == false) //case 5
                        {
                            s.NextLeft.Is_Red = false;
                            s.Is_Red = true;
                            RightRotate(s);
                            s = node.Parent.NextRight;
                        }
                        s.Is_Red = node.Parent.Is_Red; //case 6
                        node.Parent.Is_Red = false;
                        s.NextRight.Is_Red = false;
                        LeftRotate(node.Parent);
                        node = _root;
                    }
                }
                else //mirror
                {
                    Node s = node.Parent.NextLeft;
                    if (s.Is_Red == true)   //case 2
                    {
                        s.Is_Red = false;
                        node.Parent.Is_Red = true;
                        RightRotate(node.Parent);
                        s = node.Parent.NextLeft;
                    }
                    if (s.NextRight.Is_Red == false && s.NextLeft.Is_Red == false) //case 3-4
                    {
                        s.Is_Red = true;
                        node = node.Parent;
                    }
                    else
                    {
                        if (s.NextLeft.Is_Red == false) //case 5
                        {
                            s.NextRight.Is_Red = false;
                            s.Is_Red = true;
                            LeftRotate(s);
                            s = node.Parent.NextLeft;
                        }
                        s.Is_Red = node.Parent.Is_Red; //case 6
                        node.Parent.Is_Red = false;
                        s.NextLeft.Is_Red = false;
                        RightRotate(node.Parent);
                        node = _root;
                    }
                }
            }
            if (node != null)
                node.Is_Red = false;
        }
        */
        /*
        public void Delete(Node node, int key)
        {
            Node node_to_del = null;
            Node y;

            node_to_del = Find(key, node);

            if (node_to_del == null)
            {
                Console.WriteLine("Couldn't find key in the tree");
                return;
            }

            y = node_to_del;
            bool OriginalColor = y.Is_Red;
            if(node_to_del.NextLeft == null)
            {
                Transplant(node_to_del, node_to_del.NextRight);
            }
            else if (node_to_del.NextRight == null)
            {
                Transplant(node_to_del, node_to_del.NextLeft);
            }
            else
            {
                y = Minimum(node_to_del.NextRight);
                OriginalColor = y.Is_Red;
                if (y.Parent == node_to_del)
                {
                    Transplant(node_to_del, y);
                }
                else
                {
                    Transplant(y, y.NextRight);
                    y.NextRight = node_to_del.NextRight;
                    y.NextRight.Parent = y;
                    y.Parent = node_to_del.Parent;
                    Transplant(node_to_del, y);
                }
                y.NextLeft = node_to_del.NextLeft;
                y.NextLeft.Parent = y;
                y.Is_Red = node_to_del.Is_Red;
            }
            if (node_to_del.Is_Red == false)
            {
                fixDelete(y);
            }
        }
        */
        public void Delete(Node node, int key)
        {
            Node node_to_delete = null;
            Node x, y;

            node_to_delete = Find(key, node);

            if (node_to_delete == null)
            {
                Console.WriteLine("Couldn't find key in the tree");
                return;
            }

            y = node_to_delete;
            bool node_color = y.Is_Red;
            if (node_to_delete.NextLeft == null)
            {
                x = node_to_delete.NextRight;
                fixDelete(node_to_delete);
                Transplant(node_to_delete, node_to_delete.NextRight);
            }
            else if (node_to_delete.NextRight == null)
            {
                x = node_to_delete.NextLeft;
                fixDelete(node_to_delete);
                Transplant(node_to_delete, node_to_delete.NextLeft);
            }
            else
            {
                y = Minimum(node_to_delete.NextRight);
                node_color = y.Is_Red;
                x = y.NextRight;
                if (y.Parent == node_to_delete)
                {
                    fixDelete(node_to_delete.NextRight);
                    //x.Parent = y;
                }
                else
                {
                    fixDelete(y);
                    Transplant(y, y.NextRight);
                    y.NextRight = node_to_delete.NextRight;
                    y.NextRight.Parent = y;
                }

                Transplant(node_to_delete, y);
                y.NextLeft = node_to_delete.NextLeft;
                if(y.NextLeft!=null)
                    y.NextLeft.Parent = y;
                y.Is_Red = node_to_delete.Is_Red;
            }
            //if (node_color == false)
            //{
            //    fixDelete(x);
            //}
        }
        public Node Minimum(Node node)
        {
            while (node.NextLeft != null)
            {
                node = node.NextLeft;
            }
            return node;
        }
        private void Transplant(Node x, Node y)
        {
            if (x.Parent == null)
            {
                _root = y;
            }
            else if (x == x.Parent.NextLeft)
            {
                x.Parent.NextLeft = y;
            }
            else
            {
                x.Parent.NextRight = y;
            }
            if (y!=null)
                y.Parent = x.Parent;
        }

        public void Edit(Node node, int key, string edited_str)
        {
            Node cur_node = null;
            cur_node = Find(key, node);
            if (cur_node == null)
            {
                Console.WriteLine("Couldn't find key in the tree");
                return;
            }
            cur_node.Data = edited_str;
        }

        public void ConnectChilds(Node node)
        {
            if(node.NextLeft!=null)
            {
                node.NextLeft.Parent = node;
                ConnectChilds(node.NextLeft);
            }
            if (node.NextRight != null)
            {
                node.NextRight.Parent = node;
                ConnectChilds(node.NextRight);
            }
        }
    }
}
