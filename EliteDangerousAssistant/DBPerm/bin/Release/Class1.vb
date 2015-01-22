Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Configuration.Install
Imports System.Security.Principal
Imports System.Security.AccessControl
Imports System.IO

' Set 'RunInstaller' attribute to true.
<RunInstaller(True)> _
Public Class MyInstallerClass
    Inherits Installer

    Public Sub New()
        MyBase.New()
        ' Attach the 'Committed' event. 
        AddHandler Me.Committed, AddressOf MyInstaller_Committed
        ' Attach the 'Committing' event. 
        AddHandler Me.Committing, AddressOf MyInstaller_Committing
    End Sub 'New 

    ' Event handler for 'Committing' event. 
    Private Sub MyInstaller_Committing(ByVal sender As Object, _
                                       ByVal e As InstallEventArgs)

    End Sub 'MyInstaller_Committing

    ' Event handler for 'Committed' event. 
    Private Sub MyInstaller_Committed(ByVal sender As Object, _
                                      ByVal e As InstallEventArgs)

    End Sub 'MyInstaller_Committed

    ' Override the 'Install' method. 
    Public Overrides Sub Install(ByVal savedState As IDictionary)


        ' This gets the named parameters passed in from your custom action
        Dim folder As String = Context.Parameters("folder")

        ' This gets the "Authenticated Users" group, no matter what it's called
        Dim sid As New SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, Nothing)

        ' Create the rules
        Dim writerule As New FileSystemAccessRule(sid, FileSystemRights.Write, InheritanceFlags.ContainerInherit, PropagationFlags.None, AccessControlType.Allow)
        Dim writerule2 As New FileSystemAccessRule(sid, FileSystemRights.Write, InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow)
        If Not String.IsNullOrEmpty(folder) AndAlso Directory.Exists(folder) Then
            ' Get your file's ACL
            Dim fsecurity As DirectorySecurity = Directory.GetAccessControl(folder)

            ' Add the new rule to the ACL
            fsecurity.AddAccessRule(writerule)
            fsecurity.AddAccessRule(writerule2)
            ' Set the ACL back to the file
            Directory.SetAccessControl(folder, fsecurity)
        End If

        ' Explicitly call the overriden method to properly return control to the installer
        MyBase.Install(savedState)
    End Sub 'Install

    ' Override the 'Commit' method. 
    Public Overrides Sub Commit(ByVal savedState As IDictionary)
        MyBase.Commit(savedState)
    End Sub 'Commit

    ' Override the 'Rollback' method. 
    Public Overrides Sub Rollback(ByVal savedState As IDictionary)
        MyBase.Rollback(savedState)
    End Sub 'Rollback
    Public Shared Sub Main()
        Console.WriteLine("Usage : installutil.exe Installer.exe ")
    End Sub 'Main
End Class 'MyInstallerClass