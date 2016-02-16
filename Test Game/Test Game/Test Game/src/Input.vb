Public Class Input

    'Begin keybinds
    Public Shared COMMAND_LINE_KEY As Keys = Keys.OemTilde

    Public Shared UP As Keys = Keys.W
    Public Shared DOWN As Keys = Keys.S
    Public Shared LEFT As Keys = Keys.A
    Public Shared RIGHT As Keys = Keys.D
    'End keybinds

    Private Shared currentKeyState As KeyboardState
    Private Shared previousKeyStare As KeyboardState
    Private Shared currentMouseState As MouseState
    Private Shared previouseMouseState As MouseState

    Public Shared Sub update()
        previousKeyStare = currentKeyState
        currentKeyState = Keyboard.GetState()
        previouseMouseState = currentMouseState
        currentMouseState = Mouse.GetState()
    End Sub

    Public Shared Function isKeyDown(key As Keys) As Boolean
        Return currentKeyState.IsKeyDown(key)
    End Function

    Public Shared Function keyPressed(key As Keys) As Boolean
        Return Not previousKeyStare.IsKeyDown(key) AndAlso currentKeyState.IsKeyDown(key)
    End Function

    Public Shared Function keyReleased(key As Keys) As Boolean
        Return previousKeyStare.IsKeyDown(key) AndAlso Not currentKeyState.IsKeyDown(key)
    End Function

    Public Shared Function getPressedKeys() As Keys()
        Return currentKeyState.GetPressedKeys()
    End Function

End Class
