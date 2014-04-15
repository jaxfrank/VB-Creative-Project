Public Class Resources

    Public Shared terrain As Texture2D

    Public Shared Sub load()
        terrain = Globals.content.Load(Of Texture2D)("terrain")
    End Sub

    Public Shared Sub unload()
        Globals.content.Unload()
    End Sub

End Class
