/*	The Team: Data Objects
 *  Version: 1.0 Beta
 *  Author: Joey Mendoza
 *	Date: 04-26-03
 * 
 */
using System;
using System.Data;
using System.Data.OleDb;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using System.Collections.Generic;

namespace C_DentalClaimTracker
{
    /// <summary>
    /// Summary description for DataObject.
    /// </summary>
    public abstract class DataObject
    {
        private PrimaryFieldsCollection _primaryKeyFields;
        private string _tableName;
        private static ReservedKeyCollection _reservedKeys;
        private ConnectionAlias _connectionAlias;
        public static ConnectionHandler _connectionHandler;
        private static DataSet _dataSet;
        private DataTable _dataTable;
        private DataRow _dataRow;

        private OleDbCommand _dbcmdData;
        private OleDbDataAdapter _dbadpData;
        public event RecordChangedEventHandler RecordChanged;
        private SearchTypes _dataSearchType;
        private string _searchSQL;

        public delegate void RecordChangedEventHandler(object source, RecordChangedEventArgs e);

        public class PrimaryField
        {
            private string _fieldName;

            public PrimaryField()
            {
                _fieldName = "";
            }

            public PrimaryField(string fieldName)
            {
                _fieldName = fieldName;
            }

            public string Name
            {
                get { return _fieldName; }
                set { _fieldName = value; }
            }
        }

        public class PrimaryFieldsCollection : CollectionBase
        {
            public PrimaryFieldsCollection()
            { }

            public void Add(PrimaryField primaryFieldObject)
            {
                List.Add(primaryFieldObject);
            }

            public void Add(string primaryFieldName)
            {
                PrimaryField newItem = new PrimaryField(primaryFieldName);
                List.Add(newItem);
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

            public PrimaryField this[int index]
            {
                get
                {
                    if (index > Count - 1 || index < 0)
                        return null;
                    return (PrimaryField)List[index];
                }
                set
                {
                    if (index > Count - 1 || index < 0)
                    {
                        // Invalid index						
                        throw new DataObjectException("Invalid primary key index.");
                    }
                    else
                    {
                        List[index] = value;
                    }
                }
            }

            public PrimaryField this[string fieldName]
            {
                get
                {
                    for (int i = 0; i < Count; i++)
                    {
                        PrimaryField x = (PrimaryField)List[i];
                        if (x.Name == fieldName)
                            return x;
                    }
                    return null;
                }
                set
                {
                    for (int i = 0; i < Count; i++)
                    {
                        PrimaryField x = (PrimaryField)List[i];
                        if (x.Name == fieldName)
                            x = value;
                    }
                }
            }
        }

        public class RecordChangedEventArgs : EventArgs
        {
            public RowAction Action;
            public RecordChangedEventArgs(RowAction action)
            {
                this.Action = action;
            }

        }

        public enum RowAction
        {
            MoveFirst,
            MoveNext,
            MovePrevious,
            MoveLast,
            Deleted,
            Loaded
        }

        public void DefaultInitialization(string Table)
        {
            if (_dataSet == null)
                _dataSet = new DataSet("DentalClaim");

            if (_dataSet.Tables[Table] == null)
            {
                _dataTable = new DataTable(Table);
                _dataSet.Tables.Add(_dataTable);
            }
            else
                _dataTable = _dataSet.Tables[Table];

            _primaryKeyFields = new PrimaryFieldsCollection();

            if (_connectionAlias == null)
            {
                _connectionAlias = new ConnectionAlias();
            }

            if (_connectionHandler == null)
            {
                _connectionHandler = new ConnectionHandler();
                _connectionHandler.AddConnection(_connectionAlias); // Persistent Connection
            }

            if (_reservedKeys == null)
                _reservedKeys = new ReservedKeyCollection();

            _dbcmdData = new OleDbCommand();
            _dbadpData = new OleDbDataAdapter(_dbcmdData);

            _searchSQL = "";
            _dataSearchType = SearchTypes.Exact;
            _dataRow = _dataTable.NewRow();
        }

        public DataObject(string PrimaryField, string Table)
        {

            DefaultInitialization(Table);

            PrimaryKeys.Add(PrimaryField);
            TableName = Table;
            Refresh();
        }

        public DataObject(string PrimaryField, string Table, int ID)
        {

            DefaultInitialization(Table);

            PrimaryKeys.Add(PrimaryField);
            TableName = Table;
            Refresh();
            Load(ID);
        }

        public DataObject(string[] PrimaryFields, string Table)
        {
            DefaultInitialization(Table);

            for (int i = 0; i < PrimaryFields.Length; i++)
                PrimaryKeys.Add(PrimaryFields[i]);

            TableName = Table;

            Refresh();
        }

