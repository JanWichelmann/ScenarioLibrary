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
		public List<int> TriggerDisplayIndices { get; set; }

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
			TriggerDisplayIndices = new List<int>(triggerCount);
			for(int i = 0; i < triggerCount; i++)
				TriggerDisplayIndices.Add(buffer.ReadInteger());

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
			TriggerDisplayIndices.ForEach(t => buffer.WriteInteger(t));
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
			public byte State { get; set; }
			public byte ShowAsObjective { get; set; }
			public int ObjectiveDescriptionIndex { get; set; }
			public uint Unknown { get; set; }
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
				State = buffer.ReadByte();
				ShowAsObjective = buffer.ReadByte();
				ObjectiveDescriptionIndex = buffer.ReadInteger();
				Unknown = buffer.ReadUInteger();

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

				buffer.WriteByte(State);
				buffer.WriteByte(ShowAsObjective);
				buffer.WriteInteger(ObjectiveDescriptionIndex);
				buffer.WriteUInteger(Unknown);

				if(Description.Length == 0 || Description.Last() != '\0')
					Description += '\0';
				buffer.WriteInteger(Description.Length);
				buffer.WriteString(Description);

				if(Name.Length == 0 || Name.Last() != '\0')
					Name += '\0';
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

			#region Static auxiliary functions

			public static Trigger CreateNew(string name, string description, bool enabled, bool loop, bool showAsObjective, int objectiveIndex)
			{
				return new Trigger()
				{
					ConditionDisplayIndices = new List<int>(),
					Conditions = new List<Condition>(),
					Description = description,
					EffectDisplayIndices = new List<int>(),
					Effects = new List<Effect>(),
					Enabled = (uint)(enabled ? 1 : 0),
					Looping = (uint)(loop ? 1 : 0),
					Name = name,
					ObjectiveDescriptionIndex = objectiveIndex,
					ShowAsObjective = (byte)(showAsObjective ? 1 : 0),
					State = 0,
					Unknown = 0
				};
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
			public int Amount { get; set; }
			public int ResourceId { get; set; }
			public int UnitInstanceId { get; set; }
			public int UnitLocation { get; set; }
			public int UnitType { get; set; }
			public int Player { get; set; }
			public int ResearchId { get; set; }
			public int Timer { get; set; }
			public int Unknown { get; set; }
			public int AreaBottomLeftX { get; set; }
			public int AreaBottomLeftY { get; set; }
			public int AreaTopRightX { get; set; }
			public int AreaTopRightY { get; set; }
			public int UnitClass { get; set; }
			public int UnitType2 { get; set; }
			public int AiSignal { get; set; }

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

				Amount = buffer.ReadInteger();
				ResourceId = buffer.ReadInteger();
				UnitInstanceId = buffer.ReadInteger();
				UnitLocation = buffer.ReadInteger();
				UnitType = buffer.ReadInteger();
				Player = buffer.ReadInteger();
				ResearchId = buffer.ReadInteger();
				Timer = buffer.ReadInteger();
				Unknown = buffer.ReadInteger();
				AreaBottomLeftX = buffer.ReadInteger();
				AreaBottomLeftY = buffer.ReadInteger();
				AreaTopRightX = buffer.ReadInteger();
				AreaTopRightY = buffer.ReadInteger();
				UnitClass = buffer.ReadInteger();
				UnitType2 = buffer.ReadInteger();
				AiSignal = buffer.ReadInteger();

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

				buffer.WriteInteger(Amount);
				buffer.WriteInteger(ResourceId);
				buffer.WriteInteger(UnitInstanceId);
				buffer.WriteInteger(UnitLocation);
				buffer.WriteInteger(UnitType);
				buffer.WriteInteger(Player);
				buffer.WriteInteger(ResearchId);
				buffer.WriteInteger(Timer);
				buffer.WriteInteger(Unknown);
				buffer.WriteInteger(AreaBottomLeftX);
				buffer.WriteInteger(AreaBottomLeftY);
				buffer.WriteInteger(AreaTopRightX);
				buffer.WriteInteger(AreaTopRightY);
				buffer.WriteInteger(UnitClass);
				buffer.WriteInteger(UnitType2);
				buffer.WriteInteger(AiSignal);
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
				ResearchTechnology = 9,
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

			#region Static auxiliary functions

			public static Condition CreateTimerCondition(int timer)
			{
				return new Condition()
				{
					AiSignal = -1,
					Amount = -1,
					AreaBottomLeftX = -1,
					AreaBottomLeftY = -1,
					AreaTopRightX = -1,
					AreaTopRightY = -1,
					Player = -1,
					ResearchId = -1,
					ResourceId = -1,
					Timer = timer,
					Type = ConditionTypes.Timer,
					UnitClass = -1,
					UnitInstanceId = -1,
					UnitLocation = -1,
					UnitType = -1,
					UnitType2 = -1,
					Unknown = -1
				};
			}

			public static Condition CreateDestroyObjectCondition(uint unitInstanceId, int player)
			{
				return new Condition()
				{
					AiSignal = -1,
					Amount = -1,
					AreaBottomLeftX = -1,
					AreaBottomLeftY = -1,
					AreaTopRightX = -1,
					AreaTopRightY = -1,
					Player = player,
					ResearchId = -1,
					ResourceId = -1,
					Timer = -1,
					Type = ConditionTypes.DestroyObject,
					UnitClass = -1,
					UnitInstanceId = (int)unitInstanceId,
					UnitLocation = -1,
					UnitType = -1,
					UnitType2 = -1,
					Unknown = -1
				};
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
			public int SoundId { get; set; }
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
				SoundId = buffer.ReadInteger();
				DisplayTime = buffer.ReadInteger();
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
				buffer.WriteInteger(SoundId);
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

				if(Text.Length == 0 || Text.Last() != '\0')
					Text += '\0';
				buffer.WriteInteger(Text.Length);
				buffer.WriteString(Text);

				if(SoundFileName.Length == 0 || SoundFileName.Last() != '\0')
					SoundFileName += '\0';
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

			#region Static auxiliary functions

			public static Effect CreateRenamingEffect(uint unitInstanceId, string newName)
			{
				return new Effect()
				{
					AiGoal = -1,
					Amount = -1,
					AreaBottomLeftX = -1,
					AreaBottomLeftY = -1,
					AreaTopRightX = -1,
					AreaTopRightY = -1,
					Diplomacy = -1,
					DisplayTime = -1,
					InstructionPanel = -1,
					LocationUnitInstanceId = -1,
					LocationX = -1,
					LocationY = -1,
					PlayerSource = -1,
					PlayerTarget = -1,
					ResearchId = -1,
					ResourceId = -1,
					SelectedUnitInstanceIds = new List<uint>(new uint[] { unitInstanceId }),
					SoundFileName = "",
					StringId = -1,
					Text = newName,
					TriggerIndex = -1,
					Type = EffectTypes.ChangeObjectName,
					UnitClass = -1,
					UnitType = -1,
					UnitType2 = -1,
					SoundId = -1
				};
			}

			public static Effect CreateTaskObjectEffect(uint unitInstanceId, int targetUnitInstanceId, int player)
			{
				return new Effect()
				{
					AiGoal = -1,
					Amount = -1,
					AreaBottomLeftX = -1,
					AreaBottomLeftY = -1,
					AreaTopRightX = -1,
					AreaTopRightY = -1,
					Diplomacy = -1,
					DisplayTime = -1,
					InstructionPanel = -1,
					LocationUnitInstanceId = targetUnitInstanceId,
					LocationX = -1,
					LocationY = -1,
					PlayerSource = player,
					PlayerTarget = -1,
					ResearchId = -1,
					ResourceId = -1,
					SelectedUnitInstanceIds = new List<uint>(new uint[] { unitInstanceId }),
					SoundFileName = "",
					StringId = -1,
					Text = "",
					TriggerIndex = -1,
					Type = EffectTypes.TaskObject,
					UnitClass = -1,
					UnitType = -1,
					UnitType2 = -1,
					SoundId = -1
				};
			}

			public static Effect CreateActivateTriggerEffect(int triggerIndex)
			{
				return new Effect()
				{
					AiGoal = -1,
					Amount = -1,
					AreaBottomLeftX = -1,
					AreaBottomLeftY = -1,
					AreaTopRightX = -1,
					AreaTopRightY = -1,
					Diplomacy = -1,
					DisplayTime = -1,
					InstructionPanel = -1,
					LocationUnitInstanceId = -1,
					LocationX = -1,
					LocationY = -1,
					PlayerSource = -1,
					PlayerTarget = -1,
					ResearchId = -1,
					ResourceId = -1,
					SelectedUnitInstanceIds = new List<uint>(),
					SoundFileName = "",
					StringId = -1,
					Text = "",
					TriggerIndex = triggerIndex,
					Type = EffectTypes.ActivateTrigger,
					UnitClass = -1,
					UnitType = -1,
					UnitType2 = -1,
					SoundId = -1
				};
			}

			public static Effect CreateResearchTechnologyEffect(int researchId, int player)
			{
				return new Effect()
				{
					AiGoal = -1,
					Amount = -1,
					AreaBottomLeftX = -1,
					AreaBottomLeftY = -1,
					AreaTopRightX = -1,
					AreaTopRightY = -1,
					Diplomacy = -1,
					DisplayTime = -1,
					InstructionPanel = -1,
					LocationUnitInstanceId = -1,
					LocationX = -1,
					LocationY = -1,
					PlayerSource = player,
					PlayerTarget = -1,
					ResearchId = researchId,
					ResourceId = -1,
					SelectedUnitInstanceIds = new List<uint>(),
					SoundFileName = "",
					StringId = -1,
					Text = "",
					TriggerIndex = -1,
					Type = EffectTypes.ResearchTechnology,
					UnitClass = -1,
					UnitType = -1,
					UnitType2 = -1,
					SoundId = -1
				};
			}

			#endregion
		}

		#endregion
	}
}