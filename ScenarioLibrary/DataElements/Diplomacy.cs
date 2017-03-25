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
	public class Diplomacy
	{
		#region Fields

		/// <summary>
		/// The stances to all players. Length: 16 entries.
		/// </summary>
		public List<StancesToPlayers> StancesPerPlayer { get; set; }

		/// <summary>
		/// The allied victory setting per player. Obsolete. Length: 16 entries.
		/// </summary>
		public List<uint> AlliedVictoryObsolete { get; set; }

		#endregion

		#region Functions

		/// <summary>
		/// Reads the data element from the given buffer.
		/// </summary>
		/// <param name="buffer">The buffer where the data element should be deserialized from.</param>
		public Diplomacy ReadData(RAMBuffer buffer)
		{
			StancesPerPlayer = new List<StancesToPlayers>(16);
			for(int i = 0; i < 16; ++i)
				StancesPerPlayer.Add(new StancesToPlayers().ReadData(buffer));

			// Separator
			buffer.ReadByteArray(11520);
			ScenarioDataElementTools.AssertTrue(buffer.ReadUInteger() == 0xFFFFFF9D);

			AlliedVictoryObsolete = new List<uint>(16);
			for(int i = 0; i < 16; ++i)
				AlliedVictoryObsolete.Add(buffer.ReadUInteger());

			return this;
		}

		/// <summary>
		/// Writes the data element into the given buffer (at its current position).
		/// </summary>
		/// <param name="buffer">The buffer where the data element should be deserialized into.</param>
		public void WriteData(RAMBuffer buffer)
		{
			ScenarioDataElementTools.AssertListLength(StancesPerPlayer, 16);
			StancesPerPlayer.ForEach(s => s.WriteData(buffer));

			buffer.Write(new byte[11520]);
			buffer.WriteUInteger(0xFFFFFF9D);

			ScenarioDataElementTools.AssertListLength(AlliedVictoryObsolete, 16);
			AlliedVictoryObsolete.ForEach(av => buffer.WriteUInteger(av));
		}

		#endregion

		#region Sub types

		/// <summary>
		/// Contains the stances of a specific player to all other players.
		/// </summary>
		public class StancesToPlayers
		{
			#region Fields

			/// <summary>
			/// The stances to all players. Length: 16 entries.
			/// </summary>
			public List<uint> Stances { get; set; }

			#endregion

			#region Functions

			/// <summary>
			/// Reads the data element from the given buffer.
			/// </summary>
			/// <param name="buffer">The buffer where the data element should be deserialized from.</param>
			public StancesToPlayers ReadData(RAMBuffer buffer)
			{
				Stances = new List<uint>(16);
				for(int i = 0; i < 16; ++i)
					Stances.Add(buffer.ReadUInteger());

				return this;
			}

			/// <summary>
			/// Writes the data element into the given buffer (at its current position).
			/// </summary>
			/// <param name="buffer">The buffer where the data element should be deserialized into.</param>
			public void WriteData(RAMBuffer buffer)
			{
				ScenarioDataElementTools.AssertListLength(Stances, 16);
				Stances.ForEach(s => buffer.WriteUInteger(s));
			}

			#endregion
		}

		#endregion
	}
}