        public DataObject(PrimaryFieldsCollection PrimaryFields, string Table)
        {
            DefaultInitialization(Table);

            for (int i = 0; i < PrimaryFields.Count; i++)
                PrimaryKeys.Add(PrimaryFields[i]);

            TableName = Table;

            Refresh();
        }

        public DataObject(string[] PrimaryFields, string Table, int[] IDs)
        {
            DefaultInitialization(Table);

            for (int i = 0; i < PrimaryFields.Length; i++)
                PrimaryKeys.Add(PrimaryFields[i]);

            TableName = Table;

            Refresh();
            Load(IDs);
        }

        public DataObject(PrimaryFieldsCollection PrimaryFields, string Table, int[] IDs)
        {
            DefaultInitialization(Table);

            for (int i = 0; i < PrimaryFields.Count; i++)
                PrimaryKeys.Add(PrimaryFields[i]);

            TableName = Table;

            Refresh();
            Load(IDs);
        }

        private ReservedKeyCollection ReservedKeys
        {
            get { return _reservedKeys; }
        }

        protected void ReleaseKey(ReservedKey key)
        {
            for (int i = 0; i < ReservedKeys.Count; i++)
            {
                if (ReservedKeys[i] == key)
                {
                    ReservedKeys[i].Release();
                    ReservedKeys.Remove(i);
                    break;
                }
            }
        }

        protected ReservedKey ReserveKey(ReservedKey reservation)
        {
            while (IsReserved(reservation))
            { reservation++; }

            ReservedKeys.Add(reservation);

            return reservation;
        }

        private bool IsReserved(ReservedKey reservation)
        {
            for (int i = 0; i < ReservedKeys.Count; i++)
            {
                if (reservation == ReservedKeys[i])
                    return true;
            }
            return false;
        }

        public enum SearchTypes
        {
            Exact,
            EndsWith,
            BeginsWith,
            Contains,
            Before,
            After,
            NotEqual
        }

        public SearchTypes SearchType
        {
            get { return _dataSearchType; }
            set { _dataSearchType = value; }
        }

        private ConnectionHandler ConnHandler
        {
            get { return DataObject._connectionHandler; }
        }

        private ConnectionAlias Alias
        {
            get { return _connectionAlias; }
            set { _connectionAlias = value; }
        }

        private void BuildSql()
        {
            DataCommand.CommandText = "SELECT * FROM " + _dataTable.TableName + " WHERE";

            for (int i = 0; i < _dataTable.Columns.Count; i++)
            {
                if ((!DataCommand.CommandText.EndsWith(" AND")) &&
                    (!DataCommand.CommandText.EndsWith(" WHERE")))
                {
                    DataCommand.CommandText += " AND";
                }

                if ((_dataTable.Columns[i].DataType == System.Type.GetType("System.Int32"))
                    || (_dataTable.Columns[i].DataType == System.Type.GetType("System.Byte"))
                    || (_dataTable.Columns[i].DataType == System.Type.GetType("System.Single"))
                    || (_dataTable.Columns[i].DataType == System.Type.GetType("System.Double"))
                    || (_dataTable.Columns[i].DataType == System.Type.GetType("System.Boolean"))
                    || (_dataTable.Columns[i].DataType == System.Type.GetType("System.Int64"))
                    || (_dataTable.Columns[i].DataType == System.Type.GetType("System.Decimal"))
                    || (_dataTable.Columns[i].DataType == System.Type.GetType("System.Int16")))

                {
                    // Number
                    if (_dataRow[i].ToString() != String.Empty)
                    {
                        DataCommand.CommandText += " [" + _dataTable.Columns[i].ColumnName +
                            "] = " + _dataRow[i].ToString();
                    }
                }
                else if (_dataTable.Columns[i].DataType == System.Type.GetType("System.String"))
                {
                    // Text
                    if (_dataRow[i].ToString() != String.Empty)
                    {

                        DataCommand.CommandText += " [" + _dataTable.Columns[i].ColumnName + "]";
                        if (_dataRow[i].ToString().IndexOf("'") > 0)
                        {
                            _dataRow[i] = _dataRow[i].ToString().Replace("'", "''");
                        }
                        switch (_dataSearchType)
                        {
                            case SearchTypes.BeginsWith:
                                DataCommand.CommandText += " LIKE '" + _dataRow[i].ToString() + "%'";
                                break;
                            case SearchTypes.EndsWith:
                                DataCommand.CommandText += " LIKE '%" + _dataRow[i].ToString() + "'";
                                break;
                            case SearchTypes.Contains:
                                DataCommand.CommandText += " LIKE '%" + _dataRow[i].ToString() + "%'";
                                break;
                            case SearchTypes.Exact:
                                DataCommand.CommandText += " LIKE '" + _dataRow[i].ToString() + "'";
                                break;
                        }
                    }
                }
                else if (_dataTable.Columns[i].DataType == System.Type.GetType("System.DateTime"))
                {
                    // Date / Time
                    if (_dataRow[i].ToString() != String.Empty)
                    {
                        DataCommand.CommandText += " DateDiff(\"d\", " + _dataTable.Columns[i].ColumnName + "," +
                            "'" + _dataRow[i].ToString() + "') = 0";
                    }
                }
                else
                {
                    // Unsupported Type
                    DataObjectException InvalidDataType = new DataObjectException("Unsupported data type. [" +
                        _dataTable.Columns[i].DataType.ToString() + "]");
                    throw InvalidDataType;
                }
            }
            if (DataCommand.CommandText.EndsWith("WHERE"))
                DataCommand.CommandText = DataCommand.CommandText.Substring(0, DataCommand.CommandText.Length - 6);
            if (DataCommand.CommandText.EndsWith(" AND"))
                DataCommand.CommandText = DataCommand.CommandText.Substring(0, DataCommand.CommandText.Length - 4);
        }

