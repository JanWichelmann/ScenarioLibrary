using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IORAMHelper;

namespace ScenarioLibrary.DataElements
{
	/// <summary>
	/// 
	/// </summary>
	public class GlobalVictory
	{
		#region Fields

		public uint ConquestRequired { get; set; }
		public uint Unused1 { get; set; }
		public uint NumberOfRelicsRequired { get; set; }
		public uint Unused2 { get; set; }
		public uint ExploredPercentRequired { get; set; }
		public uint Unused3 { get; set; }
		public uint AllCustomConditionsRequired { get; set; }
		public VictoryMode Mode { get; set; }
		public uint ScoreRequired { get; set; }
		public uint TimeRequired { get; set; }

		#endregion

		#region Functions

		/// <summary>
		/// Reads the data element from the given buffer.
		/// </summary>
		/// <param name="buffer">The buffer where the data element should be deserialized from.</param>
		public GlobalVictory ReadData(RAMBuffer buffer)
		{
			// Separator
			ScenarioDataElementTools.AssertTrue(buffer.ReadUInteger() == 0xFFFFFF9D);

			ConquestRequired = buffer.ReadUInteger();
			Unused1 = buffer.ReadUInteger();
			NumberOfRelicsRequired = buffer.ReadUInteger();
			Unused2 = buffer.ReadUInteger();
			ExploredPercentRequired = buffer.ReadUInteger();
			Unused3 = buffer.ReadUInteger();
			AllCustomConditionsRequired = buffer.ReadUInteger();
			Mode = (VictoryMode)buffer.ReadUInteger();
			ScoreRequired = buffer.ReadUInteger();
			TimeRequired = buffer.ReadUInteger();

			return this;
		}

		/// <summary>
		/// Writes the data element into the given buffer (at its current position).
		/// </summary>
		/// <param name="buffer">The buffer where the data element should be deserialized into.</param>
		public void WriteData(RAMBuffer buffer)
		{
			buffer.WriteUInteger(0xFFFFFF9D);

			buffer.WriteUInteger(ConquestRequired);
			buffer.WriteUInteger(Unused1);
			buffer.WriteUInteger(NumberOfRelicsRequired);
			buffer.WriteUInteger(Unused2);
			buffer.WriteUInteger(ExploredPercentRequired);
			buffer.WriteUInteger(Unused3);
			buffer.WriteUInteger(AllCustomConditionsRequired);
			buffer.WriteUInteger((uint)Mode);
			buffer.WriteUInteger(ScoreRequired);
			buffer.WriteUInteger(TimeRequired);
		}

		#endregion

		#region Sub types

		/// <summary>
		/// The different victory modes.
		/// </summary>
		public enum VictoryMode : uint
		{
			Standard = 0,
			Conquest = 1,
			Score = 2,
			Timed = 3,
			Custom = 4
		};

		#endregion
	}
}