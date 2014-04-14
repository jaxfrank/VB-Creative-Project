Public Class Input
    Shared currentKeyState As KeyboardState
    Shared previousKeyStare As KeyboardState
    Shared currentMouseState As MouseState
    Shared previouseMouseState As MouseState

    Public Shared Sub update()
        previousKeyStare = currentKeyState
        currentKeyState = Keyboard.GetState()
        previouseMouseState = currentMouseState
        currentMouseState = Mouse.GetState()
    End Sub

    Public Shared Function isKeyDown(key As Keys) As Boolean
        Return currentKeyState.IsKeyDown(key)
    End Function

End Class
