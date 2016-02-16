Public Class ScreenManager

    Private screens As New Dictionary(Of String, Screen)
    Private currentScreen As String

    Public Sub update(gameTime As GameTime)
        screens(currentScreen).update(gameTime)
    End Sub

    Public Sub render(gameTime As GameTime)
        screens(currentScreen).render(gameTime)
    End Sub

    Public Sub addScreen(screenID As String, screen As Screen)
        screens.Add(screenID, screen)
    End Sub

    Public Sub removeScreen(screenID As String)
        screens.Remove(screenID)
    End Sub

    Public Function getCurrentScreen() As Screen
        Return screens.Item(currentScreen)
    End Function

    Public Function setCurrentScreen(screenID As String) As Boolean
        If screens.ContainsKey(screenID) Then
            currentScreen = screenID
            Return True
        End If
        Return False
    End Function

End Class
