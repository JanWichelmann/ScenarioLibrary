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
	/// Contains the scenario triggers.
	/// </summary>
	public class Triggers
	{
		#region Fields

		/// <summary>
		/// Unknown. Always 0?
		/// </summary>
		public byte Unknown { get; set; }

		/// <summary>
		/// The triggers.
		/// </summary>
		public List<Trigger> TriggerData { get; set; }

		/// <summary>
		/// The trigger display indices. Should have the same length as TriggerData.
		/// </summary>
		public List<uint> TriggerDisplayIndices { get; set; }

		#endregion

		#region Functions

		/// <summary>
		/// Reads the data element from the given buffer.
		/// </summary>
		/// <param name="buffer">The buffer where the data element should be deserialized from.</param>
		public Triggers ReadData(RAMBuffer buffer)
		{
			Unknown = buffer.ReadByte();

			int triggerCount = buffer.ReadInteger();
			TriggerData = new List<Trigger>(triggerCount);
			for(int i = 0; i < triggerCount; i++)
				TriggerData.Add(new Trigger().ReadData(buffer));
			TriggerDisplayIndices = new List<uint>(triggerCount);
			for(int i = 0; i < triggerCount; i++)
				TriggerDisplayIndices.Add(buffer.ReadUInteger());

			return this;
		}

		/// <summary>
		/// Writes the data element into the given buffer (at its current position).
		/// </summary>
		/// <param name="buffer">The buffer where the data element should be deserialized into.</param>
		public void WriteData(RAMBuffer buffer)
		{
			buffer.WriteByte(Unknown);

			ScenarioDataElementTools.AssertTrue(TriggerData.Count == TriggerDisplayIndices.Count);
			buffer.WriteInteger(TriggerData.Count);
			TriggerData.ForEach(t => t.WriteData(buffer));
			TriggerDisplayIndices.ForEach(t => buffer.WriteUInteger(t));
		}

		#endregion

		#region Sub types

		/// <summary>
		/// Contains the data of one trigger.
		/// </summary>
		public class Trigger
		{
			#region Fields
			
			public uint Enabled { get; set; }
			public uint Looping { get; set; }
			public byte Unknown1 { get; set; }
			public byte ShowAsObjective { get; set; }
			public int ObjectiveDescriptionIndex { get; set; }
			public uint Unknown2 { get; set; }
			public string Description { get; set; }
			public string Name { get; set; }
			public List<Effect> Effects { get; set; }
			public List<int> EffectDisplayIndices { get; set; }
			public List<Condition> Conditions { get; set; }
			public List<int> ConditionDisplayIndices { get; set; }

			#endregion

			#region Functions

			/// <summary>
			/// Reads the data element from the given buffer.
			/// </summary>
			/// <param name="buffer">The buffer where the data element should be deserialized from.</param>
			public Trigger ReadData(RAMBuffer buffer)
			{
				Enabled = buffer.ReadUInteger();
				Looping = buffer.ReadUInteger();
				Unknown1 = buffer.ReadByte();
				ShowAsObjective = buffer.ReadByte();
				ObjectiveDescriptionIndex = buffer.ReadInteger();
				Unknown2 = buffer.ReadUInteger();

				Description = buffer.ReadString(buffer.ReadInteger());
				Name = buffer.ReadString(buffer.ReadInteger());

				int effectCount = buffer.ReadInteger();
				Effects = new List<Effect>(effectCount);
				for(int i = 0; i < effectCount; i++)
					Effects.Add(new Effect().ReadData(buffer));
				EffectDisplayIndices = new List<int>(effectCount);
				for(int i = 0; i < effectCount; i++)
					EffectDisplayIndices.Add(buffer.ReadInteger());

				int conditionCount = buffer.ReadInteger();
				Conditions = new List<Condition>(conditionCount);
				for(int i = 0; i < conditionCount; i++)
					Conditions.Add(new Condition().ReadData(buffer));
				ConditionDisplayIndices = new List<int>(conditionCount);
				for(int i = 0; i < conditionCount; i++)
					ConditionDisplayIndices.Add(buffer.ReadInteger());

				return this;
			}

			/// <summary>
			/// Writes the data element into the given buffer (at its current position).
			/// </summary>
			/// <param name="buffer">The buffer where the data element should be deserialized into.</param>
			public void WriteData(RAMBuffer buffer)
			{
				buffer.WriteUInteger(Enabled);
				buffer.WriteUInteger(Looping);
				
				buffer.WriteByte(Unknown1);
				buffer.WriteByte(ShowAsObjective);
				buffer.WriteInteger(ObjectiveDescriptionIndex);
				buffer.WriteUInteger(Unknown2);

				buffer.WriteInteger(Description.Length);
				buffer.WriteString(Description);
				buffer.WriteInteger(Name.Length);
				buffer.WriteString(Name);

				ScenarioDataElementTools.AssertTrue(Effects.Count == EffectDisplayIndices.Count);
				buffer.WriteInteger(Effects.Count);
				Effects.ForEach(t => t.WriteData(buffer));
				EffectDisplayIndices.ForEach(t => buffer.WriteInteger(t));

				ScenarioDataElementTools.AssertTrue(Conditions.Count == ConditionDisplayIndices.Count);
				buffer.WriteInteger(Conditions.Count);
				Conditions.ForEach(t => t.WriteData(buffer));
				ConditionDisplayIndices.ForEach(t => buffer.WriteInteger(t));
			}

			#endregion
		}

		/// <summary>
		/// Contains the data of one trigger condition.
		/// </summary>
		public class Condition
		{
			#region Fields

			public ConditionTypes Type { get; set; }
			public uint Amount { get; set; }
			public uint ResourceId { get; set; }
			public uint UnitInstanceId { get; set; }
			public uint UnitLocation { get; set; }
			public uint UnitType { get; set; }
			public uint Player { get; set; }
			public uint ResearchId { get; set; }
			public uint Timer { get; set; }
			public uint Unknown { get; set; }
			public uint AreaBottomLeftX { get; set; }
			public uint AreaBottomLeftY { get; set; }
			public uint AreaTopRightX { get; set; }
			public uint AreaTopRightY { get; set; }
			public uint UnitClass { get; set; }
			public uint UnitType2 { get; set; }
			public uint AiSignal { get; set; }

			#endregion

			#region Functions

			/// <summary>
			/// Reads the data element from the given buffer.
			/// </summary>
			/// <param name="buffer">The buffer where the data element should be deserialized from.</param>
			public Condition ReadData(RAMBuffer buffer)
			{
				Type = (ConditionTypes)buffer.ReadUInteger();

				ScenarioDataElementTools.AssertTrue(buffer.ReadUInteger() == 0x00000010);

				Amount = buffer.ReadUInteger();
				ResourceId = buffer.ReadUInteger();
				UnitInstanceId = buffer.ReadUInteger();
				UnitLocation = buffer.ReadUInteger();
				UnitType = buffer.ReadUInteger();
				Player = buffer.ReadUInteger();
				ResearchId = buffer.ReadUInteger();
				Timer = buffer.ReadUInteger();
				Unknown = buffer.ReadUInteger();
				AreaBottomLeftX = buffer.ReadUInteger();
				AreaBottomLeftY = buffer.ReadUInteger();
				AreaTopRightX = buffer.ReadUInteger();
				AreaTopRightY = buffer.ReadUInteger();
				UnitClass = buffer.ReadUInteger();
				UnitType2 = buffer.ReadUInteger();
				AiSignal = buffer.ReadUInteger();

				return this;
			}

			/// <summary>
			/// Writes the data element into the given buffer (at its current position).
			/// </summary>
			/// <param name="buffer">The buffer where the data element should be deserialized into.</param>
			public void WriteData(RAMBuffer buffer)
			{
				buffer.WriteUInteger((uint)Type);

				buffer.WriteUInteger(0x00000010);

				buffer.WriteUInteger(Amount);
				buffer.WriteUInteger(ResourceId);
				buffer.WriteUInteger(UnitInstanceId);
				buffer.WriteUInteger(UnitLocation);
				buffer.WriteUInteger(UnitType);
				buffer.WriteUInteger(Player);
				buffer.WriteUInteger(ResearchId);
				buffer.WriteUInteger(Timer);
				buffer.WriteUInteger(Unknown);
				buffer.WriteUInteger(AreaBottomLeftX);
				buffer.WriteUInteger(AreaBottomLeftY);
				buffer.WriteUInteger(AreaTopRightX);
				buffer.WriteUInteger(AreaTopRightY);
				buffer.WriteUInteger(UnitClass);
				buffer.WriteUInteger(UnitType2);
				buffer.WriteUInteger(AiSignal);
			}

			#endregion

			#region Sub types

			/// <summary>
			/// The various condition types.
			/// </summary>
			public enum ConditionTypes : uint
			{
				BringObjectToArea = 1,
				BringObjectToObject = 2,
				OwnObjects = 3,
				OwnFewerObjects = 4,
				ObjectsInArea = 5,
				DestroyObject = 6,
				CaptureObject = 7,
				AccumulateAttribute = 8,
				ResearchTehcnology = 9,
				Timer = 10,
				ObjectSelected = 11,
				AiSignal = 12,
				PlayerDefeated = 13,
				ObjectHasTarget = 14,
				ObjectVisible = 15,
				ObjectNotVisible = 16,
				ResearchingTechnology = 17,
				UnitsGarrisoned = 18,
				DifficultyLevel = 19,
				OwnFewerFoundations = 20,
				SelectedObjectsInArea = 21,
				PoweredObjectsInArea = 22,
				UnitsQueuedPastPopCap = 23
			}

			#endregion
		}

		/// <summary>
		/// Contains the data of one trigger effect.
		/// </summary>
		public class Effect
		{
			#region Fields

			public EffectTypes Type { get; set; }
			public int AiGoal { get; set; }
			public int Amount { get; set; }
			public int ResourceId { get; set; }
			public int Diplomacy { get; set; }
			public int LocationUnitInstanceId { get; set; }
			public int UnitType { get; set; }
			public int PlayerSource { get; set; }
			public int PlayerTarget { get; set; }
			public int ResearchId { get; set; }
			public int StringId { get; set; }
			public int Unknown { get; set; }
			public int DisplayTime { get; set; }
			public int TriggerIndex { get; set; }
			public int LocationX { get; set; }
			public int LocationY { get; set; }
			public int AreaBottomLeftX { get; set; }
			public int AreaBottomLeftY { get; set; }
			public int AreaTopRightX { get; set; }
			public int AreaTopRightY { get; set; }
			public int UnitClass { get; set; }
			public int UnitType2 { get; set; }
			public int InstructionPanel { get; set; }
			public string Text { get; set; }
			public string SoundFileName { get; set; }
			public List<uint> SelectedUnitInstanceIds { get; set; }

			#endregion

			#region Functions

			/// <summary>
			/// Reads the data element from the given buffer.
			/// </summary>
			/// <param name="buffer">The buffer where the data element should be deserialized from.</param>
			public Effect ReadData(RAMBuffer buffer)
			{
				Type = (EffectTypes)buffer.ReadUInteger();

				ScenarioDataElementTools.AssertTrue(buffer.ReadUInteger() == 0x00000017);

				AiGoal = buffer.ReadInteger();
				Amount = buffer.ReadInteger();
				ResourceId = buffer.ReadInteger();
				Diplomacy = buffer.ReadInteger();

				int selectedUnitsCount = buffer.ReadInteger();
				if(selectedUnitsCount < 0)
					selectedUnitsCount = 0;

				LocationUnitInstanceId = buffer.ReadInteger();
				UnitType = buffer.ReadInteger();
				PlayerSource = buffer.ReadInteger();
				PlayerTarget = buffer.ReadInteger();
				ResearchId = buffer.ReadInteger();
				StringId = buffer.ReadInteger();
				Unknown = buffer.ReadInteger();
				DisplayTime= buffer.ReadInteger();
				TriggerIndex = buffer.ReadInteger();
				LocationX = buffer.ReadInteger();
				LocationY = buffer.ReadInteger();
				AreaBottomLeftX = buffer.ReadInteger();
				AreaBottomLeftY = buffer.ReadInteger();
				AreaTopRightX = buffer.ReadInteger();
				AreaTopRightY = buffer.ReadInteger();
				UnitClass = buffer.ReadInteger();
				UnitType2 = buffer.ReadInteger();
				InstructionPanel = buffer.ReadInteger();

				Text = buffer.ReadString(buffer.ReadInteger());
				SoundFileName = buffer.ReadString(buffer.ReadInteger());

				SelectedUnitInstanceIds = new List<uint>(selectedUnitsCount);
				for(int i = 0; i < selectedUnitsCount; i++)
					SelectedUnitInstanceIds.Add(buffer.ReadUInteger());

				return this;
			}

			/// <summary>
			/// Writes the data element into the given buffer (at its current position).
			/// </summary>
			/// <param name="buffer">The buffer where the data element should be deserialized into.</param>
			public void WriteData(RAMBuffer buffer)
			{
				buffer.WriteUInteger((uint)Type);

				buffer.WriteUInteger(0x00000017);

				buffer.WriteInteger(AiGoal);
				buffer.WriteInteger(Amount);
				buffer.WriteInteger(ResourceId);
				buffer.WriteInteger(Diplomacy);

				buffer.WriteInteger(SelectedUnitInstanceIds.Count);

				buffer.WriteInteger(LocationUnitInstanceId);
				buffer.WriteInteger(UnitType);
				buffer.WriteInteger(PlayerSource);
				buffer.WriteInteger(PlayerTarget);
				buffer.WriteInteger(ResearchId);
				buffer.WriteInteger(StringId);
				buffer.WriteInteger(Unknown);
				buffer.WriteInteger(DisplayTime);
				buffer.WriteInteger(TriggerIndex);
				buffer.WriteInteger(LocationX);
				buffer.WriteInteger(LocationY);
				buffer.WriteInteger(AreaBottomLeftX);
				buffer.WriteInteger(AreaBottomLeftY);
				buffer.WriteInteger(AreaTopRightX);
				buffer.WriteInteger(AreaTopRightY);
				buffer.WriteInteger(UnitClass);
				buffer.WriteInteger(UnitType2);
				buffer.WriteInteger(InstructionPanel);

				buffer.WriteInteger(Text.Length);
				buffer.WriteString(Text);
				buffer.WriteInteger(SoundFileName.Length);
				buffer.WriteString(SoundFileName);

				SelectedUnitInstanceIds.ForEach(u => buffer.WriteUInteger(u));
			}

			#endregion

			#region Sub types

			/// <summary>
			/// The various effect types.
			/// </summary>
			public enum EffectTypes : uint
			{
				Diplomacy = 1,
				ResearchTechnology = 2,
				SendChat = 3,
				PlaySound = 4,
				SendTribute = 5,
				UnlockGate = 6,
				LockGate = 7,
				ActivateTrigger = 8,
				DeactivateTrigger = 9,
				AiScriptGoal = 10,
				CreateObject = 11,
				TaskObject = 12,
				DeclareVictory = 13,
				KillObject = 14,
				RemoveObject = 15,
				ChangeView = 16,
				Unload = 17,
				ChangeOwnership = 18,
				Patrol = 19,
				DisplayInstructions = 20,
				ClearInstructions = 21,
				FreezeUnit = 22,
				UseAdvancedButtons = 23,
				DamageObject = 24,
				PlaceFoundation = 25,
				ChangeObjectName = 26,
				ChangeObjectHitpoints = 27,
				ChangeObjectAttack = 28,
				StopUnit = 29,
				SnapView = 30,
				Unknown = 31,
				EnableTech = 32,
				DisableTech = 33,
				EnableUnit = 34,
				DisableUnit = 35,
				FlashObjects = 36
			}

			#endregion
		}

		#endregion
	}
}