        public DataTable Search(string sqlStatement)
        {
            _searchSQL = sqlStatement;
            return Search();
        }

        public DataTable Search()
        {
            OleDbDataAdapter dbadpSearch = new OleDbDataAdapter(DataCommand);
            DataTable SearchResults = new DataTable();

            try
            {
                if (_searchSQL == String.Empty)
                    BuildSql();
                else
                {
                    _dbcmdData.CommandText = _searchSQL;
                    _searchSQL = "";
                }

                DataCommand.Connection = ConnHandler.RequestConnection(Alias, this);
                dbadpSearch.Fill(SearchResults);
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                ConnHandler.ReleaseConnection(this, DataCommand.Connection);
            }
            return SearchResults;
        }

        public string SearchSQL
        {
            get
            {
                BuildSql();
                return DataCommand.CommandText;
            }
        }

        public PrimaryFieldsCollection PrimaryKeys
        {
            get { return _primaryKeyFields; }
        }

        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }

        /// <summary>
        /// SqlCommand object associated with this object.
        /// </summary>
        public OleDbCommand DataCommand
        {
            get { return _dbcmdData; }
            set { _dbcmdData = value; }
        }

        public OleDbDataAdapter DataAdapter
        {
            get { return _dbadpData; }
            set { _dbadpData = value; }
        }

        public void ExecuteNonQuery(string sql)
        {
            DataCommand.Connection = ConnHandler.RequestConnection(Alias, this);

            DataCommand.CommandText = sql;
            DataCommand.ExecuteNonQuery();

            ConnHandler.ReleaseConnection(this, DataCommand.Connection);
        }

        /// <summary>
        /// The DataRow object that contains this object's current record.
        /// </summary>
        public DataRow Row
        {
            get
            {
                if (_dataRow == null)
                {
                    DataObjectException.NoCurrentRecordException NoRecordError =
                        new DataObjectException.NoCurrentRecordException();
                    throw NoRecordError;
                }
                else
                    return _dataRow;
            }
        }

        public object this[string columnName]
        {
            get
            {
                return Row[columnName];
            }
            set
            {
                if (value == null)
                    Row[columnName] = DBNull.Value;
                else
                    Row[columnName] = value;
            }
        }

        public object this[int columnIndex]
        {
            get
            {
                return Row[columnIndex];
            }
            set
            {
                Row[columnIndex] = value;
            }
        }

