using System.Collections.Generic;

namespace AngryWasp.Cli.Args
{
    public class Arguments
    {
        private List<Argument> options = new List<Argument>();

        public List<Argument> All => options;

        public Argument this[int index]
        {
            get
            {
                if (index < 0 || index >= options.Count)
                    return null;

                return options[index];
            }
        }

        public Argument this[string flag]
        {
            get
            {
                string f1 = string.IsNullOrEmpty(flag) ? null : flag.ToLower().Trim(new char[] { '-' });

                for (int i = 0; i < options.Count; i++)
                {
                    string f2 = string.IsNullOrEmpty(options[i].Flag) ? null : options[i].Flag.ToLower().Trim(new char[] { '-' });
                    if (f2 == f1)
                        return options[i];
                }

                return null;
            }
        }

        public static Arguments Parse(string[] args)
        {
            Arguments cmd = new Arguments();

            if (args == null)
                return cmd;

            for (int i = 0; i < args.Length; i++)
            {
                bool isFlag = args[i].StartsWith("-");
                bool hasParam = (i + 1) < args.Length ? !args[i + 1].StartsWith("-") : false;

                string parameter = hasParam ? args[i + 1] : null;
                string arg = args[i].TrimStart(new char[] { '-' });

                if (isFlag)
                {
                    char[] argChars = arg.ToCharArray();

                    if (args[i].StartsWith("--"))
                        cmd.Push(arg, parameter);
                    else
                    {
                        if (arg.Length == 1)
                            cmd.Push(arg, parameter);
                        else
                            for (int c = 0; c < argChars.Length; c++)
                                cmd.Push(argChars[c].ToString(), null);
                    }

                    if (argChars.Length == 1 && hasParam)
                        i++;
                }
                else
                    cmd.Push(null, arg);
            }

            return cmd;
        }

        public Argument Pop()
        {
            Argument o = options[0];
            options.RemoveAt(0);
            return o;
        }

        public static Arguments New(params Argument[] arguments) => new Arguments();

        public void Push(Argument o) => options.Add(o);

        public void Push(string flag, string value) => options.Add(new Argument(flag, value));

        public int Count => options.Count;

        public string GetString(string key, string defaultValue = null)
        {
            if (this[key] == null)
                return defaultValue;

            return this[key].Value;
        }

        public bool? GetBool(string key, bool? defaultValue = null)
        {

            if (this[key] == null || this[key].Value == null)
                return defaultValue;

            if (!bool.TryParse(this[key].Value, out bool i))
                return defaultValue;

            return i;
        }

        public int? GetInt(string key, int? defaultValue = null)
        {

            if (this[key] == null)
                return defaultValue;

            if (!int.TryParse(this[key].Value, out int i))
                return defaultValue;

            return i;
        }

        public uint? GetUint(string key, uint? defaultValue = null)
        {

            if (this[key] == null)
                return defaultValue;

            if (!uint.TryParse(this[key].Value, out uint i))
                return defaultValue;

            return i;
        }

        public ushort? GetUshort(string key, ushort? defaultValue = null)
        {

            if (this[key] == null)
                return defaultValue;

            if (!ushort.TryParse(this[key].Value, out ushort i))
                return defaultValue;

            return i;
        }

        public float? GetFloat(string key, float? defaultValue = null)
        {

            if (this[key] == null)
                return defaultValue;

            if (!float.TryParse(this[key].Value, out float i))
                return defaultValue;

            return i;
        }

        public double? GetDouble(string key, double? defaultValue = null)
        {

            if (this[key] == null)
                return defaultValue;

            if (!double.TryParse(this[key].Value, out double i))
                return defaultValue;

            return i;
        }

        public long? GetLong(string key, long? defaultValue = null)
        {

            if (this[key] == null)
                return defaultValue;

            if (!long.TryParse(this[key].Value, out long i))
                return defaultValue;

            return i;
        }

        public ulong? GetUlong(string key, ulong? defaultValue = null)
        {

            if (this[key] == null)
                return defaultValue;

            if (!ulong.TryParse(this[key].Value, out ulong i))
                return defaultValue;

            return i;
        }
    }
}