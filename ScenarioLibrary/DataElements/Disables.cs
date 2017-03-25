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
	public class Disables
	{
		#region Fields

		/// <summary>
		/// IDs of disabled techs per player. Length: 16 entries, max. 30 entries per list.
		/// </summary>
		public List<List<uint>> DisabledTechsPerPlayer { get; set; }

		/// <summary>
		/// IDs of disabled units per player. Length: 16 entries, max. 30 entries per list.
		/// </summary>
		public List<List<uint>> DisabledUnitsPerPlayer { get; set; }

		/// <summary>
		/// IDs of disabled buildings per player. Length: 16 entries, max. 20 entries per list.
		/// </summary>
		public List<List<uint>> DisabledBuildingsPerPlayer { get; set; }

		/// <summary>
		/// Unused.
		/// </summary>
		public uint Unused1 { get; set; }

		/// <summary>
		/// Unused.
		/// </summary>
		public uint Unused2 { get; set; }

		/// <summary>
		/// Full tech mode.
		/// </summary>
		public uint FullTechMode { get; set; }

		/// <summary>
		/// The starting ages per player. None is -1. Length: 16 entries.
		/// </summary>
		public List<int> StartingAges { get; set; }

		#endregion

		#region Functions

		/// <summary>
		/// Reads the data element from the given buffer.
		/// </summary>
		/// <param name="buffer">The buffer where the data element should be deserialized from.</param>
		public Disables ReadData(RAMBuffer buffer)
		{
			List<int> disabledTechCountPerPlayer = new List<int>(16);
			for(int i = 0; i < 16; ++i)
				disabledTechCountPerPlayer.Add(buffer.ReadInteger());
			DisabledTechsPerPlayer = new List<List<uint>>(16);
			for(int i = 0; i < 16; ++i)
			{
				if(disabledTechCountPerPlayer[i] < 0)
					disabledTechCountPerPlayer[i] = 0;
				DisabledTechsPerPlayer.Add(new List<uint>(disabledTechCountPerPlayer[i]));
				for(int j = 0; j < disabledTechCountPerPlayer[i]; ++j)
					DisabledTechsPerPlayer[i].Add(buffer.ReadUInteger());
				buffer.ReadByteArray(4 * (30 - disabledTechCountPerPlayer[i]));
			}

			List<int> disabledUnitsCountPerPlayer = new List<int>(16);
			for(int i = 0; i < 16; ++i)
				disabledUnitsCountPerPlayer.Add(buffer.ReadInteger());
			DisabledUnitsPerPlayer = new List<List<uint>>(16);
			for(int i = 0; i < 16; ++i)
			{
				if(disabledUnitsCountPerPlayer[i] < 0)
					disabledUnitsCountPerPlayer[i] = 0;
				DisabledUnitsPerPlayer.Add(new List<uint>(disabledUnitsCountPerPlayer[i]));
				for(int j = 0; j < disabledUnitsCountPerPlayer[i]; ++j)
					DisabledUnitsPerPlayer[i].Add(buffer.ReadUInteger());
				buffer.ReadByteArray(4 * (30 - disabledUnitsCountPerPlayer[i]));
			}

			List<int> disabledBuildingsCountPerPlayer = new List<int>(16);
			for(int i = 0; i < 16; ++i)
				disabledBuildingsCountPerPlayer.Add(buffer.ReadInteger());
			DisabledBuildingsPerPlayer = new List<List<uint>>(16);
			for(int i = 0; i < 16; ++i)
			{
				if(disabledBuildingsCountPerPlayer[i] < 0)
					disabledBuildingsCountPerPlayer[i] = 0;
				DisabledBuildingsPerPlayer.Add(new List<uint>(disabledBuildingsCountPerPlayer[i]));
				for(int j = 0; j < disabledBuildingsCountPerPlayer[i]; ++j)
					DisabledBuildingsPerPlayer[i].Add(buffer.ReadUInteger());
				buffer.ReadByteArray(4 * (20 - disabledBuildingsCountPerPlayer[i]));
			}

			Unused1 = buffer.ReadUInteger();
			Unused2 = buffer.ReadUInteger();
			FullTechMode = buffer.ReadUInteger();

			StartingAges = new List<int>(16);
			for(int i = 0; i < 16; ++i)
				StartingAges.Add(buffer.ReadInteger());

			return this;
		}

		/// <summary>
		/// Writes the data element into the given buffer (at its current position).
		/// </summary>
		/// <param name="buffer">The buffer where the data element should be deserialized into.</param>
		public void WriteData(RAMBuffer buffer)
		{
			ScenarioDataElementTools.AssertListLength(DisabledTechsPerPlayer, 16);
			DisabledTechsPerPlayer.ForEach(p => buffer.WriteInteger(p.Count));
			DisabledTechsPerPlayer.ForEach(p =>
			{
				p.ForEach(e => buffer.WriteUInteger(e));
				buffer.Write(new byte[4 * (30 - p.Count)]);
			});

			ScenarioDataElementTools.AssertListLength(DisabledUnitsPerPlayer, 16);
			DisabledUnitsPerPlayer.ForEach(p => buffer.WriteInteger(p.Count));
			DisabledUnitsPerPlayer.ForEach(p =>
			{
				p.ForEach(e => buffer.WriteUInteger(e));
				buffer.Write(new byte[4 * (30 - p.Count)]);
			});

			ScenarioDataElementTools.AssertListLength(DisabledBuildingsPerPlayer, 16);
			DisabledBuildingsPerPlayer.ForEach(p => buffer.WriteInteger(p.Count));
			DisabledBuildingsPerPlayer.ForEach(p =>
			{
				p.ForEach(e => buffer.WriteUInteger(e));
				buffer.Write(new byte[4 * (20 - p.Count)]);
			});

			buffer.WriteUInteger(Unused1);
			buffer.WriteUInteger(Unused2);
			buffer.WriteUInteger(FullTechMode);

			ScenarioDataElementTools.AssertListLength(StartingAges, 16);
			StartingAges.ForEach(a => buffer.WriteInteger(a));
		}

		#endregion
	}
}