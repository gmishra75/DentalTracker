using System;

namespace C_DentalClaimTracker
{
	/// <summary>
	/// Summary description for ReservedKey.
	/// </summary>
	public class ReservedKey
	{
		private string _tableName;		
		private int _keyValue;
		
		public ReservedKey()
		{
			// Default constructor
			_tableName = "";			
			_keyValue = 0;			
		}

		public ReservedKey(string tableName, int keyValue)
		{
			_tableName = tableName;			
			_keyValue = keyValue;
		}

		// Properties
		
		public int Value
		{
			get { return _keyValue; }
		}
		
		public string TableName
		{
			get { return _tableName; }
		}

		// Operators
		public static bool operator==(ReservedKey x, ReservedKey y)
		{
			if (x.Value == y.Value)
			{
				return x._tableName.ToLower() == y._tableName.ToLower();				
			}
			return false;
		}
		
		public static bool operator!=(ReservedKey x, ReservedKey y)
		{			
			if (x.Value == y.Value)
			{
				return !(x._tableName.ToLower() == y._tableName.ToLower());
			}
			return true;
		}

		public override bool Equals(object o)
		{
			try 
			{
				return (bool) (this == (ReservedKey) o);
			}
			catch
			{
				return false;
			}
		}

		public override int GetHashCode()
		{
			return _keyValue;
		}

		
		public override string ToString()
		{
			return this._keyValue.ToString();
		}
		
		public static ReservedKey operator++(ReservedKey x)
		{
			x._keyValue++;
			return x;
		}

		// Methods

		/// <summary>
		/// Removes the reservation for this key if one exists.
		/// </summary>
		public void Release()
		{
			_keyValue = 0;
			_tableName = "";			
		}
	}

	public class ReservedKeyCollection : System.Collections.CollectionBase
	{
		public ReservedKeyCollection()
		{}

		public void Add (ReservedKey keyObject)
		{
			List.Add(keyObject);
		}

		public void Remove(int index)
		{				
			if (index > Count - 1 || index < 0)					
			{
				// Invalid index
				DataObjectException InvalidIndex = new DataObjectException("Invalid index.");
				throw InvalidIndex;
			}
			else
			{
				List.RemoveAt(index); 
			}
		}

		public ReservedKey this[int index]
		{
			get 
			{
				return (ReservedKey) List[index];				
			}
		}
	}
}
