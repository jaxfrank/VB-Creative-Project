Imports Microsoft.CSharp
Imports System.CodeDom.Compiler

Public Class Compiler

    Public cp As CompilerParameters

    Private vbCodeProvider As CodeDomProvider
    Private csCodeProvider As CodeDomProvider

    Public Sub New()
        vbCodeProvider = CodeDomProvider.CreateProvider("VisualBasic")
        csCodeProvider = CodeDomProvider.CreateProvider("CSharp")

        cp = New CompilerParameters()
        cp.GenerateExecutable = False
        cp.OutputAssembly = "test.dll"
        cp.GenerateInMemory = True
        cp.MainClass = "Main"
        cp.ReferencedAssemblies.Add("System.dll")
        cp.ReferencedAssemblies.Add("Test Game.exe")
        cp.ReferencedAssemblies.Add(Globals.XNA_INSTALL_LOCATION & "\References\Windows\x86\Microsoft.Xna.Framework.dll")
    End Sub

    Public Function compileVB(ByRef code As String) As CompilerResults
        Return vbCodeProvider.CompileAssemblyFromSource(cp, code)
    End Function

    Public Function compileCS(ByRef code As String) As CompilerResults
        Return csCodeProvider.CompileAssemblyFromSource(cp, code)
    End Function

    Public Function compileCSFromFiles(ByRef files As String()) As CompilerResults
        Return csCodeProvider.CompileAssemblyFromFile(cp, files)
    End Function

End Class
