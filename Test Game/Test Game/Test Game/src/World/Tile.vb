Public Class Tile
    Public Property textureIndex As Integer
    Public Delegate Sub DrawMethod(spriteBatch As SpriteBatch, ByRef texture As Texture2D, ByVal x As Integer, ByVal y As Integer, ByVal textureWidth As Integer, ByVal textureHeight As Integer)

    Private renderMethod As DrawMethod

    Public Sub New(ByVal textureIndex As Integer)
        Me.textureIndex = textureIndex
        renderMethod = AddressOf defaultDrawMethod
    End Sub

    Public Sub New(ByVal textureIndex As Integer, ByVal renderMethod As DrawMethod)
        Me.textureIndex = textureIndex
        Me.renderMethod = renderMethod
    End Sub

    Public Sub render(spriteBatch As SpriteBatch, ByRef texture As Texture2D, ByVal x As Integer, ByVal y As Integer, Optional ByVal textureWidth As Integer = 32, Optional ByVal textureHeight As Integer = 32)
        renderMethod(spriteBatch, texture, x, y, textureWidth, textureHeight)
    End Sub

    Public Sub defaultDrawMethod(spriteBatch As SpriteBatch, ByRef texture As Texture2D, ByVal x As Double, ByVal y As Double, Optional ByVal textureWidth As Integer = 32, Optional ByVal textureHeight As Integer = 32)
        If textureIndex >= 0 Then
            Globals.spriteBatch.Draw(texture, New Rectangle(CInt(x * 32.0 * Globals.ZOOM_FACTOR), CInt(y * 32.0 * Globals.ZOOM_FACTOR), 32 * Globals.ZOOM_FACTOR, 32 * Globals.ZOOM_FACTOR), New Rectangle((textureIndex Mod 32) * 32, (textureIndex \ 32) * 32, 32, 32), Color.White)
        End If
    End Sub

End Class
