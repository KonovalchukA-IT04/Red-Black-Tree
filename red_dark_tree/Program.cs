using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace red_dark_tree
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Directory.GetCurrentDirectory();
            path = path.Replace("bin\\Debug","text.json");
            string text = Read(path);
            Tree tree = new Tree(0);

            int[] vector = new int[] /*{ 8, 4, 12, 2, 6, 10, 14, 1, 3, 5, 7, 9, 11, 13, 15, 20, 18, 19, 17 }; */ { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22 };       //, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32   

            ConsoleKeyInfo insert_key;
            bool switcher = true;
            while (switcher)
            {
                Console.Clear();
                switcher = false;
                Console.WriteLine("Hardcode, Read, None, Generete: h/r/n/g");
                insert_key = Console.ReadKey();
                switch (insert_key.KeyChar)
                {
                    case 'h':
                        tree = new Tree(vector[0]);
                        for (int i = 1; i < vector.Length; i++)
                        {
                            tree.Add(vector[i]);
                        }
                        break;
                    case 'r':
                        Node k = JsonSerializer.Deserialize<Node>(text);
                        tree.ConnectChilds(k);
                        tree._root = k;
                        break;
                    case 'n':
                        Console.WriteLine("Add first node");
                        int add_key = Get_Key();
                        Console.Write("Enter the data: ");
                        var add_val = Console.ReadLine();
                        var nd = new Node(add_key, null);
                        nd.Data = add_val;
                        tree._root = nd;
                        break;
                    case 'g':
                        Console.Write("Enter node count: ");
                        int count_key = Get_Key();
                        Random rnd = new Random();
                        tree = new Tree(Generete_Key(ref rnd));
                        for(int i=0; i<count_key; i++)
                        {
                            tree.Add(Generete_Key(ref rnd));
                        }
                        break;
                    default:
                        switcher = true;
                        break;
                }
            }           


            switcher = true;
            while (switcher)
            {
                Console.Clear();
                Console.WriteLine("Add, Delete, Edit, Find, Print, Break: a/d/e/f/p/b");
                insert_key = Console.ReadKey();
                switch (insert_key.KeyChar)
                {
                    case 'a':
                        int add_key = Get_Key();
                        Console.Write("Enter the data: ");
                        var add_val = Console.ReadLine();
                        var nd = tree.Add(add_key);
                        nd.Data = add_val;
                        break;
                    case 'd':
                        int del = Get_Key();
                        tree.Delete(tree._root, del);
                        break;
                    case 'e':
                        int edit_key = Get_Key();
                        Console.Write("Enter the data: ");
                        var edit_val = Console.ReadLine();
                        tree.Edit(tree._root, edit_key, edit_val);
                        break;
                    case 'f':
                        int find_key = Get_Key();
                        int cnt_rbt=0;
                        Node find_node = tree.Find(find_key, tree._root, ref cnt_rbt);
                        if(find_node != null)
                            Console.WriteLine($"For key {find_key}: "+ find_node.Data);
                        else
                            Console.WriteLine("Nothing found");
                        Console.WriteLine($"Iterations: {cnt_rbt}\nclk any_button");
                        Console.ReadKey();
                        break;
                    case 'p':
                        Print_Tree_Horizontal(tree._root, 10);
                        Console.WriteLine("\n\n");
                        Console.WriteLine("clk any_button");
                        Console.ReadKey();
                        break;
                    case 'b':
                        switcher = false;
                        break;
                    default:
                        break;
                }
            }

            switcher = true;
            while(switcher)
            {
                Console.Clear();
                Console.Write("Want to save?(y/n): ");
                insert_key = Console.ReadKey();
                if (insert_key.KeyChar == 'y')
                {
                    List<Node> node_list = new List<Node>();
                    var options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve,
                    };
                    string json = JsonSerializer.Serialize<Node>(tree._root, options);
                    Write(json, (path));
                    switcher = false;
                }
                else if(insert_key.KeyChar == 'n')
                {
                    switcher = false;
                }
                else
                {
                    continue;
                }
            }                        
        }

        static void Print_Tree_Horizontal(Node node, int space)
        {
            if(node != null)
            {
                Print_Tree_Horizontal(node.NextRight, space + 4);
                for (int i = 1; i <= space; i++)
                {
                    Console.Write(" ");
                }
                if (node.Is_Red)
                    Paint_Red(node);
                else
                    Console.WriteLine(node.Key);
                Print_Tree_Horizontal(node.NextLeft, space + 4);
            }
        }
        static void Paint_Red(Node node)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine(node.Key);
            Console.BackgroundColor = ConsoleColor.Black;
        }

        static string Read(string Path)
        {
            StreamReader sr = new StreamReader(Path);
            string @string = sr.ReadToEnd();
            sr.Close();
            return @string;
        }
        static void Write(string @string, string Path)
        {
            StreamWriter sw = new StreamWriter(Path);
            sw.WriteLine(@string);
            sw.Close();
        }

        static int Get_Key()
        {
            int key=0;
            while(true)
            {
                Console.Write("Enter the key: ");
                bool success = int.TryParse(Console.ReadLine(), out key);
                if (!success)
                {
                    Console.WriteLine("invalid data");
                    continue;
                }
                else
                    break;
            }
            return key;
        }

        static int Generete_Key(ref Random rnd)
        {
            int key = 0;
            key = rnd.Next(0,10000);
            return key;
        }
    }
}
