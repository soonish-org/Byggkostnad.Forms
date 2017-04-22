using System;
using System.Reflection;
using System.IO;

namespace ByggKostnad.Forms.PhoneNumbers
{
    public class Data
    {
		public static string Get()
		{
            var dataT = typeof(Data).GetTypeInfo();
            using (var stream = dataT.Assembly.GetManifestResourceStream(dataT.Namespace+ ".Data.json"))
			using (var read = new StreamReader(stream))
			{
				return read.ReadToEnd();
			}
		}
    }
}