        /// <summary>
        /// Loads a data object using a primary key
        /// </summary>
        /// <param name="ID">Primary key value to load.</param>
        public void Load(int ID)
        {
            // Load a dataobject by it's Primary[0] ID

            if (PrimaryKeys.Count == 0)
            {
                DataObjectException ValidationError = new DataObjectException("Only one key was given for a multiple key object.");
                throw ValidationError;
            }

            if (TableName.Trim().Length == 0)
            {
                DataObjectException ValidationError = new DataObjectException("The object's table has not been set.");
                throw ValidationError;
            }

            try
            {
                DataCommand.Connection = ConnHandler.RequestConnection(Alias, this);

                DataCommand.CommandText = "SELECT * FROM " +
                    TableName + " WHERE " + PrimaryKeys[0].Name +
                    " = " + ID.ToString();

                DataAdapter.SelectCommand = _dbcmdData;
                int x = (int)DataAdapter.Fill(_dataTable);

                if (x == 0)
                {
                    // Record doesn't exist
                    _dataRow = null;
                    DataObjectException.InvalidRecordException PrimaryKeyError =
                        new DataObjectException.InvalidRecordException();
                    throw PrimaryKeyError;
                }
                else
                {
                    _dataRow = _dataTable.Rows[_dataTable.Rows.Count - 1];
                    if (RecordChanged != null)
                    {
                        RecordChangedEventArgs args = new RecordChangedEventArgs(RowAction.Loaded);
                        RecordChanged(this, args);
                    }
                }
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                ConnHandler.ReleaseConnection(this, DataCommand.Connection);
            }
        }

        /// <summary>
        /// Loads a data object using multiple primary keys
        /// </summary>
        /// <param name="IDs">Primary key array to load.</param>
        public void Load(int[] IDs)
        {
            // Load a dataobject by all of it's IDs

            if (PrimaryKeys.Count != IDs.Length)
            {
                DataObjectException ValidationError = new DataObjectException("The object's primary keys do match the load values given.");
                throw ValidationError;
            }

            if (TableName.Trim().Length == 0)
            {
                DataObjectException ValidationError = new DataObjectException("The object's table has not been set.");
                throw ValidationError;
            }

            try
            {
                DataCommand.Connection = ConnHandler.RequestConnection(Alias, this);

                DataCommand.CommandText = "SELECT * FROM " +
                    TableName + " WHERE";

                for (int i = 0; i < PrimaryKeys.Count; i++)
                {
                    if (i > 0)
                        DataCommand.CommandText += " AND";

                    DataCommand.CommandText += " " + PrimaryKeys[i].Name + " = " + IDs[i].ToString();
                }

                DataAdapter.SelectCommand = _dbcmdData;
                int x = (int)DataAdapter.Fill(_dataTable);

                if (x == 0)
                {
                    // Record doesn't exist
                    _dataRow = null;
                    DataObjectException.InvalidRecordException PrimaryKeyError =
                        new DataObjectException.InvalidRecordException();
                    throw PrimaryKeyError;
                }
                else
                {
                    _dataRow = _dataTable.Rows[_dataTable.Rows.Count - 1];
                    if (RecordChanged != null)
                    {
                        RecordChangedEventArgs args = new RecordChangedEventArgs(RowAction.Loaded);
                        RecordChanged(this, args);
                    }
                }
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                ConnHandler.ReleaseConnection(this, DataCommand.Connection);
            }
        }

        /// <summary>
        /// Loads a record from pre-populated data row.
        /// </summary>
        /// <param name="dataRow"></param>
        public void Load(DataRow dataRow)
        {
            try
            {
                for (int i = 0; i < dataRow.Table.Columns.Count; i++)
                {
                    try
                    {
                        this[dataRow.Table.Columns[i].ColumnName] = dataRow[i];
                    }
                    catch
                    {
                        // Debug.WriteLine("Invalid column in data source, ignoring. (" + dataRow.Table.Columns[i].ColumnName + ")");
                    }
                }
                if (_dataRow.RowState == DataRowState.Detached)
                    _dataTable.Rows.Add(_dataRow);

                if (RecordChanged != null)
                {
                    RecordChangedEventArgs args = new RecordChangedEventArgs(RowAction.Loaded);
                    RecordChanged(this, args);
                }
            }
            catch (Exception err)
            {
                throw new DataObjectException("Load from a prepopulated data row failed.", err);
            }
        }

        public DataTable GetAllData(int maxRecords)
        {
            DataTable AllRecords = new DataTable(TableName);

            try
            {
                if (maxRecords > 0)
                {
                    _dbcmdData.CommandText = "SELECT TOP " + maxRecords + " * FROM " + TableName +
                        " ORDER BY ";
                }
                else
                {
                    _dbcmdData.CommandText = "SELECT * FROM " + TableName +
                        " ORDER BY ";
                }

                for (int i = 0; i < PrimaryKeys.Count; i++)
                {
                    if (i > 0)
                        _dbcmdData.CommandText += ", ";
                    _dbcmdData.CommandText += PrimaryKeys[i].Name;
                }

                DataCommand.Connection = ConnHandler.RequestConnection(Alias, this);

                DataAdapter.Fill(AllRecords);

                return AllRecords;
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                ConnHandler.ReleaseConnection(this, DataCommand.Connection);
            }
        }

