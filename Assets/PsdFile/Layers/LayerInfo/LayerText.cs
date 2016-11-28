using System;
using System.Collections.Generic;
using PhotoshopFile.Text;

namespace PhotoshopFile
{
	public class LayerText : LayerInfo
	{
		public override string Key
		{
			get { return "TySh"; }
		}

		public Matrix2D Transform;
		public DynVal TxtDescriptor;
		public TdTaStylesheetReader StylesheetReader;
		public Dictionary<string, object> engineData;
		public Boolean isTextHorizontal
		{
			get
			{
				return ((string)TxtDescriptor.Children.Find(c => c.Name.Equals("Orientation", StringComparison.InvariantCultureIgnoreCase)).Value)
					 .Equals("Orientation.Horizontal", StringComparison.InvariantCultureIgnoreCase);
			}
		}

		public string Text
		{
			get;
			private set;
		}

		public double FontSize
		{
			get;
			private set;
		}

		public string FontName
		{
			get;
			private set;
		}

		public bool FauxBold
		{
			get;
			private set;
		}

		public bool FauxItalic
		{
			get;
			private set;
		}

		public bool Underline
		{
			get;
			private set;
		}

		public int FillColor
		{
			get;
			private set;
		}

		public LayerText()
		{

		}

		public LayerText(PsdBinaryReader reader, int dataLength)
		{
			var endPos = reader.BaseStream.Position + dataLength;

			// PhotoShop version
			reader.ReadUInt16();

			Transform = new Matrix2D(reader);

			// TextVersion
			reader.ReadUInt16(); //2 bytes, =50. For Photoshop 6.0.

			// DescriptorVersion
			reader.ReadUInt32(); //4 bytes,=16. For Photoshop 6.0.

			TxtDescriptor = DynVal.ReadDescriptor(reader); //Text descriptor

			// WarpVersion
			reader.ReadUInt16(); //2 bytes, =1. For Photoshop 6.0.

			engineData = (Dictionary<string, object>)TxtDescriptor.Children.Find(c => c.Name == "EngineData").Value;
			StylesheetReader = new TdTaStylesheetReader(engineData);

			// WarpDescriptor = DynVal.ReadDescriptor(reader); //Warp descriptor

			reader.BaseStream.Position = endPos;
			
			Dictionary<string, object> d = StylesheetReader.GetStylesheetDataFromLongestRun();
			Text = StylesheetReader.Text;
			FontName = TdTaParser.getString(StylesheetReader.getFontSet()[(int)TdTaParser.query(d, "Font")], "Name$");
			FontSize = (double)TdTaParser.query(d, "FontSize");

			try
			{
				FauxBold = TdTaParser.getBool(d, "FauxBold");
			}
			catch (KeyNotFoundException)
			{
				FauxBold = false;
			}

			try
			{
				FauxItalic = TdTaParser.getBool(d, "FauxItalic");
			}
			catch (KeyNotFoundException)
			{
				FauxItalic = false;
			}

			try
			{
				Underline = TdTaParser.getBool(d, "Underline");
			}
			catch (KeyNotFoundException)
			{
				Underline = false;
			}

			FillColor = TdTaParser.getColor(d, "FillColor");
		}

		protected override void WriteData(PsdBinaryWriter writer)
		{
			throw new NotImplementedException("LayerText.WriteData not implemented!");
		}
	}
}