using System;
using DevExpress.XtraDataLayout;
using System.Xml.Serialization;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Reflection;

namespace xwcs.core.ui.datalayout.attributes
{
	public enum PolymorphKind
	{
		Undef = 0,
		XmlSerialization = 1,
		JsonSerialization = 2
	}

	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class PolymorphFlag : FlagsAttribute
	{
		public PolymorphKind Kind { get; set; } = PolymorphKind.Undef;
		public string SourcePropertyName { get; set; } = "";	
	}

	static class ExtensionMethods
	{
		

		public static string TypedSerialize(this object objectInstance, PolymorphKind kind = PolymorphKind.XmlSerialization)
		{
			if (objectInstance == null) return null;

			if(kind == PolymorphKind.XmlSerialization) {
				var serializer = new XmlSerializer(objectInstance.GetType());
				var sb = new StringBuilder();

				using (TextWriter writer = new StringWriter(sb))
				{
					writer.WriteLine(objectInstance.GetType().FullName);
					serializer.Serialize(writer, objectInstance);
				}

				return sb.ToString();
			}
			else {
				return objectInstance.ToString();
            }
		}

		public static object TypedDeserialize(this string objectData, PolymorphKind kind = PolymorphKind.XmlSerialization)
		{
			if (objectData == null || objectData.Length == 0) return null;

			if (kind == PolymorphKind.XmlSerialization)
			{
				try {
					using (TextReader reader = new StringReader(objectData))
					{
						string tn = reader.ReadLine();
						Type tt;
						if(plgs.SPluginsLoader.getInstance().TryFindType(tn, out tt)){
							var serializer = new XmlSerializer(tt);
							return serializer.Deserialize(reader);
						}
						else {
							return objectData;
						}
					}
				}catch(Exception) {
					return objectData;
				}				
			}
			else {
				return objectData;
			}
		}
	}


	

	
}