        public DataTable GetAllData()
        {
            return GetAllData(0);
        }

        /// <summary>
        /// Retrieves all data from the specified object
        /// </summary>
        /// <param name="orderBy">The field name you wish to order by</param>
        /// <returns></returns>
        public DataTable GetAllData(string orderBy)
        {
            DataTable AllRecords = new DataTable(TableName);

            try
            {
                _dbcmdData.CommandText = "SELECT * FROM " + TableName +
                    " ORDER BY " + orderBy;

                DataCommand.Connection = ConnHandler.RequestConnection(Alias, this);

                DataAdapter.Fill(AllRecords);
                return AllRecords;
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                ConnHandler.ReleaseConnection(this, DataCommand.Connection);
            }
        }

        /// <summary>
        /// Looks up the next incremental primary key value to use for a new record.
        /// </summary>
        /// <returns></returns>
        private ReservedKey GetNextKey()
        {
            object dbResult;
            int nextid = 0;
            ReservedKey keyReservation;

            try
            {
                DataCommand.Connection = ConnHandler.RequestConnection(Alias, this);
                DataCommand.CommandText = "SELECT MAX(" + PrimaryKeys[0].Name + ") FROM " + TableName;
                dbResult = DataCommand.ExecuteScalar();
                if (dbResult is int)
                    nextid = (int)dbResult;

                nextid++;

                keyReservation = ReserveKey(new ReservedKey(TableName, nextid));
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                ConnHandler.ReleaseConnection(this, DataCommand.Connection);
            }

            return keyReservation;
        }

