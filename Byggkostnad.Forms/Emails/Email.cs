using System;
using System.Text.RegularExpressions;

namespace ByggKostnad.Forms.Emails
{
    public struct Email
    {
        public readonly string Value;
        private Email(string value)
        {
            Value = value;
        }
		/// <summary>
		/// https://msdn.microsoft.com/en-us/library/01escwtf(v=vs.110).aspx
		/// </summary>
		private static Regex regex = new Regex(
            @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
			@"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
			RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        public static Email Parse(string value)
        {
            if (TryParse(value, out Email e)) return e;
            throw new ArgumentException("Invalid Email");
        }

        public static bool TryParse(string value, out Email email)
        {
            if (regex.IsMatch(value))
            {
                email = new Email(value);
                return true;
            }
            email = default(Email);
            return false;
        }
        public override bool Equals(object obj)
        {
            if (obj is Email)
            {
                var e = (Email)obj;
                return Value.Equals(e.Value);
            }
            return false;
        }
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
        public override string ToString()
        {
            return Value;
        }
    }
}
