Public Class Util

    Private Shared numDebugStringsDrawn As Integer = 0

    Public Shared Sub newFrame()
        numDebugStringsDrawn = 0
    End Sub

    Public Shared Sub drawDebugText(ByRef text As String, ByVal color As Color)
        If Globals.inDebugMode() Then
            Globals.spriteBatch.DrawString(Resources.debugModeFont, text, New Vector2(0, numDebugStringsDrawn * (Resources.debugModeFont.MeasureString("Y").Y)), color)
            numDebugStringsDrawn += 1
        End If
    End Sub
End Class