        /// <summary>
        /// Saves the current record to the database.
        /// </summary>
        public virtual void Save()
        {
            string Sql;
            try
            {
                // Ensure the object is ready to be saved
                ValidateSave();

                ReservedKey savekey = null;

                
                if (_dataRow.RowState == DataRowState.Detached)
                {
                    // Build SQL Insert Statement
                    // Set All Auto Keys

                    if (PrimaryKeys.Count > 1)
                    {
                        for (int i = 0; i < PrimaryKeys.Count; i++)
                        {
                            if (_dataRow[PrimaryKeys[i].Name].ToString() == String.Empty)
                            {
                                // Error Primary Key Not Set
                                DataObjectException PrimaryKeyError = new DataObjectException("One or more required fields do not have valid values.");
                                throw PrimaryKeyError;
                            }
                        }
                    }
                    else
                    {
                        // Only a single primary key, check to see if it has already been assigned
                        if (_dataRow[PrimaryKeys[0].Name].ToString() == String.Empty)
                        {
                            // Autonumber
                            savekey = GetNextKey();
                            Row[PrimaryKeys[0].Name] = savekey.Value;
                        }
                        // Else key has already been assigned
                    }

                    _dataTable.Rows.Add(_dataRow);

                    Sql = "INSERT INTO " + TableName + "(";
                    // Column names
                    for (int i = 0; i < _dataTable.Columns.Count; i++)
                        Sql += "[" + _dataTable.Columns[i].ColumnName + "], ";
                    Sql = Sql.Substring(0, Sql.Length - 2) + ") VALUES(";
                    // Column values
                    for (int i = 0; i < _dataTable.Columns.Count; i++)
                    {
                        if (_dataTable.Columns[i].DataType == System.Type.GetType("System.DateTime"))
                        {
                            if (_dataRow[i].ToString() == String.Empty)
                                Sql += "null";
                            else
                            {
                                Sql += "'" + ((DateTime?)_dataRow[i]).Value.ToString("yyyy/MM/dd HH:mm:ss") + "'";
                            }
                        }
                        else if (_dataTable.Columns[i].DataType == System.Type.GetType("System.String"))
                            Sql += "'" + _dataRow[i].ToString().Replace("'", "''") + "'";
                        else if (_dataTable.Columns[i].DataType == System.Type.GetType("System.Int32"))
                        {
                            if (_dataRow[i].ToString() == String.Empty)
                                Sql += "null";
                            else
                                Sql += _dataRow[i].ToString();
                        }
                        else if ((_dataTable.Columns[i].DataType == System.Type.GetType("System.Single")) 
                            || (_dataTable.Columns[i].DataType == System.Type.GetType("System.Double"))
                            || (_dataTable.Columns[i].DataType == System.Type.GetType("System.Decimal"))
                       || (_dataTable.Columns[i].DataType == System.Type.GetType("System.Int64"))
                            || (_dataTable.Columns[i].DataType == System.Type.GetType("System.Int16")))
                        {
                            if (_dataRow[i].ToString() == String.Empty)
                                Sql += "null";
                            else
                                Sql += _dataRow[i].ToString();
                        }
                        else if (_dataTable.Columns[i].DataType == System.Type.GetType("System.Boolean"))
                        {
                            if ((bool)_dataRow[i] == true)
                                Sql += "1";
                            else
                                Sql += "0";
                        }
                        else
                        {
                            DataObjectException InvalidDataType = new DataObjectException("Unsupported data type. [" + _dataTable.Columns[i].DataType.ToString() + "]");
                            throw InvalidDataType;
                        }

                        if (i != _dataTable.Columns.Count - 1)
                            Sql += ", ";
                    }
                    Sql += ")";
                }
                else
                {
                    // Build SQL Update Statement
                    Sql = "UPDATE " + TableName + " SET ";

                    for (int i = 0; i < _dataTable.Columns.Count; i++)
                    {
                        if (!IsPrimaryKey(_dataTable.Columns[i].ColumnName))
                        {
                            Sql += "[" + _dataTable.Columns[i].ColumnName + "]" + " = ";

                            if (_dataTable.Columns[i].DataType == System.Type.GetType("System.DateTime"))
                            {
                                if (_dataRow[i].ToString() == String.Empty)
                                    Sql += "null";
                                else
                                {

                                    Sql += "'" + ((DateTime)_dataRow[i]).ToString("yyyy/MM/dd HH:mm:ss") + "'";
                                    
                                }
                            }
                            else if ((_dataTable.Columns[i].DataType == System.Type.GetType("System.Int32"))
                                || (_dataTable.Columns[i].DataType == System.Type.GetType("System.Decimal"))
                                || (_dataTable.Columns[i].DataType == System.Type.GetType("System.Double"))
                                || (_dataTable.Columns[i].DataType == System.Type.GetType("System.Int64"))
                                || (_dataTable.Columns[i].DataType == System.Type.GetType("System.Int16")))

                            {
                                if (_dataRow[i].ToString() == String.Empty)
                                    Sql += "null";
                                else
                                    Sql += _dataRow[i].ToString();
                            }
                            else if (_dataTable.Columns[i].DataType == System.Type.GetType("System.String"))
                                Sql += "'" + _dataRow[i].ToString().Replace("'", "''") + "'";
                            else if (_dataTable.Columns[i].DataType == System.Type.GetType("System.Boolean"))
                            {
                                if ((bool)_dataRow[i] == true)
                                    Sql += "1";
                                else
                                    Sql += "0";
                            }
                            else
                                Sql += _dataRow[i].ToString();
                            if (i != _dataTable.Columns.Count - 1)
                                Sql += ", ";
                        }
                    }

                    Sql += " WHERE";

                    string GetID = "0";
                    for (int i = 0; i < PrimaryKeys.Count; i++)
                    {
                        if (i > 0)
                            Sql += " AND";

                        if (_dataRow[PrimaryKeys[i].Name] is int)
                            GetID = ((int)_dataRow[PrimaryKeys[i].Name]).ToString();
                        else if (_dataRow[PrimaryKeys[i].Name] is string)
                            GetID = (string)_dataRow[PrimaryKeys[i].Name];
                        else
                        {
                            DataObjectException InvalidDataType = new DataObjectException("Unsupported data type. [" + _dataTable.Columns[i].DataType.ToString() + "]");
                            throw InvalidDataType;
                        }
                        Sql += " " + PrimaryKeys[i].Name + " = " + GetID;
                    }
                }

                DataCommand.CommandText = Sql;
                DataCommand.Connection = ConnHandler.RequestConnection(Alias, this);

                while (true) // Loop until Break for success
                {
                    try
                    {
                        DataCommand.ExecuteNonQuery();
                        break; // Save Successful
                    }
                    catch (OleDbException err)
                    {
                        if (err.ErrorCode == -2147217873) // Primary Key violation
                        {
                            // Primary Key that was used already existed
                            savekey++;
                            DataCommand.CommandText = DataCommand.CommandText.Replace("VALUES (" + (savekey.Value - 1) + ",", "VALUES (" + savekey.Value.ToString() + ",");
                        }
                        else
                        {
                            throw err;
                        }
                    }
                }
                if (Sql.StartsWith("INSERT INTO"))
                    ReleaseKey(savekey);
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                ConnHandler.ReleaseConnection(this, DataCommand.Connection);
            }
        }

        private bool IsPrimaryKey(string columnName)
        {
            for (int i = 0; i < PrimaryKeys.Count; i++)
            {
                if (columnName == PrimaryKeys[i].Name)
                    return true;
            }
            return false;
        }

