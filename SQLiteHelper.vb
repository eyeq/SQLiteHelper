Imports System.Data
Imports System.Data.SQLite

Public Class SQLiteHelper

    Private connectionString As String

    Public Sub New(ByVal dbPath As String)
        Me.connectionString = "Data Source=" + dbPath
    End Sub

    Public Function ExecuteNonQuery(ByVal ParamArray command() As SQLiteCommand) As Integer
        Using connection = New SQLiteConnection(Me.connectionString)
            connection.Open()

            Using transaction = connection.BeginTransaction()
                Dim affectedRows As Integer
                For Each c In command
                    c.Connection = connection
                    c.Transaction = transaction

                    affectedRows += c.ExecuteNonQuery()
                Next

                transaction.Commit()
                Return affectedRows
            End Using
        End Using
    End Function

    Public Function ExecuteDataTable(ByVal command As SQLiteCommand) As DataTable
        Using connection = New SQLiteConnection(Me.connectionString)
            connection.Open()

            command.Connection = connection

            Dim adapter = New SQLiteDataAdapter(command)

            Dim dataTable = New DataTable()
            adapter.Fill(dataTable)
            Return dataTable
        End Using
    End Function

    Public Function ExecuteReader(ByVal command As SQLiteCommand) As SQLiteDataReader
        Using connection = New SQLiteConnection(Me.connectionString)
            connection.Open()

            command.Connection = connection

            Return command.ExecuteReader()
        End Using
    End Function

    Public Function ExecuteScalar(ByVal command As SQLiteCommand) As Object
        Using connection = New SQLiteConnection(Me.connectionString)
            connection.Open()

            command.Connection = connection

            Return command.ExecuteScalar()
        End Using
    End Function
End Class