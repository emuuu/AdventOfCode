using AdventOfCode.Configuration;
using AdventOfCode.Extensions;
using AdventOfCode.Services;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AdventOfCode.App
{
    public class Application
    {
        public readonly AdventOfCode2025 _advent2025Service;
        public readonly AdventOfCode2024 _advent2024Service;
        public readonly AdventOfCode2023 _advent2023Service;
        public readonly AdventOfCode2022 _advent2022Service;
        public readonly AdventOfCode2021 _advent2021Service;
        public readonly AdventOfCode2020 _advent2020Service;
        public readonly AdventOfCode2019 _advent2019Service;

        private List<KeyValuePair<string, MethodInfo>> _settingMethods;
        private List<MethodInfo> _adventMethods;
        private string _inputPath;
        private string _sessionID;
        private int _year;

        public Application(
            AdventOfCode2025 advent2025Service,
            AdventOfCode2024 advent2024Service,
            AdventOfCode2023 advent2023Service,
            AdventOfCode2022 advent2022Service,
            AdventOfCode2021 advent2021Service,
            AdventOfCode2020 advent2020Service,
            AdventOfCode2019 advent2019Service,
            IOptions<Settings> settings)
        {
            _advent2025Service = advent2025Service;
            _advent2024Service = advent2024Service;
            _advent2023Service = advent2023Service;
            _advent2022Service = advent2022Service;
            _advent2021Service = advent2021Service;
            _advent2020Service = advent2020Service;
            _advent2019Service = advent2019Service;
            _settingMethods = new List<KeyValuePair<string, MethodInfo>>();
            _adventMethods = new List<MethodInfo>();
            _inputPath = settings.Value.InputPath;
            _sessionID = settings.Value.SessionID;
            _year = DateTime.Now.Year;
        }

        [NotInMethodList]
        public void InitComponents()
        {
            Console.WriteLine("Current inputFile path:");
            Console.WriteLine(_inputPath);
            Console.WriteLine("");

            Console.WriteLine("Current year selected:");
            Console.WriteLine(_year);
            Console.WriteLine("");

            InitMethodlist();
            PrintMethodList();

            Console.WriteLine("Press ESC to exit");
            Console.WriteLine("");

            Run();
        }

        [NotInMethodList]
        public void Run()
        {
            /*
            if (string.IsNullOrEmpty(_sessionID))
            {
                Console.WriteLine("Insert your Advent of Code SessionID:");
                _sessionID = ReadLineOrEsc();
            }
            */
            Console.WriteLine("");
            
            do
            {
                Console.WriteLine("Select day:");
                var input = ReadLineOrEsc();
                if (input == null)
                {
                    Environment.Exit(0);
                }
                input = input.ToUpper();

                var settingsMethod = _settingMethods.FirstOrDefault(x => x.Key == input);
                
                if (_settingMethods.Any(x => x.Key == input))
                {
                    ExecuteMethod(_settingMethods.First(x => x.Key == input).Value);
                }
                else
                {
                    if (int.TryParse(input, out int day))
                    {
                        var daySolver = _adventMethods.FirstOrDefault(x => x.DeclaringType.Name == "AdventOfCode" + _year && x.Name == "SolveDay");
                        ExecuteMethod(daySolver, new object[] { _inputPath, day });
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a number between 1 and 25 or a setting key");
                    }
                }
            }
            while (true);
        }

        private void InitMethodlist()
        {
            GetTypeMethods(this);
            foreach (var field in this.GetType().GetFields())
            {
                GetTypeMethods(field.GetValue(this));
            }
        }

        private void GetTypeMethods(Object input)
        {
            if (input == null)
                return;

            var type = input.GetType();
            var methods = type.GetMethods().Where(x => x.DeclaringType == type);

            foreach (MethodInfo method in methods)
            {
                {
                    if (method.DeclaringType != typeof(Application))
                    {
                        if (Attribute.IsDefined(method, typeof(MethodInList)))
                        {
                            _adventMethods.Add(method);
                        }
                    }
                    else
                    {
                        if (!Attribute.IsDefined(method, typeof(NotInMethodList)))
                        {
                            var selector = method.Name[0].ToString().ToUpper();
                            if (_settingMethods.Any(x => x.Key == selector))
                            {
                                var count = 2;
                                while (_settingMethods.Any(x => x.Key == selector + count.ToString()))
                                {
                                    count++;
                                }
                                selector += count;
                            }
                            _settingMethods.Add(new KeyValuePair<string, MethodInfo>(selector, method));
                        }
                    }
                }
            }
        }

        public void PrintMethodList()
        {
            foreach (var declaringType in _settingMethods.Select(x => x.Value.DeclaringType).Distinct())
            {
                Console.WriteLine(declaringType.Name + ":");
                foreach (var method in _settingMethods.Where(x => x.Value.DeclaringType == declaringType))
                {
                    Console.WriteLine(method.Key + " - " + method.Value.Name);
                }
                Console.WriteLine("");
            }
        }

        public void SetInputPath()
        {
            Console.WriteLine("Update inputPath temporarily:");
            var pathToFile = Console.ReadLine();
            if (File.Exists(pathToFile))
            {
                _inputPath = pathToFile;
                Console.WriteLine("Updated inputPath");
            }
            else
            {
                Console.WriteLine("File not found! Input-Path remains:");
                Console.WriteLine(_inputPath);
            }
        }

        //disabled
        [NotInMethodList]
        public void UpdateSessionID()
        {
            Console.WriteLine("Update SessionID temporarily:");
            var tempSessionID = Console.ReadLine();
            if (!string.IsNullOrEmpty(tempSessionID))
            {
                _sessionID = tempSessionID;
                Console.WriteLine("Updated SessionID");
            }
            else
            {
                Console.WriteLine("Invalid input! SessionID remains:");
                Console.WriteLine(_sessionID);
            }
        }

        public void ChangeYear()
        {
            Console.WriteLine("Change AdventOfCode year:");
            var year = Console.ReadLine();
            if (_adventMethods.Any(x => x.DeclaringType.Name == "AdventOfCode" + year))
            {
                _year = int.Parse(year);
                Console.WriteLine("Updated year");
            }
            else
            {
                Console.WriteLine("Selected year is not implemented yet. Year remains:");
                Console.WriteLine(_year);
            }
        }

        private void ExecuteMethod(MethodInfo method, object[] parameters = null)
        {
            object target = this;
            foreach (var field in this.GetType().GetFields())
            {
                if (field.FieldType == method.DeclaringType)
                {
                    target = field.GetValue(this);
                }
            }
            var result = method.Invoke(target, parameters);
            if (result != null)
            {
                if(result.GetType() == typeof(List<string>))
                {
                    foreach (var line in result as List<string>)
                    {
                        Console.WriteLine(line);
                    }
                }
            }
            Console.WriteLine("");
            Run();
        }

        private static string ReadLineOrEsc()
        {
            string retString = "";

            int curIndex = 0;
            do
            {
                ConsoleKeyInfo readKeyResult = Console.ReadKey(true);

                // handle Esc
                if (readKeyResult.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine();
                    return null;
                }

                // handle Enter
                if (readKeyResult.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    return retString;
                }

                // handle backspace
                if (readKeyResult.Key == ConsoleKey.Backspace)
                {
                    if (curIndex > 0)
                    {
                        retString = retString.Remove(retString.Length - 1);
                        Console.Write(readKeyResult.KeyChar);
                        Console.Write(' ');
                        Console.Write(readKeyResult.KeyChar);
                        curIndex--;
                    }
                }
                else
                // handle all other keypresses
                {
                    retString += readKeyResult.KeyChar;
                    Console.Write(readKeyResult.KeyChar);
                    curIndex++;
                }
            }
            while (true);
        }
    }
}