        private void ValidateSave()
        {
            try
            {
                if (_dataTable == null)
                {
                    DataObjectException ValidationError = new DataObjectException("The object's data table has not been set.");
                    throw ValidationError;
                }

                for (int i = 0; i < PrimaryKeys.Count; i++)
                    if (_dataTable.Columns[PrimaryKeys[i].Name] == null)
                    {
                        DataObjectException ValidationError = new DataObjectException("The primary key field does not exist in the datatable.");
                        throw ValidationError;
                    }

                if (_dataRow == null)
                {
                    DataObjectException.NoCurrentRecordException ValidationError = new DataObjectException.NoCurrentRecordException();
                    throw ValidationError;
                }
            }
            catch (DataObjectException err)
            {
                throw err;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        /// <summary>
        /// CAUTION! Deletes every record in the table.
        /// </summary>
        public void Zap()
        {
            try
            {
                DataCommand.Connection = ConnHandler.RequestConnection(Alias, this);
                DataCommand.CommandTimeout = 120;
                DataCommand.CommandText = "TRUNCATE TABLE " + TableName;
                DataCommand.ExecuteNonQuery();
            }
            catch (Exception err)
            {
                throw new DataObjectException("Table Zap failed.", err);
            }
            finally
            {
                ConnHandler.ReleaseConnection(this, DataCommand.Connection);
            }
        }

        /// <summary>
        /// Deletes this record from the database. 
        /// </summary>
        public virtual void Delete()
        {
            try
            {
                // Ensure the object is ready to be deleted
                ValidateSave();

                string Sql;

                if (_dataRow == null)
                {
                    // No current row
                    DataObjectException.NoCurrentRecordException NoRecordError =
                        new DataObjectException.NoCurrentRecordException();
                    throw NoRecordError;
                }

                if (_dataRow.RowState == DataRowState.Detached)
                {
                    _dataRow = null;
                    return;
                }
                else
                {
                    Sql = "DELETE FROM " + TableName + " WHERE";

                    for (int i = 0; i < PrimaryKeys.Count; i++)
                    {
                        if (i > 0)
                            Sql += " AND";

                        if (_dataRow[PrimaryKeys[i].Name].ToString() != String.Empty)
                            Sql += " " + PrimaryKeys[i].Name + " = " + _dataRow[PrimaryKeys[i].Name].ToString();
                        else
                        {
                            DataObjectException PrimaryKeyError = new DataObjectException("One or more required fields do not have valid values.");
                            throw PrimaryKeyError;
                        }
                    }

                    DataCommand.Connection = ConnHandler.RequestConnection(Alias, this);
                    _dbcmdData.CommandText = Sql;
                    _dbcmdData.ExecuteNonQuery();
                    _dataRow = null;
                }
                if (RecordChanged != null)
                {
                    RecordChangedEventArgs args = new RecordChangedEventArgs(RowAction.Deleted);
                    RecordChanged(this, args);
                }
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                ConnHandler.ReleaseConnection(this, DataCommand.Connection);
            }
        }

        /// <summary>
        /// Delete the object with this ID from the database.
        /// </summary>
        public void Delete(int ID)
        {
            if (PrimaryKeys.Count > 1)
            {
                DataObjectException ValidationError = new DataObjectException("Only one key was given for a multiple key object.");
                throw ValidationError;
            }

            if (_dataRow != null)
            {
                if (_dataRow[PrimaryKeys[0].Name] is int)
                    if ((int)_dataRow[PrimaryKeys[0].Name] == ID)
                    {
                        Delete();
                        return;
                    }
            }

            try
            {
                string Sql;
                Sql = "DELETE FROM " + TableName + " WHERE " + PrimaryKeys[0].Name + " = " + ID.ToString();

                DataCommand.Connection = ConnHandler.RequestConnection(Alias, this);

                _dbcmdData.CommandText = Sql;
                int x = (int)_dbcmdData.ExecuteNonQuery();
                if (x == 0)
                {
                    // Record doesn't exist
                    _dataRow = null;
                    DataObjectException.InvalidRecordException PrimaryKeyError =
                        new DataObjectException.InvalidRecordException();
                    throw PrimaryKeyError;
                }
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                ConnHandler.ReleaseConnection(this, DataCommand.Connection);
            }
        }

        /// <summary>
        /// Delete the object with these IDs from the database.
        /// </summary>
        public void Delete(int[] IDs)
        {
            if (PrimaryKeys.Count != IDs.Length)
            {
                DataObjectException ValidationError = new DataObjectException("The object's primary keys do match the ID values given.");
                throw ValidationError;
            }

            if (_dataRow != null)
            {
                for (int i = 0; i < PrimaryKeys.Count; i++)
                {
                    if (_dataRow[PrimaryKeys[i].Name] is int)
                    {
                        if ((int)_dataRow[PrimaryKeys[i].Name] != IDs[i])
                        {
                            break;
                        }
                        else if (i == PrimaryKeys.Count - 1)
                        {
                            // The object trying to be deleted is this object
                            Delete(); // Delete self
                            return;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            try
            {
                string Sql;
                Sql = "DELETE FROM " + TableName + " WHERE";

                for (int i = 0; i < PrimaryKeys.Count; i++)
                {
                    if (i > 0)
                        Sql += " AND";

                    Sql += " " + PrimaryKeys[i].Name + " = " + IDs[i].ToString();
                }

                DataCommand.Connection = ConnHandler.RequestConnection(Alias, this);
                _dbcmdData.CommandText = Sql;
                int x = (int)_dbcmdData.ExecuteNonQuery();
                if (x == 0)
                {
                    // Record doesn't exist
                    _dataRow = null;
                    DataObjectException.InvalidRecordException PrimaryKeyError =
                        new DataObjectException.InvalidRecordException();
                    throw PrimaryKeyError;
                }
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                ConnHandler.ReleaseConnection(this, DataCommand.Connection);
            }
        }

        /// <summary>
        /// Reloads the data for this object from the database.
        /// </summary>
        public virtual void Refresh()
        {
            try
            {
                DataCommand.Connection = this.ConnHandler.RequestConnection(Alias, this);

                int GetID = -1;

                if (_dataRow == null)
                {
                    // No current record
                    DataObjectException.NoCurrentRecordException NoRecordError =
                        new DataObjectException.NoCurrentRecordException();
                    throw NoRecordError;
                }

                DataCommand.CommandText = "SELECT * FROM " +
                    TableName + " WHERE";

                for (int i = 0; i < PrimaryKeys.Count; i++)
                {
                    if (i > 0)
                        DataCommand.CommandText += " AND";

                    if (_dataTable.Columns.Count > 0)
                    {
                        if (_dataRow[PrimaryKeys[i].Name] is int)
                            GetID = (int)_dataRow[PrimaryKeys[i].Name];
                        else
                            GetID = -1;
                        DataCommand.CommandText += " " + PrimaryKeys[i].Name + " = " + GetID.ToString();
                    }
                    else
                    {
                        // This command will only return the table's structure, no actual records
                        DataCommand.CommandText += " " + PrimaryKeys[i].Name + " = -1";
                    }
                }


                if ((int)DataAdapter.Fill(_dataTable) > 0)
                {
                    // Existing record
                    _dataRow = _dataTable.Rows[_dataTable.Rows.Count - 1];
                }
                else
                {
                    // New record
                    _dataRow = _dataTable.NewRow();
                }
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                ConnHandler.ReleaseConnection(this, DataCommand.Connection);
            }
        }

        public object DateTimeDBNull(DateTime? value)
        {
            if (value == null)
                return System.DBNull.Value;
            else if (value.Value.Date == new DateTime(1900, 1, 1).Date)
                return System.DBNull.Value;
            else
                return value;
        }

        public DateTime? GetDateTimeNullable(object dt, bool returnNull)
        {
            if (dt is DateTime)
                return (DateTime)dt;
            else
            {
                try
                {
                    if (dt == DBNull.Value)
                        return null;
                    else
                        return Convert.ToDateTime(dt);
                }
                catch(Exception ex)
                {
                    if (returnNull)
                        return null;
                    else
                        throw ex;
                }
            }
        }

        public DateTime GetDateTime(object dt)
        {
            if (dt is DateTime)
                return (DateTime)dt;
            else
            {
                try
                {
                    return Convert.ToDateTime(dt);
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
        }
    }

    public class TheTeamException : ApplicationException
    {
        public TheTeamException()
            : base("An unknown error occured in the application.")
        { }
        public TheTeamException(string message)
            : base(message)
        { }
        public TheTeamException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }

    public class DataObjectException : TheTeamException
    {
        public DataObjectException()
            : base("An unknown error occured in the data object.")
        { }
        public DataObjectException(string message)
            : base(message)
        { }
        public DataObjectException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public class NoCurrentRecordException : DataObjectException
        {
            public NoCurrentRecordException()
                : base("No current record.")
            { }
        }

        public class InvalidRecordException : DataObjectException
        {
            public InvalidRecordException()
                : base("Record does not exist.")
            { }
        }
    }


}
