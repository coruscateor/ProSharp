using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProSharp;

namespace ProSharpTest
{

    class Program
    {

        static void Main(string[] args)
        {

            dynamic Pro1 = new ProObject();

            Pro1.Name = "Steve";

            Pro1.Something = 1;

            Pro1["Text"] = "Some text";

            Pro1[1234] = "456";

            WriteProContents(Pro1);

            Divide();

            dynamic Pro2 = new ProObject(Pro1);

            Pro2.Steve = true;

            Pro2.Something = "overridden";

            Pro2.Text = "Some more text";

            WriteProContents(Pro2);

            Divide();

            Console.WriteLine("Pro2.Prototype");

            Console.WriteLine();

            WriteProContents(Pro2.Prototype);

            Divide();

            Console.WriteLine("Constructors");

            Divide();

            ProConstructor ProC1 = new ProConstructor();

            ProC1.ConstructorAction = (TheObject, ThePrameters) =>
            {

                if(ThePrameters.Length > 0)
                {

                    TheObject.Something1 = ThePrameters[0];

                    if(ThePrameters.Length > 1)
                        TheObject.Item = ThePrameters[1];

                    if(ThePrameters.Length > 2)
                        TheObject.X = ThePrameters[2];

                    if(ThePrameters.Length > 3)
                        TheObject.Y = ThePrameters[3];

                }

            };

            ProObject OC1 = ProC1.New("Something");

            WriteProContents(OC1);

            Divide();

            ProObject OC2 = ProC1.New("Something", true);

            WriteProContents(OC2);

            Divide();

            ProObject OC3 = ProC1.New("Something", true, 50);

            WriteProContents(OC3);

            Divide();

            ProObject OC4 = ProC1.New("Something", true, 50, 60);

            WriteProContents(OC4);

            Console.Read();

        }

        static void WriteProContents(dynamic Pro)
        {

            foreach(var Item in Pro)
            {

                Console.WriteLine(Item.Key + ": " + Item.Value);

            }

        }

        static void Divide()
        {

            Console.WriteLine();

            Console.WriteLine("------------------------------------------");

            Console.WriteLine();

        }

    }

}
