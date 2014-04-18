Imports Microsoft.CSharp
Imports System.CodeDom.Compiler

Public Class Compiler

    Private vbCodeProvider As CodeDomProvider
    Private csCodeProvider As CodeDomProvider

    Public Sub New()
        vbCodeProvider = CodeDomProvider.CreateProvider("VisualBasic")
        csCodeProvider = CodeDomProvider.CreateProvider("CSharp")
    End Sub

    Public Function compileVB(ByRef code As String) As CompilerResults
        Return vbCodeProvider.CompileAssemblyFromSource(getCompilerParameters(), code)
    End Function

    Public Function compileCS(ByRef code As String) As CompilerResults
        Return csCodeProvider.CompileAssemblyFromSource(getCompilerParameters(), code)
    End Function

    Public Function compileCSFromFiles(ByRef files As String()) As CompilerResults
        Return csCodeProvider.CompileAssemblyFromFile(getCompilerParameters(), files)
    End Function

    Private Function getCompilerParameters() As CompilerParameters
        Dim cp As New CompilerParameters()
        cp.GenerateExecutable = False
        cp.OutputAssembly = "test.dll"
        cp.GenerateInMemory = False
        cp.MainClass = "Main"
        cp.ReferencedAssemblies.Add("System.dll")
        cp.ReferencedAssemblies.Add("Test Game.exe")

        Return cp
    End Function

End Class
