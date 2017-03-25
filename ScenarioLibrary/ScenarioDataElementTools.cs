using IORAMHelper;
using System;

namespace ScenarioLibrary
{
	/// <summary>
	/// Defines some auxiliary functions for scenario data elements.
	/// </summary>
	internal class ScenarioDataElementTools
	{
		#region Auxiliary functions

		/// <summary>
		/// Asserts that the given list has the given expected length, else an exception is raised.
		/// </summary>
		/// <param name="list">The list which length shall be checked.</param>
		/// <param name="length">The expected list length.</param>
		public static void AssertListLength(System.Collections.ICollection list, int length)
		{
			// Compare lengths
			if(list.Count != length)
				throw new AssertionException($"The list length ({list.Count}) does not equal the expected length ({length}).");
		}

		/// <summary>
		/// Asserts that the given boolean expression is true, else an exception is raised.
		/// </summary>
		/// <param name="value">The value that should be true.</param>
		public static void AssertTrue(bool value)
		{
			// Check value
			if(!value)
				throw new AssertionException("The expression evaluated to false.");
		}

		#endregion

		#region Exceptions

		/// <summary>
		/// Exception that is raised when an assertion fails.
		/// </summary>
		public class AssertionException : Exception
		{
			/// <summary>
			/// Raises a new AssertionException.
			/// </summary>
			public AssertionException()
				: base("Assertion failed.")
			{
			}

			/// <summary>
			/// Raises a new AssertionException with the given error message.
			/// </summary>
			/// <param name="message">The error message.</param>
			public AssertionException(string message)
				: base(message)
			{
			}
		}

		#endregion
	}
}