using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using IORAMHelper;

namespace ScenarioLibrary.DataElements
{
	/// <summary>
	/// Contains instructions, video file names and the scenario bitmap.
	/// </summary>
	public class MessagesCinematics
	{
		#region Fields

		public uint InstructionsStringDllId { get; set; }
		public uint HintsStringDllId { get; set; }
		public uint VictoryStringDllId { get; set; }
		public uint LossStringDllId { get; set; }
		public uint HistoryStringDllId { get; set; }
		public uint ScoutsStringDllId { get; set; }
		public string InstructionsString { get; set; }
		public string HintsString { get; set; }
		public string VictoryString { get; set; }
		public string LossString { get; set; }
		public string HistoryString { get; set; }
		public string ScoutsString { get; set; }
		public string PregameCinematicFileName { get; set; }
		public string VictoryCinematicFileName { get; set; }
		public string LossCinematicFileName { get; set; }
		public string BackgroundFileName { get; set; }
		public uint BitmapIncluded { get; set; }
		public BitmapLibrary.BitmapLoader Bitmap { get; set; }

		#endregion

		#region Functions

		/// <summary>
		/// Reads the data element from the given buffer.
		/// </summary>
		/// <param name="buffer">The buffer where the data element should be deserialized from.</param>
		public MessagesCinematics ReadData(RAMBuffer buffer)
		{
			InstructionsStringDllId = buffer.ReadUInteger();
			HintsStringDllId = buffer.ReadUInteger();
			VictoryStringDllId = buffer.ReadUInteger();
			LossStringDllId = buffer.ReadUInteger();
			HistoryStringDllId = buffer.ReadUInteger();
			ScoutsStringDllId = buffer.ReadUInteger();
			InstructionsString = buffer.ReadString(buffer.ReadShort());
			HintsString = buffer.ReadString(buffer.ReadShort());
			VictoryString = buffer.ReadString(buffer.ReadShort());
			LossString = buffer.ReadString(buffer.ReadShort());
			HistoryString = buffer.ReadString(buffer.ReadShort());
			ScoutsString = buffer.ReadString(buffer.ReadShort());
			PregameCinematicFileName = buffer.ReadString(buffer.ReadShort());
			VictoryCinematicFileName = buffer.ReadString(buffer.ReadShort());
			LossCinematicFileName = buffer.ReadString(buffer.ReadShort());
			BackgroundFileName = buffer.ReadString(buffer.ReadShort());

			BitmapIncluded = buffer.ReadUInteger();
			buffer.ReadInteger(); // Width
			buffer.ReadInteger(); // Height
			buffer.ReadShort(); // Unknown, -1 for Bitmaps, 1 for no bitmaps
			Bitmap = BitmapIncluded != 0 ? new BitmapLibrary.BitmapLoader(buffer, readFileHeader: false) : null;

			return this;
		}

		/// <summary>
		/// Writes the data element into the given buffer (at its current position).
		/// </summary>
		/// <param name="buffer">The buffer where the data element should be deserialized into.</param>
		public void WriteData(RAMBuffer buffer)
		{
			buffer.WriteUInteger(InstructionsStringDllId);
			buffer.WriteUInteger(HintsStringDllId);
			buffer.WriteUInteger(VictoryStringDllId);
			buffer.WriteUInteger(LossStringDllId);
			buffer.WriteUInteger(HistoryStringDllId);
			buffer.WriteUInteger(ScoutsStringDllId);

			buffer.WriteShort((short)InstructionsString.Length);
			buffer.WriteString(InstructionsString);

			buffer.WriteShort((short)HintsString.Length);
			buffer.WriteString(HintsString);

			buffer.WriteShort((short)VictoryString.Length);
			buffer.WriteString(VictoryString);

			buffer.WriteShort((short)LossString.Length);
			buffer.WriteString(LossString);

			buffer.WriteShort((short)HistoryString.Length);
			buffer.WriteString(HistoryString);

			buffer.WriteShort((short)ScoutsString.Length);
			buffer.WriteString(ScoutsString);

			buffer.WriteShort((short)PregameCinematicFileName.Length);
			buffer.WriteString(PregameCinematicFileName);

			buffer.WriteShort((short)VictoryCinematicFileName.Length);
			buffer.WriteString(VictoryCinematicFileName);

			buffer.WriteShort((short)LossCinematicFileName.Length);
			buffer.WriteString(LossCinematicFileName);

			buffer.WriteShort((short)BackgroundFileName.Length);
			buffer.WriteString(BackgroundFileName);

			if(Bitmap == null)
			{
				buffer.WriteUInteger(0);
				buffer.WriteInteger(0);
				buffer.WriteInteger(0);
				buffer.WriteShort(1);
			}
			else
			{
				buffer.WriteUInteger(1);
				buffer.WriteInteger(Bitmap.Width);
				buffer.WriteInteger(Bitmap.Height);
				buffer.WriteShort(-1);
				Bitmap.SaveToBuffer(buffer, false);
			}
		}

		#endregion

		#region Sub types

		[StructLayout(LayoutKind.Sequential)]
		public struct BITMAPINFOHEADER
		{
			public uint biSize;
			public int biWidth;
			public int biHeight;
			public ushort biPlanes;
			public ushort biBitCount;
			public BitmapCompressionMode biCompression;
			public uint biSizeImage;
			public int biXPelsPerMeter;
			public int biYPelsPerMeter;
			public uint biClrUsed;
			public uint biClrImportant;

			public void SetSize()
			{
				biSize = (uint)Marshal.SizeOf(this);
			}
		}

		public enum BitmapCompressionMode : uint
		{
			BI_RGB = 0,
			BI_RLE8 = 1,
			BI_RLE4 = 2,
			BI_BITFIELDS = 3,
			BI_JPEG = 4,
			BI_PNG = 5
		}

		#endregion
	}
}