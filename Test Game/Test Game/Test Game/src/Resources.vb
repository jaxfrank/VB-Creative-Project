Public Class Resources

    Public Shared terrain As Texture2D
    Public Shared playerTexture As Texture2D
    Public Shared debugTextures As Texture2D

    Public Shared georgia_16 As SpriteFont
    Public Shared debugModeFont As SpriteFont

    Public Shared Sub load()
        'Textures
        terrain = Globals.content.Load(Of Texture2D)("terrain")
        playerTexture = Globals.content.Load(Of Texture2D)("george")
        debugTextures = Globals.content.Load(Of Texture2D)("debugTextures")

        'Fonts
        georgia_16 = Globals.content.Load(Of SpriteFont)("Georgia_16")
        debugModeFont = georgia_16
    End Sub

    Public Shared Sub unload()
        Globals.content.Unload()
    End Sub

End Class
