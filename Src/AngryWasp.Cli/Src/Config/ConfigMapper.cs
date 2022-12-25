using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AngryWasp.Cli.Args;

namespace AngryWasp.Cli.Config
{
    public static class ConfigMapper<T>
    {
        //Class instance to populate with argument values
        private static T classInstance;

        //The extra list is for command line options you want to show in the help list
        // as valid command line options, but don't want to save in the Config file
        private static string[,] extraList;

        private static Dictionary<string, (CommandLineArgumentAttribute Attribute, PropertyInfo Property)> map = 
            new Dictionary<string, (CommandLineArgumentAttribute, PropertyInfo)>();

        private static bool ExtraListContains(string flag)
        {
            if (extraList == null)
                return false;

            for (int i = 0; i < extraList.GetLength(0); i++)
                if (extraList[i, 0] == flag)
                    return true;
            
            return false;
        }

        public static bool Process(Arguments arguments, T instance, string[,] extras)
        {
            classInstance = instance;
            extraList = extras;

            foreach (var p in typeof(T).GetProperties())
            {
                CommandLineArgumentAttribute a = p.GetCustomAttributes(true).OfType<CommandLineArgumentAttribute>().FirstOrDefault();
                if (a == null || !p.CanWrite)
                    continue;

                map.Add(a.Flag, (a, p));
            }

            while (arguments.Count > 0)
            {
                Argument arg = arguments.Pop();
                if (arg.Flag == null)
                    continue;

                if (arg.Flag == "help")
                {
                    ShowHelp();
                    return false;
                }

                if (ExtraListContains(arg.Flag))
                    continue;
                
                //provided a flag wrong
                if (!map.ContainsKey(arg.Flag))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"No flag matches {arg.Flag}");
                    Console.ForegroundColor = ConsoleColor.White;
                    ShowHelp();
                    return false;
                }

                var dat = map[arg.Flag];

                //boolean flags do not need a value
                if (dat.Property.PropertyType == typeof(bool))
                {
                    dat.Property.SetValue(instance, true);
                    continue;
                }

                //check we have a value if the flag expects a value
                if (string.IsNullOrEmpty(arg.Value))
                {
                    if (string.IsNullOrEmpty(dat.Attribute.DefaultValue))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"No value provided for flag {arg.Flag}");
                        Console.ForegroundColor = ConsoleColor.White;
                        ShowHelp();
                        return false;
                    }
                    else
                    {
                        if (!Parse(dat.Property, dat.Attribute.DefaultValue))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Could not parse default value for flag {arg.Flag}");
                            Console.ForegroundColor = ConsoleColor.White;
                            ShowHelp();
                            return false;
                        }
                        else
                            continue;
                    }
                }

                if (!Parse(dat.Property, arg.Value))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Could not parse value for flag {arg.Flag}");
                    Console.ForegroundColor = ConsoleColor.White;
                    ShowHelp();
                    return false;
                }
            }

            return true;
        }

        private static bool Parse(PropertyInfo datProperty, string argValue)
        {
            if (datProperty.PropertyType.IsGenericType)
            {
                try
                {
                    object obj = AngryWasp.Serializer.Serializer.Deserialize(datProperty.PropertyType.GenericTypeArguments[0], argValue);
                    object instance = datProperty.GetValue(classInstance);
                    datProperty.PropertyType.GetMethod("Add").Invoke(instance, new object[] { obj });
                    return true;
                }
                catch { return false; }
            }
            else
            {
                try
                {
                    object obj = AngryWasp.Serializer.Serializer.Deserialize(datProperty.PropertyType, argValue);
                    datProperty.SetValue(classInstance, obj);
                    return true;
                }
                catch { return false; }
            }
        }

        private static void ShowHelp()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("EMS command line help");

            if (extraList != null)
            {
                for (int i = 0; i < extraList.GetLength(0); i++)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write($"{("--" + extraList[i, 0]).PadLeft(16)}");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($": {extraList[i, 1]}");
                }
            }

            foreach (var i in map.Values)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"{("--" + i.Attribute.Flag).PadLeft(16)}");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($": {i.Attribute.Description}");
            }

            Console.ForegroundColor = ConsoleColor.White;
        }
    }

}