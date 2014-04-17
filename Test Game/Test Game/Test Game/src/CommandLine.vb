Public Class CommandLine

    Public Delegate Sub CommandHandler(ByRef parameters As String())

    Private open As Boolean = False
    Private currentCommand As String = ""
    Private history As New List(Of String)
    Private commands As New Dictionary(Of String, CommandHandler)

    Public Sub update(ByVal gameTime As GameTime)
        If Input.keyPressed(Input.COMMAND_LINE_KEY) And Not open Then
            open = Not open
            Globals.gamePaused = True
        ElseIf Input.keyPressed(Keys.Escape) And open Then
            open = False
            currentCommand = ""
            Globals.gamePaused = false
        ElseIf open Then
            For Each key As Keys In Input.getPressedKeys()
                If Input.keyPressed(key) Then
                    Select Case key
                        Case Keys.Enter
                            parse(currentCommand)
                            currentCommand = ""
                            'open = False
                        Case Keys.Back
                            If currentCommand.Length() > 0 Then currentCommand = currentCommand.Substring(0, currentCommand.Length() - 1)
                        Case Else
                            currentCommand = currentCommand & keyToChar(key, Input.isKeyDown(Keys.LeftShift) OrElse Input.isKeyDown(Keys.RightShift))
                    End Select
                End If
            Next
        End If
    End Sub

    Public Sub draw(ByVal gameTime As GameTime)
        If open Then
            Dim stringsDrawn As Integer = 0
            If currentCommand.Length() <> 0 Then
                Globals.spriteBatch.DrawString(Resources.georgia_16, "> " & currentCommand, New Vector2(0, 11 * 64 - 25), Color.White)
            Else
                Globals.spriteBatch.DrawString(Resources.georgia_16, "> ", New Vector2(0, 11 * 64 - 25), Color.White)
            End If
            history.Reverse()
            For Each s As String In history
                stringsDrawn += 1
                Globals.spriteBatch.DrawString(Resources.georgia_16, s, New Vector2(0, 11 * 64 - 25 - (25 * stringsDrawn)), Color.White)
            Next
            history.Reverse()
        End If
    End Sub

    Private Sub parse(ByRef command As String)
        Dim tokens As String() = command.Split(" "c)
        Dim commandToken As String = tokens(0)
        If commands.ContainsKey(commandToken) Then
            Dim handler As CommandHandler = commands.Item(commandToken)
            Dim parameters As New List(Of String)
            For i As Integer = 0 To tokens.Length() - 2
                parameters.Add(tokens(i + 1))
            Next
            handler(parameters.ToArray())
        Else
            history.Add("Command: " & commandToken & " does not exist")
        End If
    End Sub

    Public Sub addCommand(ByRef commandName As String, ByVal handler As CommandHandler)
        commands.Add(commandName, handler)
    End Sub

    Public Sub print(ByVal text As String)
        history.Add(text)
    End Sub

    Private Function keyToChar(ByVal key As Keys, ByVal shift As Boolean) As String
        If key >= Keys.A AndAlso key <= Keys.Z Then
            If shift Then
                Return key.ToString()
            Else : Return key.ToString().ToLower()
            End If
        ElseIf key >= Keys.NumPad0 AndAlso key <= Keys.NumPad9 Then
            Return CInt(key - Keys.NumPad0).ToString()
        ElseIf key >= Keys.D0 AndAlso key <= Keys.D9 Then
            Dim val As Integer = CInt(key - Keys.D0)
            If Not shift Then
                Return val.ToString()
            Else
                Select Case val
                    Case 1
                        Return "!"
                    Case 2
                        Return "@"
                    Case 3
                        Return "#"
                    Case 4
                        Return "$"
                    Case 5
                        Return "%"
                    Case 6
                        Return "^"
                    Case 7
                        Return "&"
                    Case 8
                        Return "*"
                    Case 9
                        Return "("
                    Case 0
                        Return ")"
                End Select
            End If
        Else
            Select Case key
                Case Keys.Space
                    Return " "
            End Select
        End If
        Return ""
    End Function

End Class
